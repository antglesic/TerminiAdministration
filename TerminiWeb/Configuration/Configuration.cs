using Microsoft.Extensions.Primitives;
using TerminiWeb.Infrastructure.Common.Configuration;

namespace TerminiWeb.Configuration
{
	public class Configuration : IConfiguration
	{
		public ApiEndpointSettings ApiEndpointSettings { get; set; }
		public TerminiAppSettings TerminiAppSettings { get; set; }

		public string this[string key]
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public IEnumerable<IConfigurationSection> GetChildren()
		{
			throw new NotImplementedException();
		}

		public IConfigurationSection GetSection(string key)
		{
			throw new NotImplementedException();
		}

		public IChangeToken GetReloadToken()
		{
			throw new NotImplementedException();
		}
	}
}
