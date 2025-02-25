using Microsoft.EntityFrameworkCore;
using Serilog;
using TerminiDataAccess.TerminiContext;
using TerminiService.PlayerService.Dtos;
using TerminiService.PlayerService.Models;

namespace TerminiService.PlayerService
{
	public class PlayerService : IPlayerService
	{
		#region Fields

		private readonly TerminiContext _terminiContext;
		private readonly ILogger _logger = Log.ForContext<PlayerService>();

		#endregion

		#region Constructor

		public PlayerService(TerminiContext terminiContext)
		{
			_terminiContext = terminiContext;
		}

		#endregion

		#region Methods

		public async Task<GetPlayersResponse> GetPlayersList(GetPlayersRequest request)
		{
			GetPlayersResponse response = new GetPlayersResponse()
			{
				Request = request
			};

			try
			{
				IEnumerable<PlayerDto> players = await _terminiContext.Player
					.AsNoTracking()
					.Where(p => p.Active)
					.Select(p => new PlayerDto
					{
						Id = p.Id,
						Active = p.Active,
						DateCreated = p.DateCreated
					})
					.TagWith("PlayerService.GetPlayersList")
					.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger
						.ForContext("GetPlayersList", request.RequestToken, true)
						.Error(ex, ex.Message);
				throw;
			}

			return response;
		}

		#endregion
	}
}
