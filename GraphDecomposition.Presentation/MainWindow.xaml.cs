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

        private int numVertex;

        private SteinerTripleSystem sts;

        private IDecompositionAlgorithm algorithm;

        private Dictionary<String, Ellipse> vertexDictionary = new Dictionary<string, Ellipse>();
        private Dictionary<String, Line> edgeDictionary = new Dictionary<string, Line>();

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
                openInputSection();
            }
            else
            {
                closeInputSection();
            }

        }

        private void closeInputSection()
        {
            createButtonClicked = false;

            Grid.SetRow(buttonDecompose, 3);

            labelInput1.Visibility = Visibility.Hidden;
            labelInput2.Visibility = Visibility.Hidden;
            textBoxInput.Visibility = Visibility.Hidden;
            buttonDrawGraph.Visibility = Visibility.Hidden;
            textBoxInput.Text = "";
        }

        private void openInputSection()
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
                this.numVertex = Int16.Parse(textBoxInput.Text);

                drawGraph();
            }
            else
            {
                MessageBox.Show("Error inputing number of vertices.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void drawGraph()
        {
            this.canvasMain.Children.Clear();

            createVertices();

            createEdges();

            this.statusLabel.Content = "Begin decomposition";
        }

        private void createEdges()
        {
            for (int i = 1; i <= this.numVertex; i++)
            {
                for (int j = i + 1; j <= this.numVertex; j++)
                {
                    //skip edges that would connect to the same vertex
                    if (i == j)
                    {
                        continue;
                    }

                    Line newEdge = createNewEdge(i, j);
                    this.edgeDictionary.Add(getEdgeName(i, j), newEdge);
                    this.canvasMain.Children.Add(newEdge);
                }
            }
        }

        private static string getEdgeName(int i, int j)
        {
            if (i < j)
            {
                return i.ToString() + "_" + j.ToString();
            }
            else
            {
                return j.ToString() + "_" + i.ToString();
            }
            
        }

        private void createVertices()
        {
            for (int i = 1; i <= this.numVertex; i++)
            {
                Ellipse newVertex = createNewVertex(i);

                newVertex.MouseEnter += new System.Windows.Input.MouseEventHandler(Vertex_MouseEnter);
                this.vertexDictionary.Add(i.ToString(), newVertex);
                this.canvasMain.Children.Add(newVertex);
            }
        }

        private Line createNewEdge(int x, int y)
        {
            const double VERTEX_SIZE = 5;

            Ellipse firstVertex = this.vertexDictionary[x.ToString()];
            Ellipse secondVertex = this.vertexDictionary[y.ToString()];

            Line newEdge = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                X1 = Canvas.GetLeft(firstVertex) + (VERTEX_SIZE / 2),
                X2 = Canvas.GetLeft(secondVertex) + (VERTEX_SIZE / 2),
                Y1 = Canvas.GetTop(firstVertex) + (VERTEX_SIZE / 2),
                Y2 = Canvas.GetTop(secondVertex) + (VERTEX_SIZE / 2)
            };


            return newEdge;
        }

        private Ellipse createNewVertex(int vertexNumber)
        {
            const double VERTEX_SIZE = 5;

            double canvasX = this.canvasMain.ActualWidth / 2;
            double canvasY = this.canvasMain.ActualHeight / 2;

            double GRAPH_SIZE = System.Math.Min(canvasX / 2, canvasY / 2); //određivanje veličine grafa

            double coordinateRad = vertexNumber * 360 / this.numVertex * System.Math.PI / 180;

            double coordinateX = canvasX + GRAPH_SIZE * System.Math.Cos(coordinateRad);
            double coordinateY = canvasY + GRAPH_SIZE * System.Math.Sin(coordinateRad);

            Ellipse newVertex = new Ellipse { Height = VERTEX_SIZE, Width = VERTEX_SIZE, Stroke = new SolidColorBrush(Colors.Black) };

            newVertex.SetValue(Canvas.LeftProperty, coordinateX);
            newVertex.SetValue(Canvas.TopProperty, coordinateY);

            return newVertex;
        }

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
            bool noGraphCreated = this.canvasMain.Children.Count == 0;
            if (noGraphCreated)   //graph has not been created
            {
                MessageBox.Show("Please create a graph before decomposition.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //creates an algorithm object and runs the algorithm
            switch (GraphUtils.ChooseConstruction(this.numVertex))
            {
                case ConstructionType.Bose:
                    this.algorithm = new Bose();
                    this.sts = this.algorithm.StartAlgorithm(this.numVertex);
                    break;
                case ConstructionType.Skolem:
                    this.algorithm = new Skolem();
                    this.sts = this.algorithm.StartAlgorithm(this.numVertex);
                    break;
                case ConstructionType.None:
                    MessageBox.Show("Cannot decompose a graph with this number of vertices", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }

            closeInputSection();

            showDecompositionButtons();

        }

        private void showDecompositionButtons()
        {
            buttonNextTriple.Visibility = Visibility.Visible;
            buttonPrevTriple.Visibility = Visibility.Visible;
        }

        private void buttonNextTriple_Click(object sender, RoutedEventArgs e)
        {
            bool isEndOfDecomposition = this.index == this.sts.Count();
            if (isEndOfDecomposition)
            {
                MessageBox.Show("You have reached the end of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                statusLabel.Content = "Decomposition has ended.";
                return;
            }

            Triple myTriple = this.sts.Element(this.index);
            this.index++;
            statusLabel.Content = "Removing triple from graph: (" + myTriple.X.ToString() + ", " + myTriple.Y.ToString() + ", " + myTriple.Z.ToString() + ")";


            Ellipse firstVertex = this.vertexDictionary[myTriple.X.ToString()];
            Ellipse secondVertex = this.vertexDictionary[myTriple.Y.ToString()];
            Ellipse thirdVertex = this.vertexDictionary[myTriple.Z.ToString()];

            Line firstEdge = this.edgeDictionary[getEdgeName(myTriple.X, myTriple.Y)];
            Line secondEdge = this.edgeDictionary[getEdgeName(myTriple.X, myTriple.Z)];
            Line thirdEdge = this.edgeDictionary[getEdgeName(myTriple.Z, myTriple.Y)];

            AnimVertex(firstVertex, opacityAnimVertexF);
            AnimVertex(secondVertex, opacityAnimVertexF);
            AnimVertex(thirdVertex, opacityAnimVertexF);

            AnimEdge(firstEdge, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            AnimEdge(secondEdge, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            AnimEdge(thirdEdge, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);


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

        private void AnimVertex(Ellipse vertex, DoubleAnimation opacityAnim)
        {
            vertex.BeginAnimation(Ellipse.OpacityProperty, opacityAnim);
        }

        private void AnimEdge(Line edge, DoubleAnimation opacityAnim, SolidColorBrush animatedBrush, ColorAnimation colorAnim)
        {
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
            edge.Stroke = animatedBrush;
            edge.BeginAnimation(Line.OpacityProperty, opacityAnim);
        }     

    }


}
