using TerminiDomain.Core;
using TerminiWeb.Infrastructure.TerminService.Dtos;

namespace TerminiWeb.Infrastructure.TerminService.Models
{
	public class GetTerminsRequest : RequestBase
	{
	}

	public class GetTerminsResponse : ResponseBase<GetTerminsRequest>
	{
		public IEnumerable<TerminDto> Termins { get; set; } = new List<TerminDto>();
	}
}
