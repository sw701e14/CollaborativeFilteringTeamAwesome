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

        public static Dictionary<int, List<Tuple<int, int>>> parseshit(string dirPath)
        {
            Dictionary<int, List<Tuple<int, int>>> matrix = new Dictionary<int, List<Tuple<int, int>>>();
            List<Tuple<int, int>> userRatingsPerMovie = new List<Tuple<int, int>>();
            int count = 0;
            foreach (var file in Directory.GetFiles(dirPath))
            {
                count++;
                if (count > 2000)
                    return matrix;
                string[] lines = File.ReadAllLines(Path.Combine(file));
                foreach (var line in lines)
                {
                    int movieId = -1;
                    if (line.Contains(':'))
                    {
                        movieId = line.Split(':')[0].ToInt();
                        if (userRatingsPerMovie.Count != 0)
                        {
                            matrix.Add(movieId, new List<Tuple<int, int>>(userRatingsPerMovie));
                            userRatingsPerMovie.Clear();
                        }
                    }
                    else
                    {
                        string[] userInfo = line.Split(',');
                        int userId = userInfo[0].ToInt();
                        if(userInfo.Count() > 1)
                            userRatingsPerMovie.Add(Tuple.Create<int, int>(userId, userInfo[1].ToInt()));
                        else
                            userRatingsPerMovie.Add(Tuple.Create<int, int>(userId, 0));
                    }
                }
            }
            return matrix;
        }

        public static void ExtractNewTrainingData(string probePath, string trainingPath)
        {
            string[] probe = File.ReadAllLines(probePath);
            Dictionary<int, List<Tuple<int, int>>> probeMatrix = new Dictionary<int, List<Tuple<int, int>>>(parseshit("_ignoreProbe/"));
        }

        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str, 10);
        }

    }
}
