using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GraphDecomposition.Algorithms;
using GraphDecomposition.GraphElements;
using GraphDecomposition.Interfaces;
using GraphDecomposition.Utils;

namespace GraphDecomposition.Presentation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Regex checks if the string is a number
        /// </summary>
        private Regex isNum = new Regex("^[0-9]+$");

        /// <summary>
        /// True if the UI section for the new graph creation is opened
        /// </summary>
        private bool createButtonClicked = false;

        /// <summary>
        /// True if the graph decomposition is started
        /// </summary>
        private bool isDecompositionStarted = false;

        /// <summary>
        /// Index of current triple that is removed in the graph decomposition
        /// </summary>
        private int index = 0;

        /// <summary>
        /// Number of vertices in the created complete graph
        /// </summary>
        private int numVertex;

        /// <summary>
        /// Steiner triple system created by the decomposition
        /// </summary>
        private SteinerTripleSystem sts;

        /// <summary>
        /// Algorithm used for the decomposition
        /// </summary>
        private IDecompositionAlgorithm algorithm;

        /// <summary>
        /// Dictionary used for indexing vertex elements drawn on the canvas
        /// </summary>
        private Dictionary<String, Ellipse> vertexDictionary = new Dictionary<string, Ellipse>();

        /// <summary>
        /// Dictionary used for indexing edge elements drawn on the canvas
        /// </summary>
        private Dictionary<String, Line> edgeDictionary = new Dictionary<string, Line>();

        #region Animation parameters
        private DoubleAnimation opacityAnimEdgeF;
        private DoubleAnimation opacityAnimVertexF;
        private ColorAnimation colorAnimFoward;
        private SolidColorBrush animatedBrushForward;
        private DoubleAnimation opacityAnimBack;
        private ColorAnimation colorAnimBack;
        private SolidColorBrush animatedBrushBack; 
        #endregion


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

        /// <summary>
        /// Button event for opening the UI section for entering the number of vertices for the complete graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (createButtonClicked == false)  
            {
                openInputSection();
            }
            else
            {
                closeInputSection();
            }

        }

        /// <summary>
        /// Method closes the part of UI for inputing the number of vertices
        /// </summary>
        private void closeInputSection()
        {
            createButtonClicked = false;

            Grid.SetRow(buttonDecompose, 3);

            labelInput1.Visibility = Visibility.Hidden;
            labelInput2.Visibility = Visibility.Hidden;
            textBoxInput.Visibility = Visibility.Hidden;
            buttonDrawGraph.Visibility = Visibility.Hidden;
            textBoxInput.Text = String.Empty;
        }


        /// <summary>
        /// Method opens the part of the UI for inputing the number of vertices
        /// </summary>
        private void openInputSection()
        {
            createButtonClicked = true;

            Grid.SetRow(buttonDecompose, 9);

            labelInput1.Visibility = Visibility.Visible;
            labelInput2.Visibility = Visibility.Visible;
            textBoxInput.Visibility = Visibility.Visible;
            buttonDrawGraph.Visibility = Visibility.Visible;
            buttonNextTriple.Visibility = Visibility.Hidden;
            buttonPrevTriple.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Button event for drawing the complete graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method clears the old graph elements and creates new ones
        /// </summary>
        private void drawGraph()
        {
            clearGraphElements();

            createVertices();

            createEdges();

            this.statusLabel.Content = "Begin decomposition";
        }

        /// <summary>
        /// Removes old graph elements
        /// </summary>
        private void clearGraphElements()
        {
            this.canvasMain.Children.Clear();
            this.vertexDictionary.Clear();
            this.edgeDictionary.Clear();
            this.sts = null;
            this.index = 0;
            this.isDecompositionStarted = false;
        }

        /// <summary>
        /// Creates edges for the complete graph
        /// </summary>
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

        /// <summary>
        /// Creates vertices for the complete graph
        /// </summary>
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

        /// <summary>
        /// Creates a single edge for the complete graph
        /// </summary>
        /// <param name="x">First incident vertex</param>
        /// <param name="y">Second incident vertex</param>
        /// <returns>Line element representing an edge</returns>
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

        /// <summary>
        /// Creates a single vertex for the complete graph
        /// </summary>
        /// <param name="vertexNumber">Number of the vertex</param>
        /// <returns>Ellipse element representing a vertex</returns>
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

        /// <summary>
        /// Creates a name for the edge UI element
        /// </summary>
        /// <param name="i">First incident vertex number</param>
        /// <param name="j">Second incident vertex number</param>
        /// <returns>Vertex name</returns>
        private string getEdgeName(int i, int j)
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

        /// <summary>
        /// Button event for starting the decompostition process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDecompose_Click(object sender, RoutedEventArgs e)
        {
            if (isDecompositionStarted)
            {
                MessageBox.Show("Decomposition has already started. Please create a new graph.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool noGraphCreated = this.canvasMain.Children.Count == 0;
            if (noGraphCreated)   //graph has not been created
            {
                MessageBox.Show("Please create a graph before decomposition.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool canDecomposeGraph = GraphUtils.CanDecomposeGraph(this.numVertex);
            if (!canDecomposeGraph)
            {
                MessageBox.Show("Cannot decompose a graph with this number of vertices.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            closeInputSection();

            showAlgorithmPickerButtons();

            setLabelOnConstructionButton();
        }

        /// <summary>
        /// Sets the label of the construction button depending on the vertex number
        /// </summary>
        private void setLabelOnConstructionButton()
        {
            switch (GraphUtils.ChooseConstruction(this.numVertex))
            {
                case ConstructionType.Bose:
                    this.buttonConstruction.Content = "Bose";
                    break;
                case ConstructionType.Skolem:
                    this.buttonConstruction.Content = "Skolem";
                    break;
                case ConstructionType.None:
                    return;
                default:
                    break;
            }
        }

        /// <summary>
        /// Button event used to pick a heuristic decompostition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHeuristic_Click(object sender, RoutedEventArgs e)
        {
            this.algorithm = new Stinson();
            startDecompostion();
        }

        /// <summary>
        /// Button event used to pick a construction decompostiton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConstruction_Click(object sender, RoutedEventArgs e)
        {
            switch (GraphUtils.ChooseConstruction(this.numVertex))
            {
                case ConstructionType.Bose:
                    this.algorithm = new Bose();
                    break;
                case ConstructionType.Skolem:
                    this.algorithm = new Skolem();
                    break;
                case ConstructionType.None:
                    return;
                default:
                    break;
            }

            startDecompostion();
        }

        /// <summary>
        /// Starts the graph decompostition
        /// </summary>
        private void startDecompostion()
        {
            this.isDecompositionStarted = true;
            this.sts = this.algorithm.StartAlgorithm(this.numVertex);

            showDecompositionButtons();
            hideAlgorithmPickerButtons();
        }

        /// <summary>
        /// Makes the algorithm picker buttons visible
        /// </summary>
        private void showAlgorithmPickerButtons()
        {
            this.buttonConstruction.Visibility = System.Windows.Visibility.Visible;
            this.buttonHeuristic.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Hides the algorithm picker buttons
        /// </summary>
        private void hideAlgorithmPickerButtons()
        {
            this.buttonConstruction.Visibility = System.Windows.Visibility.Hidden;
            this.buttonHeuristic.Visibility = System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// Shows the decomposition buttons
        /// </summary>
        private void showDecompositionButtons()
        {
            buttonNextTriple.Visibility = Visibility.Visible;
            buttonPrevTriple.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Next triple button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNextTriple_Click(object sender, RoutedEventArgs e)
        {
            bool isEndOfDecomposition = this.index == this.sts.NumTriples();
            if (isEndOfDecomposition)
            {
                MessageBox.Show("You have reached the end of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                statusLabel.Content = "Decomposition has ended.";
                return;
            }

            Triple myTriple = this.sts.GetElement(this.index);
            this.index++;
            statusLabel.Content = "Removing triple from graph: (" + myTriple.X.ToString() + ", " + myTriple.Y.ToString() + ", " + myTriple.Z.ToString() + ")";

            animateTriple(myTriple, true);
        }

        /// <summary>
        /// Previous triple button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPrevTriple_Click(object sender, RoutedEventArgs e)
        {

            bool isBeginningOfDecomposition = this.index == 0;
            if (isBeginningOfDecomposition)  
            {
                MessageBox.Show("You have reached the beginning of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                statusLabel.Content = "Begin decomposition";
                return;
            }

            this.index--;
            Triple myTriple = this.sts.GetElement(this.index);
            statusLabel.Content = "Returning triple to graph: (" + myTriple.X.ToString() + ", " + myTriple.Y.ToString() + ", " + myTriple.Z.ToString() + ")";

            animateTriple(myTriple, false);
        }

        /// <summary>
        /// Starts the animation of the triple 
        /// </summary>
        /// <param name="myTriple">Triple to be animated</param>
        /// <param name="isRemoval">Is the triple being removed or put back</param>
        private void animateTriple(Triple myTriple, bool isRemoval)
        {
            Ellipse firstVertex = this.vertexDictionary[myTriple.X.ToString()];
            Ellipse secondVertex = this.vertexDictionary[myTriple.Y.ToString()];
            Ellipse thirdVertex = this.vertexDictionary[myTriple.Z.ToString()];

            Line firstEdge = this.edgeDictionary[getEdgeName(myTriple.X, myTriple.Y)];
            Line secondEdge = this.edgeDictionary[getEdgeName(myTriple.X, myTriple.Z)];
            Line thirdEdge = this.edgeDictionary[getEdgeName(myTriple.Z, myTriple.Y)];

            if (isRemoval)
            {
                AnimVertex(firstVertex, opacityAnimVertexF);
                AnimVertex(secondVertex, opacityAnimVertexF);
                AnimVertex(thirdVertex, opacityAnimVertexF);

                AnimEdge(firstEdge, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
                AnimEdge(secondEdge, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
                AnimEdge(thirdEdge, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            }
            else
            {
                AnimVertex(firstVertex, opacityAnimBack);
                AnimVertex(secondVertex, opacityAnimBack);
                AnimVertex(thirdVertex, opacityAnimBack);

                AnimEdge(firstEdge, opacityAnimBack, animatedBrushBack, colorAnimBack);
                AnimEdge(secondEdge, opacityAnimBack, animatedBrushBack, colorAnimBack);
                AnimEdge(thirdEdge, opacityAnimBack, animatedBrushBack, colorAnimBack);
            }


        }

        /// <summary>
        /// Animates a single vertex
        /// </summary>
        /// <param name="vertex">Vertex to be animated</param>
        /// <param name="opacityAnim">Animation to be used</param>
        private void AnimVertex(Ellipse vertex, DoubleAnimation opacityAnim)
        {
            vertex.BeginAnimation(Ellipse.OpacityProperty, opacityAnim);
        }

        /// <summary>
        /// Animates a single edge
        /// </summary>
        /// <param name="edge">Edge to be animated</param>
        /// <param name="opacityAnim">Animation to be used</param>
        /// <param name="animatedBrush">Brush to be used</param>
        /// <param name="colorAnim">Color to be used</param>
        private void AnimEdge(Line edge, DoubleAnimation opacityAnim, SolidColorBrush animatedBrush, ColorAnimation colorAnim)
        {
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
            edge.Stroke = animatedBrush;
            edge.BeginAnimation(Line.OpacityProperty, opacityAnim);
        }

        /// <summary>
        /// Event fired when the mouse hovers over a vertex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Vertex_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse vertex = (Ellipse)sender;

            String name = "This is vertex: ";

            foreach (KeyValuePair<String, Ellipse> pair in vertexDictionary)
            {
                if (vertex.Equals(pair.Value))
                {
                    name += pair.Key;
                    break;
                }
            }

            this.statusLabel.Content = name;
        }

    }


}
