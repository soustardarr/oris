using System;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;

namespace MyHttpSrver
{
	public class ConfigApp
	{
		public static AppSettings SettingTheConfig()
		{
			try
			{
				string appSettingsPath = "/Users/ruslan/Projects/MyHttpSrver/MyHttpSrver/appset.txt";
				string json = File.ReadAllText(appSettingsPath);
				var appSet = JsonConvert.DeserializeObject<AppSettings>(json);
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string pathFile = Path.Combine(directory, appSet.StaticFilesPath);
                Directory.CreateDirectory(pathFile);
				Console.WriteLine($"успешно создана директория со статическими файлами");
				return appSet;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
				return null;
                
            }
        }
	}
}

