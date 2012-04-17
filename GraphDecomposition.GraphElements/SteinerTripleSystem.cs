using System.Collections;

namespace GraphDecomposition.GraphElements
{
    public class SteinerTripleSystem
    {
        /// <summary>
        /// Array of triples in the STS(v)
        /// </summary>
        private Triple[] tripleArray;

        /// <summary>
        /// Index of the triple array
        /// </summary>
        private int index;

        /// <summary>
        /// Number of vertices
        /// </summary>
        private int v;

        /// <summary>
        /// Constructor initialises the STS(v) with the number of vertices and triples
        /// </summary>
        /// <param name="v">Number of vertices</param>
        /// <param name="b">Number of triples</param>
        public SteinerTripleSystem(int v, int b)
        {
            this.v = v;
            this.tripleArray = new Triple[b];
            this.index = 0;
        }

        /// <summary>
        /// Iterator used to fetch all the triples
        /// </summary>
        /// <returns>A triple from the STS(v)</returns>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < tripleArray.Length; i++)
            {
                yield return tripleArray[i];
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
            this.tripleArray[this.index] = new Triple(x, y, z);
            index++;
        }

        /// <summary>
        /// Gets the number of triples in the STS(v)
        /// </summary>
        /// <returns>Number of triples</returns>
        public int NumTriples()
        {
            return tripleArray.Length;
        }

        /// <summary>
        /// Gets the number of vertices in the STS(v)
        /// </summary>
        /// <returns>Number of vertices</returns>
        public int NumVertex()
        {
            return this.v;
        }

        /// <summary>
        /// Gets the triple with the specifiedw index
        /// </summary>
        /// <param name="index">Index of the triple</param>
        /// <returns>Triple with the specidied index</returns>
        public Triple GetElement(int index)
        {
            return tripleArray[index];
        }


            
    }
}
