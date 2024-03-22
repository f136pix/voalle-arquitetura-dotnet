using GrupoVoalle.Utility.Extensions;
using System.Reflection;
using WebMarkupMin.Core;

namespace GrupoVoalle.Treinamento.RenderHtml
{
    public class ConvertHtml
    {
        /// <summary>
        /// Converte Html para String
        /// </summary>
        /// <param name="typeRenderHtml"></param>
        /// <param name="model"></param>
        /// <param name="minified"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Render<T>(FilesHtmlEnum typeRenderHtml, T model, bool minified = true)
        {
            try
            {
                return Instance.InternalRender(
                    typeRenderHtml: typeRenderHtml,
                    model: model,
                    minified: minified
                );
            }
            catch (Exception ex)
            {
                throw new RenderHtmlException($"Erro ao gerar html. [{ex.Message}]", ex);
            }
        }

        private readonly Assembly _assembly;

        private ConvertHtml()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        /// <summary>
        /// Renderizador
        /// </summary>
        /// <param name="typeRenderHtml"></param>
        /// <param name="model"></param>
        /// <param name="minified"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string InternalRender<T>(FilesHtmlEnum typeRenderHtml, T model, bool minified)
        {
            var htmlRaw = UsingTemplateFromEmbedded(
                typeRenderHtml.ToDescription(),
                model
            );

            try
            {
                return (minified)
                    ? Minify(htmlRaw)
                    : htmlRaw;
            }
            catch
            {
                return htmlRaw;
            }
        }

        /// <summary>
        /// Usa template injetado
        /// </summary>
        /// <param name="path"></param>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string UsingTemplateFromEmbedded<T>(string path, T model)
        {
            var resourceName = GenerateFileAssemblyPath(path);

            using var stream = _assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new Exception($"Recurso {resourceName} não encontrado no assembly {_assembly.FullName}.");
            }

            using var reader = new StreamReader(stream);

            var htmlContent = reader.ReadToEnd();

            ReplaceTags(ref htmlContent, model);

            return htmlContent;
        }

        /// <summary>
        /// Busca template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private string GenerateFileAssemblyPath(string template)
        {
            var assemblyName = _assembly.GetName().Name;
            return $"{assemblyName}.{template}.html";
        }

        /// <summary>
        /// Substitui as tags em um conteúdo HTML pelos valores do modelo.
        /// </summary>
        /// <typeparam name="T">Tipo do modelo.</typeparam>
        /// <param name="htmlContent">Conteúdo HTML com marcadores de posição.</param>
        /// <param name="model">Modelo contendo valores para substituir os marcadores de posição.</param>
        private static void ReplaceTags<T>(ref string htmlContent, T model)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var placeholder = $"{{{{{property.Name}}}}}";
                var value = GetFormattedValue(property, model);

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var list = GetListReplacement(property, model);
                    htmlContent = htmlContent.Replace(placeholder, list);
                }
                else
                {
                    htmlContent = htmlContent.Replace(placeholder, value);
                }
            }
        }

        /// <summary>
        /// Obtém o valor formatado para uma propriedade do modelo.
        /// </summary>
        /// <typeparam name="T">Tipo do modelo.</typeparam>
        /// <param name="property">Propriedade para obter o valor.</param>
        /// <param name="model">Modelo contendo a propriedade.</param>
        /// <returns>Valor formatado como string.</returns>
        private static string GetFormattedValue<T>(PropertyInfo property, T model)
        {
            var propertyValue = property.GetValue(model);
            return propertyValue?.ToString();
        }

        /// <summary>
        /// Obtém a substituição para uma propriedade de lista no conteúdo HTML.
        /// </summary>
        /// <typeparam name="T">Tipo do modelo.</typeparam>
        /// <param name="property">Propriedade de lista para obter a substituição.</param>
        /// <param name="model">Modelo contendo a propriedade de lista.</param>
        /// <returns>Substituição da lista formatada como string.</returns>
        private static string GetListReplacement<T>(PropertyInfo property, T model)
        {
            var listType = property.PropertyType.GetGenericArguments()[0];
            var listItemProperty = listType.GetProperty("Value");

            var list = "";
            var items = property.GetValue(model) as IEnumerable<object>;

            if (items == null)
                return list;

            foreach (var item in items)
            {
                var listItemValue = listItemProperty?.GetValue(item);
                list += $"<li>{listItemValue}</li>";
            }

            return list;
        }

        /// <summary>
        /// Minificar
        /// </summary>
        /// <param name="htmlRaw"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string Minify(string htmlRaw)
        {
            var config = new HtmlMinificationSettings();
            config.RemoveOptionalEndTags = false;

            var process = new HtmlMinifier(config).Minify(htmlRaw);
            if (process.Errors.Count > 0)
                throw new Exception("Erro na minificação do html");

            return process.MinifiedContent;
        }

        private static ConvertHtml _instance;
        private static readonly object Padlock = new object();
        private static ConvertHtml Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (Padlock)
                {
                    _instance ??= new ConvertHtml();
                }

                return _instance;
            }
        }
    }
}