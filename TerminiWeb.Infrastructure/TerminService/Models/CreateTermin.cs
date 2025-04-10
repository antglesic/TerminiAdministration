using TerminiDomain.Core;
using TerminiWeb.Infrastructure.TerminService.Dtos;

namespace TerminiWeb.Infrastructure.TerminService.Models
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
