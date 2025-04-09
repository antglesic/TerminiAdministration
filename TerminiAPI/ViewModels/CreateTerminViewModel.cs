using TerminiService.PlayerService.Dtos;

namespace TerminiAPI.ViewModels
{
	public class CreateTerminViewModel
	{
		public DateOnly ScheduleDate { get; set; }
		public TimeOnly StartTime { get; set; }
		public int DurationMinutes { get; set; }
		public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
	}
}
