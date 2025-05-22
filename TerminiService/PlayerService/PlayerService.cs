using Microsoft.EntityFrameworkCore;
using Serilog;
using TerminiDataAccess.TerminiContext;
using TerminiDataAccess.TerminiContext.Models;
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
							string.IsNullOrEmpty(request.Name) || (p.Name != null && p.Name.Contains(request.Name))
						)
						&&
						(
							string.IsNullOrEmpty(request.Surname) || (p.Surname != null && p.Surname.Contains(request.Surname))
						)
						&&
						(
							string.IsNullOrEmpty(request.FullName) ||
							(
								(p.Name != null || p.Surname != null) &&
								EF.Functions.Like(
									(p.Name ?? "") + " " + (p.Surname ?? ""),
									"%" + request.FullName + "%"
								)
							)
						)
						&&
						(
							request.PlayerRating == null || (p.Rating == request.PlayerRating)
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
						Foot = p.Foot ?? string.Empty,
						Rating = p.Rating
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

		public async Task<CreatePlayerResponse> CreatePlayer(CreatePlayerRequest request)
		{
			CreatePlayerResponse response = new()
			{
				Request = request
			};

			try
			{
				if (request != null)
				{
					Player newPlayer = new()
					{
						Active = true,
						Name = request.Name,
						Surname = request.Surname,
						Foot = request.Foot,
						Sex = request.Sex
					};

					_terminiContext.Player.Add(newPlayer);
					await _terminiContext.SaveChangesAsync();

					if (newPlayer != null && newPlayer.Id > 0)
					{
						PlayerDto player = new()
						{
							Id = newPlayer.Id,
							Active = newPlayer.Active,
							DateCreated = newPlayer.DateCreated,
							Name = newPlayer.Name ?? string.Empty,
							Surname = newPlayer.Surname ?? string.Empty,
							Foot = newPlayer.Foot ?? string.Empty,
							Sex = newPlayer.Sex ?? string.Empty
						};

						response.Success = true;
						response.Player = player;
					}
				}
				else
				{
					response.Message = "No player was created!";
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("CreatePlayer", request.RequestToken, true)
					.Error(ex, ex.Message);
				response.Message = $"An error occurred while processing your request. Could not create a Player: {request}";
			}

			return response;
		}

		#endregion
	}
}
