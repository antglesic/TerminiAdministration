using TerminiDomain.Core;
using TerminiWeb.Infrastructure.PlayerService.Dtos;

namespace TerminiWeb.Infrastructure.PlayerService.Models
{
	public class CreatePlayerRequest : RequestBase
	{
		public CreatePlayerDto CreatePlayer { get; set; } = new CreatePlayerDto();
	}

	public class CreatePlayerResponse : ResponseBase<CreatePlayerRequest>
	{
		public PlayerDto? Player { get; set; }
	}
}
