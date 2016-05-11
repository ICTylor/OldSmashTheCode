using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashTheCode
{
    public class ReinforcementLearning
    {
        public class Reward
        {
            public int R { get; set; } = 0;
            public Reward(int r)
            {
                R = r;
            }

        }
        public class Action
        {
            public int Column { get; set; } = 0;
            public int Rotation { get; set; } = 0;
            public Action FromHash(int hash)
            {
                int column = hash % 6;
                int rotation = hash / 4;
                Column = column;
                Rotation = rotation;
                return this;
            }
            public override int GetHashCode()
            {
                return Column + Rotation * 6;
            }
        }
        public class Agent
        {
            public Action Act(State s, Reward r)
            {
                Action action = new Action();
                return action;
            }
            public class Representation
            {
                
                public Representation(State s)
                {
                    s.GetHashCode();
                }
            }

            public class LearningAlgorithm
            {

            }
            public class Policy
            {
                public int[,] Q { get; set; } = new int[100000,24];
            }
        }

        public class RandomAgent : Agent
        {
            Random rand = new Random();
            public Action Act(State s)
            {
                Action a = new Action();
                a.Column = (int)Math.Truncate(rand.NextDouble() * 6);
                a.Rotation = (int)Math.Truncate(rand.NextDouble() * 4);
                return a;
            }
        }
    }
}
