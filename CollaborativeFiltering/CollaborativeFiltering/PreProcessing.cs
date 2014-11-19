using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeFiltering
{
    public class PreProcessing
    {
        public void SubtractMeans(double[,] input)
        {
            List<double> aur = new List<double>();

            double ar = AverageRating(input);

            for (int m = 0; m < input.GetLength(0); m++)
            {
                double amr = AverageMovieRating(input, m);

                for (int u = 0; u < input.GetLength(1); u++)
                {
                    if (m == 0)
                        aur.Add(AverageUserRating(input, u));
                    if (input[m, u] != 0)
                        input[m, u] = input[m, u] - amr - aur[u] + ar;
                }
            }
        }

        private double AverageRating(double[,] matrix)
        {
            double ratingsum = 0;
            foreach (var item in matrix)
            {
                ratingsum += item;
            }

            return 1 / N(matrix) * ratingsum;
        }

        private double AverageMovieRating(double[,] matrix, int m)
        {
            double ratingsum = 0;
            for (int s = 0; s < matrix.GetLength(1); s++)
            {
                ratingsum += matrix[m, s];
            }

            return 1 / NumberOfMovieRatings(matrix, m) * ratingsum;
        }

        private double AverageUserRating(double[,] matrix, int u)
        {
            double ratingsum = 0;
            for (int s = 0; s < matrix.GetLength(0); s++)
            {
                ratingsum += matrix[s, u];
            }

            return 1 / NumberOfUserRatings(matrix, u) * ratingsum;
        }

        /// <summary>
        /// Finds the total number of observed ratings for movie m
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        public int NumberOfMovieRatings(double[,] matrix, int m)
        {
            int Um = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[m, i] != 0)
                    Um += 1;
            }

            return Um;
        }

        /// <summary>
        /// Finds the total number of observed ratings for user 𝑢
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="u">The u.</param>
        /// <returns></returns>
        public int NumberOfUserRatings(double[,] matrix, int u)
        {
            int Mu = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[i, u] != 0)
                    Mu += 1;
            }

            return Mu;
        }

        /// <summary>
        /// Finds the total number of observed movie-user pairs.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        public int N(double[,] matrix)
        {
            int N = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                N += NumberOfMovieRatings(matrix, i);
            }

            return N;
        }
    }
}
