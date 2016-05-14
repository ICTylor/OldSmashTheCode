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
            public void FromHash(int hash)
            {
                int column = hash % 6;
                int rotation = (int)Math.Truncate(hash / 6.0f);
                Column = column;
                Rotation = rotation;
            }
            public override int GetHashCode()
            {
                return Column + Rotation * 6;
            }
        }
        public class Experience
        {
            public State OldState { get; set; }
            public Action Action { get; set; }
            public Reward Reward { get; set; }
            public State NewState { get; set; }
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
            Experience[] Experiences { get; set; } = new Experience[100];
            float LearningSpeed { get; set; } = 0.8f;
            public float DiscountFactor { get; set; } = 0.1f;
            public float ExplorationFactor { get; set; } = 0.7f;
            Random Random { get; set; } = new Random();
            int NumberOfStates { get; set; } = (2<<16)-1; 
            public float[,] Q { get; set; } = new float[131072, 24];

            public void UpdateQ(Experience experience)
            {
                float nextMixedReward = -10000.0f;
                int newStateIndex = experience.NewState.GetHashCode() & NumberOfStates;
                int bestA = 0;
                for(int a = 0; a < 24; a++)
                {
                    float tmp = Q[newStateIndex, a];
                    if (tmp > nextMixedReward)
                    {
                        nextMixedReward = tmp;
                        bestA = a;
                    }
                }
                int stateIndex = experience.OldState.GetHashCode() & NumberOfStates;
                int actionIndex = experience.Action.GetHashCode();
                float currentQ = Q[stateIndex, actionIndex];
                Q[stateIndex, actionIndex] += LearningSpeed * (experience.Reward.R + DiscountFactor * nextMixedReward - currentQ);
            }

            public Action GetBestAction(State state)
            {
                var action = new Action();
                if (Random.NextDouble() < ExplorationFactor)
                {
                    action.FromHash(Random.Next(0, 24));
                }
                else
                {
                    int stateIndex = state.GetHashCode() & NumberOfStates; //Representation
                    float maxQ = -10000.0f;
                    int bestA = 0;
                    for (int a = 0; a < 24; a++)
                    {
                        float tmp = Q[stateIndex, a];
                        if (tmp > maxQ)
                        {
                            maxQ = tmp;
                            bestA = a;
                        }
                    }
                    action.FromHash(bestA); //Terrible,  deberia ser estatico ou distinto al menos
                    return action;
                }
                return action;
            }

        }
        public class Agent
        {
            public LearningAlgorithm LearningAlgorithm { get; set; } = new LearningAlgorithm();
            public virtual Action Act(State state)
            {
                Action action = this.LearningAlgorithm.GetBestAction(state);
                return action;
            }

            /*public class Policy
            {
                public float[,] Q { get; set; } = new float[131072,24];
            }*/
        }

        public class RandomAgent : Agent
        {
            Random rand = new Random();
            public override Action Act(State s)
            {
                Action a = new Action();
                a.Column = (int)Math.Truncate(rand.NextDouble() * 6);
                a.Rotation = (int)Math.Truncate(rand.NextDouble() * 4);
                return a;
            }
            public void SetSeed(int seed)
            {
                rand = new Random(seed);
            }
        }
    }
}
