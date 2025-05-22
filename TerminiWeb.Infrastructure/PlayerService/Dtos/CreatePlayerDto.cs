namespace TerminiWeb.Infrastructure.PlayerService.Dtos
{
	public class CreatePlayerDto
	{
		public string Name { get; set; } = string.Empty;
		public string Surname { get; set; } = string.Empty;
		public string Sex { get; set; } = string.Empty;
		public string Foot { get; set; } = string.Empty;
	}
}
