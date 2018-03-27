using System.Collections.Generic;

namespace Yutaka.DataSettingsManager
{
	public class DataSettings
	{
		public DataSettings()
		{
			RawDataSettings = new Dictionary<string, string>();
		}

		public string Password { get; set; }

		public IDictionary<string, string> RawDataSettings { get; private set; }
	}
}