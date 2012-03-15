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
        /// Number of triples in a STS(v)
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
        private int[,] Other;

        /// <summary>
        /// Number of blocks in the PSTS(v)
        /// </summary>
        private int NumBlocks;

        /// <summary>
        /// Starts the algorithm
        /// </summary>
        /// <param name="v">Number of vertices in a complete graph.</param>
        public void StartAlgorithm(int v)
        {
            this.v = v;
            this.b = v * (v - 1) / 6;

            StinsonsAlgorithm(v);
        }

        /// <summary>
        /// Constructs the block set B that contains all the triples in the STS(v)
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

            LivePoints = new int[arraySize];
            IndexLivePoints = new int[arraySize];

            LivePairs = new int[arraySize, arraySize];
            NumLivePairs = new int[arraySize];
            IndexLivePairs = new int[arraySize, arraySize];

            Other = new int[arraySize, arraySize];

            NumLivePoints = v;

            for (int x = 1; x <= v; x++)
            {
                LivePoints[x] = x;
                IndexLivePoints[x] = x;
                NumLivePairs[x] = v - 1;

                for (int y = 1; y <= v - 1; y++)
                {
                    LivePairs[x, y] = ((y + x - 1) % v) + 1;
                }

                for (int y = 1; y <= v; y++)
                {
                    int index = (y - x);
                    if (index < 0)
                    {
                        index += v;
                    }

                    IndexLivePairs[x, y] = index; 
                    Other[x, y] = 0;
                }


            }
        }

        /// <summary>
        /// Gives the pair a status of a live pair
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
        /// Removes the Live Pair entry from the first vertex for the second vertex
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        private void DeletePair(int x, int y)
        {
            int posn = IndexLivePairs[x, y];    //LivePairs[x, posn] = y -> IndexLivePairs[x, y] = posn
            int num = NumLivePairs[x];          //NumLivePairs[x] = num -> posn of the last entry in the LivePairs array
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

        /// <summary>
        /// Creates a block of three vertices
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        /// <param name="z">Third vertex</param>
        private void AddBlock(int x, int y, int z)
        {
            Other[x, y] = z;
            Other[y, x] = z;
            Other[x, z] = y;
            Other[z, x] = y;
            Other[y, z] = x;
            Other[z, y] = x;

            DeletePair(x, y);
            DeletePair(y, x);
            DeletePair(x, z);
            DeletePair(z, x);
            DeletePair(y, z);
            DeletePair(z, y);
        }

        /// <summary>
        /// Removes an old vertex from a block and inserts a new vertex
        /// </summary>
        /// <param name="x">New vertex</param>
        /// <param name="y">Second vertex</param>
        /// <param name="z">Third vertex</param>
        /// <param name="w">Old vertex</param>
        private void ExchangeBlock(int x, int y, int z, int w)
        {
            Other[x, y] = z;
            Other[y, x] = z;
            Other[x, z] = y;
            Other[z, x] = y;
            Other[y, z] = x;
            Other[z, y] = x;

            Other[w, y] = 0;
            Other[y, w] = 0;
            Other[w, z] = 0;
            Other[z, w] = 0;

            InsertPair(w, y);
            InsertPair(y, w);
            InsertPair(w, z);
            InsertPair(z, w);

            DeletePair(x, y);
            DeletePair(y, x);
            DeletePair(x, z);
            DeletePair(z, x);
        }

        /// <summary>
        /// If all three pairs in a block are live creates an new block and adds it to the solution,
        /// if two are live it exchanges an old block with the new one
        /// </summary>
        private void Switch()
        {
            Random rand = new Random();

            int r = rand.Next(1, NumLivePoints + 1);

            int x = LivePoints[r];

            //1 <= s < t <= NumLivePairs[x]
            int t = rand.Next(2, NumLivePairs[x] + 1);
            int s = rand.Next(1, t);

            int y = LivePairs[x, s];
            int z = LivePairs[x, t];

            int w = 0;

            if (Other[y, z] == 0)
            {
                AddBlock(x, y, z);
                NumBlocks = NumBlocks + 1;
            }
            else
            {
                w = Other[y, z];
                ExchangeBlock(x, y, z, w);
            }
        }

        private Triple[] StinsonsAlgorithm(int v)
        {
            NumBlocks = 0;
            Initialize(v);

            while (NumBlocks < v * (v - 1) / 6)
            {
                Switch();
            }

            Triple[] B = ConstructBlocks(v, Other);

            return B;
        }
    }
}
