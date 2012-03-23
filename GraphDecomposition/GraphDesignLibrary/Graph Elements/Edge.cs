using System.Windows.Shapes;

namespace GraphDesignLibrary
{
    public class Edge : GraphElement
    {
        private Vertex firstIncidentVertex;

        public Vertex FirstIncidentVertex
        {
            get { return firstIncidentVertex; }
            set { firstIncidentVertex = value; }
        }
        private Vertex secondIncidentVertex;

        public Vertex SecondIncidentVertex
        {
            get { return secondIncidentVertex; }
            set { secondIncidentVertex = value; }
        }

        public Edge(Vertex firstIncidentVertex, Vertex secondIncidentVertex)
        {
            this.firstIncidentVertex = firstIncidentVertex;
            this.secondIncidentVertex = secondIncidentVertex;
        }

        private Line edgeLine;

        public Line EdgeLine
        {
            get { return edgeLine; }
            set { edgeLine = value; }
        }
    }
}
