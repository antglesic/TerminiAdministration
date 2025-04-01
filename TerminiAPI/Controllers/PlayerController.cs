using Microsoft.AspNetCore.Mvc;
using TerminiService.PlayerService;
using TerminiService.PlayerService.Models;

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
		public async Task<ActionResult<GetPlayersResponse>> GetPlayersList([FromQuery] string? name, [FromQuery] string? surname)
		{
			GetPlayersRequest request = new GetPlayersRequest();
			request.Name = name ?? string.Empty;
			request.Surname = surname ?? string.Empty;

			GetPlayersResponse response = await _playerService.GetPlayersList(request);

			if (!response.Success)
				return BadRequest(response.Message);

			return Ok(response);
		}

		#endregion
	}
}
