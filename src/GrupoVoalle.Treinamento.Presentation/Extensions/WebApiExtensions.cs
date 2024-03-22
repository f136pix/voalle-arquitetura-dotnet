using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace GrupoVoalle.Treinamento.Presentation.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class WebApiExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMvcBuilder AddWebApi(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var builder = services.AddMvcCore();
            builder.AddNewtonsoftJson();
            builder.AddApiExplorer();

            builder.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    b => b
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader());
            });

            return new MyMvcBuilder(builder.Services, builder.PartManager);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMvcBuilder AddWebApi(this IServiceCollection services, Action<MvcOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            var builder = services.AddWebApi();
            builder.Services.Configure(setupAction);

            return builder;
        }
    }

    /// <inheritdoc />
    public class MyMvcBuilder : IMvcBuilder
    {
        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <inheritdoc />
        public ApplicationPartManager PartManager { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="partManager"></param>
        public MyMvcBuilder(IServiceCollection services, ApplicationPartManager partManager)
        {
            Services = services;
            PartManager = partManager;
        }
    }
}