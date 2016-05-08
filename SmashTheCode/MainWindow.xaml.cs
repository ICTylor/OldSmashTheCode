using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmashTheCode
{
    public class Reward
    {
        public int R{ get; set; } = 0;
    }
    public class Action
    {
        public int Col { get; set; } = 0;
    }
    public class Agent
    {
        public Action Act(State s, Reward r)
        {
            Action action = new Action();
            return action;
        }
    }

    public class Policy
    {
        public int[] PossibleMoves { get; set; } = new int[6];
    }

    /*public Tuple<State, Reward> ProcessAction()
    {
        Tuple<State, Reward> output = new Tuple<State, Reward>(new State(), new Reward());

        return output;
    }*/

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Environment env = new Environment();
        public MainWindow()
        {
            InitializeComponent();
            PuyoConsole.main(null);
             
            for (int i = 0; i < 12; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                //rowDefinition.Height = GridLength.Auto;
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 6; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                ///columnDefinition.Width = GridLength.Auto;
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            for (int i = 0; i < 12; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                //rowDefinition.Height = GridLength.Auto;
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                grid2.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 6; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                ///columnDefinition.Width = GridLength.Auto;
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                grid2.ColumnDefinitions.Add(columnDefinition);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Background = Brushes.DarkGray;
            for (int i=0; i < 12; i++)
            {
                for (int j=0; j < 6; j++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Background = Brushes.Gray;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Margin = new Thickness(2);
                    textBlock.Text = "-1";
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding()
                    {
                        Path = new PropertyPath("state.PlayerGameboard["+(j+i*6)+"]"),
                        Source = this.env
                    });
                    textBlock.DataContext = this.env;
                    grid.Children.Add(textBlock);
                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);
                }
            }
            int[] initialStateEmpty = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1};
//            env.state.PlayerGameboard = initialStateEmpty;
        }

        private void grid2_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Background = Brushes.Gray;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Margin = new Thickness(2);
                    textBlock.Text = "-1";
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding()
                    {
                        Path = new PropertyPath("state.EnemyGameboard[" + (j + i * 6) + "]"),
                        Source = this.env
                    });
                    textBlock.DataContext = this.env;
                    grid.Children.Add(textBlock);
                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);
                }
            }
            int[] initialStateEmpty = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1};
            //env.state.PlayerGameboard = initialStateEmpty;
        }

        private void gridScore_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
/*            Grid grid = sender as Grid;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Background = Brushes.Gray;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Margin = new Thickness(2);
                    textBlock.Text = "-1";
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding()
                    {
                        Path = new PropertyPath("state.NextPairs[" + i + "]"),
                        Source = this.env
                    });
                    textBlock.DataContext = this.env;
                    grid.Children.Add(textBlock);
                    Grid.SetRow(textBlock, j);
                    Grid.SetColumn(textBlock, i);
                }
            }
            */
        }
    }
}
