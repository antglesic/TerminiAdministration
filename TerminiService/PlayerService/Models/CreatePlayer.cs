using TerminiDomain.Core;
using TerminiService.PlayerService.Dtos;

namespace TerminiService.PlayerService.Models
{
	public class CreatePlayerRequest : RequestBase
	{
		public string? Name { get; set; } = string.Empty;
		public string? Surname { get; set; } = string.Empty;
		public string? Foot { get; set; } = string.Empty;
		public string? Sex { get; set; } = string.Empty;
	}

	public class CreatePlayerResponse : ResponseBase<CreatePlayerRequest>
	{
		public PlayerDto? Player { get; set; } = new PlayerDto();
	}
}
