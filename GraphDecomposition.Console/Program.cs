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
        private static int NUM_VERTEX = 7;

        /// <summary>
        /// Number of iterations for the StinsonExtended algorithm
        /// </summary>
        private static int ITERATIONS_COUNT = 5;

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
            }

            writeDecompositionInfo(sts);
            
        }

        /// <summary>
        /// Writes info about the decomposition to the console
        /// </summary>
        /// <param name="sts">STS(v)</param>
        private static void writeDecompositionInfo(SteinerTripleSystem sts)
        {
            Console.WriteLine("v: {0} b: {1}", NUM_VERTEX, sts.NumTriples());
            Console.WriteLine("Decomposition finished");

            if (ITERATIONS_COUNT != 0)
            {
                Console.WriteLine("Number of iterations: {0}", ITERATIONS_COUNT);
                Console.WriteLine("Log file locaton: " + LOG_FILE_PATH); 
            } 

        }

        /// <summary>
        /// Writes the decomposition result to the console
        /// </summary>
        /// <param name="sts">STS(v)</param>
        private static void writeDecompositionResult(SteinerTripleSystem sts)
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
