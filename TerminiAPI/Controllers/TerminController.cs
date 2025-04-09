using Microsoft.AspNetCore.Mvc;
using TerminiService.TerminService;
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

		#endregion
	}
}
