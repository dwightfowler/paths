using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paths.Tests
{
    [TestClass()]
    public class OutputLineTests
    {
        [TestMethod()]
        public void CompareToTest()
        {
            var output1 = new OutputLine() { IsSystem = true, Output = "Howdy", Color = ConsoleColor.DarkGreen };
            Assert.IsTrue(output1.CompareTo(output1) == 0, "An instance is equal to itself");
            Assert.IsTrue(output1.CompareTo(null) > 0, "A null instance should be after any instantiated instance of OutputLine");

            var output2 = new OutputLine() { IsSystem = false, Output = "Howdy", Color = ConsoleColor.DarkGreen };
            Assert.IsTrue(output1.CompareTo(output2) < 0, "System comes before");
            Assert.IsTrue(output2.CompareTo(output1) > 0, "User comes after");

            output2 = new OutputLine() { IsSystem = output1.IsSystem, Output = output1.Output, Color = ConsoleColor.DarkYellow };
            Assert.IsTrue(output1.CompareTo(output2) < 0, "Green comes before Yellow");
            Assert.IsTrue(output2.CompareTo(output1) > 0, "Yellow comes after Green");

            output2 = new OutputLine() { IsSystem = output1.IsSystem, Output = "Howdy!", Color = output1.Color };
            Assert.IsTrue(output1.CompareTo(output2) < 0, "Shorter string comes before!");
            Assert.IsTrue(output2.CompareTo(output1) > 0, "Larger string comes after!");

            output2 = new OutputLine() { IsSystem = output1.IsSystem, Output = "Howd", Color = output1.Color };
            Assert.IsTrue(output1.CompareTo(output2) > 0, "Larger string comes after!");
            Assert.IsTrue(output2.CompareTo(output1) < 0, "Shorter string comes before!");
        }

        public class SubLine : OutputLine
        {
            public bool Check { get; set; }
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsTrue((new OutputLine()).Equals(new OutputLine()), "Empty Is equal to empty");

            var line1 = new OutputLine() { IsSystem = true, Output = "Howdy", Color = ConsoleColor.DarkGreen };
            Assert.IsTrue(line1.Equals(line1), "Is equal to itself");

            Assert.IsFalse(line1.Equals(null), "Is not equal to null");
            Assert.IsFalse(line1.Equals(new Object()), "Must be derived from same type");

            Assert.IsFalse(line1.Equals(new OutputLine() { IsSystem = !line1.IsSystem, Output = line1.Output, Color = line1.Color }), "All fields must be equal");
            Assert.IsFalse(line1.Equals(new OutputLine() { IsSystem = line1.IsSystem, Output = line1.Output+"!", Color = line1.Color }), "All fields must be equal");
            Assert.IsFalse(line1.Equals(new OutputLine() { IsSystem = line1.IsSystem, Output = line1.Output, Color = ConsoleColor.Red }), "All fields must be equal");
            Assert.IsFalse(line1.Equals(new OutputLine() { IsSystem = line1.IsSystem, Output = null, Color = line1.Color }), "All fields must be equal");
            Assert.IsFalse(line1.Equals(new OutputLine() { IsSystem = line1.IsSystem, Output = "", Color = line1.Color }), "All fields must be equal");

            var sub = new SubLine() { IsSystem = line1.IsSystem, Output = line1.Output, Color = line1.Color, Check = false };
            Assert.IsTrue(line1.Equals(sub), "Subclass is equal");
        }
    }
}