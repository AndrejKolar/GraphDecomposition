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

            int numVertex = 9;

            SteinerTripleSystem sts = aBose.StartAlgorithm(numVertex);

            foreach (Triple t in sts)
            {
                Console.Write("|");
                Console.Write(t.X);
                Console.Write(t.Y);
                Console.Write(t.Z);
                Console.WriteLine("|");
            }

            
        }
    }
}
