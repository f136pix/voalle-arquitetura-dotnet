using GrupoVoalle.Utility.Security.Http;
using GrupoVoalle.Web.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GrupoVoalle.Treinamento.Presentation.Controllers.WebApi
{
    /// <inheritdoc />
    [TransactionDb]
    public abstract class DbController : ControllerBase
    {
        /// <summary>
        /// User
        /// </summary>
        public UserHttp UserHttp { get; private set; }

        /// <summary>
        /// Provider para injeção de dependência
        /// </summary>
        protected readonly IServiceProvider _provider;

        /// <inheritdoc />
        public DbController(IServiceProvider provider)
        {
            UserHttp = provider.GetService<UserHttp>();
            _provider = provider;
        }

        /// <summary>
        /// User
        /// </summary>
        protected long? CurrentUserId
        {
            get
            {
                if (User.Identity != null && (!User.Identity.IsAuthenticated || UserHttp == null))
                    return null;

                return UserHttp.Id;
            }
        }
    }
}