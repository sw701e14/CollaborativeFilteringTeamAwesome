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

        public static Dictionary<int, List<User>> parseshit(string dirPath)
        {
            Dictionary<int, List<User>> matrix = new Dictionary<int, List<User>>();
            List<User> userRatingsPerMovie = new List<User>();
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
                            userRatingsPerMovie.Sort((u1, u2) => u1.UserId.CompareTo(u2.UserId));
                            matrix.Add(movieId, new List<User>(userRatingsPerMovie));
                            userRatingsPerMovie.Clear();
                        }
                    }
                    else
                    {
                        string[] userInfo = line.Split(',');
                        int userId = userInfo[0].ToInt();
                        if (userInfo.Count() > 1)
                            userRatingsPerMovie.Add(new User(userId, userInfo[1].ToInt()));
                        else
                            userRatingsPerMovie.Add(new User(userId, 0));
                    }
                }
            }
            return matrix;
        }

        public static IEnumerable<User> parseGetUsers(string filePath)
        {
            string[] lines = File.ReadAllLines(Path.Combine(filePath));
            foreach (var line in lines)
            {
                if(!line.Contains(':'))
                {
                    string[] userInfo = line.Split(',');
                    yield return new User( userInfo[0].ToInt(), userInfo[1].ToInt());
                }
            }
        }

        public static void ExtractNewTrainingData(string probePath, string trainingPath)
        {
            string[] probe = File.ReadAllLines(probePath);
            StreamWriter sw = new StreamWriter("output.txt");

            Dictionary<int, List<User>> matrix = parseshit(probePath);
            foreach (var movieId in matrix)
            {
                List<User> users = parseGetUsers(Path.Combine(trainingPath, "mv_" + movieId.Key.ToString().PadLeft(7, '0') + ".txt")).ToList();
                users.Sort((u1, u2) => u1.UserId.CompareTo(u2.UserId));
                sw.WriteLine(movieId + ":");
                foreach (var probeUser in movieId.Value)
                {
                    foreach (var user in users)
                    {
                        if (probeUser.UserId == user.UserId)
                        {
                            sw.WriteLine(user.UserId + ", " + user.Rating);
                        }
                    }
                }
            }
        }

        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str, 10);
        }

    }
}
