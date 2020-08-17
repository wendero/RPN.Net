using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class MathOperationsTests : TestBase
    {
        [Test]
        public void TestPi()
        {
            Test("PI", "pi", Math.PI);
        }
        [Test]
        public void TestExponentiation()
        {
            Test("Exponentiation", "3 2 pow", Math.Pow(3, 2));
        }
        [Test]
        public void TestPowersOf10()
        {
            Test("Powers of 10", "3 3 E", 3000);
        }
        [Test]
        public void TestAbsolute()
        {
            Test("Abs", "-1 abs", 1);
        }
        [Test]
        public void TestSignChange()
        {
            Test("Sign Change", "5 +-", -5);
        }
        [Test]
        public void TestRound()
        {
            Test("Round", "pi 2 round", 3.14);
        }
        [Test]
        public void TestCeiling()
        {
            Test("Ceiling", "3.12534 ceiling", Math.Ceiling(3.12534));
        }
        [Test]
        public void TestFloor()
        {
            Test("Floor", "pi floor", Math.Floor(Math.PI));
        }
        [Test]
        public void TestRandom()
        {
            Test("Random", "rnd rnd 0 >= swap 1 <= &", true);
        }
        [Test]
        public void TestBetween()
        {
            Test("Between", "100 200 btw dup 100 >= swap 200 <= &", true);
        }
        [Test]
        public void TestTruncate()
        {
            Test("Truncate", "pi truncate", Math.Truncate(Math.PI));
        }
        [Test]
        public void TestSquareRoot()
        {
            Test("Sqrt", "230 230 * sqrt", 230);
        }
        [Test]
        public void TestSum()
        {
            Test("Sum", "1 2 3 5 8 13 21 sum", 53);
        }
        [Test]
        public void TestSumX()
        {
            Test("Sum x items", "1 2 3 5 8 13 21 3 sumx", 42);
        }
        [Test]
        public void TestLogBaseB()
        {
            Test("Log base b", "100 10 logb", 2);
        }
        [Test]
        public void TestLogBase10()
        {
            Test("Lob base 10", "pi log10", Math.Log10(Math.PI));
        }
        [Test]
        public void TestLog()
        {
            Test("Log", "pi log", Math.Log(Math.PI));
        }
        [Test]
        public void TestEulerExp()
        {
            Test("Exp", "pi exp", Math.Exp(Math.PI));
        }
    }
}