using GrupoVoalle.Treinamento.Business.Business.Interfaces.WebService;
using GrupoVoalle.Utility.Extensions;
using GrupoVoalle.Utility.Messages;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace GrupoVoalle.Treinamento.ExternalCommunications.WebService
{
    public class ExempleWebService : IExempleWebService
    {
        private string BaseUrl { get; }
        private int TimeoutInMilliseconds { get; }

        public ExempleWebService()
        {
            // https://webhook.site/#!/view/c30f315f-fd53-453c-9c10-27df3b9c86a4
            BaseUrl = "https://webhook.site/c30f315f-fd53-453c-9c10-27df3b9c86a4";
            TimeoutInMilliseconds = (int)TimeSpan.FromMinutes(3).TotalMilliseconds;
        }

        /// <summary>
        /// Prepara a requisição de integração com o servidor com base nos parâmetros passados
        /// </summary>
        /// <param name="method">Método HTTP da requisição</param>
        /// <param name="body">Corpo da requisição</param>
        public RestRequest PrepareRequest(Method method, object body = null)
        {
            var request = new RestRequest
            {
                Method = method,
                Timeout = TimeoutInMilliseconds
            };

            if (body == null) return request;

            var jsonBody = JsonConvert.SerializeObject(
                value: body,
                formatting: Formatting.None
            );
            request.AddJsonBody(jsonBody);

            return request;
        }

        /// <summary>
        /// Executa a requisição de integração com o servidor com base nos parâmetros passados
        /// </summary>
        /// <param name="endpoint">Endpoint/recurso</param>
        /// <param name="request">Requisição</param>
        public async Task<RestResponse> ExecuteRequestAsync(string endpoint, RestRequest request)
        {
            var restClientOptions = new RestClientOptions
            {
                BaseUrl = new Uri($"{BaseUrl}/{endpoint}"),
                RemoteCertificateValidationCallback = (_, _, _, _) => true,
                MaxTimeout = TimeoutInMilliseconds
            };

            var client = new RestClient(restClientOptions);

            var response = await client.ExecuteAsync(request);

            if (response.Content == null)
                throw new Exception("Falha ao se comunicar com o servidor");

            return response;
        }

        /// <summary>
        /// Com base em um tipo genérico T, realiza o tratamento e validação do conteúdo da resposta HTTP para o tipo concreto parametrizado
        /// </summary>
        /// <param name="response">Objeto response da requisição enviada ao servidor</param>
        /// <typeparam name="T">Tipo do dado retornado pelo servidor no campo response dentro do objeto de retorno</typeparam>
        public ResponseMessage<T> ParseResponseContent<T>(RestResponse response)
        {
            var isSuccess = response.IsSuccessful && response.StatusCode == HttpStatusCode.OK;
            if (!isSuccess)
                throw new Exception("Erro ao se comunicar com o servidor");

            var isContentValid = response.Content.TryParseJson(out ResponseMessage<T> result) && result.Response != null;
            if (!isContentValid)
                throw new Exception("Erro ao processar comunicação com o servidor");

            return result;
        }
    }
}