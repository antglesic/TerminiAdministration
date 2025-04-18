using TerminiService.TerminService.Dtos;

namespace TerminiAPI.ViewModels;

public class SetPlayerRatingsViewModel
{
	public IEnumerable<SetTerminPlayerRatingDto> TerminPlayers { get; set; } = new List<SetTerminPlayerRatingDto>();
}
