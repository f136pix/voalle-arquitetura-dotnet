using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GrupoVoalle.Treinamento.Presentation.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerWebApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GrupoVoalle.Treinamento.Presentation",
                    Version = "v9.0.32",
                    Description = "Projeto base back-end Voalle Technology.",
                    Contact = new OpenApiContact
                    {
                        Name = "Voalle",
                        Email = string.Empty,
                        Url = new Uri("https://grupovoalle.com.br/")
                    }
                });

                // var security = new Dictionary<string, IEnumerable<string>>
                // {
                //     {"Bearer", new string[] { }},
                // };

                // s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                // {
                //     Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\". Coloque no campo Value o seguinte formato: Bearer {token}",
                //     Name = "Authorization",
                //     In = "header",
                //     Type = "apiKey"
                // });
                // s.AddSecurityRequirement(security);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\". Coloque no campo Value o seguinte formato: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                var security = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                };
                s.AddSecurityRequirement(security);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }
    }
}