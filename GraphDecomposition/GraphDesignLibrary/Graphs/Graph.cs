using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphDesignLibrary
{
    public class Graph
    {
        private List<Edge> edges;

        private List<Vertex> vertices;

        public List<Edge> Edges
        {
            get { return edges; }
            set { edges = value; }
        }

        public List<Vertex> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
    }
}
