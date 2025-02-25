using TerminiService.PlayerService.Models;

namespace TerminiService.PlayerService
{
	public interface IPlayerService
	{
		Task<GetPlayersResponse> GetPlayersList(GetPlayersRequest request);
	}
}
