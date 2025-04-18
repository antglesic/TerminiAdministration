using TerminiDomain.Core;
using TerminiWeb.Infrastructure.TerminService.Dtos;

namespace TerminiWeb.Infrastructure.TerminService.Models;

public class SetPlayerRatingsRequest : RequestBase
{
	public IEnumerable<TerminPlayerDto> TerminPlayers { get; set; } = new List<TerminPlayerDto>();
}

public class SetPlayerRatingsResponse : ResponseBase<SetPlayerRatingsRequest>
{
}
