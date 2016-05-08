using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashTheCode
{
    public class PuyoConsole
    {
        public string OutputEnvironment(Environment env)
        {
            StringBuilder builder = new StringBuilder();
            for(int playerIndex = 0; playerIndex < 2; playerIndex++)
            {
                int score = env.state.Players[playerIndex].Score;
                int nuissance = (int)Math.Truncate(env.state.Players[playerIndex].Nuissance);
                builder.AppendFormat("PLAYER {0:D} \n", playerIndex + 1);
                builder.AppendFormat("SCORE: {0:D7} \n", score);
                builder.AppendFormat("NUISSANCE: {0:D3} \n", nuissance);
                if (nuissance > 6)
                {
                    nuissance = 6;
                }
                int freeNuissance = 6 - nuissance;
                for (int i=0; i < freeNuissance; i++)
                {
                    builder.AppendFormat("{0} ", "_");
                }
                for (int i = 0; i < Math.Abs(nuissance-freeNuissance); i++)
                {
                    builder.AppendFormat("{0} ", "0");
                }
                builder.Append('\n');
                for (int row = 0; row < 12; row++)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        int color = env.state.Players[playerIndex].Gameboard[row, col];
                        if (color == -1)
                        {
                            builder.AppendFormat("{0} ", ".");
                        }
                        else
                        {
                            builder.AppendFormat("{0:D} ", color);
                        }

                    }
                    builder.Append('\n');
                }
            }
            foreach (var pearlPair in env.state.NextPairs)
            {
                builder.AppendFormat("{0:D} ",pearlPair.Colors.Item1);
            }
            builder.Append("\n");
            foreach (var pearlPair in env.state.NextPairs)
            {
                builder.AppendFormat("{0:D} ", pearlPair.Colors.Item2);
            }
            builder.Append("\n");
            string result = builder.ToString();
            System.Console.Write(result);
            return result;
        }
        public void InputEnvironment(Environment env)
        {
            System.Console.Write("Press rotation(0-3)[0]: ");
            int rotation = -1;
            while (rotation == -1) {
                var rotationKey = System.Console.ReadKey();
                switch (rotationKey.KeyChar)
                {
                    case '0':
                        rotation = 0;
                        break;
                    case '1':
                        rotation = 1;
                        break;
                    case '2':
                        rotation = 2;
                        break;
                    case '3':
                        rotation = 3;
                        break;
                    default:
                        break;
                }
            }
            System.Console.WriteLine("");
            System.Console.Write("Press column(0-5)[0]: ");
            
            int column = -1;
            while (column == -1) {
                var columnKey = System.Console.ReadKey();
                switch (columnKey.KeyChar)
                {
                    case '0':
                        column = 0;
                        break;
                    case '1':
                        column = 1;
                        break;
                    case '2':
                        column = 2;
                        break;
                    case '3':
                        column = 3;
                        break;
                    case '4':
                        column = 4;
                        break;
                    case '5':
                        column = 5;
                        break;
                    default:
                        break;
                }
            }
            System.Console.WriteLine("");
            env.ProcessMove(rotation,column); 
        }
        public void Gameloop()
        {
            Environment env = new Environment();
            env.state = new State();
            OutputEnvironment(env);
            while (true) { 
                InputEnvironment(env);
                OutputEnvironment(env);
            }
        }
        public static void main(string[] args)
        {
            PuyoConsole pc = new PuyoConsole();
            pc.Gameloop();
        }
    }
}
