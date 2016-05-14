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
                for (int i = 0; i < nuissance; i++)
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
            ReinforcementLearning.Experience experience = new ReinforcementLearning.Experience();
            ReinforcementLearning.RandomAgent randomAgent1 = new ReinforcementLearning.RandomAgent();
            randomAgent1.SetSeed(1);
            ReinforcementLearning.Agent agent1 = new ReinforcementLearning.Agent();
            ReinforcementLearning.Agent agent2 = new ReinforcementLearning.Agent();
            float accumulatedReward = 0f;
            int episodesAccumulated = 0;
            int accumulatedTurns = 0;
            int accumulatedScore = 0;
            while (true) { 
                Environment env = new Environment();
                env.state = new State();
                //OutputEnvironment(env);
                StringBuilder builder = new StringBuilder();
                while (!env.state.Winner.HasValue) {
                    if (env.state.CurrentPlayer == 0) {
                        ReinforcementLearning.Action action1;
                        if (episodesAccumulated < 10)
                        {
                            action1 = randomAgent1.Act(env.state);
                        }
                        else
                        {
                            action1 = agent1.Act(env.state);
                        }
                        env.ProcessMove(action1.Rotation, action1.Column);
                        builder.AppendFormat("Agent {0:D} takes action Rot:{1:D} Col:{2:D}", env.state.CurrentPlayer + 1, action1.Rotation, action1.Column);

                        //InputEnvironment(env);
                    }
                    else{
                        var action2 = agent2.Act(env.state);
                        experience.OldState = env.state;
                        experience.Action = action2;
                        //env.ProcessMove(action2.Rotation, action2.Column);
                        ReinforcementLearning.Reward reward = env.TakeAction(action2);
                        experience.NewState = env.state;
                        experience.Reward = reward;
                        accumulatedReward += reward.R;
                        agent2.LearningAlgorithm.UpdateQ(experience);
                        builder.AppendFormat("Agent {0:D} takes action Rot:{1:D} Col:{2:D}", env.state.CurrentPlayer + 1, action2.Rotation, action2.Column);
                    }
                    env.state.CurrentPlayer = (env.state.CurrentPlayer + 1) % 2;
                    //System.Console.WriteLine(builder.ToString());
                    builder.Clear();
                    //OutputEnvironment(env);
                }
                if (env.state.Winner.Value == 1)
                {
                    experience.NewState = env.state;
                    ReinforcementLearning.Reward reward = new ReinforcementLearning.Reward(1);
                    experience.Reward = reward;
                    accumulatedReward += reward.R;
                }
                accumulatedScore += env.state.Players[1].Score;
                accumulatedTurns += env.state.Turn;
                //System.Console.WriteLine("Player "+(env.state.Winner.Value + 1)+" wins!");
                //System.Console.WriteLine("Accumulated reward: " + accumulatedReward);
                //System.Console.WriteLine("Current score: " + env.state.Players[1].Score);
                //System.Console.WriteLine("Current turn: " + env.state.Turn);
                episodesAccumulated += 1;
                if ((episodesAccumulated % 100)==0)
                {
                    agent1.LearningAlgorithm.Q = (float[,])agent2.LearningAlgorithm.Q.Clone();
                    if((episodesAccumulated % 1000) == 0)
                    {
                        System.Console.WriteLine("Accumulated reward: " + accumulatedReward);
                        System.Console.WriteLine("Mean score: " + (float)accumulatedScore / episodesAccumulated);
                        System.Console.WriteLine("Mean turns: " + (float)accumulatedTurns / episodesAccumulated);
                        System.Console.WriteLine("Exploration factor: " + agent2.LearningAlgorithm.ExplorationFactor);
                        System.Console.WriteLine("Discount factor: " + agent2.LearningAlgorithm.DiscountFactor);
                    }

                    if (agent2.LearningAlgorithm.ExplorationFactor > 0.01)
                    {
                        agent2.LearningAlgorithm.ExplorationFactor *= 0.99f;
                    }
                    agent1.LearningAlgorithm.ExplorationFactor = agent2.LearningAlgorithm.ExplorationFactor;

                    if (agent2.LearningAlgorithm.DiscountFactor < 0.95)
                    {
                        agent2.LearningAlgorithm.DiscountFactor *= 1.01f;
                    }
                }

                //System.Console.ReadLine();
            }
        }
        public static void main(string[] args)
        {
            PuyoConsole pc = new PuyoConsole();
            pc.Gameloop();
        }
    }
}
