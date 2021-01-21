using System;
using System.IO;
using MainApp.Models;
using Newtonsoft.Json;

namespace MainApp.Services
{
    public static class SettingsService
    {
        static Settings settings;

        public static void LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Settings file does not exist");

            var text = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(text))
                throw new Exception("Settings file is empty");

            //get data settings from the JSON file
            settings = JsonConvert.DeserializeObject<Settings>(text);
        }

        public static void SaveSettings(string filePath)
        {
            if (!File.Exists(filePath))
            {
                //we use 'using' to close the file after it's created
                using (File.Create(filePath)) { }
            }

            //save data settings to the file
            string content = JsonConvert.SerializeObject(settings);
            File.WriteAllText(filePath, content);
        }

        public static string GetDbConnectionString()
        {
            return settings.DbConnectionString;
        }

        public static string GetTelegramOperatorBotToken()
        {
            return settings.TelegramBotToken;
        }
    }
}
