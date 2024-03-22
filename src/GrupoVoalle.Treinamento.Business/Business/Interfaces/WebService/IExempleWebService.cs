using GrupoVoalle.Utility.Messages;
using RestSharp;

namespace GrupoVoalle.Treinamento.Business.Business.Interfaces.WebService
{
    public interface IExempleWebService
    {
        /// <summary>
        /// Prepara a requisição de integração com o servidor com base nos parâmetros passados
        /// </summary>
        /// <param name="method">Método HTTP da requisição</param>
        /// <param name="body">Corpo da requisição</param>
        RestRequest PrepareRequest(Method method, object body = null);

        /// <summary>
        /// Executa a requisição de integração com o servidor com base nos parâmetros passados
        /// </summary>
        /// <param name="endpoint">Endpoint/recurso</param>
        /// <param name="request">Requisição</param>
        Task<RestResponse> ExecuteRequestAsync(string endpoint, RestRequest request);

        /// <summary>
        /// Com base em um tipo genérico T, realiza o tratamento e validação do conteúdo da resposta HTTP para o tipo concreto parametrizado
        /// </summary>
        /// <param name="response">Objeto response da requisição enviada ao servidor</param>
        /// <typeparam name="T">Tipo do dado retornado pelo servidor no campo response dentro do objeto de retorno</typeparam>
        ResponseMessage<T> ParseResponseContent<T>(RestResponse response);
    }
}