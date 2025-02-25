using Microsoft.AspNetCore.Mvc;
using TerminiService.PlayerService;
using TerminiService.PlayerService.Models;

namespace TerminiAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
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
		public async Task<ActionResult<GetPlayersResponse>> GetPlayersList()
		{
			GetPlayersRequest request = new GetPlayersRequest();

			GetPlayersResponse response = await _playerService.GetPlayersList(request);

			if (!response.Success)
				return BadRequest(response.Message);

			return Ok(response);
		}

		#endregion
	}
}
