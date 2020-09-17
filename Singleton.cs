using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton {

    class Config {
        private static Config _Instance;
        private string _IP, _Login, _Password;
        private Config() { }

        public static Config GetInstance() {
            if (_Instance == null) {
                _Instance = new Config();                
            }   
            return _Instance;
        }

        private static bool IsValidIP(List<String> IP) {
            try {
                return IP.Where(x => Int32.Parse(x) <= 255).ToList().Count == 4;
            } catch {
                return false;
            }
        }

        public void Update(string IP, string Login, string Password) {
            _IP = IsValidIP(IP.Split('.').ToList()) ? IP : throw new Exception("Wrong IP!.");
            _Login = Login;
            _Password = Password;
        }

        public void GetConfig() {
            Console.WriteLine($"IP = {_IP} \nLogin = {_Login} \nPassword = {_Password}");
        }

        public void UpdateFromFile(string path) {
            List<string> param = new List<string>();
            using (StreamReader Out = new StreamReader(path)) {                
                string line;
                while((line = Out.ReadLine()) != null) {
                    param.Add(line.Substring(line.IndexOf('=') + 2));
                }
            }
            if (param.Count == 3) {
                _IP = IsValidIP(param[0].Split('.').ToList()) 
                    ? param[0] 
                    : throw new FormatException("Wrong IP!");
                _Login = param[1];
                _Password = param[2];
            }
            else {
                throw new FormatException("Wrong params! Count of params must be 3 (IP, Login, Password)");
            }
        }

        public void WriteToFile(string path) {
            using (StreamWriter In = new StreamWriter(path)) {
                In.WriteLine($"IP = {_IP} \nLogin = {_Login} \nPassword = {_Password}");
            }
        }
    }
}
