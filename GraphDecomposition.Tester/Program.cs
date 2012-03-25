﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphDecomposition.Algorithms;
using GraphDecomposition.Utils;
using GraphDecomposition.GraphElements;

namespace GraphDecomposition.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Stinson aStinson = new Stinson();
            Skolem aSkolem = new Skolem();

            int numVertex = 7;

            SteinerTripleSystem sts = aSkolem.StartAlgorithm(numVertex);

            foreach (Triple t in sts)
            {
                Console.Write("|");
                Console.Write(t.X);
                Console.Write(t.Y);
                Console.Write(t.Z);
                Console.WriteLine("");
            }

            
        }
    }
}
