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
    public class State
    {
        public int[] PlayerGameboard { get; set; } = new int[6 * 12];
        public int[] EnemyGameboard { get; set; } = new int[6*12];
        public Tuple<int, int>[] NextPairs { get; set; } = new Tuple<int, int>[8];
        public int PlayerNuissance { get; set; } = 0;
        public int EnemyNuissance { get; set; } = 0;
        public int PlayerScore { get; set; } = 0;
        public int EnemyScore { get; set; } = 0;
    }
    public class Policy
    {
        public int[] PossibleMoves { get; set; } = new int[6];
    }
    public class Enviroment
    {
        public State state { get; set; } = new State();
        public int[] ChainPower { get; set; } = { 0, 5, 9, 18, 37, 100, 100, 100, 100, 100, 100};
        public int[] ScorePower { get; set; } = { 1, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096};
        public List<Tuple<int,int>> GetConnected(int col, int row, List<Tuple<int,int>> discovered)
        {
            Tuple<int, int> currentCell = new Tuple<int, int>(col, row);
            if (discovered.Exists(x => x.Equals(currentCell)))
            {
                return discovered;
            }
            discovered.Add(currentCell);
            int color = state.PlayerGameboard[col + row * 6];
            int leftNeighbor = -1;
            if (col > 0)
            {
                leftNeighbor = state.PlayerGameboard[col - 1 + row * 6];
                if (leftNeighbor == color)
                {
                    GetConnected(col - 1, row, discovered);
                }
            }
            int rightNeighbor = -1;
            if (col < 5)
            {
                rightNeighbor = state.PlayerGameboard[col + 1 + row * 6];
                if (rightNeighbor == color)
                {
                    GetConnected(col + 1, row, discovered);
                }
            }
            int topNeighbor = -1;
            if (row > 0)
            {
                topNeighbor = state.PlayerGameboard[col + (row - 1) * 6];
                if (topNeighbor == color)
                {
                    GetConnected(col, row - 1, discovered);
                }
            }
            int bottomNeighbor = -1;
            if (row < 11)
            {
                bottomNeighbor = state.PlayerGameboard[col + (row + 1) * 6];
                if (bottomNeighbor == color)
                {
                    GetConnected(col, row + 1, discovered);
                }
            }
            return discovered;
        }

        public void RemoveConnected(List<Tuple<int,int>> connected)
        {
            List<Tuple<int, int>> nuissanceToRemove = new List<Tuple<int, int>>();
            foreach (var item in connected)
            {
                int col = item.Item1;
                int row = item.Item2;
                state.PlayerGameboard[col + row * 6] = -1;
                if (col > 0)
                {
                    if (state.PlayerGameboard[col-1 + row * 6] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(col - 1, row));
                    }
                }
                if (col < 5)
                {
                    if (state.PlayerGameboard[col + 1 + row * 6] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(col + 1, row));
                    }
                }
                if (row > 0)
                {
                    if (state.PlayerGameboard[col + (row-1) * 6] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(col, row-1));
                    }
                }
                if (row < 11)
                {
                    if (state.PlayerGameboard[col + (row+1) * 6] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(col, row+1));
                    }
                }
            }
            foreach (var item in nuissanceToRemove)
            {
                int col = item.Item1;
                int row = item.Item2;
                state.PlayerGameboard[col + row * 6] = -1;
            }
        }

        public Tuple<int,int> GetHighestColor(int col)
        {
            int row = 0;
            for(row = 0; row < 12; row++)
            {
                if (state.PlayerGameboard[col + row * 6] != -1)
                {
                    break;
                }
            }
            if (row == 12)
            {
                return new Tuple<int, int>(0, -1);
            }
            else {
                return new Tuple<int, int>(11 - row, state.PlayerGameboard[col + row * 6]);
            }
        }

        public List<List<Tuple<int,int>>> GetAllConnected()
        {
            List<List<Tuple<int, int>>> result = new List<List<Tuple<int, int>>>();
            List<Tuple<int, int>> allPositions = new List<Tuple<int, int>>();
            for(int col = 0; col < 6; col++)
            {
                for(int row = 0; row < 12; row++)
                {
                    int color = state.PlayerGameboard[col + row * 6];
                    if ((color != -1)&&(color != 0)) allPositions.Add(new Tuple<int, int>(col, row));
                }
            }
            while (allPositions.Count > 0)
            {
                Tuple<int, int> pos = allPositions.First();
                var connected = GetConnected(pos.Item1, pos.Item2, new List<Tuple<int, int>>());
                result.Add(connected);
                allPositions.RemoveAll(x => connected.Contains(x));
            }
            return result;
        }

        public void ProcessMove(int col)
        {
            DropPair(state.NextPairs[0], col);
            bool changesInState = true;
            int currentChain = 1;
            while (changesInState) {
                changesInState = false;
                var allConnected = GetAllConnected();
                var removable = allConnected.Where(x => x.Count > 3).ToList();
                foreach (var item in removable)
                {
                    var first = item.First();
                    int firstItemCol = first.Item1;
                    int firstItemRow = first.Item2;
                    if (state.PlayerGameboard[firstItemCol + firstItemRow * 6] != 0)
                    {
                        changesInState = true;
                        RemoveConnected(item);
                        state.PlayerScore += 10 * item.Count * ScorePower[currentChain-1];
                    }
                }
                if (changesInState)
                {
                    state.EnemyNuissance += ChainPower[currentChain - 1];
                    ApplyGravity();
                    currentChain++;
                }
            }
            DropNuissance();
        }

        public bool DropNuissance()
        {
            int remainingNuissance = 0;
            int rowsToDrop = Math.DivRem(state.PlayerNuissance, 6,out remainingNuissance);
            state.PlayerNuissance = remainingNuissance;
            for(int col=0; col < 6; col++)
            {
                for(int row = 0; row < rowsToDrop; row++)
                {
                    if (state.PlayerGameboard[col + row * 6] != -1)
                    {
                        return false;
                    }else
                    {
                        state.PlayerGameboard[col + row * 6] = 0;
                    }
                }
            }
            ApplyGravity();
            return true;
        }

        public void ApplyGravity()
        {
            List<int> column = new List<int>();
            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < 12; row++)
                {
                    if (state.PlayerGameboard[col + row * 6] != -1)
                    {
                        column.Add(state.PlayerGameboard[col + row * 6]);
                    }
                }
                int nonFilled = 12 - column.Count;
                int []emptyArray = new int[nonFilled];
                for (int i = 0; i < nonFilled; i++)
                {
                    emptyArray[i] = -1;
                }
                column.InsertRange(0, emptyArray);
                for (int row = 0; row<12; row++)
                {
                    state.PlayerGameboard[col + row * 6] = column[row];
                }
                column.Clear();
            }
        }

        public bool DropPair(Tuple<int, int> pair, int col)
        {
            int i = 0;
            for (i = 0; i < 12; i++)
            {
                if (state.PlayerGameboard[col + i * 6] != -1)
                {
                    break;
                }
            }
            if (i < 2)
            {
                return false;
            }else
            {
                state.PlayerGameboard[col + (i - 1) * 6] = pair.Item2;
                state.PlayerGameboard[col + (i - 2) * 6] = pair.Item1;
                ApplyGravity();
            }
            return true;
        }

        public Tuple<State,Reward> ProcessAction()
        {
            Tuple<State,Reward> output = new Tuple<State, Reward>(new State(), new Reward());

            return output;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Enviroment env = new Enviroment();
        public MainWindow()
        {
            InitializeComponent();
             
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
            env.state.PlayerGameboard = initialStateEmpty;
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
            env.state.PlayerGameboard = initialStateEmpty;
        }

        private void gridScore_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
