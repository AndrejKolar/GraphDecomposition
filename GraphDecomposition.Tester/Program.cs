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

            int numVertex = 7;

            if (VertexUtils.CanDecomposeGraph(numVertex))
            {
                aStinson.StartAlgorithm(numVertex);
            }

            
        }
    }
}
