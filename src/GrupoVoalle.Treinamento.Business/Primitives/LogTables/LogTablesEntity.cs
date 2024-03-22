using GrupoVoalle.Domain.Core.Common.Enum;
using GrupoVoalle.Domain.Core.Entities;
using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using System.ComponentModel.DataAnnotations;

namespace GrupoVoalle.Treinamento.Business.Primitives.LogTables
{
    /// <summary>
    /// Registro de log de alteração de uma entidade
    /// </summary>
    public class LogTablesEntity : EntityBase<long>
    {
        public LogTablesEntity()
        {
            LogItems = new HashSet<LogItemsEntity>();
        }
        /// <summary>
        /// Identificador único da alteração
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Guid de Transação é obrigatório")]
        public virtual Guid TransactionGuid { get; set; }

        /// <summary>
        /// Entidade a qual foi realizado a alteração
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Nome da Tabela é obrigatório")]
        [StringLength(64, ErrorMessage = "O tamanho máximo do campo Nome da Tabela é de 64 caracteres")]
        public virtual string TableName { get; set; }

        /// <summary>
        /// Nome amigável da entidade
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Nome da Tabela Amigável é obrigatório")]
        [StringLength(64, ErrorMessage = "O tamanho máximo do campo Nome da Tabela Amigável é de 64 caracteres")]
        public virtual string FriendlyTableName { get; set; }

        /// <summary>
        /// Identificador que foi alterado
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Registro Chave é obrigatório")]
        [StringLength(36, ErrorMessage = "O tamanho máximo do campo Registro chave é de 36 caracteres")]
        public virtual string RegisterKey { get; set; }

        /// <summary>
        /// Identificador do usuário
        /// </summary>
        /// <value></value>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        /// <value></value>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Login do usuário
        /// </summary>
        /// <value></value>
        public virtual string UserLogin { get; set; }

        /// <summary>
        /// Origem
        /// </summary>
        /// <value></value>
        public virtual int Origin { get; set; }

        /// <summary>
        /// Data em que foi gerado a alteração
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Data do Log é obrigatório")]
        public virtual DateTime DateLog { get; set; }

        /// <summary>
        /// Tipo da operação que foi realizada
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Operação é obrigatório")]
        public virtual LogTablesTypeOperationEnum Operation { get; set; }

        /// <summary>
        /// Nome da aplicação que gerou a alteração
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Aplicação é obrigatório")]
        [StringLength(128, ErrorMessage = "O tamanho máximo do campo Aplicação é de 128 caracteres")]
        public virtual string Application { get; set; }

        /// <summary>
        /// Identificador da pessoa
        /// </summary>
        /// <value></value>
        public virtual long? PersonId { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        /// <value></value>
        public virtual string PersonName { get; set; }

        /// <summary>
        /// Campos da entidade que foram alterados
        /// </summary>
        /// <value></value>
        public virtual ICollection<LogItemsEntity> LogItems { get; set; }
    }
}