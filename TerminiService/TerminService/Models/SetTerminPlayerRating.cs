using TerminiDomain.Core;
using TerminiService.TerminService.Dtos;

namespace TerminiService.TerminService.Models;

public class SetTerminPlayerRatingRequest : RequestBase
{
	public IEnumerable<SetTerminPlayerRatingDto> PlayerRatings { get; set; } = new List<SetTerminPlayerRatingDto>();
}

public class SetTerminPlayerRatingResponse : ResponseBase<SetTerminPlayerRatingRequest>
{
}