using GrupoVoalle.Utility.Security.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoVoalle.Treinamento.Presentation.Controllers.WebApi
{
    /// <inheritdoc />
    [Authorize(Policy = "syngw")]
    public abstract class BaseController : DbController
    {
        /// <inheritdoc />
        public BaseController(IServiceProvider provider) : base(provider) { }

        /// <summary>
        /// File
        /// </summary>
        /// <param name="fileContents"></param>
        /// <param name="contentType"></param>
        /// <param name="fileDownloadName"></param>
        /// <returns></returns>
        public override FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName)
        {
            AddHeaderPermission();
            return base.File(fileContents, contentType, fileDownloadName);
        }

        /// <summary>
        /// File
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="contentType"></param>
        /// <param name="fileDownloadName"></param>
        /// <returns></returns>
        public override FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName)
        {
            AddHeaderPermission();
            return base.File(fileStream, contentType, fileDownloadName);
        }

        private void AddHeaderPermission()
        {
            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
        }
    }
}