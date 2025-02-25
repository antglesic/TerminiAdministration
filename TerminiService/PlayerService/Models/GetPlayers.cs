using TerminiDomain.Core;
using TerminiService.PlayerService.Dtos;

namespace TerminiService.PlayerService.Models
{
	public class GetPlayersRequest : RequestBase
	{
	}

	public class GetPlayersResponse : ResponseBase<GetPlayersRequest>
	{
		public IEnumerable<PlayerDto> Players { get; set; }
	}
}
