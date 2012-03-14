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
        private int v;

        /// <summary>
        /// Number of triples in a STS
        /// </summary>
        private int b;

        /// <summary>
        /// LivePoints array contains all the live points 
        /// </summary>
        private int[] LivePoints;
        private int[] IndexLivePoints;
        private int NumLivePoints;

        /// <summary>
        /// LivePairs array contains for each live point an array of all the points that make a live pair with that point
        /// </summary>
        private int[,] LivePairs;
        private int[,] IndexLivePairs;
        private int[] NumLivePairs;


        /// <summary>
        /// Array that keeps track of the third point in a block containing x and y 
        /// </summary>
        int[,] Other;

        /// <summary>
        /// Starts the algorithm
        /// </summary>
        /// <param name="v">Number of vertices in a complete graph.</param>
        public void StartAlgorithm(int v)
        {
            this.v = v;
            this.b = v * (v - 1) / 6;
        }

        /// <summary>
        /// Constructs the block set B that contains all the triples in the STS
        /// </summary>
        /// <param name="v">Number of vertices</param>
        /// <param name="Other">Array that keeps track of the third point in a block containing x and y</param>
        /// <returns></returns>
        private Triple[] ConstructBlocks(int v, int[,] Other)
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

        /// <summary>
        /// Initailises the arrays at at the begining of the hill-climbing algorithm
        /// </summary>
        /// <param name="v">Number of vertices in the complete graph</param>
        private void Initialize(int v)
        {
            int arraySize = v + 1;

            #region Array initialisation
            LivePoints = new int[arraySize];
            IndexLivePoints = new int[arraySize];
            NumLivePairs = new int[arraySize];

            LivePairs = new int[arraySize, arraySize];
            IndexLivePairs = new int[arraySize, arraySize];

            Other = new int[arraySize, arraySize]; 
            #endregion

            NumLivePoints = v;

            for (int x = 1; x <= v; x++)
            {
                LivePoints[x] = x;
                IndexLivePoints[x] = x;
                NumLivePairs[x] = v - 1;

                for (int y = 1; y <= v - 1; y++)
                {
                    LivePairs[x, y] = (y + x - 1) % v + 1;
                }

                for (int y = 01; y <= v; y++)
                {
                    IndexLivePairs[x, y] = (y - x) % v;
                    Other[x, y] = 0;
                }


            }
        }

        /// <summary>
        /// Performs the insertion of a pair in the arrays
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        private void InsertPair(int x, int y)
        {
            if (NumLivePairs[x] == 0)
            {
                NumLivePoints = NumLivePoints + 1;
                LivePoints[NumLivePoints] = x;
                IndexLivePoints[x] = NumLivePoints;
            }

            NumLivePairs[x] = NumLivePairs[x] + 1;
            int posn = NumLivePairs[x];
            LivePairs[x, posn] = y;
            IndexLivePairs[x, y] = posn;
        }

        /// <summary>
        /// Performs the deletion of a pair in the arrays
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        private void DeletePair(int x, int y)
        {
            int posn = IndexLivePairs[x, y];
            int num = NumLivePairs[x];
            int z = LivePairs[x, num];
            LivePairs[x, posn] = z;
            IndexLivePairs[x, z] = posn;
            LivePairs[x, num] = 0;
            IndexLivePairs[x, y] = 0;
            NumLivePairs[x] = NumLivePairs[x] - 1;

            if (NumLivePairs[x] == 0)
            {
                posn = IndexLivePoints[x];
                z = LivePoints[NumLivePoints];
                LivePoints[posn] = z;
                IndexLivePoints[z] = posn;
                LivePoints[NumLivePoints] = 0;
                IndexLivePoints[x] = 0;
                NumLivePoints = NumLivePoints - 1;
            }
        }
    }
}
