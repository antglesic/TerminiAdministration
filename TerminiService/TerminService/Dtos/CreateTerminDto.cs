using TerminiService.PlayerService.Dtos;

namespace TerminiService.TerminService.Dtos
{
	public class CreateTerminDto
	{
		public DateOnly ScheduleDate { get; set; }
		public TimeOnly StartTime { get; set; }
		public int DurationMinutes { get; set; }
		public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
	}
}
