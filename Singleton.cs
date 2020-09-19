using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Singleton {

    class Config {
        private static Config _Instance;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "login")]
        public string _Login { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ip")]
        public string _IP { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "passwd")]
        public string _Password { get; private set; }

        private List<string> Properties = new List<string> { "login", "ip", "passwd" };

        private Config() { }

        public static Config GetInstance() {
            if (_Instance == null) {
                _Instance = new Config();
            }
            return _Instance;
        }

        private bool IsValidIP(string IP) {
            try {
                return IP
                    .Split('.')
                    .ToList()
                    .Where(x => Int32.Parse(x) >= 0)
                    .Count() == 4;
            }
            catch {
                return false;
            }
        }

        public void Update(string IP, string Login, string Password) {
            _IP = IsValidIP(IP) ? IP : throw new Exception("Не корректный IP!.");
            _Login = Login;
            _Password = Password;
        }

        public void GetConfig() {
            if (_IP == null || _Password == null || _Login == null)
                Console.WriteLine("Конфиг пустой.");
            else
                Console.WriteLine($"IP = {_IP} \nLogin = {_Login} \nPassword = {_Password}");
        }

        public void UpdateFromFile(string path) {
            JObject obj = JObject.Parse(File.ReadAllText(path));
            try {
                obj.Properties().ToList().ForEach(item => {
                    if (Properties.IndexOf(item.Name) == -1)
                        throw new FormatException($"Не удалось найти свойство {item.Name}. Данные не были обновлены");
                });

                _IP = IsValidIP(obj.GetValue("ip").ToString())
                    ? obj.GetValue("ip").ToString()
                    : throw new ArgumentException("Не корректный IP!");

                _Login = obj.GetValue("login").ToString();
                _Password = obj.GetValue("passwd").ToString();
            }
            catch(FormatException ex) {
                Console.WriteLine(ex.Message);
            }
            catch(ArgumentException ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteToFile(string path) {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
