using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Serilog;
using TerminiWeb.Infrastructure.Common.Client;
using TerminiWeb.Infrastructure.Common.Configuration;
using TerminiWeb.Infrastructure.PlayerService.Dtos;
using TerminiWeb.Infrastructure.PlayerService.Models;

namespace TerminiWeb.Infrastructure.PlayerService
{
	public class PlayerService : ExternalServiceBase, IPlayerService
	{
		#region Constants

		private string _controllerEndpoint = "Player";

		#endregion

		#region Fields

		private readonly ApiEndpointSettings _apiEndpointSettings;
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _jsonSerializerOptions;
		private readonly ILogger _logger = Log.ForContext<PlayerService>();

		#endregion

		#region Constructor

		public PlayerService(HttpClient httpClient, IOptions<ApiEndpointSettings> apiEndpointSettings) : base(apiEndpointSettings)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_apiEndpointSettings = apiEndpointSettings.Value;
			_jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		}

		#endregion

		#region Methods

		public async Task<GetPlayerListResponse> GetPlayerList(GetPlayerListRequest request)
		{
			GetPlayerListResponse response = new GetPlayerListResponse()
			{
				Request = request
			};

			if (request != null)
			{
				try
				{
					string Apiurl = $"{_apiEndpointSettings.TerminiApiBaseUrl}/{_controllerEndpoint}/GetPlayersList";
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(@"Bearer", ExternalServicesHelper.CreateToken());
					using (HttpResponseMessage responseContent = await _httpClient.GetAsync(Apiurl))
					{
						if (responseContent.IsSuccessStatusCode)
						{
							Stream responseStream = await responseContent.Content.ReadAsStreamAsync();

							if (responseStream != null && responseStream.Length > 0)
							{
								PlayerListResponseDto? playerList = await JsonSerializer.DeserializeAsync<PlayerListResponseDto>(responseStream, _jsonSerializerOptions);

								if (playerList != null && playerList.Players != null && playerList.Players.Any())
								{
									response.Players = playerList.Players;
									response.Success = true;
								}
								else
								{
									response.Message = "No players were found!";
								}
							}
						}
						else
						{
							response.Message = "Failed to get players list";
						}
					}
				}
				catch (Exception ex)
				{
					_logger
						.ForContext("GetPlayerList", request.RequestToken, true)
						.Error(ex, ex.Message);
					throw;
				}
			}
			else
			{
				response.Message = "Request is null";
			}

			return response;
		}

		#endregion
	}
}
