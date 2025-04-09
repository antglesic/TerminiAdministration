namespace TerminiWeb.Infrastructure.PlayerService.Dtos
{
	public class PlayerListResponseDto
	{
		public IEnumerable<PlayerDto>? Players { get; set; } = new List<PlayerDto>();
	}
}
