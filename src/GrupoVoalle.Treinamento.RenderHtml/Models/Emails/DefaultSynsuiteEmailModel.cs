namespace GrupoVoalle.Treinamento.RenderHtml.Models.Emails
{
    public class DefaultSynsuiteEmailModel
    {
        /// <summary>
        /// Título do email
        /// </summary>
        public string HeaderTitle { get; set; }

        /// <summary>
        /// Subtítulo do email
        /// </summary>
        public string HeaderSubTitle { get; set; }

        /// <summary>
        /// Lista de itens que vão compor o conteúdo do email
        /// </summary>
        public IDictionary<string, string> Items { get; set; }

        /// <summary>
        /// Url da imagem do footer
        /// </summary>
        public string ImageFooter { get; set; }

        /// <summary>
        /// Url da imagem do header
        /// </summary>
        public string ImageHeader { get; set; }
    }
}