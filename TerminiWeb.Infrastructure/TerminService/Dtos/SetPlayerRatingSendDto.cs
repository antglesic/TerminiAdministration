using TerminiWeb.Infrastructure.PlayerService.Dtos;

namespace TerminiWeb.Infrastructure.TerminService.Dtos;

public class SetPlayerRatingSendDto
{
	public IEnumerable<TerminPlayerDto> TerminPlayers { get; set; } = new List<TerminPlayerDto>();

	public SetPlayerRatingSendDto()
	{
		TerminPlayers = new List<TerminPlayerDto>();
	}

	public SetPlayerRatingSendDto(IEnumerable<TerminPlayerDto> terminPlayers)
	{
		TerminPlayers = terminPlayers;
	}
}
