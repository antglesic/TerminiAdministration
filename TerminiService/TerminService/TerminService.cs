using System.Data.Entity;
using Serilog;
using TerminiDataAccess.TerminiContext;
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
					TerminDto termin = await _terminiContext.Termin
						.AsNoTracking()
						.Where(x => x.Active && x.Id == request.TerminId)
						.Select(x => new TerminDto()
						{
							Id = x.Id,
							Active = x.Active,
							DateCreated = x.DateCreated,
							ScheduledDate = x.ScheduledDate,
							StartTime = x.StartTime,
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
				IEnumerable<TerminDto> termins = await _terminiContext.Termin
					.AsNoTracking()
					.Where(x => x.Active)
					.Select(x => new TerminDto()
					{
						Id = x.Id,
						Active = x.Active,
						DateCreated = x.DateCreated,
						ScheduledDate = x.ScheduledDate,
						StartTime = x.StartTime,
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
					.ToListAsync();

				if (termins != null && termins.Any())
				{
					response.Termins = termins;
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

		#endregion
	}
}
