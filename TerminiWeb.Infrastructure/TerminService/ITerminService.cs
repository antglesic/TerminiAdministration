using TerminiWeb.Infrastructure.TerminService.Models;

namespace TerminiWeb.Infrastructure.TerminService
{
	public interface ITerminService
	{
		Task<GetTerminsResponse> GetTermins(GetTerminsRequest request);
		Task<CreateTerminResponse> CreateTermin(CreateTerminRequest request);
	}
}
