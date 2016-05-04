using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmashTheCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashTheCode.Tests
{
    [TestClass()]
    public class EnviromentTests
    {
        [TestMethod()]
        public void GetConnectedTest()
        {
            Enviroment env = new Enviroment();
            env.state = new State();
            env.state.PlayerGameboard = new int[] {
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
            var connected = env.GetConnected(0, 0, new List<Tuple<int, int>>());
            Assert.AreEqual(72, connected.Count);
            env.state.PlayerGameboard = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1, 2,-1,-1,
            -1,-1, 2,-1,-1,-1,
            -1,-1, 2,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1};
            connected = env.GetConnected(0, 0, new List<Tuple<int, int>>());
            Assert.AreEqual(69, connected.Count);
            connected = env.GetConnected(2, 6, new List<Tuple<int, int>>());
            Assert.AreEqual(2, connected.Count);
        }

        [TestMethod()]
        public void ApplyGravityTest()
        {
            Enviroment env = new Enviroment();
            env.state = new State();
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
            int[] initialStateGaps = new int[] {
            -1,-1, 2,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1, 4,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1, 2,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1};
            int[] expectedStateGaps = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1, 4,-1,-1,-1,-1,
            -1, 2,-1,-1,-1,-1,
            -1, 3, 2,-1,-1,-1};
            env.state.PlayerGameboard = initialStateEmpty;
            env.ApplyGravity();
            CollectionAssert.AreEqual(initialStateEmpty, env.state.PlayerGameboard);
            env.state.PlayerGameboard = initialStateGaps;
            env.ApplyGravity();
            CollectionAssert.AreEqual(expectedStateGaps, env.state.PlayerGameboard);
        }

        [TestMethod()]
        public void RemoveConnectedTest()
        {
            Enviroment env = new Enviroment();
            env.state = new State();
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
            var connected = env.GetConnected(0, 0, new List<Tuple<int, int>>());
            env.RemoveConnected(connected);
            CollectionAssert.AreEqual(initialStateEmpty, env.state.PlayerGameboard);
            int[] initialStateConnected = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
             3,-1,-1,-1,-1,-1,
             0, 0,-1,-1,-1,-1,
             2, 0,-1,-1,-1,-1,
             2, 0, 0,-1,-1,-1,
             2, 2, 3,-1,-1,-1};
            int[] expectedStateConnected = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
             3,-1,-1,-1,-1,-1,
            -1, 0,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1, 0,-1,-1,-1,
            -1,-1, 3,-1,-1,-1};
            env.state.PlayerGameboard = initialStateConnected;
            connected = env.GetConnected(0, 11, new List<Tuple<int, int>>());
            env.RemoveConnected(connected);
            CollectionAssert.AreEqual(expectedStateConnected, env.state.PlayerGameboard);
        }

        [TestMethod()]
        public void DropPairTest()
        {
            Enviroment env = new Enviroment();
            env.state = new State();
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
            int[] expectedStateEmpty = new int[] {
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
            -1,-1, 2,-1,-1,-1,
            -1,-1, 2,-1,-1,-1};
            int[] initialStateFull = new int[] {
            -1, 0,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1,
            -1, 3,-1,-1,-1,-1};
            int[] expectedStateFull = initialStateFull;
            env.state.PlayerGameboard = initialStateEmpty;
            bool okMove = env.DropPair(new Tuple<int, int>(2, 2), 2);
            Assert.IsTrue(okMove);
            CollectionAssert.AreEqual(expectedStateEmpty, env.state.PlayerGameboard);
            env.state.PlayerGameboard = initialStateFull;
            okMove = env.DropPair(new Tuple<int, int>(2, 2), 1);
            Assert.IsFalse(okMove);
            CollectionAssert.AreEqual(expectedStateFull, env.state.PlayerGameboard);
        }

        [TestMethod()]
        public void ProcessMoveTest()
        {
            int[] initialFiveCombo = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1, 0, 0, 0, 3,
            -1, 0, 5, 1, 2, 3,
            -1, 2, 5, 1, 2, 0,
            -1, 2, 4, 2, 4, 2,
            -1, 1, 4, 2, 4, 2,
             0, 1, 2, 0, 2, 3,
             4, 5, 2, 3, 2, 3,
             4, 5, 0, 3, 0, 0};
            int[] expectedFiveCombo = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1, 0, 0,-1,-1,
            -1,-1, 5, 1,-1,-1,
            -1,-1, 5, 1,-1,-1,
             4, 5, 4, 3, 4,-1,
             4, 5, 4, 3, 4,-1};
            int col = 0;
            Enviroment env = new Enviroment();
            env.state = new State();
            env.state.PlayerGameboard = initialFiveCombo;
            env.state.NextPairs = new Tuple<int, int>[8];
            env.state.NextPairs[0] = new Tuple<int, int>(1, 1);
            env.state.NextPairs[1] = new Tuple<int, int>(1, 1);
            env.state.NextPairs[2] = new Tuple<int, int>(1, 1);
            env.state.NextPairs[3] = new Tuple<int, int>(5, 5);
            env.state.NextPairs[4] = new Tuple<int, int>(2, 2);
            env.state.NextPairs[5] = new Tuple<int, int>(3, 3);
            env.state.NextPairs[6] = new Tuple<int, int>(3, 3);
            env.state.NextPairs[7] = new Tuple<int, int>(3, 3);
            env.state.PlayerScore = 920;
            env.state.EnemyNuissance = 1;
            env.state.EnemyScore = 880;
            env.ProcessMove(col);
            /*int[,] helper = new int[12, 6];
            for(int row=0;row < 12; row++)
            {
                for (int colTemp = 0; colTemp < 6; colTemp++)
                {
                    helper[row, colTemp] = env.state.PlayerGameboard[colTemp + row * 6];
                }
            }*/

            CollectionAssert.AreEqual(expectedFiveCombo, env.state.PlayerGameboard);
            Assert.AreEqual(70, env.state.EnemyNuissance);
            Assert.AreEqual(5760, env.state.PlayerScore);
        }

        [TestMethod()]
        public void GetHighestColorTest()
        {
            int[] initialTest = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1, 0,-1,-1,
            -1,-1,-1, 1,-1,-1,
            -1,-1, 0, 1,-1,-1,
            -1,-1, 5, 2,-1,-1,
            -1,-1, 5, 2,-1,-1,
             4, 5, 4, 3, 4,-1,
             4, 5, 4, 3, 4,-1};
            Enviroment env = new Enviroment();
            env.state = new State();
            env.state.PlayerGameboard = initialTest;
            Tuple<int, int> tupleCol0 = new Tuple<int, int>(1, 4);
            Tuple<int, int> tupleCol2 = new Tuple<int, int>(4, 0);
            Tuple<int, int> tupleCol5 = new Tuple<int, int>(0, -1);
            Assert.AreEqual(tupleCol0, env.GetHighestColor(0));
            Assert.AreEqual(tupleCol2, env.GetHighestColor(2));
            Assert.AreEqual(tupleCol5, env.GetHighestColor(5));
        }

        [TestMethod()]
        public void GetAllConnectedTest()
        {
            Enviroment env = new Enviroment();
            env.state = new State();
            int[] initialState = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1, 0,-1,-1,
             4,-1,-1, 0,-1,-1,
             4,-1, 2, 2,-1, 3};
            List<List<Tuple<int, int>>> expectedResult = new List<List<Tuple<int, int>>>();
            List<Tuple<int, int>> listFourConnected = new List<Tuple<int, int>>();
            listFourConnected.Add(new Tuple<int, int>(0, 10));
            listFourConnected.Add(new Tuple<int, int>(0, 11));
            List<Tuple<int, int>> listTwoConnected = new List<Tuple<int, int>>();
            listTwoConnected.Add(new Tuple<int, int>(2, 11));
            listTwoConnected.Add(new Tuple<int, int>(3, 11));
            List<Tuple<int, int>> listOneConnected = new List<Tuple<int, int>>();
            listOneConnected.Add(new Tuple<int, int>(5, 11));
            expectedResult.Add(listFourConnected);
            expectedResult.Add(listTwoConnected);
            expectedResult.Add(listOneConnected);
            env.state.PlayerGameboard = initialState;
            List<List<Tuple<int, int>>> actualResult = env.GetAllConnected();
            //CollectionAssert.AreEquivalent(expectedResult, actualResult);
            CollectionAssert.AreEquivalent(expectedResult[0], actualResult[0]);
            CollectionAssert.AreEquivalent(expectedResult[1], actualResult[1]);
            CollectionAssert.AreEquivalent(expectedResult[2], actualResult[2]);
        }

        [TestMethod()]
        public void DropNuissanceTest()
        {
            Enviroment env = new Enviroment();
            env.state = new State();
            int[] initialStateDead = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1, 2,-1,-1,-1,
            -1,-1, 2,-1,-1,-1,
            -1, 1, 0, 3,-1,-1,
            -1, 1, 2, 3, 4,-1,
             0, 0, 2, 0, 4,-1};
            int[] initialStateOk = new int[] {
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
            -1,-1,-1, 2,-1,-1,
             1, 1, 1, 2,-1,-1};
            int[] expectedStateOk = new int[] {
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,
            -1,-1,-1, 0,-1,-1,
             0, 0, 0, 0,-1,-1,
             0, 0, 0, 2, 0, 0,
             1, 1, 1, 2, 0, 0};
            env.state.PlayerGameboard = initialStateDead;
            env.state.PlayerNuissance = 70;
            Assert.IsFalse(env.DropNuissance());
            env.state.PlayerGameboard = initialStateOk;
            env.state.PlayerNuissance = 14;
            Assert.IsTrue(env.DropNuissance());
            Assert.AreEqual(env.state.PlayerNuissance, 2);
            CollectionAssert.AreEqual(expectedStateOk, env.state.PlayerGameboard);
        }
    }
}