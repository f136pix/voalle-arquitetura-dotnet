using GrupoVoalle.Treinamento.Presentation.Controllers.WebApi.Enums;
using Microsoft.AspNetCore.Mvc;
using GrupoVoalle.CrossCutting.Core.Bus;
using GrupoVoalle.Domain.Core.Interfaces;
using GrupoVoalle.Utility.CustomExceptions;
using GrupoVoalle.Utility.Messages;
using System.Diagnostics;

namespace GrupoVoalle.Treinamento.Presentation.Controllers.WebApi;

/// <summary>
/// Controller CQRS
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class WebApiMediatorControllerBase : BaseController
{
    /// <summary>
    /// Bus
    /// </summary>
    protected readonly IMediatorHandler Bus;
    /// <summary>
    /// Uow
    /// </summary>
    protected readonly IUnitOfWork Uow;

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="provider"></param>
    protected WebApiMediatorControllerBase(IServiceProvider provider) : base(provider)
    {
        Bus = provider.GetService<IMediatorHandler>();
        Uow = provider.GetService<IUnitOfWork>();
    }

    /// <summary>
    /// Executa função, trata exceptions e código HTTP
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="responseTypeEnum"></param>
    /// <returns></returns>
    protected async Task<IActionResult> DefaultActionResult(
        Func<Task<ResponseMessage>> sender,
        ResponseTypeEnum responseTypeEnum = ResponseTypeEnum.Ok
    )
    {
        var elapsedTime = new Stopwatch();
        elapsedTime.Start();

        try
        {
            var result = await sender();

            if (!result.Success)
                return BadRequest(result);

            result.SetElapsedTime(elapsedTime);

            return responseTypeEnum switch
            {
                ResponseTypeEnum.Ok => Ok(result),
                ResponseTypeEnum.FilePDF => File((byte[])result.Response, "application/Pdf"),
                _ => throw new ArgumentOutOfRangeException(nameof(responseTypeEnum), responseTypeEnum, null)
            };
        }
        catch (NotFoundException nex)
        {
            Uow.ThereIsError();

            return NotFound(ResponseMessage.ToError(nex));
        }
        catch (BusinessException bex)
        {
            Uow.ThereIsError();

            return BadRequest(ResponseMessage.ToError(bex));
        }
        catch (ArgumentException argException)
        {
            Uow.ThereIsError();

            return BadRequest(ResponseMessage.ToError(argException));
        }
        catch (NotImplementedException nImpException)
        {
            Uow.ThereIsError();

            return BadRequest(ResponseMessage.ToError(nImpException));
        }
        catch (Exception ex)
        {
            Uow.ThereIsError();

            return StatusCode(500, ResponseMessage.ToError(ex));
        }
    }
}
