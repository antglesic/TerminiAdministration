namespace TerminiDomain.Core
{
	public interface IResponse
	{
		Guid ResponseToken { get; }

		bool Success { get; set; }

		string Message { get; set; }

		List<ResponseStatus> Statuses { get; set; }
	}
}
