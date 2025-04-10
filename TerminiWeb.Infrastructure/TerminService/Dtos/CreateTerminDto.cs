using TerminiWeb.Infrastructure.PlayerService.Dtos;

namespace TerminiWeb.Infrastructure.TerminService.Dtos
{
	public class CreateTerminDto
	{
		public DateOnly ScheduleDate { get; set; }
		public TimeOnly StartTime { get; set; }
		public int DurationMinutes { get; set; }
		public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
	}
}
