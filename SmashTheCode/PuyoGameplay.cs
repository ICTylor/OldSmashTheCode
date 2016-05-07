using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashTheCode
{
    public class PearlPair
    {
        public Tuple<int, int> Colors { get; set; } = new Tuple<int, int>(0, 0);
        public int Rotation { get; set; } = 0;
        public PearlPair(Tuple<int,int> colors, int rotation)
        {
            Colors = colors;
            Rotation = rotation;
        }
    }

    public class Player
    {
        public int[,] Gameboard { get; set; } = new int[12, 6];
        public int Score { get; set; } = 0;
        public int Nuissance { get; set; } = 0;
    }

    public class State
    {
        public Player[] Players { get; set; } = new Player[2];
        public int Turn { get; set; } = 0;
        public PearlPair[] NextPairs { get; set; } = new PearlPair[8];
        public int CurrentPlayer = 0;

        public State()
        {
            Players[0] = new Player();
            Players[1] = new Player();
        }
    }
    public class Environment
    {
        public State state { get; set; } = new State();
        public int[] ChainPower { get; set; } = { 0, 5, 9, 18, 37, 100, 100, 100, 100, 100, 100 };
        public int[] ScorePower { get; set; } = { 1, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        public List<Tuple<int, int>> GetConnected(int row, int col, List<Tuple<int, int>> discovered)
        {
            Tuple<int, int> currentCell = new Tuple<int, int>(row,col);
            if (discovered.Exists(x => x.Equals(currentCell)))
            {
                return discovered;
            }
            discovered.Add(currentCell);
            int color = state.Players[state.CurrentPlayer].Gameboard[row, col];
            int leftNeighbor = -1;
            if (col > 0)
            {
                leftNeighbor = state.Players[state.CurrentPlayer].Gameboard[row, col-1];
                if (leftNeighbor == color)
                {
                    GetConnected(row, col - 1, discovered);
                }
            }
            int rightNeighbor = -1;
            if (col < 5)
            {
                rightNeighbor = state.Players[state.CurrentPlayer].Gameboard[row, col+1];
                if (rightNeighbor == color)
                {
                    GetConnected(row, col + 1, discovered);
                }
            }
            int topNeighbor = -1;
            if (row > 0)
            {
                topNeighbor = state.Players[state.CurrentPlayer].Gameboard[row-1, col];
                if (topNeighbor == color)
                {
                    GetConnected(row - 1, col, discovered);
                }
            }
            int bottomNeighbor = -1;
            if (row < 11)
            {
                bottomNeighbor = state.Players[state.CurrentPlayer].Gameboard[row+1, col];
                if (bottomNeighbor == color)
                {
                    GetConnected(row + 1, col, discovered);
                }
            }
            return discovered;
        }

        public void RemoveConnected(List<Tuple<int, int>> connected)
        {
            List<Tuple<int, int>> nuissanceToRemove = new List<Tuple<int, int>>();
            foreach (var item in connected)
            {
                int row = item.Item1;
                int col = item.Item2;
                state.Players[state.CurrentPlayer].Gameboard[row, col] = -1;
                if (col > 0)
                {
                    if (state.Players[state.CurrentPlayer].Gameboard[row, col-1] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(row, col - 1));
                    }
                }
                if (col < 5)
                {
                    if (state.Players[state.CurrentPlayer].Gameboard[row, col+1] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(row, col + 1));
                    }
                }
                if (row > 0)
                {
                    if (state.Players[state.CurrentPlayer].Gameboard[row-1, col] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(row - 1,col));
                    }
                }
                if (row < 11)
                {
                    if (state.Players[state.CurrentPlayer].Gameboard[row+1, col] == 0)
                    {
                        nuissanceToRemove.Add(new Tuple<int, int>(row + 1,col));
                    }
                }
            }
            foreach (var item in nuissanceToRemove)
            {
                int row = item.Item1;
                int col = item.Item2;
                state.Players[state.CurrentPlayer].Gameboard[row, col] = -1;
            }
        }

        public Tuple<int, int> GetHighestColor(int col)
        {
            int row = 0;
            for (row = 0; row < 12; row++)
            {
                if (state.Players[state.CurrentPlayer].Gameboard[row, col] != -1)
                {
                    break;
                }
            }
            if (row == 12)
            {
                return new Tuple<int, int>(0, -1);
            }
            else
            {
                return new Tuple<int, int>(11 - row, state.Players[state.CurrentPlayer].Gameboard[row, col]);
            }
        }

        public List<List<Tuple<int, int>>> GetAllConnected()
        {
            List<List<Tuple<int, int>>> result = new List<List<Tuple<int, int>>>();
            List<Tuple<int, int>> allPositions = new List<Tuple<int, int>>();
            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < 12; row++)
                {
                    int color = state.Players[state.CurrentPlayer].Gameboard[row, col];
                    if ((color != -1) && (color != 0)) allPositions.Add(new Tuple<int, int>(row,col));
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
            while (changesInState)
            {
                changesInState = false;
                var allConnected = GetAllConnected();
                var removable = allConnected.Where(x => x.Count > 3).ToList();
                foreach (var item in removable)
                {
                    var first = item.First();
                    int firstItemRow = first.Item1;
                    int firstItemCol = first.Item2;
                    if (state.Players[state.CurrentPlayer].Gameboard[firstItemRow,firstItemCol] != 0)
                    {
                        changesInState = true;
                        RemoveConnected(item);
                        state.Players[state.CurrentPlayer].Score += 10 * item.Count * ScorePower[currentChain - 1];
                    }
                }
                if (changesInState)
                {
                    state.Players[state.CurrentPlayer+1].Nuissance += ChainPower[currentChain - 1];
                    ApplyGravity();
                    currentChain++;
                }
            }
            DropNuissance();
        }

        public bool DropNuissance()
        {
            int remainingNuissance = 0;
            int rowsToDrop = Math.DivRem(state.Players[state.CurrentPlayer].Nuissance, 6, out remainingNuissance);
            state.Players[state.CurrentPlayer].Nuissance = remainingNuissance;
            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < rowsToDrop; row++)
                {
                    if (state.Players[state.CurrentPlayer].Gameboard[row, col] != -1)
                    {
                        return false;
                    }
                    else
                    {
                        state.Players[state.CurrentPlayer].Gameboard[row, col] = 0;
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
                    if (state.Players[state.CurrentPlayer].Gameboard[row, col] != -1)
                    {
                        column.Add(state.Players[state.CurrentPlayer].Gameboard[row, col]);
                    }
                }
                int nonFilled = 12 - column.Count;
                int[] emptyArray = new int[nonFilled];
                for (int i = 0; i < nonFilled; i++)
                {
                    emptyArray[i] = -1;
                }
                column.InsertRange(0, emptyArray);
                for (int row = 0; row < 12; row++)
                {
                    state.Players[state.CurrentPlayer].Gameboard[row, col] = column[row];
                }
                column.Clear();
            }
        }

        public bool DropPair(PearlPair pair, int col)
        {
            int row = 0;
            for (row = 0; row < 12; row++)
            {
                if (state.Players[state.CurrentPlayer].Gameboard[row, col] != -1)
                {
                    break;
                }
            }
            if (row < 2)
            {
                return false;
            }
            else
            {
                switch (pair.Rotation)
                {
                    case 0:
                        state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item2;
                        state.Players[state.CurrentPlayer].Gameboard[row - 2, col] = pair.Colors.Item1;
                        break;
                    case 1:
                        state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item2;
                        state.Players[state.CurrentPlayer].Gameboard[row - 1, col+1] = pair.Colors.Item1;
                        break;
                    case 2:
                        state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item1;
                        state.Players[state.CurrentPlayer].Gameboard[row - 2, col] = pair.Colors.Item2;
                        break;
                    case 3:
                        state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item2;
                        state.Players[state.CurrentPlayer].Gameboard[row - 1, col - 1] = pair.Colors.Item1;
                        break;
                    default:
                        break;
                }
                ApplyGravity();
            }
            return true;
        }
    }
}
