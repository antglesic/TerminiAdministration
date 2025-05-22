using Microsoft.AspNetCore.Mvc;
using TerminiAPI.ViewModels;
using TerminiService.PlayerService;
using TerminiService.PlayerService.Dtos;
using TerminiService.PlayerService.Models;
using TerminiService.TerminService.Models;

namespace TerminiAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
		#region Fields

		private readonly IPlayerService _playerService;

		#endregion

		#region Constructor

		public PlayerController(IPlayerService playerService)
		{
			_playerService = playerService;
		}

		#endregion

		#region Methods

		[HttpGet("GetPlayersList")]
		[ProducesResponseType(typeof(GetPlayersResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<GetPlayersResponse>> GetPlayersList([FromQuery] string? name, [FromQuery] string? surname, [FromQuery] string? fullName)
		{
			GetPlayersRequest request = new GetPlayersRequest();
			request.Name = name ?? string.Empty;
			request.Surname = surname ?? string.Empty;
			request.FullName = fullName ?? string.Empty;

			GetPlayersResponse response = await _playerService.GetPlayersList(request);

			if (!response.Success)
				return BadRequest(response.Message);

			return Ok(response);
		}

		[HttpPost("CreatePlayer")]
		[ProducesResponseType(typeof(CreatePlayerResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<CreateTerminResponse>> CreateTermin([FromBody] CreatePlayerTerminViewModel? createPlayerViewModel)
		{
			if (createPlayerViewModel != null)
			{
				CreatePlayerDto createPlayer = new CreatePlayerDto()
				{
					Name = createPlayerViewModel.Name,
					Surname = createPlayerViewModel.Surname,
					Foot = createPlayerViewModel.Foot,
					Sex = createPlayerViewModel.Sex
				};

				CreatePlayerRequest request = new();
				request.Name = createPlayerViewModel.Name;
				request.Surname = createPlayerViewModel.Surname;
				request.Foot = createPlayerViewModel.Foot;
				request.Sex = createPlayerViewModel.Sex;

				CreatePlayerResponse response = await _playerService.CreatePlayer(request);

				if (!response.Success)
					return BadRequest(response.Message);

				return Ok(response);
			}
			else
			{
				return BadRequest("CreatePlayerTerminViewModel is null.");
			}
		}

		#endregion
	}
}
