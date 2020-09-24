using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Singleton
{
    class Config
    {
        private static Config _Instance;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "login")]
        public string Login { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "passwd")]
        public string Password { get; private set; }

        private string _IP;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ip")]
        public string IP
        {
            get { return _IP; }
            private set
            {
                if (IsValidIP(value))
                    _IP = value;
                else
                    throw new FormatException("Не корректный IP! Данные не были установлены.");
            }
        }

        private readonly List<string> Properties = new List<string> { "login", "ip", "passwd" };

        private Config() { }

        public static Config GetInstance()
        {
            if (_Instance == null)
                _Instance = new Config();

            return _Instance;
        }

        private bool IsValidIP(string IP)
        {
            try
            {
                return IP
                    .Split('.')
                    .ToList()
                    .Where(x => Int32.Parse(x) >= 0 && Int32.Parse(x) <= 255)
                    .Count() == 4;
            }
            catch { return false; }
        }

        public void Update(string IP, string Login, string Password)
        {
            this.IP = IP;
            this.Login = Login;
            this.Password = Password;
        }

        public void GetConfig()
        {
            if (IP == null || Password == null || Login == null)
                Console.WriteLine("Конфиг пустой.");
            else
                Console.WriteLine($"IP = {IP} \nLogin = {Login} \nPassword = {Password}");
        }

        public void UpdateFromFile(string path)
        {
            JObject obj = JObject.Parse(File.ReadAllText(path));

            try
            {
                var propertiesList = obj.Properties().ToList();

                if (propertiesList.Count != 3)
                    throw new FormatException($"Число свойств не валидно!");

                propertiesList.ForEach(item =>
                {
                    if (!Properties.Contains(item.Name))
                        throw new FormatException("Обнаружено не валидное свойство!");
                });

                IP = obj.GetValue("ip").ToString();
                Login = obj.GetValue("login").ToString();
                Password = obj.GetValue("passwd").ToString();
            }
            catch (FormatException ex) { Console.WriteLine(ex.Message); }
        }

        public void WriteToFile(string path)
        { File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented)); }
    }
}
