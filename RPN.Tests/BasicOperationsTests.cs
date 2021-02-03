using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class BasicOperationsTests : TestBase
    {
        [TestCase("Add", "1 1 + 2 2 2 + + +", 8)]
        [TestCase("Subtract", "1 1 - 2 2 2 - - -", -2)]
        [TestCase("Multiplication", "2 5 * 2 3 * *", 60)]
        [TestCase("Division", "20 2 / 2 /", 5)]
        [TestCase("Remainder", "11 3 %", 2)]
        [TestCase("Percent", "500 10% +", 550)]
        [TestCase("Percent", "500 20% -", 400)]
        [TestCase("Percentage", "1000 670 perc", 67)]
        [TestCase("Floating Percentage", "1000 670 percf", 0.67)]
        [TestCase("Percentage and Round", "1000 675.38 2 percd", 67.54)]
        [TestCase("Difference Minimum", "1000 1000 diff", 0)]
        [TestCase("Difference Maximum Positive", "0 1000 diff", 100)]
        [TestCase("Difference Maximum Negative", "0 -1000 diff", -100)]
        [TestCase("Difference Lesser", "1000 600 diff", -40)]
        [TestCase("Difference Greater", "1000 1400 diff", 40)]
        [TestCase("Floating Difference Minimum", "1000 1000 difff", 0)]
        [TestCase("Floating Difference Maximum Positive", "0 1000 difff", 1)]
        [TestCase("Floating Difference Maximum Negative", "0 -1000 difff", -1)]
        [TestCase("Floating Difference Lesser", "1000 600 difff", -0.4)]
        [TestCase("Floating Difference Greater", "1000 1400 difff", 0.4)]
        [TestCase("Difference and Round", "10 pi 2 diffd", -68.58)]
        public void TestBasicOperations(string testName, string expression, dynamic expectedValue, params object[] objects)
        {
            Test(testName, expression, expectedValue, objects);
        }
    }
}