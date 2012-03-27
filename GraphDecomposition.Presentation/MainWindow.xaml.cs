using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

using GraphDecomposition.Algorithms;
using GraphDecomposition.GraphElements;
using GraphDecomposition.Utils;
using GraphDecomposition.Interfaces;

namespace GraphDecomposition.Presentation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Regex isNum = new Regex("^[0-9]+$");   //to check if input is Int
        private bool createButtonClicked = false;

        //private CompleteGraph myGraph;
        //private SteinerTripleSystem mySTS;
        private int index = 0;

        private IDecompositionAlgorithm algorithm;

        private Dictionary<String, Ellipse> vertexDictionary = new Dictionary<string,Ellipse>();
        private Dictionary<String, Line> edgeDictionary = new Dictionary<string,Line>();

        private DoubleAnimation opacityAnimEdgeF;
        private DoubleAnimation opacityAnimVertexF;
        private ColorAnimation colorAnimFoward;
        private SolidColorBrush animatedBrushForward;
        private DoubleAnimation opacityAnimBack;
        private ColorAnimation colorAnimBack;
        private SolidColorBrush animatedBrushBack;


        public MainWindow()
        {
            InitializeComponent();

            statusLabel.Content = "Create a graph to begin decomposition";

            #region Initialisinig animation parameters
            opacityAnimEdgeF = new DoubleAnimation()
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    BeginTime = TimeSpan.FromSeconds(1.2)
                };

            opacityAnimVertexF = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1.7)
            };

            colorAnimFoward = new ColorAnimation()
            {
                From = Colors.Black,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            animatedBrushForward = new SolidColorBrush() { Color = Colors.Black };

            opacityAnimBack = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.5),
                BeginTime = TimeSpan.FromSeconds(0.3)

            };

            colorAnimBack = new ColorAnimation()
            {
                From = Colors.Red,
                To = Colors.Black,
            };

            animatedBrushBack = new SolidColorBrush() { Color = Colors.Red };
            #endregion
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (createButtonClicked == false)  //toggle vertices number display
            {
                OpenInputSection();
            }
            else
            {
                CloseInputSection();
            }

        }

        private void CloseInputSection()
        {
            createButtonClicked = false;

            Grid.SetRow(buttonDecompose, 3);

            labelInput1.Visibility = Visibility.Hidden;
            labelInput2.Visibility = Visibility.Hidden;
            textBoxInput.Visibility = Visibility.Hidden;
            buttonDrawGraph.Visibility = Visibility.Hidden;
            textBoxInput.Text = "";
        }

        private void OpenInputSection()
        {
            createButtonClicked = true;

            Grid.SetRow(buttonDecompose, 9); //set row of a button in a grid

            labelInput1.Visibility = Visibility.Visible;
            labelInput2.Visibility = Visibility.Visible;
            textBoxInput.Visibility = Visibility.Visible;
            buttonDrawGraph.Visibility = Visibility.Visible;
            buttonNextTriple.Visibility = Visibility.Hidden;
            buttonPrevTriple.Visibility = Visibility.Hidden;
        }

        private void buttonDrawGraph_Click(object sender, RoutedEventArgs e)
        {
            if (isNum.IsMatch(textBoxInput.Text))
            {
                int numVertex = Int16.Parse(textBoxInput.Text);

                drawGraph(numVertex);

                //myGraph = new CompleteGraph(numVertex);

                //myGraph.SetElementCoordinates(canvasMain.ActualWidth / 2, canvasMain.ActualHeight / 2);

                //DrawGraph(myGraph);

                //index = 0; //set the index of the current triple to beginning
            }
            else
            {
                MessageBox.Show("Error inputing number of vertices.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void drawGraph(int numVertex)
        {
            this.canvasMain.Children.Clear();

            //create vertices
            for (int i = 1; i <= numVertex; i++)
            {
                Ellipse newVertex = createNewVertex(i, numVertex);

                newVertex.MouseEnter += new System.Windows.Input.MouseEventHandler(Vertex_MouseEnter);
                this.vertexDictionary.Add(i.ToString(), newVertex);
                this.canvasMain.Children.Add(newVertex);
            }

            //create edges
            for (int i = 1; i <= numVertex; i++)
            {
                for (int j = 1; j <= numVertex; j++)
                {
                    //skip edges that would connect to the same vertex
                    if (i == j)
                    {
                        continue;
                    }

                    Line newEdge = createNewEdge(i, j);
                    this.edgeDictionary.Add(i.ToString() + "_" + j.ToString(), newEdge);
                    this.canvasMain.Children.Add(newEdge);
                }
            }

            this.statusLabel.Content = "Begin decomposition";
        }


        private Line createNewEdge(int firstVertex, int secondVertex)
        {
            return null;
        }

        private Ellipse createNewVertex(int vertexNumber, int countVertex)
        {
            const double VERTEX_SIZE = 5;

            double canvasX = this.canvasMain.ActualWidth / 2;
            double canvasY = this.canvasMain.ActualHeight / 2;

            double GRAPH_SIZE = System.Math.Min(canvasX / 2, canvasY / 2); //određivanje veličine grafa

            double coordinateRad = vertexNumber * 360 / countVertex * System.Math.PI / 180;

            double coordinateX = canvasX + GRAPH_SIZE * System.Math.Cos(coordinateRad);
            double coordinateY = canvasY + GRAPH_SIZE * System.Math.Sin(coordinateRad);

            Ellipse newVertex = new Ellipse { Height = VERTEX_SIZE, Width = VERTEX_SIZE, Stroke = new SolidColorBrush(Colors.Black) };

            newVertex.SetValue(Canvas.LeftProperty, coordinateX);
            newVertex.SetValue(Canvas.TopProperty, coordinateY);

            return newVertex;
        }

        //private void DrawGraph(CompleteGraph myGraph)
        //{
        //    canvasMain.Children.Clear();

        //    foreach (Vertex ver in myGraph.Vertices)
        //    {
        //        ver.Point.MouseEnter += new System.Windows.Input.MouseEventHandler(Vertex_MouseEnter);
        //        canvasMain.Children.Add(ver.Point);
        //    }

        //    foreach (Edge edg in myGraph.Edges)
        //    {
        //        canvasMain.Children.Add(edg.EdgeLine);
        //    }

        //    statusLabel.Content = "Begin decomposition";
        //}

        void Vertex_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Ellipse vertex = (Ellipse)sender;

            //String name = "This is vertex: (";
            //for (int i = 1; i < vertex.Name.Length-1; i++)
            //{
            //    name += vertex.Name[i];
            //}

            //name += "," + vertex.Name[vertex.Name.Length-1] + ")";

            //statusLabel.Content = name;
        }


        private void buttonDecompose_Click(object sender, RoutedEventArgs e)
        {
            //NEW CODE !!!

            ////creates an algorithm object and runs the algorithm
            //switch (GraphUtils.ChooseConstruction(numVertices))
            //{
            //    case ConstructionType.Bose:
            //        this.algorithm = new Bose();
            //        this.algorithm.StartAlgorithm(numVertices);
            //        break;
            //    case ConstructionType.Skolem:
            //        this.algorithm = new Skolem();
            //        this.algorithm.StartAlgorithm(numVertices);
            //        break;
            //    case ConstructionType.None:
            //        MessageBox.Show("Cannot decompose a graph with this number of vertices", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //        break;
            //    default:
            //        break;
            //}


            //if (myGraph == null)   //graph has not been created
            //{
            //    MessageBox.Show("Please create a graph before decomposition.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //if (myGraph.Vertices.Count % 6 == 3 % 6)
            //{
            //    mySTS = new BoseConstruction(myGraph.Vertices.Count);
            //}
            //else if (myGraph.Vertices.Count % 6 == 1 % 6)
            //{
            //    mySTS = new SkolemConstruction(myGraph.Vertices.Count);
            //}
            //else
            //{
            //    MessageBox.Show("Cannot decompose this number of vertices", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //CloseInputSection();
            //buttonNextTriple.Visibility = Visibility.Visible;
            //buttonPrevTriple.Visibility = Visibility.Visible;            

        }

        private void buttonNextTriple_Click(object sender, RoutedEventArgs e)
        {
            //if (index == mySTS.TripleList.Count)  //checks if it's the end of decomposition
            //{
            //    MessageBox.Show("You have reached the end of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
            //    statusLabel.Content = "Decomposition has ended.";
            //    return;
            //}

            //Triple myTriple = mySTS.TripleList[index]; //read the next triple

            //statusLabel.Content = "Removing triple from graph: " + myTriple.FirstPair +" "+ myTriple.SecondPair +" "+ myTriple.ThirdPair;

            //index++;

            //Vertex firstV = null;
            //Vertex secondV = null;
            //Vertex thirdV = null;   //finding the three vertices of a triple
            //foreach (Vertex ver in myGraph.Vertices)
            //{
            //    if (ver.Name == myTriple.FirstPair)
            //    {
            //        firstV = ver;
            //    }
            //    if (ver.Name == myTriple.SecondPair)
            //    {
            //        secondV = ver;
            //    }
            //    if (ver.Name == myTriple.ThirdPair)
            //    {
            //        thirdV = ver;
            //    }
            //}

            //AnimVertex(firstV, opacityAnimVertexF);
            //AnimVertex(secondV, opacityAnimVertexF);
            //AnimVertex(thirdV, opacityAnimVertexF);

            //AnimEdge(firstV, secondV, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            //AnimEdge(firstV, thirdV, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            //AnimEdge(secondV, thirdV, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);


        }

        private void buttonPrevTriple_Click(object sender, RoutedEventArgs e)
        {

            //if (index == 0)  //checks if it's the end of decomposition
            //{
            //    MessageBox.Show("You have reached the beginning of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
            //    statusLabel.Content = "Begin decomposition";
            //    return;
            //}

            //index--;

            //Triple myTriple = mySTS.TripleList[index]; //read the prev triple

            //statusLabel.Content = "Returning triple to graph: " + myTriple.FirstPair + " " + myTriple.SecondPair + " " + myTriple.ThirdPair;

            //Vertex firstV = null;
            //Vertex secondV = null;
            //Vertex thirdV = null;   //finding the three vertices of a triple
            //foreach (Vertex ver in myGraph.Vertices)
            //{
            //    if (ver.Name == myTriple.FirstPair)
            //    {
            //        firstV = ver;
            //    }
            //    if (ver.Name == myTriple.SecondPair)
            //    {
            //        secondV = ver;
            //    }
            //    if (ver.Name == myTriple.ThirdPair)
            //    {
            //        thirdV = ver;
            //    }
            //}


            //AnimVertex(firstV, opacityAnimBack);
            //AnimVertex(secondV, opacityAnimBack);
            //AnimVertex(thirdV, opacityAnimBack);

            //AnimEdge(firstV, secondV, opacityAnimBack, animatedBrushBack, colorAnimBack);
            //AnimEdge(firstV, thirdV, opacityAnimBack, animatedBrushBack, colorAnimBack);
            //AnimEdge(secondV, thirdV, opacityAnimBack, animatedBrushBack, colorAnimBack);

        }

        //private void AnimVertex(Vertex vertex, DoubleAnimation opacityAnim)
        //{


        //    foreach (var elem in canvasMain.Children)
        //    {

        //        if (elem is Ellipse)
        //        {
        //            Ellipse myEllipse = (Ellipse)elem;

        //            if (myEllipse.Name == vertex.ObjectName)
        //            {
        //                myEllipse.BeginAnimation(Ellipse.OpacityProperty, opacityAnim);
        //            }
        //        }
        //    }

        //}

        //private void AnimEdge(Vertex firstV, Vertex secondV, DoubleAnimation opacityAnim, SolidColorBrush animatedBrush, ColorAnimation colorAnim)
        //{

        //    foreach (var elem in canvasMain.Children)
        //    {

        //        if (elem is Line)
        //        {
        //            Line myLine = (Line)elem;

        //            if (CompareLineName(myLine, firstV, secondV))
        //            {
        //                Storyboard myBoard = new Storyboard();

        //                animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
        //                myLine.Stroke = animatedBrush;
        //                myLine.BeginAnimation(Line.OpacityProperty, opacityAnim);
        //            }
        //        }

        //    }
        //}

        //private bool CompareLineName(Line myLine, Vertex firstV, Vertex secondV)
        //{
        //    if (myLine.Name == "_" + firstV.ObjectName + secondV.ObjectName)
        //    {
        //        return true;
        //    }
        //    else if (myLine.Name == "_" + secondV.ObjectName + firstV.ObjectName)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}        


    }


}
