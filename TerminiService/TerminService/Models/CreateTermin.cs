using TerminiDomain.Core;
using TerminiService.TerminService.Dtos;

namespace TerminiService.TerminService.Models
{
	public class CreateTerminRequest : RequestBase
	{
		public CreateTerminDto CreateTermin { get; set; } = new CreateTerminDto();
	}

	public class CreateTerminResponse : ResponseBase<CreateTerminRequest>
	{
		public TerminDto? Termin { get; set; } = new TerminDto();
	}
}
