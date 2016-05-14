using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashTheCode.Tests
{
    [TestClass()]
    public class ActionTests
    {
        [TestMethod()]
        public void FromHashTest()
        {
            var action = new ReinforcementLearning.Action();
            action.FromHash(0);
            Assert.AreEqual(0, action.Column);
            Assert.AreEqual(0, action.Rotation);
            action.FromHash(1);
            Assert.AreEqual(1, action.Column);
            Assert.AreEqual(0, action.Rotation);
            action.FromHash(6);
            Assert.AreEqual(0, action.Column);
            Assert.AreEqual(1, action.Rotation);
            action.FromHash(23);
            Assert.AreEqual(5, action.Column);
            Assert.AreEqual(3, action.Rotation);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            var action = new ReinforcementLearning.Action();
            action.Column = 0;
            action.Rotation = 0;
            Assert.AreEqual(0, action.GetHashCode());
            action.Column = 1;
            action.Rotation = 0;
            Assert.AreEqual(1, action.GetHashCode());
            action.Column = 0;
            action.Rotation = 1;
            Assert.AreEqual(6, action.GetHashCode());
        }
    }
}