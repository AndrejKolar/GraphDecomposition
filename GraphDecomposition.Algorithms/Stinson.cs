using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void StartAlgorithm(int v)
        {
            this.v = v;
            this.b = v * (v - 1) / 6;
        }

        private void ConstructBlocks()
        {
            throw new NotImplementedException();
        }
    }
}
