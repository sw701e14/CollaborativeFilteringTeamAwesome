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
            int movieId = -1;
            foreach (var file in Directory.GetFiles(dirPath).Take(13000))
            {
                Console.WriteLine(movieId);
                string[] lines = File.ReadAllLines(Path.Combine(file));
                
                foreach (var line in lines)
                {
                    if (line.Contains(':'))
                    {
                        if (userRatingsPerMovie.Count != 0)
                        {
                            userRatingsPerMovie.Sort((u1, u2) => u1.UserId.CompareTo(u2.UserId));
                            matrix.Add(movieId, new List<User>(userRatingsPerMovie));
                            userRatingsPerMovie.Clear();
                        }
                        movieId = line.Split(':')[0].ToInt();
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

        public static double[,] ToMatrix(Dictionary<int, List<User>> movies)
        {
            List<User> users = new List<User>();
            foreach (var movie in movies)
            {
                Console.WriteLine("movie Id: " + movie.Key);
                foreach (var user in movie.Value)
                {
                    if (users.Where(u => u.UserId == user.UserId).Count() == 0)
                        users.Add(user);
                }
            }

            double[,] matrix = new double[movies.Keys.Count, users.Max(u =>u.UserId)];

            for (int i = 0; i < movies.Keys.Count; i++)
            {
                Console.WriteLine("movies ID: " + i);
                for (int j = 0; j < movies[i].Count; j++)
                    matrix[i, j] = movies[i][j].Rating;
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
            string[] probe = File.ReadAllLines(probePath + "probe.txt");
            StreamWriter sw = new StreamWriter("output.txt");

            Dictionary<int, List<User>> matrix = parseshit(probePath);
            int count = 0, temp = 0;
            foreach (var movieId in matrix)
            {
                count++;
                if (count > temp + 250)
                {
                    temp = count;
                    Console.WriteLine(count);
                }
                string filePath = Path.Combine(trainingPath, "mv_" + movieId.Key.ToString().PadLeft(7, '0') + ".txt");
                List<User> users = parseGetUsers(filePath).ToList();
                users.Sort((u1, u2) => u1.UserId.CompareTo(u2.UserId));
                sw.WriteLine(movieId.Key + ":");
                foreach (var probeUser in movieId.Value)
                {
                    User user = users[users.BinarySearch(probeUser, (u1,u2) => u1.UserId.CompareTo(u2.UserId))];
                    sw.WriteLine(user.UserId + ", " + user.Rating);
                    users.Remove(user);
                }
                saveUsers(users, filePath, movieId.Key.ToString()); 
            }
            sw.Close();
        }

        private static void saveUsers(List<User> users, string filePath, string movieId)
        {
            StreamWriter sw = new StreamWriter(filePath+"test,txt");
            sw.WriteLine(movieId + ":");
            foreach (var user in users)
                sw.WriteLine(user.UserId + ", " + user.Rating);
            sw.Close();
        }

        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str.Trim(), 10);
        }

    }
}
