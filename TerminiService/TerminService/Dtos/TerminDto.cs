using TerminiService.PlayerService.Dtos;

namespace TerminiService.TerminService.Dtos
{
	public class TerminDto
	{
		public int Id { get; set; }
		public bool Active { get; set; }
		public DateTime DateCreated { get; set; }
		public DateOnly ScheduledDate { get; set; }
		public TimeOnly StartTime { get; set; }
		public int DurationMinutes { get; set; }
		public IEnumerable<PlayerDto>? Players { get; set; } = new List<PlayerDto>();
	}
}
