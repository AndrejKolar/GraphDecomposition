using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphDecomposition.Utils
{
    public static class GraphUtils
    {
        /// <summary>
        /// Check if a complete graph with this number of vertices can be decomposed
        /// </summary>
        /// <param name="numVertex">Number of vertices</param>
        /// <returns></returns>
        public static bool CanDecomposeGraph(int numVertex)
        {
            return (numVertex % 6 == 1) || (numVertex % 6 == 3); 
        }
    }
}
