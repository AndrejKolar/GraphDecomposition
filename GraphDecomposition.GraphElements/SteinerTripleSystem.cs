using System;
using System.Collections;
using System.Linq;
using System.Text;

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
    }
}
