using TerminiWeb.Infrastructure.PlayerService.Models;

namespace TerminiWeb.Infrastructure.PlayerService
{
	public interface IPlayerService
	{
		Task<GetPlayerListResponse> GetPlayerList(GetPlayerListRequest request);
	}
}
