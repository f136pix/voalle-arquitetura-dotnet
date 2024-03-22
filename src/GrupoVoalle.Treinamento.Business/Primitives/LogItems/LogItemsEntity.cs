using GrupoVoalle.Domain.Core.Entities;
using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
using System.ComponentModel.DataAnnotations;

namespace GrupoVoalle.Treinamento.Business.Primitives.LogItems
{
    public class LogItemsEntity : EntityBase<long>
    {
        /// <summary>
        /// Cabeçalho das alterações
        /// </summary>
        /// <value></value>
        public virtual LogTablesEntity LogTable { get; set; }
        public virtual long LogTablesId { get; set; }

        [Required(ErrorMessage = "O campo Nome da Coluna é obrigatório")]
        [StringLength(64, ErrorMessage = "O tamanho máximo do campo Nome da Coluna é de 64 caracteres")]
        public virtual string ColumnName { get; set; }

        [Required(ErrorMessage = "O campo Nome da Coluna Amigavel é obrigatório")]
        [StringLength(64, ErrorMessage = "O tamanho máximo do campo Nome da Coluna Amigavel é de 64 caracteres")]
        public virtual string FriendlyColumnName { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string FriendlyOldValue { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string FriendlyNewValue { get; set; }

        public virtual string ColumnType { get; set; }
    }
}