using Microsoft.AspNetCore.Mvc;
using TerminiAPI.ViewModels;
using TerminiService.TerminService;
using TerminiService.TerminService.Dtos;
using TerminiService.TerminService.Models;

namespace TerminiAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TerminController : ControllerBase
	{
		#region Fields

		private readonly ITerminService _terminService;

		#endregion

		#region Constructor

		public TerminController(ITerminService terminService)
		{
			_terminService = terminService;
		}

		#endregion

		#region Methods

		[HttpGet("GetTermins")]
		[ProducesResponseType(typeof(GetTerminsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<GetTerminsResponse>> GetTermins()
		{
			GetTerminsRequest request = new GetTerminsRequest();

			GetTerminsResponse response = await _terminService.GetTermins(request);

			if (!response.Success)
				return BadRequest(response.Message);

			return Ok(response);
		}

		[HttpGet("GetTermin/{Id:int}")]
		[ProducesResponseType(typeof(GetTerminByIdResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<GetTerminsResponse>> GetTerminById([FromRoute] int Id)
		{
			GetTerminByIdRequest request = new GetTerminByIdRequest();
			request.TerminId = Id;

			GetTerminByIdResponse response = await _terminService.GetTerminById(request);

			if (!response.Success)
				return BadRequest(response.Message);

			return Ok(response);
		}

		[HttpPost("CreateTermin")]
		[ProducesResponseType(typeof(CreateTerminResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<CreateTerminResponse>> CreateTermin([FromBody] CreateTerminViewModel createTerminViewModel)
		{
			if (createTerminViewModel != null)
			{
				CreateTerminDto createTermin = new CreateTerminDto()
				{
					ScheduleDate = createTerminViewModel.ScheduleDate,
					StartTime = createTerminViewModel.StartTime,
					DurationMinutes = createTerminViewModel.DurationMinutes,
					Players = createTerminViewModel.Players
				};

				CreateTerminRequest request = new CreateTerminRequest();
				request.CreateTermin = createTermin;

				CreateTerminResponse response = await _terminService.CreateTermin(request);

				if (!response.Success)
					return BadRequest(response.Message);

				return Ok(response);
			}
			else
			{
				return BadRequest("CreateTerminViewModel is null.");
			}
		}

		[HttpPost("SetPlayerRatings")]
		[ProducesResponseType(typeof(SetTerminPlayerRatingResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<SetTerminPlayerRatingResponse>> SetPlayerRatings([FromBody] SetPlayerRatingsViewModel setPlayerRatingsViewModel)
		{
			if (setPlayerRatingsViewModel != null)
			{
				SetTerminPlayerRatingRequest request = new SetTerminPlayerRatingRequest()
				{
					PlayerRatings = setPlayerRatingsViewModel.TerminPlayers
				};

				SetTerminPlayerRatingResponse response = await _terminService.SetTerminPlayerRating(request);

				if (!response.Success)
					return BadRequest(response.Message);

				return Ok(response);
			}
			else
			{
				return BadRequest("SetPlayerRatingsViewModel is null.");
			}
		}

		#endregion
	}
}
