namespace TerminiWeb.Infrastructure.TerminService.Dtos
{
	public class TerminListResponseDto
	{
		public IEnumerable<TerminDto>? Termins { get; set; } = new List<TerminDto>();
	}
}
