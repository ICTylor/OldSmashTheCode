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
    public class EnvironmentTests
    {
        [TestMethod()]
        public void GetConnectedTest()
        {
            Environment env = new Environment();
            env.state = new State();
            env.state.Players[env.state.CurrentPlayer].Gameboard = new int[12,6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 } };
            var connected = env.GetConnected(0, 0, new List<Tuple<int, int>>());
            Assert.AreEqual(72, connected.Count);
            env.state.Players[env.state.CurrentPlayer].Gameboard = new int[12,6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1, 2,-1,-1 },
            { -1,-1, 2,-1,-1,-1 },
            { -1,-1, 2,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 } };
            connected = env.GetConnected(0, 0, new List<Tuple<int, int>>());
            Assert.AreEqual(69, connected.Count);
            connected = env.GetConnected(6, 2, new List<Tuple<int, int>>());
            Assert.AreEqual(2, connected.Count);
        }

        [TestMethod()]
        public void ApplyGravityTest()
        {
            Environment env = new Environment();
            env.state = new State();
            int[,] initialStateEmpty = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 } };
            int[,] initialStateGaps = new int[12, 6] {
            { -1,-1, 2,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1, 4,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1, 2,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 } };
            int[,] expectedStateGaps = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1, 4,-1,-1,-1,-1 },
            { -1, 2,-1,-1,-1,-1 },
            { -1, 3, 2,-1,-1,-1 } };
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateEmpty;
            env.ApplyGravity();
            CollectionAssert.AreEqual(initialStateEmpty, env.state.Players[env.state.CurrentPlayer].Gameboard);
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateGaps;
            env.ApplyGravity();
            CollectionAssert.AreEqual(expectedStateGaps, env.state.Players[env.state.CurrentPlayer].Gameboard);
        }

        [TestMethod()]
        public void RemoveConnectedTest()
        {
            Environment env = new Environment();
            env.state = new State();
            int[,] initialStateEmpty = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 } };
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateEmpty;
            var connected = env.GetConnected(0, 0, new List<Tuple<int, int>>());
            env.RemoveConnected(connected);
            CollectionAssert.AreEqual(initialStateEmpty, env.state.Players[env.state.CurrentPlayer].Gameboard);
            int[,] initialStateConnected = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            {  3,-1,-1,-1,-1,-1 },
            {  0, 0,-1,-1,-1,-1 },
            {  2, 0,-1,-1,-1,-1 },
            {  2, 0, 0,-1,-1,-1 },
            {  2, 2, 3,-1,-1,-1 } };
            int[,] expectedStateConnected = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            {  3,-1,-1,-1,-1,-1 },
            { -1, 0,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1, 0,-1,-1,-1 },
            { -1,-1, 3,-1,-1,-1 } };
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateConnected;
            connected = env.GetConnected(11, 0, new List<Tuple<int, int>>());
            env.RemoveConnected(connected);
            CollectionAssert.AreEqual(expectedStateConnected, env.state.Players[env.state.CurrentPlayer].Gameboard);
        }

        [TestMethod()]
        public void DropPairTest()
        {
            Environment env = new Environment();
            env.state = new State();
            int[,] initialStateEmpty = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 } };
            int[,] expectedStateEmpty = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1, 2,-1,-1,-1 },
            { -1,-1, 2,-1,-1,-1 } };
            int[,] initialStateFull = new int[12, 6] {
            { -1, 0,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 },
            { -1, 3,-1,-1,-1,-1 } };
            int[,] expectedStateFull = initialStateFull;
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateEmpty;
            bool okMove = env.DropPair(new PearlPair(new Tuple<int, int>(2, 2),3), 2);
            Assert.IsTrue(okMove);
            CollectionAssert.AreEqual(expectedStateEmpty, env.state.Players[env.state.CurrentPlayer].Gameboard);
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateFull;
            okMove = env.DropPair(new PearlPair(new Tuple<int, int>(2, 2),3), 1);
            Assert.IsFalse(okMove);
            CollectionAssert.AreEqual(expectedStateFull, env.state.Players[env.state.CurrentPlayer].Gameboard);
        }

        [TestMethod()]
        public void ProcessMoveTest()
        {
            int[,] initialFiveCombo = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1, 0, 0, 0, 3 },
            { -1, 0, 5, 1, 2, 3 },
            { -1, 2, 5, 1, 2, 0 },
            { -1, 2, 4, 2, 4, 2 },
            { -1, 1, 4, 2, 4, 2 },
            {  0, 1, 2, 0, 2, 3 },
            {  4, 5, 2, 3, 2, 3 },
            {  4, 5, 0, 3, 0, 0 } };
            int[,] expectedFiveCombo = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1, 0, 0,-1,-1 },
            { -1,-1, 5, 1,-1,-1 },
            { -1,-1, 5, 1,-1,-1 },
            {  4, 5, 4, 3, 4,-1 },
            {  4, 5, 4, 3, 4,-1 } };
            int col = 0;
            Environment env = new Environment();
            env.state = new State();
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialFiveCombo;
            env.state.NextPairs = new PearlPair[8];
            env.state.NextPairs[0] = new PearlPair(new Tuple<int, int>(1, 1),0);
            env.state.NextPairs[1] = new PearlPair(new Tuple<int, int>(1, 1),0);
            env.state.NextPairs[2] = new PearlPair(new Tuple<int, int>(1, 1),0);
            env.state.NextPairs[3] = new PearlPair(new Tuple<int, int>(5, 5),0);
            env.state.NextPairs[4] = new PearlPair(new Tuple<int, int>(2, 2),0);
            env.state.NextPairs[5] = new PearlPair(new Tuple<int, int>(3, 3),0);
            env.state.NextPairs[6] = new PearlPair(new Tuple<int, int>(3, 3),0);
            env.state.NextPairs[7] = new PearlPair(new Tuple<int, int>(3, 3),0);
            env.state.Players[env.state.CurrentPlayer].Score = 920;
            env.state.Players[env.state.CurrentPlayer + 1].Nuissance = 1;
            env.state.Players[env.state.CurrentPlayer + 1].Score = 880;
            env.ProcessMove(3,col);
            CollectionAssert.AreEqual(expectedFiveCombo, env.state.Players[env.state.CurrentPlayer].Gameboard);
            Assert.AreEqual(70, (int)Math.Truncate(env.state.Players[env.state.CurrentPlayer+1].Nuissance));
            Assert.AreEqual(5760, env.state.Players[env.state.CurrentPlayer].Score);
        }

        [TestMethod()]
        public void GetHighestColorTest()
        {
            int[,] initialTest = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1, 0,-1,-1 },
            { -1,-1,-1, 1,-1,-1 },
            { -1,-1, 0, 1,-1,-1 },
            { -1,-1, 5, 2,-1,-1 },
            { -1,-1, 5, 2,-1,-1 },
            {  4, 5, 4, 3, 4,-1 },
            {  4, 5, 4, 3, 4,-1 } };
            Environment env = new Environment();
            env.state = new State();
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialTest;
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
            Environment env = new Environment();
            env.state = new State();
            int[,] initialState = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1, 0,-1,-1 },
            {  4,-1,-1, 0,-1,-1 },
            {  4,-1, 2, 2,-1, 3 } };
            List<List<Tuple<int, int>>> expectedResult = new List<List<Tuple<int, int>>>();
            List<Tuple<int, int>> listFourConnected = new List<Tuple<int, int>>();
            listFourConnected.Add(new Tuple<int, int>(10, 0));
            listFourConnected.Add(new Tuple<int, int>(11, 0));
            List<Tuple<int, int>> listTwoConnected = new List<Tuple<int, int>>();
            listTwoConnected.Add(new Tuple<int, int>(11, 2));
            listTwoConnected.Add(new Tuple<int, int>(11, 3));
            List<Tuple<int, int>> listOneConnected = new List<Tuple<int, int>>();
            listOneConnected.Add(new Tuple<int, int>(11, 5));
            expectedResult.Add(listFourConnected);
            expectedResult.Add(listTwoConnected);
            expectedResult.Add(listOneConnected);
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialState;
            List<List<Tuple<int, int>>> actualResult = env.GetAllConnected();
            //CollectionAssert.AreEquivalent(expectedResult, actualResult);
            CollectionAssert.AreEquivalent(expectedResult[0], actualResult[0]);
            CollectionAssert.AreEquivalent(expectedResult[1], actualResult[1]);
            CollectionAssert.AreEquivalent(expectedResult[2], actualResult[2]);
        }

        [TestMethod()]
        public void DropNuissanceTest()
        {
            Environment env = new Environment();
            env.state = new State();
            int[,] initialStateDead = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1, 2,-1,-1,-1 },
            { -1,-1, 2,-1,-1,-1 },
            { -1,-1, 0, 3,-1,-1 },
            { -1,-1, 2, 3, 4,-1 },
            {  0, 0, 2, 0, 4,-1 } };
            int[,] initialStateOk = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1, 2,-1,-1 },
            {  1, 1, 1, 2,-1,-1 } };
            int[,] expectedStateOk = new int[12, 6] {
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1 },
            { -1,-1,-1, 0,-1,-1 },
            {  0, 0, 0, 0,-1,-1 },
            {  0, 0, 0, 2, 0, 0 },
            {  1, 1, 1, 2, 0, 0 } };
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateDead;
            env.state.Players[env.state.CurrentPlayer].Nuissance = 70;
            Assert.IsFalse(env.DropNuissance());
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialStateOk;
            env.state.Players[env.state.CurrentPlayer].Nuissance = 14;
            Assert.IsTrue(env.DropNuissance());
            Assert.AreEqual(env.state.Players[env.state.CurrentPlayer].Nuissance, 2);
            CollectionAssert.AreEqual(expectedStateOk, env.state.Players[env.state.CurrentPlayer].Gameboard);
        }
    }
}