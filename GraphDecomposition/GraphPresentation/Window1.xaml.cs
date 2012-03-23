using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GraphDesignLibrary;


namespace GraphPresentation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        #region Fields
        private Regex isNum = new Regex("^[0-9]+$");   //to check if input is Int
        private bool createButtonClicked = false;
        
        private CompleteGraph myGraph;
        private SteinerTripleSystem mySTS;
        private int index = 0;

        private DoubleAnimation opacityAnimEdgeF;
        private DoubleAnimation opacityAnimVertexF;
        private ColorAnimation colorAnimFoward;
        private SolidColorBrush animatedBrushForward;
        private DoubleAnimation opacityAnimBack;
        private ColorAnimation colorAnimBack;
        private SolidColorBrush animatedBrushBack;
        #endregion

        public Window1()
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

        #region Reading the number of vertices for a complete graph
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

        #region Toggle Input num vertices section
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
        #endregion 
        #endregion

        #region Create a complete graph
        private void buttonDrawGraph_Click(object sender, RoutedEventArgs e)
        {
            if (isNum.IsMatch(textBoxInput.Text))
            {
                int numVertices = Int16.Parse(textBoxInput.Text);
                myGraph = new CompleteGraph(numVertices);

                myGraph.SetElementCoordinates(canvasMain.ActualWidth / 2, canvasMain.ActualHeight / 2);

                DrawGraph(myGraph);

                index = 0; //set the index of the current triple to beginning
            }
            else
            {
                MessageBox.Show("Error inputing number of vertices.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void DrawGraph(CompleteGraph myGraph)
        {
            canvasMain.Children.Clear();

            foreach (Vertex ver in myGraph.Vertices)
            {
                ver.Point.MouseEnter += new System.Windows.Input.MouseEventHandler(Vertex_MouseEnter);
                canvasMain.Children.Add(ver.Point);
            }

            foreach (Edge edg in myGraph.Edges)
            {
                canvasMain.Children.Add(edg.EdgeLine);
            }

            statusLabel.Content = "Begin decomposition";
        }

        void Vertex_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse vertex = (Ellipse)sender;

            String name = "This is vertex: (";
            for (int i = 1; i < vertex.Name.Length-1; i++)
            {
                name += vertex.Name[i];
            }

            name += "," + vertex.Name[vertex.Name.Length-1] + ")";

            statusLabel.Content = name;
        } 

        #endregion

        #region Create a STS for the complete graph
        private void buttonDecompose_Click(object sender, RoutedEventArgs e)
        {
            if (myGraph == null)   //graph has not been created
            {
                MessageBox.Show("Please create a graph before decomposition.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (myGraph.Vertices.Count % 6 == 3 % 6)
            {
                mySTS = new BoseConstruction(myGraph.Vertices.Count);
            }
            else if (myGraph.Vertices.Count % 6 == 1 % 6)
            {
                mySTS = new SkolemConstruction(myGraph.Vertices.Count);
            }
            else
            {
                MessageBox.Show("Cannot decompose this number of vertices", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CloseInputSection();
            buttonNextTriple.Visibility = Visibility.Visible;
            buttonPrevTriple.Visibility = Visibility.Visible;            

        } 
        #endregion

        #region Next/Previous Buttons
        private void buttonNextTriple_Click(object sender, RoutedEventArgs e)
        {
            if (index == mySTS.TripleList.Count)  //checks if it's the end of decomposition
            {
                MessageBox.Show("You have reached the end of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                statusLabel.Content = "Decomposition has ended.";
                return;
            }

            Triple myTriple = mySTS.TripleList[index]; //read the next triple

            statusLabel.Content = "Removing triple from graph: " + myTriple.FirstPair +" "+ myTriple.SecondPair +" "+ myTriple.ThirdPair;

            index++;

            Vertex firstV = null;
            Vertex secondV = null;
            Vertex thirdV = null;   //finding the three vertices of a triple
            foreach (Vertex ver in myGraph.Vertices)
            {
                if (ver.Name == myTriple.FirstPair)
                {
                    firstV = ver;
                }
                if (ver.Name == myTriple.SecondPair)
                {
                    secondV = ver;
                }
                if (ver.Name == myTriple.ThirdPair)
                {
                    thirdV = ver;
                }
            }

            AnimVertex(firstV, opacityAnimVertexF);
            AnimVertex(secondV, opacityAnimVertexF);
            AnimVertex(thirdV, opacityAnimVertexF);

            AnimEdge(firstV, secondV, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            AnimEdge(firstV, thirdV, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);
            AnimEdge(secondV, thirdV, opacityAnimEdgeF, animatedBrushForward, colorAnimFoward);


        }

        private void buttonPrevTriple_Click(object sender, RoutedEventArgs e)
        {

            if (index == 0)  //checks if it's the end of decomposition
            {
                MessageBox.Show("You have reached the beginning of the decomposition.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                statusLabel.Content = "Begin decomposition";
                return;
            }

            index--;

            Triple myTriple = mySTS.TripleList[index]; //read the prev triple

            statusLabel.Content = "Returning triple to graph: " + myTriple.FirstPair + " " + myTriple.SecondPair + " " + myTriple.ThirdPair;

            Vertex firstV = null;
            Vertex secondV = null;
            Vertex thirdV = null;   //finding the three vertices of a triple
            foreach (Vertex ver in myGraph.Vertices)
            {
                if (ver.Name == myTriple.FirstPair)
                {
                    firstV = ver;
                }
                if (ver.Name == myTriple.SecondPair)
                {
                    secondV = ver;
                }
                if (ver.Name == myTriple.ThirdPair)
                {
                    thirdV = ver;
                }
            }


            AnimVertex(firstV, opacityAnimBack);
            AnimVertex(secondV, opacityAnimBack);
            AnimVertex(thirdV, opacityAnimBack);

            AnimEdge(firstV, secondV, opacityAnimBack, animatedBrushBack, colorAnimBack);
            AnimEdge(firstV, thirdV, opacityAnimBack, animatedBrushBack, colorAnimBack);
            AnimEdge(secondV, thirdV, opacityAnimBack, animatedBrushBack, colorAnimBack);

        } 
        #endregion

        #region Canvas elements animation
        private void AnimVertex(Vertex vertex, DoubleAnimation opacityAnim)
        {


            foreach (var elem in canvasMain.Children)
            {

                if (elem is Ellipse)
                {
                    Ellipse myEllipse = (Ellipse)elem;

                    if (myEllipse.Name == vertex.ObjectName)
                    {
                        myEllipse.BeginAnimation(Ellipse.OpacityProperty, opacityAnim);
                    }
                }
            }

        }

        private void AnimEdge(Vertex firstV, Vertex secondV, DoubleAnimation opacityAnim, SolidColorBrush animatedBrush, ColorAnimation colorAnim)
        {

            foreach (var elem in canvasMain.Children)
            {

                if (elem is Line)
                {
                    Line myLine = (Line)elem;

                    if (CompareLineName(myLine, firstV, secondV))
                    {
                        Storyboard myBoard = new Storyboard();

                        animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
                        myLine.Stroke = animatedBrush;
                        myLine.BeginAnimation(Line.OpacityProperty, opacityAnim);
                    }
                }

            }
        }

        private bool CompareLineName(Line myLine, Vertex firstV, Vertex secondV)
        {
            if (myLine.Name == "_" + firstV.ObjectName + secondV.ObjectName)
            {
                return true;
            }
            else if (myLine.Name == "_" + secondV.ObjectName + firstV.ObjectName)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
        #endregion 
        
       

    }

    
}
