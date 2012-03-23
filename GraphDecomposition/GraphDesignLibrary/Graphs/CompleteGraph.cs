using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphDesignLibrary
{
    public class CompleteGraph : Graph
    {

        public CompleteGraph(int numVertices)  //constructor for a complete graph
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();

            CreateVertices(numVertices);
            CreateEdges(numVertices);
           
        }

        

        private void CreateVertices(int numVertices)
        {
            if (numVertices % 3 == 0)
            {
                for (int i = 1; i <= numVertices / 3; i++)  //create Vertices
                {
                    Vertices.Add(new Vertex() { Name = "(" + i.ToString() + ",1)", ObjectName = "_" + i.ToString() + "1" });

                    Vertices.Add(new Vertex() { Name = "(" + i.ToString() + ",2)", ObjectName = "_" + i.ToString() + "2" });

                    Vertices.Add(new Vertex() { Name = "(" + i.ToString() + ",3)", ObjectName = "_" + i.ToString() + "3" });
                }
            }
            else if (numVertices % 3 == 1)
            {
                Vertices.Add(new Vertex() { Name = "(0,0)", ObjectName = "_00" });

                for (int i = 1; i <= (numVertices - 1) / 3; i++)  //create Vertices
                {
                    Vertices.Add(new Vertex() { Name = "(" + i.ToString() + ",1)", ObjectName = "_" + i.ToString() + "1" });

                    Vertices.Add(new Vertex() { Name = "(" + i.ToString() + ",2)", ObjectName = "_" + i.ToString() + "2" });

                    Vertices.Add(new Vertex() { Name = "(" + i.ToString() + ",3)", ObjectName = "_" + i.ToString() + "3" });
                }

            }
            else
            {
                for (int i = 1; i <= numVertices; i++)
                {
                    Vertices.Add(new Vertex() { Name = i.ToString(), ObjectName = "_" + i.ToString()});
                }
            }
        }

        private void CreateEdges(int numVertices)
        {
            Edge newEdge;
            for (int i = 0; i < numVertices - 1; i++)
            {
                for (int j = 1; j < numVertices; j++)
                {
                    newEdge = new Edge(Vertices[i], Vertices[j]) { Name = Vertices[i].Name + " - " + Vertices[j].Name, ObjectName = "_" + Vertices[i].ObjectName + Vertices[j].ObjectName };
                    Vertices[i].IncidentEdges.Add(newEdge);
                    Vertices[j].IncidentEdges.Add(newEdge);
                    Edges.Add(newEdge);

                }
            }
        }


        public void SetElementCoordinates(double canvasX, double canvasY)
        {
            const double VERTEX_SIZE = 5;

            int numVertices = Vertices.Count;
            for (int i = 0; i < numVertices; i++)   //set vertices coordinates
            {
                double GRAPH_SIZE = System.Math.Min(canvasX / 2, canvasY / 2); //određivanje veličine grafa

                double coordinateRad = (i+1) * 360 / numVertices * System.Math.PI / 180;

                double coordinateX = canvasX + GRAPH_SIZE * System.Math.Cos(coordinateRad);
                double coordinateY = canvasY + GRAPH_SIZE * System.Math.Sin(coordinateRad);

                Ellipse myEllipse = new Ellipse { Height = VERTEX_SIZE, Width = VERTEX_SIZE, Stroke = new SolidColorBrush(Colors.Black), Name = Vertices[i].ObjectName };

                myEllipse.SetValue(Canvas.LeftProperty, coordinateX);
                myEllipse.SetValue(Canvas.TopProperty, coordinateY);

                Vertices[i].Point = myEllipse;
                Vertices[i].CoordinateX = coordinateX;
                Vertices[i].CoordinateY = coordinateY;
            }

            foreach (Edge edg in Edges)  //set edges coordinates
            {
                edg.EdgeLine = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    X1 = edg.FirstIncidentVertex.CoordinateX + (VERTEX_SIZE / 2),
                    X2 = edg.SecondIncidentVertex.CoordinateX + (VERTEX_SIZE / 2),
                    Y1 = edg.FirstIncidentVertex.CoordinateY + (VERTEX_SIZE / 2),
                    Y2 = edg.SecondIncidentVertex.CoordinateY + (VERTEX_SIZE / 2),
                    Name = edg.ObjectName
                };
            }
        }
    }


}
