using Microsoft.Extensions.Options;
using Serilog;
using System.Net.Http.Headers;
using System.Text.Json;
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

					List<string> queryParameters = new List<string>();

					if (!string.IsNullOrEmpty(request.Name))
						queryParameters.Add($"name={Uri.EscapeDataString(request.Name)}");

					if (!string.IsNullOrEmpty(request.Surname))
						queryParameters.Add($"surname={Uri.EscapeDataString(request.Surname)}");

					if (!string.IsNullOrEmpty(request.FullName))
						queryParameters.Add($"fullName={Uri.EscapeDataString(request.FullName)}");

					if (request.PlayerRating != null)
						queryParameters.Add($"playerRating={request.PlayerRating}");

					if (queryParameters.Any())
						Apiurl += "?" + string.Join("&", queryParameters);

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

		public async Task<CreatePlayerResponse> CreatePlayer(CreatePlayerRequest request)
		{
			CreatePlayerResponse response = new CreatePlayerResponse()
			{
				Request = request
			};

			if (request != null)
			{
				try
				{
					string apiUrl = $"{_apiEndpointSettings.TerminiApiBaseUrl}/{_controllerEndpoint}/CreatePlayer";
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExternalServicesHelper.CreateToken());

					var content = JsonSerializer.Serialize(request.CreatePlayer, _jsonSerializerOptions);
					var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

					using (HttpResponseMessage responseContent = await _httpClient.PostAsync(apiUrl, bodyContent))
					{
						if (responseContent.IsSuccessStatusCode)
						{
							Stream responseStream = await responseContent.Content.ReadAsStreamAsync();

							if (responseStream != null && responseStream.Length > 0)
							{
								PlayerDto? createdPlayer = await JsonSerializer.DeserializeAsync<PlayerDto>(responseStream, _jsonSerializerOptions);

								if (createdPlayer != null)
								{
									response.Player = createdPlayer;
									response.Success = true;
								}
								else
								{
									response.Message = "Failed to deserialize the created player";
								}
							}
						}
						else
						{
							response.Message = $"Failed to create a player. Status: {responseContent.StatusCode}";
						}
					}
				}
				catch (Exception ex)
				{
					_logger
						.ForContext("CreatePlayer", request.RequestToken, true)
						.Error(ex, ex.Message);
					throw;
				}
			}

			return response;
		}

		#endregion
	}
}
