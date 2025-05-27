namespace TerminiWeb.Infrastructure.PlayerService.Dtos
{
	public class PlayerDto
	{
		public int Id { get; set; }
		public bool Active { get; set; }
		public DateTime DateCreated { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Surname { get; set; } = string.Empty;
		public string Sex { get; set; } = string.Empty;
		public string Foot { get; set; } = string.Empty;
		public int? Rating { get; set; }
		public string FullName
		{
			get
			{
				return $"{Name} {Surname}";
			}
		}
		public int? TeamNumber { get; set; }
	}
}
