using System;
using GraphDecomposition.Algorithms;
using GraphDecomposition.GraphElements;

namespace GraphDecomposition.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Stinson aStinson = new Stinson();
            Skolem aSkolem = new Skolem();
            Bose aBose = new Bose();
            StinsonContinous aStinsonContinous = new StinsonContinous();

            int numVertex = 31;

            SteinerTripleSystem sts = aStinsonContinous.StartAlgorithm(numVertex);

            writeDecomposition(sts);

            for (int i = 0; i < 5; i++)
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
