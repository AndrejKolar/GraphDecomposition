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
        private static int ITERATIONS_COUNT = 5;

        /// <summary>
        /// Path of the logfile for the generated incidence matrices
        /// </summary>
        private static string LOG_FILE_PATH = "test_log.txt";

        /// <summary>
        /// Main function of the console program
        /// </summary>
        /// <param name="args">Command line arguments not used</param>
        static void Main(string[] args)
        {
            StinsonExtended aStinsonExtended = new StinsonExtended();
            LogUtils.CreateLogFile(LOG_FILE_PATH);

            SteinerTripleSystem sts = aStinsonExtended.StartAlgorithm(NUM_VERTEX);            
            LogUtils.AppendIncidenceMatrix(sts);

            for (int i = 0; i < ITERATIONS_COUNT; i++)
            {
                sts = aStinsonExtended.NextDecomposition(sts);
                LogUtils.AppendIncidenceMatrix(sts);
            }
            
        }

        /// <summary>
        /// Writes the tripple to the console
        /// </summary>
        /// <param name="sts">Triple for console output</param>
        private static void writeDecomposition(SteinerTripleSystem sts)
        {
            foreach (Triple t in sts)
            {
                Console.Write("|");
                Console.Write(t.X + " ");
                Console.Write(t.Y + " ");
                Console.Write(t.Z);
                Console.WriteLine("|");
            }

            Console.WriteLine("------------------");
        }
    }
}
