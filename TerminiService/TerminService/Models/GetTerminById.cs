using TerminiDomain.Core;
using TerminiService.TerminService.Dtos;

namespace TerminiService.TerminService.Models
{
	public class GetTerminByIdRequest : RequestBase
	{
		public int TerminId { get; set; }
	}

	public class GetTerminByIdResponse : ResponseBase<GetTerminByIdRequest>
	{
		public TerminDto Termin { get; set; } = new TerminDto();
	}
}
