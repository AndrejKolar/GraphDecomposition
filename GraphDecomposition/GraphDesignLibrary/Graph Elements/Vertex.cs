using System.Collections.Generic;
using System.Windows.Shapes;

namespace GraphDesignLibrary
{
    public class Vertex : GraphElement
    {
        private List<Edge> incidentEdges;
        private Ellipse point;
        private double coordinateX;

        public double CoordinateX
        {
            get { return coordinateX; }
            set { coordinateX = value; }
        }
        private double coordinateY;

        public double CoordinateY
        {
            get { return coordinateY; }
            set { coordinateY = value; }
        }

        public Ellipse Point
        {
            get { return point; }
            set { point = value; }
        }

        public List<Edge> IncidentEdges
        {
            get { return incidentEdges; }
            set { incidentEdges = value; }
        }

        public Vertex()
        {
            incidentEdges = new List<Edge>();
        }


    }
}
