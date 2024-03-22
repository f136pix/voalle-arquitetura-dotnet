using System.ComponentModel.DataAnnotations.Schema;

namespace GrupoVoalle.Treinamento.Business.Business.DTO.Functions
{
    public class FunctionFriendlyName
    {
        /// <summary>
        /// Nome amigavel
        /// </summary>
        /// <value></value>
        [Column("name")]
        public string Name { get; set; }
    }
}