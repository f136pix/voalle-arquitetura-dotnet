using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using GrupoVoalle.Utility.ConfigBuilders;
using GrupoVoalle.Treinamento.CrossCutting.IoC;
using GrupoVoalle.Treinamento.Presentation.Extensions;
using GrupoVoalle.Queues.Core.Helper;
using System.Reflection;
using GrupoVoalle.Utility.Globalization;
using GrupoVoalle.Utility.Middlewares;
using GrupoVoalle.Utility.Security.Data;

namespace GrupoVoalle.Treinamento.Presentation
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var configAuth = ConfigAuthSettings.Instance.Configuration;
            var authority = configAuth["Auth:AuthorizeServer"];
            var requireHttpsMetadata = bool.Parse(configAuth["Auth:RequireHttpsMetadata"] ?? "false");

            services.AddMvc();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddWebApi(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.UseCentralRoutePrefix(new RouteAttribute("api/v1"));
            });

            services.AddSwaggerWebApi();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "syngw", //TODO HOFFMANN arrumar para policy do serviço
                    policy => policy
                        .RequireScope("syngw")
                        .RequireClaim("type", "user-system")
                );
            });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = requireHttpsMetadata;
                    options.ApiName = "syngw";
                    options.JwtValidationClockSkew = new TimeSpan(0, 10, 0);
                });

            RegisterServices(services);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Globalization.SetToPtBR();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors("AllowAll");

            app.UseStaticFiles();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware(typeof(LogAccessHandlingMiddleware));

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger(x =>
            {
                x.PreSerializeFilters.Add((document, _) =>
                {
                    var paths = new OpenApiPaths();
                    foreach (var path in document.Paths)
                    {
                        paths.Add(path.Key.ToLowerInvariant(), path.Value);
                    }
                    document.Paths = paths;
                });
            });

            // Em ambiente de produção o Swagger é desabilitado
            // if (env.IsDevelopment())
            // {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrupoVoalle.Treinamento.Presentation v1");
                c.RoutePrefix = string.Empty; // Abra o Swagger na raiz do aplicativo
            });
            // }
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAccessorData, AccessorDataHttp>();

            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            // RabbitMQ
            services.AddRabbitConnection();

            NativeInjectorBootStrapper.RegisterServices(services);

            services.AddControllersWithViews(options =>
            {
                options.EnableEndpointRouting = false;
            });
        }
    }
}