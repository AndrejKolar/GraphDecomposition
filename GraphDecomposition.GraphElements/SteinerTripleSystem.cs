﻿using System.Collections;

namespace GraphDecomposition.GraphElements
{
    public class SteinerTripleSystem
    {
        /// <summary>
        /// Array of triples in the STS(v)
        /// </summary>
        private Triple[] tripleList;

        /// <summary>
        /// Index of the triple array
        /// </summary>
        private int index;

        /// <summary>
        /// Constructor initialises the STS(v) with the number of vertices
        /// </summary>
        /// <param name="b">Number of triples</param>
        public SteinerTripleSystem(int b)
        {
            this.tripleList = new Triple[b];
            this.index = 0;
        }

        /// <summary>
        /// Iterator used to fetch all the triples
        /// </summary>
        /// <returns>A triple from the STS(v)</returns>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < tripleList.Length; i++)
            {
                yield return tripleList[i];
            }
        }

        /// <summary>
        /// Creates a new triple and adds it to the STS(v)
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        /// <param name="z">Third vertex</param>
        public void AddTriple(int x, int y, int z)
        {
            this.tripleList[this.index] = new Triple(x, y, z);
            index++;
        }

        /// <summary>
        /// Gets the number of triples in the STS(v)
        /// </summary>
        /// <returns>Number of triples</returns>
        public int Count()
        {
            return tripleList.Length;
        }

        /// <summary>
        /// Gets the triple with the specifiedw index
        /// </summary>
        /// <param name="index">Index of the triple</param>
        /// <returns>Triple with the specidied index</returns>
        public Triple Element(int index)
        {
            return tripleList[index];
        }


            
    }
}
