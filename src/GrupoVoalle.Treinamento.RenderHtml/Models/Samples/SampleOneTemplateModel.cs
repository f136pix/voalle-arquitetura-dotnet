namespace GrupoVoalle.Treinamento.RenderHtml.Models.Samples
{
    public class SampleOneTemplateModel
    {
        public string Text { get; set; }

        public List<SampleOnePartialViewTemplateModel> ListValues { get; set; }

        public SampleOneTemplateModel()
        {
            ListValues = new List<SampleOnePartialViewTemplateModel>();
        }
    }

    public class SampleOnePartialViewTemplateModel
    {
        public string Value { get; set; }
    }
}