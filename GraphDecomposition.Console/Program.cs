using System;
using System.Configuration;
using GraphDecomposition.Algorithms;
using GraphDecomposition.GraphElements;
using GraphDecomposition.Utils;

namespace GraphDecomposition.Tester
{
    class Program
    {
        /// <summary>
        /// Main function of the console program
        /// </summary>
        /// <param name="args">First command line argument is the number of vertices in the graph
        /// Second argument is the number of iterations</param>
        static void Main(string[] args)
        {
            string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];

            if (args.Length != 2)
            {
                Console.WriteLine("Program must be started with two parameters.");
                Console.WriteLine("First argument is the number of vertices of the complete graph.");
                Console.WriteLine("Second argument is the number of iterations.");
                return;
            }

            int numVertex = 0;
            bool isNumVertexOk = int.TryParse(args[0], out numVertex);

            int numIterations = 0;
            bool isNumIterationsOk = int.TryParse(args[1], out numIterations);

            bool parametarsOk = isNumIterationsOk && isNumVertexOk;            
            if (!parametarsOk)
            {
                Console.WriteLine("Error inputing parameters.");
                Console.WriteLine("First argument is the number of vertices of the complete graph.");
                Console.WriteLine("Second argument is the number of iterations.");
                return;
            }

            if (!GraphUtils.CanDecomposeGraph(numVertex))
            {
                Console.WriteLine("Cannot decompose a graph with this number of vertices: {0}", numVertex);
                return;
            }

            StinsonExtended aStinsonExtended = new StinsonExtended();

            SteinerTripleSystem sts = aStinsonExtended.StartAlgorithm(numVertex);
            LogUtils.CreateLogFile(LogFilePath, sts);

            for (int i = 1; i < numIterations; i++)
            {                
                sts = aStinsonExtended.NextDecomposition(sts);
                LogUtils.AppendIncidenceMatrix(sts);

                Console.WriteLine("Iteration {0} / {1}", i.ToString(), numIterations.ToString());
            }

            Console.WriteLine("Iteration {0} / {1}", numIterations.ToString(), numIterations.ToString());
            Console.WriteLine("Decomposition done.");
        }

    }
}
