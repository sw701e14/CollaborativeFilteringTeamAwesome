using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeFiltering
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = Parser.parseshit("_ignoreNewTraining/");

            var stream = new FileStream("Dict.bin", FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);

            var matrix = Parser.ToMatrix(a);
        }
    }
}
