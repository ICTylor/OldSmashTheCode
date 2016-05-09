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
    public class PuyoConsoleTests
    {
        [TestMethod()]
        public void OutputEnvironmentTest()
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
            int[,] initialState2 = new int[12, 6] {
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
            {  2,-1,-1, 0,-1,-1 },
            {  2,-1, 2, 2,-1, 3 } };
            env.state.Players[env.state.CurrentPlayer].Gameboard = initialState;
            env.state.Players[env.state.CurrentPlayer+1].Gameboard = initialState2;
            env.state.Players[env.state.CurrentPlayer].Nuissance = 2;
            env.state.Players[env.state.CurrentPlayer+1].Nuissance = 14;
            env.state.Players[env.state.CurrentPlayer].Score = 0;
            env.state.Players[env.state.CurrentPlayer+1].Score = 200;
            StringBuilder builderTopNextPairs = new StringBuilder();
            StringBuilder builderBotNextPairs = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                builderTopNextPairs.AppendFormat("{0:D} ",env.state.NextPairs[i].Colors.Item1);
                builderBotNextPairs.AppendFormat("{0:D} ",env.state.NextPairs[i].Colors.Item2);
            }
            builderTopNextPairs.Append("\n");
            builderBotNextPairs.Append("\n");

            var puyoConsole = new PuyoConsole();
            string result = puyoConsole.OutputEnvironment(env);
            string expected =
                "PLAYER 1 \n" +
                "SCORE: 0000000 \n" +
                "NUISSANCE: 002 \n" +
                "_ _ _ _ 0 0 \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . 0 . . \n" +
                "4 . . 0 . . \n" +
                "4 . 2 2 . 3 \n" +
                "PLAYER 2 \n" +
                "SCORE: 0000200 \n" +
                "NUISSANCE: 014 \n" +
                "0 0 0 0 0 0 \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . . . . \n" +
                ". . . 0 . . \n" +
                "2 . . 0 . . \n" +
                "2 . 2 2 . 3 \n" +
                builderTopNextPairs.ToString() +
                builderBotNextPairs.ToString()
            ;
            Assert.AreEqual(expected, result);
        }
    }
}