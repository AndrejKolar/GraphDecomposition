using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphDecomposition.GraphElements;

namespace GraphDecomposition.Algorithms
{
    public class Stinson
    {
        /// <summary>
        /// Number of vertices in a graph
        /// </summary>
        int v;

        /// <summary>
        /// Number of triples in a STS
        /// </summary>
        int b;

        /// <summary>
        /// Array that keeps track of the third point in a block containing x and y 
        /// </summary>
        int[,] Other;

        public void StartAlgorithm(int v)
        {
            this.v = v;
            this.b = v * (v - 1) / 6;
        }

        private Triple[] ConstructBlocks()
        {
            Triple[] B = new Triple[b];

            int z;
            int index = 0;
            for (int x = 1; x <= v; x++)
            {
                for (int y = x + 1; y <= v; y++)
                {
                    z = Other[x, y];

                    if (z > y)
                    {
                        B[index] = new Triple(x, y, z);
                        index++;
                    }
                }
            }


            return B;
        }
    }
}
