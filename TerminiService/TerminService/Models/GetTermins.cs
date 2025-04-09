using TerminiDomain.Core;
using TerminiService.TerminService.Dtos;

namespace TerminiService.TerminService.Models
{
	public class GetTerminsRequest : RequestBase
	{
	}

	public class GetTerminsResponse : ResponseBase<GetTerminsRequest>
	{
		public IEnumerable<TerminDto> Termins { get; set; } = new List<TerminDto>();
	}
}
