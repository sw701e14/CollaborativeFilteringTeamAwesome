using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeFiltering
{
    public static class Parser
    {

        public static double[,] parseshit(string dirPath)
        {
            string[] lines = File.ReadAllLines(dirPath);
            Dictionary<int, Tuple<int, int>> matrix = new Dictionary<int, Tuple<int, int>>();
            foreach (var line in lines)
            {
                int movieId = -1;
                if(line.Contains(':'))
                    movieId = line.Split(':')[0].ToInt();
                else
                {
                    string[] userInfo = line.Split(',');
                    matrix.Add(movieId, Tuple.Create<int,int>(userInfo[0].ToInt(), userInfo[1].ToInt()));
                }
            }
            throw new NotImplementedException  ();
        }
            public static int ToInt(this string str)
            {
                return Convert.ToInt32(str, 10);
            }            

    }
}
