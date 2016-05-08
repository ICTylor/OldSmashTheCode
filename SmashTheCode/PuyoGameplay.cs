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
        public float Nuissance { get; set; } = 0.0f;
    }

    public class State
    {
        public Player[] Players { get; set; } = new Player[2];
        public int Turn { get; set; } = 0;
        public PearlPair[] NextPairs { get; set; } = new PearlPair[8];
        public int CurrentPlayer { get; set; } = 0;
        public Random RandomGenerator { get; set; } = new Random(); 

        public State()
        {
            Players[0] = new Player();
            Players[1] = new Player();
            for (int i = 0;i<8; i++)
            {
                NextPairs[i] = GetRandomPair();
            }
            foreach (var player in Players)
            {
                for (int row = 0; row < 12; row++)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        player.Gameboard[row, col] = -1;
                    }
                }
            }
        }
        public PearlPair GetRandomPair()
        {
            int color = (int)Math.Truncate(RandomGenerator.NextDouble() * 5 + 1.0);
            var colorsPair = new Tuple<int, int>(color, color);
            return new PearlPair(colorsPair, 0);
        }
    }
    public class Environment
    {
        public State state { get; set; } = new State();
        public int[] ChainPower { get; set; } = { 0, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        public int[] ColorBonus { get; set; } = { 0, 2, 4, 8, 16 };
        public int[] GroupBonus { get; set; } = { 0, 1, 2, 3, 4, 5, 6, 8 };
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

        public bool ProcessMove(int rotation, int column)
        {
            state.NextPairs[0].Rotation = rotation;
            bool valid = ProcessMove(column);
            state.NextPairs[0].Rotation = 0;
            return valid;
        }

        public bool ProcessMove(int col)
        {
            if (!DropPair(state.NextPairs[0], col)) return false;
            var newNextPairs = state.NextPairs.Skip(1).ToList();
            newNextPairs.Add(state.GetRandomPair());
            state.NextPairs = newNextPairs.ToArray();
            bool changesInState = true;
            int currentChain = 1;
            while (changesInState)
            {
                int stepScore = 0;
                changesInState = false;
                var allConnected = GetAllConnected();
                var removable = allConnected.Where(x => x.Count > 3).ToList();
                int differentColors = -1;
                int groupN = 0;
                List<int> colorsUsed = new List<int>();
                foreach (var item in removable)
                {
                    var first = item.First();
                    int firstItemRow = first.Item1;
                    int firstItemCol = first.Item2;
                    int color = state.Players[state.CurrentPlayer].Gameboard[firstItemRow, firstItemCol];
                    
                    if (color != 0)
                    {
                        if (!colorsUsed.Contains(color))
                        {
                            differentColors++;
                            colorsUsed.Add(color);
                        }
                        groupN += item.Count;
                        changesInState = true;
                        RemoveConnected(item);
                        int bonus = (ChainPower[currentChain - 1] + ColorBonus[differentColors] + GroupBonus[groupN - 4]);
                        bonus = Math.Min(Math.Max(1, bonus),999);
                        stepScore += (10 * item.Count) * bonus;
                        state.Players[state.CurrentPlayer].Score += (10 * item.Count) * bonus;
                    }
                }
                if (changesInState)
                {
                    ApplyGravity();
                    currentChain++;
                }
                //Not like this
                state.Players[state.CurrentPlayer + 1].Nuissance += stepScore/70.0f;
            }
            DropNuissance();
            return true;
        }

        public bool DropNuissance()
        {
            float remainingNuissance = 0;
            int rowsToDrop = (int)Math.Truncate(state.Players[state.CurrentPlayer].Nuissance / 6.0f);
            remainingNuissance = state.Players[state.CurrentPlayer].Nuissance - rowsToDrop * 6;
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
            switch (pair.Rotation)
            {
                case 3:
                    if (row < 2) return false;
                    state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item2;
                    state.Players[state.CurrentPlayer].Gameboard[row - 2, col] = pair.Colors.Item1;
                    break;
                case 0://Careful col limits
                    if (col > 4) return false;
                    state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item2;
                    state.Players[state.CurrentPlayer].Gameboard[row - 1, col + 1] = pair.Colors.Item1;
                    break;
                case 1:
                    if (row < 2) return false;
                    state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item1;
                    state.Players[state.CurrentPlayer].Gameboard[row - 2, col] = pair.Colors.Item2;
                    break;
                case 2://Careful col limits
                    if (col < 1) return false;
                    state.Players[state.CurrentPlayer].Gameboard[row - 1, col] = pair.Colors.Item2;
                    state.Players[state.CurrentPlayer].Gameboard[row - 1, col - 1] = pair.Colors.Item1;
                    break;
                default:
                    break;
            }
            ApplyGravity();
            return true;
        }
    }
}
