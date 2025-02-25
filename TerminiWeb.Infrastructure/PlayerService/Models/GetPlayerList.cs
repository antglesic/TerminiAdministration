using TerminiDomain.Core;
using TerminiWeb.Infrastructure.PlayerService.Dtos;

namespace TerminiWeb.Infrastructure.PlayerService.Models
{
	public class GetPlayerListRequest : RequestBase
	{
	}

	public class GetPlayerListResponse : ResponseBase<GetPlayerListRequest>
	{
		public IEnumerable<PlayerDto> Players { get; set; }
	}
}
