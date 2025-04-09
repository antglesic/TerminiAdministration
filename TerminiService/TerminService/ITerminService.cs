using TerminiService.TerminService.Models;

namespace TerminiService.TerminService
{
	public interface ITerminService
	{
		Task<GetTerminByIdResponse> GetTerminById(GetTerminByIdRequest request);
		Task<GetTerminsResponse> GetTermins(GetTerminsRequest request);
	}
}
