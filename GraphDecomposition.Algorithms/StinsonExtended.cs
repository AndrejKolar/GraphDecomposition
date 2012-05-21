using System;
using GraphDecomposition.Interfaces;
using GraphDecomposition.GraphElements;

namespace GraphDecomposition.Algorithms
{
    public class StinsonExtended : Stinson, IDecompositionAlgorithm
    {
        /// <summary>
        /// Probability with which a block will be removed
        /// </summary>
        public const double ALPHA = 0.66;

        /// <summary>
        /// Generates a different decomposition (possibly isomorphic)
        /// </summary>
        /// <returns>STS(v)</returns>
        public SteinerTripleSystem NextDecomposition(SteinerTripleSystem sts)
        {
            RemoveRandBlocks(sts);

            while (NumBlocks < v * (v - 1) / 6)
            {
                Switch();
            }

            return ConstructBlocks(v, Other);
        }

        /// <summary>
        /// Removes a random number of blocks from the decomposition.
        /// </summary>
        private void RemoveRandBlocks(SteinerTripleSystem sts)
        {
            Random rand = new Random();

            for (int i = 0; i < sts.NumTriples(); i++)
            {
                bool removeBlock = rand.NextDouble() > ALPHA;

                if (removeBlock)
                {
                    Triple triple = sts.GetElement(i);
                    RemoveBlock(triple.X, triple.Y, triple.Z);
                }
            }

        }

        /// <summary>
        /// Removes a block from the deconstruction
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        /// <param name="z">Third vertex</param>
        private void RemoveBlock(int x, int y, int z)
        {
            Other[x, y] = 0;
            Other[y, x] = 0;
            Other[x, z] = 0;
            Other[z, x] = 0;
            Other[y, z] = 0;
            Other[z, y] = 0;

            InsertPair(x, y);
            InsertPair(y, x);
            InsertPair(x, z);
            InsertPair(z, x);
            InsertPair(y, z);
            InsertPair(z, y);

            this.NumBlocks = this.NumBlocks - 1;
        }
    }
}
