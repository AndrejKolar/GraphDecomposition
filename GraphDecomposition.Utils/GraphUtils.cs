
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

        /// <summary>
        /// Chooses construction type based on the number of vertices in the graph
        /// </summary>
        /// <param name="numVertex"></param>
        /// <returns></returns>
        public static ConstructionType ChooseConstruction(int numVertex)
        {
            if (numVertex % 6 == 1)
            {
                return ConstructionType.Skolem;
            }
            else if (numVertex % 6 == 3)
            {
                return ConstructionType.Bose;
            }
            else
            {
                return ConstructionType.None;
            }
        }
    }

    /// <summary>
    /// Enum defines which construction can be used
    /// </summary>
    public enum ConstructionType
    {
        Bose, Skolem, None
    }
}
