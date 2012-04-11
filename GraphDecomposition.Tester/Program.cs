using System;
using GraphDecomposition.Algorithms;
using GraphDecomposition.GraphElements;

namespace GraphDecomposition.Tester
{
    class Program
    {
        private static int NUM_VERTEX = 13;
        private static int ITERATIONS_COUNT = 5;

        static void Main(string[] args)
        {
            Stinson aStinson = new Stinson();
            Skolem aSkolem = new Skolem();
            Bose aBose = new Bose();
            StinsonExtended aStinsonContinous = new StinsonExtended();

            SteinerTripleSystem sts = aStinsonContinous.StartAlgorithm(NUM_VERTEX);

            writeDecomposition(sts);

            for (int i = 0; i < ITERATIONS_COUNT; i++)
            {
                sts = aStinsonContinous.NextDecomposition(sts);

                writeDecomposition(sts);
            }
            
        }

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
