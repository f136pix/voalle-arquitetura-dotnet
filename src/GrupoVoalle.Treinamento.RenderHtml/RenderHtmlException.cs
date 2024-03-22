namespace GrupoVoalle.Treinamento.RenderHtml
{
    [Serializable]
    public class RenderHtmlException : Exception
    {
        public RenderHtmlException() { }
        public RenderHtmlException(string message) : base(message) { }
        public RenderHtmlException(string message, Exception inner) : base(message, inner) { }
    }
}