namespace TerminiWeb.Infrastructure.Common.Client.Security
{
	public class Token
	{
		public Guid UserId { get; set; }

		public string Username { get; set; }

		public DateTime DateCreated { get; set; }
	}
}
