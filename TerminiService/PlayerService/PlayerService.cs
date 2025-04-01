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
					.Where(p => p.Active
						&&
						(
							string.IsNullOrEmpty(request.Name) || p.Name.Contains(request.Name)
						)
						&&
						(
							string.IsNullOrEmpty(request.Surname) || p.Surname.Contains(request.Surname)
						)
					)
					.Select(p => new PlayerDto
					{
						Id = p.Id,
						Active = p.Active,
						DateCreated = p.DateCreated,
						Name = p.Name ?? string.Empty,
						Surname = p.Surname ?? string.Empty,
						Sex = p.Sex ?? string.Empty,
						Foot = p.Foot ?? string.Empty
					})
					.TagWith("PlayerService.GetPlayersList")
					.ToListAsync();

				if (players != null && players.Any())
				{
					response.Players = players;
					response.Success = true;
				}
				else
				{
					response.Message = "No players found.";
				}
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
