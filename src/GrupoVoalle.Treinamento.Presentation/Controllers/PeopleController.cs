using GrupoVoalle.Base.Business.Cqrs.People;
using GrupoVoalle.Treinamento.Presentation.Controllers.WebApi;
using GrupoVoalle.Utility.CustomExceptions;
using GrupoVoalle.Utility.DynamicFields.Binders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace GrupoVoalle.Base.Presentation.Controllers;

/// <summary>
/// Controller pessoas
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class PeopleController : WebApiMediatorControllerBase
{
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="provider"></param>
    public PeopleController(IServiceProvider provider) : base(provider)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(PeopleCreateCommand command)
    {
        return await DefaultActionResult(async () =>
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            return await Bus.SendCommand(command);
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(long id, PeopleUpdateCommand command)
    {
        return await DefaultActionResult(async () =>
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            if (id != command.Id)
                throw new BusinessException("Id não corresponde ao Id do model");

            return await Bus.SendCommand(command);
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        return await DefaultActionResult(async () =>
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            if (id == default)
                throw new ArgumentException("Id não pode ser zero");

            var command = new PeopleDeleteCommand
            {
                Id = id
            };

            return await Bus.SendCommand(command);
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        return await DefaultActionResult(async () =>
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            if (id == default)
                throw new ArgumentNullException("Id não pode ser zero");

            var command = new PeopleGetCommand
            {
                Id = id
            };

            return await Bus.SendCommand(command);
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] DataSourceRequest request)    
    {
        return await DefaultActionResult(async () =>
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var command = new PeopleGetPagedCommand
            {
                PageSize = request.PageSize,
                Page = request.Page,
                Filter = request.Filter,
                OrderBy = request.OrderBy
            };

            return await Bus.SendCommand(command);
        });
    }
}