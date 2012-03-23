using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphDecomposition.Algorithms;
using GraphDecomposition.Utils;

namespace GraphDecomposition.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Stinson aStinson = new Stinson();

            int numVertex = 9;

            if (GraphUtils.CanDecomposeGraph(numVertex))
            {
                aStinson.StartAlgorithm(numVertex);
            }

            
        }
    }
}
