using System;
using System.Collections.Generic;
using System.IO;

namespace Yutaka.DataSettingsManager
{
	public class DataSettingsManager
	{
		protected char separator = ':';
		protected string filename = "settings.txt";
		protected string defaultFolder = @"C:\TEMP\";

		/// <summary>
		/// Parse settings
		/// </summary>
		/// <param name="text">Text of settings file</param>
		/// <returns>Parsed data settings</returns>
		protected DataSettings ParseSettings(string text)
		{
			var shellSettings = new DataSettings();
			if (String.IsNullOrEmpty(text))
				return shellSettings;

			//Old way of file reading. This leads to unexpected behavior when a user's FTP program transfers these files as ASCII (\r\n becomes \n).
			//var settings = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var settings = new List<string>();
			using (var reader = new StringReader(text)) {
				string str;
				while ((str = reader.ReadLine()) != null)
					settings.Add(str);
			}

			for (int i=0; i< settings.Count; i++) {
				var separatorIndex = settings[i].IndexOf(separator);

				if (separatorIndex == -1)
					continue; // skip this line //

				var key = settings[i].Substring(0, separatorIndex).Trim();
				var value = settings[i].Substring(separatorIndex + 1).Trim();

				switch (key) {
					case "Password":
						shellSettings.Password = value;
						break;
					case "EncodedPassword":
						shellSettings.EncodedPassword = value;
						break;
					default:
						shellSettings.RawDataSettings.Add(key, value);
						break;
				}
			}

			return shellSettings;
		}

		/// <summary>
		/// Convert data settings to string representation
		/// </summary>
		/// <param name="settings">Settings</param>
		/// <returns>Text</returns>
		protected string ComposeSettings(DataSettings settings)
		{
			if (settings == null)
				return "";

			return string.Format("Password: {0}{2}EncodedPassword: {1}{2}",
								 settings.Password,
								 settings.EncodedPassword,
								 Environment.NewLine);
		}

		/// <summary>
		/// Load settings
		/// </summary>
		/// <param name="filePath">File path; pass null to use default settings file path</param>
		/// <returns></returns>
		public DataSettings LoadSettings(string filePath = null)
		{
			if (String.IsNullOrEmpty(filePath))
				filePath = Path.Combine(defaultFolder, filename);
			
			if (File.Exists(filePath))
				return ParseSettings(File.ReadAllText(filePath));

			return new DataSettings();
		}

		/// <summary>
		/// Save settings to a file
		/// </summary>
		/// <param name="settings"></param>
		//public void SaveSettings(DataSettings settings)
		//{
		//	if (settings == null)
		//		throw new ArgumentNullException("settings");

		//	string filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), filename);
		//	if (!File.Exists(filePath)) {
		//		using (File.Create(filePath)) {
		//			//we use 'using' to close the file after it's created
		//		}
		//	}

		//	var text = ComposeSettings(settings);
		//	File.WriteAllText(filePath, text);
		//}
	}
}