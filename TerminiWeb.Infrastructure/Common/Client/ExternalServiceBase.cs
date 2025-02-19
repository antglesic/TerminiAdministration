using System.Collections.Specialized;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using TerminiWeb.Infrastructure.Common.Client.Security;
using TerminiWeb.Infrastructure.Common.Configuration;

namespace TerminiWeb.Infrastructure.Common.Client
{
	public abstract class ExternalServiceBase
	{
		#region Fields

		protected readonly RestClient _client;
		protected readonly ApiEndpointSettings _apiEndpointSettings;
		protected readonly ILogger _logger;

		#endregion

		#region Constructor

		public ExternalServiceBase(IOptions<ApiEndpointSettings> apiEndpointSettings)
		{
			_apiEndpointSettings = apiEndpointSettings.Value;
			_client = new RestClient();
			_logger = Log.ForContext(this.GetType());
		}

		#endregion

		#region Methods

		/// <summary>
		/// Helper method for client request
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="url"></param>
		/// <param name="method"></param>
		/// <param name="parameters">request parameters</param>
		/// <returns></returns>
		protected async Task<T> ClientAppRequest<T>(string url, Method method, NameValueCollection parameters = null)
		{
			RestRequest request = new RestRequest(url, Method.Get);

			request.AddHeader("Authorization", CreateToken());
			if (parameters != null)
			{
				foreach (var prmName in parameters.AllKeys)
				{
					//one parameter/key can have multiple values
					List<string> prmValues = parameters.GetValues(prmName).ToList();
					prmValues.ForEach(x => request.AddParameter(prmName, x));
				}
			}

			RestResponse response = await _client.ExecuteAsync(request);
			if (!response.IsSuccessful)
			{
				_logger.Warning("Api returned unsuccessful response. Requested url - {url} ", request.Resource);
				return default(T);
			}

			return JsonConvert.DeserializeObject<T>(response.Content);
		}

		protected static string CreateToken()
		{
			Token token = new Token
			{
				UserId = Guid.Parse("7B7F41E1-11F5-43A7-AA2D-F95A0CC8CE93"),
				Username = "Aplikacija",
				DateCreated = DateTime.Now
			};

			string json = JsonConvert.SerializeObject(token);
			byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);
			string base64 = Convert.ToBase64String(byteArray);
			return $"{AuthenticationDefaults.Mixed} {base64}";
		}

		#endregion
	}
}
