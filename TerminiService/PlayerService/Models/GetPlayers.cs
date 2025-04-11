using TerminiDomain.Core;
using TerminiService.PlayerService.Dtos;

namespace TerminiService.PlayerService.Models
{
	public class GetPlayersRequest : RequestBase
	{
		public string Name { get; set; } = string.Empty;
		public string Surname { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public int? PlayerRating { get; set; }
	}

	public class GetPlayersResponse : ResponseBase<GetPlayersRequest>
	{
		public IEnumerable<PlayerDto>? Players { get; set; }
	}
}
