using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = Config.GetInstance();
            a.Update("0.0.0.0", "PC", "12345");
            a.GetConfig();
            a.UpdateFromFile("./ConfigUpdate.txt");
            a.GetConfig();
            //a.WriteToFile("./Config.txt");
        }
    }
}
