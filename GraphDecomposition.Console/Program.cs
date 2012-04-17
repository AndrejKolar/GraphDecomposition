using System;
using GraphDecomposition.Algorithms;
using GraphDecomposition.GraphElements;
using GraphDecomposition.Utils;

namespace GraphDecomposition.Tester
{
    class Program
    {
        /// <summary>
        /// Number of vertices in the graph
        /// </summary>
        private static int NUM_VERTEX = 13;

        /// <summary>
        /// Number of iterations for the StinsonExtended algorithm
        /// </summary>
        private static int ITERATIONS_COUNT = 500;

        /// <summary>
        /// Path of the logfile for the generated incidence matrices
        /// </summary>
        private static string LOG_FILE_PATH = "decomposition_log.txt";

        /// <summary>
        /// Main function of the console program
        /// </summary>
        /// <param name="args">Command line arguments not used</param>
        static void Main(string[] args)
        {
            StinsonExtended aStinsonExtended = new StinsonExtended();         

            SteinerTripleSystem sts = aStinsonExtended.StartAlgorithm(NUM_VERTEX);
            LogUtils.CreateLogFile(LOG_FILE_PATH, sts);

            for (int i = 1; i < ITERATIONS_COUNT; i++)
            {                
                sts = aStinsonExtended.NextDecomposition(sts);
                LogUtils.AppendIncidenceMatrix(sts);

                Console.WriteLine("Iteration {0} / {1}", i.ToString(), ITERATIONS_COUNT.ToString());
            }

            Console.WriteLine("Iteration {0} / {1}", ITERATIONS_COUNT.ToString(), ITERATIONS_COUNT.ToString());
            Console.WriteLine("Decomposition done.");
        }

    }
}
