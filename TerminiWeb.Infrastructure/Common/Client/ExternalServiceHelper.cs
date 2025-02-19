using System.Text;
using System.Text.Json;
using TerminiWeb.Infrastructure.Common.Client.Security;

namespace TerminiWeb.Infrastructure.Common.Client
{
	public static class ExternalServicesHelper
	{
		#region CreateToken

		public static string CreateToken()
		{
			Token token = new Token
			{
				UserId = Guid.Parse("7B7F41E1-11F5-43A7-AA2D-F95A0CC8CE93"),
				Username = "Aplikacija",
				DateCreated = DateTime.Now
			};

			string json = JsonSerializer.Serialize(token);
			byte[] byteArray = Encoding.UTF8.GetBytes(json);
			string base64 = Convert.ToBase64String(byteArray);
			return $"{AuthenticationDefaults.CeresAuthenticationScheme} {base64}";
		}

		#endregion
	}
}
