namespace TerminiDomain.Core
{
	[Serializable]
	public abstract class RequestBase : IRequest
	{
		public Guid RequestToken { get; init; } = Guid.NewGuid();
		public Guid AuthenticatedUserId { get; set; }
		public List<string> AuthenticatedUserRoles { get; set; } = new List<string>();
	}
}
