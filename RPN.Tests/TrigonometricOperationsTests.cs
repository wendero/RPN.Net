using System;
using NUnit.Framework;

namespace RPN.Tests
{
    public class TrigonometricOperationsTests : TestBase
    {
        [Test]
        public void TestSine()
        {
            Test("Sine", "pi sin", Math.Sin(Math.PI));
        }
        [Test]
        public void TestHyperbolicSine()
        {
            Test("Hyperbolic Sine", "pi sinh", Math.Sinh(Math.PI));
        }
        [Test]
        public void TestArcsine()
        {
            Test("Arcsine", "0.5 asin", Math.Asin(0.5));
        }
        [Test]
        public void TestCosine()
        {
            Test("Cosine", "pi cos", Math.Cos(Math.PI));
        }
        [Test]
        public void TestHyperbolicCosine()
        {
            Test("Hyperbolic Cosine", "pi cosh", Math.Cosh(Math.PI));
        }
        [Test]
        public void TestArccosine()
        {
            Test("Arccosine", "0.5 acos", Math.Acos(0.5));
        }
        [Test]
        public void TestTangent()
        {
            Test("Tangent", "0.5 tan", Math.Tan(0.5));
        }
        [Test]
        public void TestHyperbolicTangent()
        {
            Test("Hyperbolic Tangent", "0.5 tanh", Math.Tanh(0.5));
        }
        [Test]
        public void TestArctangent()
        {
            Test("Arctangent", "0.5 atan", Math.Atan(0.5));
        }
        [Test]
        public void TestArctangentOfXAndY()
        {
            Test("Arctangent Of x And y", "0.6 0.7 atan2", Math.Atan2(0.6, 0.7));
        }
    }
}