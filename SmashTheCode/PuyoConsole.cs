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
                int nuissance = env.state.Players[playerIndex].Nuissance;
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
                for (int i = 0; i < Math.Abs(freeNuissance-nuissance); i++)
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

            string result = builder.ToString();
            System.Console.Write(result);
            return result;
        }
    }
}
