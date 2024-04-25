using GrupoVoalle.Domain.Core.Common.Attributes;
using GrupoVoalle.Domain.Core.Entities;
using GrupoVoalle.Utility.Extensions;
using GrupoVoalle.Utility.Validations;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.ComponentModel.DataAnnotations;

namespace GrupoVoalle.Base.Business.Primitives.People
{
    [HasSelfValidation]
    [LogTable("people", "Pessoas")] 
    public class PeopleEntity : EntityBase<long>
    {
        public PeopleEntity()
        {
        }
        public virtual long TypeTxId { get; set; }

        public virtual string TxId { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Apelido
        /// </summary>
        /// <value></value>
        public virtual string Name2 { get; set; }

        /// <summary>
        /// Deletado
        /// </summary>
        /// <value></value>
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// Criação
        /// </summary>
        /// <value></value>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Criado por
        /// </summary>
        /// <value></value>
        public virtual long CreatedBy { get; set; }

        /// <summary>
        /// Modificado
        /// </summary>
        /// <value></value>
        public virtual DateTime? Modified { get; set; }

        /// <summary>
        /// Modificado por
        /// </summary>
        /// <value></value>
        public virtual long? ModifiedBy { get; set; }

        /// <summary>
        /// Valida o Cpf e Cnpj
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public virtual void ValidateTxIdIsValid(ValidationResults results)
        {
            if (TxId.IsEmpty() || TypeTxId == 3) return;

            switch (TypeTxId)
            {
                case 0:
                {
                    AddError(results, "Tipo de documento inválido.");
                    break;
                }
                case 1:
                {
                    if (!GenericValidation.IsValidCNPJ(TxId))
                        AddError(results, "Cnpj inválido.");
                    break;
                }
                case 2:
                {
                    if (!GenericValidation.IsValidCPF(TxId))
                        AddError(results, "Cpf inválido.");
                    break;
                }
            }
        }
    }
}