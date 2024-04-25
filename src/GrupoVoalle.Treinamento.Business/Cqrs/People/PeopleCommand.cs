using GrupoVoalle.CrossCutting.Core.Cqrs.Commands;
using GrupoVoalle.Domain.Core.Common.Attributes;

namespace GrupoVoalle.Base.Business.Cqrs.People
{
    /// <summary>
    /// Create
    /// </summary>
    public class PeopleCreateCommand : Command
    {
        [LogTableItem("Name", "Nome")]
        public string Name { get; set; }

        [LogTableItem("TypeTxId", "Tipo documento")]
        public long TypeTxId { get; set; }

        [LogTableItem("TxId", "CPF/CNPJ")]
        public string TxId { get; set; }
    }

    /// <summary>
    /// Update
    /// </summary>
    public class PeopleUpdateCommand : PeopleCreateCommand
    {
        public long Id { get; set; }
    }

    /// <summary>
    /// Delete
    /// </summary>
    public class PeopleDeleteCommand : TIdCommand<long>
    {}

    /// <summary>
    /// Get
    /// </summary>
    public class PeopleGetCommand : TIdCommand<long>
    {}

    public class PeopleGetPagedCommand : GetPagedCommand
    {}
}