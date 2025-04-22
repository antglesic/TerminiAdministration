using Microsoft.EntityFrameworkCore;
using Serilog;
using TerminiDataAccess.TerminiContext;
using TerminiDataAccess.TerminiContext.Models;
using TerminiService.PlayerService.Dtos;
using TerminiService.TerminService.Dtos;
using TerminiService.TerminService.Models;

namespace TerminiService.TerminService
{
	public class TerminService : ITerminService
	{
		#region Fields

		private readonly TerminiContext _terminiContext;
		private readonly ILogger _logger = Log.ForContext<TerminService>();

		#endregion

		#region Constructor

		public TerminService(TerminiContext terminiContext)
		{
			_terminiContext = terminiContext;
		}

		#endregion

		#region Methods

		public async Task<GetTerminByIdResponse> GetTerminById(GetTerminByIdRequest request)
		{
			GetTerminByIdResponse response = new GetTerminByIdResponse
			{
				Request = request
			};

			try
			{
				if (request.TerminId > 0)
				{
					TerminDto? termin = await _terminiContext.Termin
						.AsNoTracking()
						.Where(x => x.Active && x.Id == request.TerminId)
						.Select(x => new TerminDto()
						{
							Id = x.Id,
							Active = x.Active,
							DateCreated = x.DateCreated,
							ScheduledDate = x.ScheduledDate,
							StartTime = x.StartTime,
							DurationMinutes = x.DurationMinutes,
							IsFinished = x.IsFinished,
							Players = x.TerminPlayers
								.Where(p => p.Active)
								.Select(p => new PlayerDto()
								{
									Id = p.Id,
									Active = p.Active,
									DateCreated = p.DateCreated,
									Name = p.Player.Name ?? string.Empty,
									Surname = p.Player.Surname ?? string.Empty,
									Sex = p.Player.Sex ?? string.Empty,
									Foot = p.Player.Foot ?? string.Empty,
									Rating = p.PlayerRating ?? p.Player.Rating
								})
						})
						.FirstOrDefaultAsync();

					if (termin != null)
					{
						response.Termin = termin;
						response.Success = true;
					}
					else
					{
						response.Message = $"Termin with Id {request.TerminId} not found.";
					}
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("GetTerminById", request.RequestToken, true)
					.ForContext("TerminId", request.TerminId, true)
					.Error(ex, ex.Message);
				response.Message = $"An error occurred while processing your request. Could not get a Termin with Id: {request.TerminId}";
			}

			return response;
		}

		public async Task<GetTerminsResponse> GetTermins(GetTerminsRequest request)
		{
			GetTerminsResponse response = new GetTerminsResponse
			{
				Request = request
			};

			try
			{
				// Fetch the data from the database first
				var termins = await _terminiContext.Termin
					.AsNoTracking()
					.Where(x => x.Active)
					.Include(x => x.TerminPlayers) // Include related data
					.ThenInclude(tp => tp.Player) // Include nested related data
					.ToListAsync();

				// Map the data to DTOs after materialization
				IEnumerable<TerminDto> terminsList = termins
					.Select(x => new TerminDto()
					{
						Id = x.Id,
						Active = x.Active,
						DateCreated = x.DateCreated,
						ScheduledDate = x.ScheduledDate,
						StartTime = x.StartTime,
						DurationMinutes = x.DurationMinutes,
						IsFinished = x.IsFinished,
						Players = x.TerminPlayers
							.Where(p => p.Active)
							.Select(p => new PlayerDto()
							{
								Id = p.Player.Id,
								Active = p.Player.Active,
								DateCreated = p.Player.DateCreated,
								Name = p.Player.Name ?? string.Empty,
								Surname = p.Player.Surname ?? string.Empty,
								Sex = p.Player.Sex ?? string.Empty,
								Foot = p.Player.Foot ?? string.Empty,
								Rating = p.PlayerRating ?? p.Player.Rating
							})
					});

				if (terminsList != null && terminsList.Any())
				{
					response.Termins = terminsList;
					response.Success = true;
				}
				else
				{
					response.Message = "No Termins found.";
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("GetTermins", request.RequestToken, true)
					.Error(ex, ex.Message);
				response.Message = $"An error occurred while processing your request. Could not get any Termin with the request: {request}";
			}

			return response;
		}

		public async Task<CreateTerminResponse> CreateTermin(CreateTerminRequest request)
		{
			CreateTerminResponse response = new CreateTerminResponse()
			{
				Request = request
			};

			try
			{
				if (request != null)
				{
					Termin newTermin = new Termin()
					{
						Active = true,
						ScheduledDate = request.CreateTermin.ScheduleDate,
						StartTime = request.CreateTermin.StartTime,
						DurationMinutes = request.CreateTermin.DurationMinutes
					};

					_terminiContext.Termin.Add(newTermin);
					await _terminiContext.SaveChangesAsync();

					if (newTermin.Id > 0 && request.CreateTermin.Players != null && request.CreateTermin.Players.Any())
					{
						List<TerminPlayers> listOfPlayers = new List<TerminPlayers>();

						foreach (PlayerDto player in request.CreateTermin.Players)
						{
							TerminPlayers newTerminPlayer = new TerminPlayers()
							{
								Active = true,
								TerminId = newTermin.Id,
								PlayerId = player.Id
							};

							listOfPlayers.Add(newTerminPlayer);
						}

						if (listOfPlayers.Any())
						{
							await _terminiContext.TerminPlayers.AddRangeAsync(listOfPlayers);
							await _terminiContext.SaveChangesAsync();

							if (listOfPlayers.All(p => p.Id > 0))
							{
								TerminDto? termin = await _terminiContext.Termin
									.AsNoTracking()
									.Where(x => x.Active && x.Id == newTermin.Id)
									.Select(x => new TerminDto()
									{
										Id = x.Id,
										Active = x.Active,
										DateCreated = x.DateCreated,
										ScheduledDate = x.ScheduledDate,
										StartTime = x.StartTime,
										DurationMinutes = x.DurationMinutes,
										Players = x.TerminPlayers
											.Where(p => p.Active)
											.Select(p => new PlayerDto()
											{
												Id = p.Id,
												Active = p.Active,
												DateCreated = p.DateCreated,
												Name = p.Player.Name ?? string.Empty,
												Surname = p.Player.Surname ?? string.Empty,
												Sex = p.Player.Sex ?? string.Empty,
												Foot = p.Player.Foot ?? string.Empty
											})
									})
									.FirstOrDefaultAsync();

								if (termin != null)
								{
									response.Success = true;
									response.Termin = termin;
								}
							}
						}
					}
					else
					{
						response.Message = "No termin with players were created!";
					}
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("CreateTermin", request.RequestToken, true)
					.Error(ex, ex.Message);
				response.Message = $"An error occurred while processing your request. Could not get create a Termin: {request}";
			}

			return response;
		}

		public async Task<SetTerminPlayerRatingResponse> SetTerminPlayerRating(SetTerminPlayerRatingRequest request)
		{
			SetTerminPlayerRatingResponse response = new SetTerminPlayerRatingResponse()
			{
				Request = request
			};

			try
			{
				if (request != null && request.PlayerRatings != null && request.PlayerRatings.Any() && !request.PlayerRatings.Any(x => x.Rating <= 0))
				{
					bool canFinish = false;

					foreach (SetTerminPlayerRatingDto terminPlayerRating in request.PlayerRatings)
					{
						TerminPlayers? terminPlayer = await _terminiContext.TerminPlayers
							.Where(x => x.Active
								&& x.PlayerId == terminPlayerRating.PlayerId
								&& x.TerminId == terminPlayerRating.TerminId)
							.FirstOrDefaultAsync();

						if (terminPlayer != null)
						{
							terminPlayer.PlayerRating = terminPlayerRating.Rating;
							_terminiContext.Update(terminPlayer);

							await _terminiContext.SaveChangesAsync();

							await UpdateOverallPlayerRating(terminPlayer.PlayerId);

							_terminiContext.Entry(terminPlayer).State = EntityState.Detached;
							canFinish = true;
						}
					}

					if (canFinish)
					{
						await UpdateTerminFinished(request.PlayerRatings?.FirstOrDefault()?.TerminId);
						await _terminiContext.SaveChangesAsync();
					}

					response.Success = true;
				}
				else
				{
					response.Message = "No player ratings were set!";
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("SetTerminPlayerRating", request.RequestToken, true)
					.Error(ex, ex.Message);
				response.Message = $"An error occurred while processing your request. Could not set a player rating: {request}";
			}

			return response;
		}

		#endregion

		#region  Private Methods

		private async Task UpdateOverallPlayerRating(int playerId)
		{
			if (playerId > 0)
			{
				try
				{
					Player? player = await _terminiContext.Player
						.Where(x => x.Active
							&& x.Id == playerId)
						.FirstOrDefaultAsync();

					if (player != null)
					{
						IEnumerable<TerminPlayers> terminPlayers = await _terminiContext.TerminPlayers
							.AsNoTracking()
							.Where(x => x.Active
								&& x.PlayerId == playerId
								&& x.PlayerRating.HasValue
								&& x.PlayerRating.Value > 0)
							.ToListAsync();

						if (terminPlayers != null && terminPlayers.Any())
						{
							double overallRating = terminPlayers.Average(x => x.PlayerRating ?? 0);
							player.Rating = (int)Math.Round(overallRating);

							_terminiContext.Update(player);
							await _terminiContext.SaveChangesAsync();

							_terminiContext.Entry(player).State = EntityState.Detached;
						}
					}
				}
				catch (Exception ex)
				{
					_logger
						.ForContext("Error updating player overral rating", ex.Message, true)
						.ForContext("UpdateOverallPlayerRating", playerId, true)
						.Error(ex, ex.Message);
				}
			}
		}

		private async Task UpdateTerminFinished(int? terminId)
		{
			if (terminId.HasValue && terminId.Value > 0)
			{
				try
				{
					Termin? termin = await _terminiContext.Termin
						.Where(x => x.Active
							&& x.Id == terminId)
						.FirstOrDefaultAsync();

					if (termin != null)
					{
						termin.IsFinished = true;
						_terminiContext.Update(termin);
						await _terminiContext.SaveChangesAsync();

						_terminiContext.Entry(termin).State = EntityState.Detached;
					}
				}
				catch (Exception ex)
				{
					_logger
						.ForContext("Error updating termin finished", ex.Message, true)
						.ForContext("UpdateTerminFinished", terminId, true)
						.Error(ex, ex.Message);
				}
			}
		}

		#endregion
	}
}
