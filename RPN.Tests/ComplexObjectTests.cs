using System.Collections.Generic;
using NUnit.Framework;

namespace RPN.Tests
{
    public class ComplexObjectTests : TestBase
    {
        private Mock mock1 = new Mock() { Num = 5, Label = "bazinga", SubMock = new Mock() { Num = 11 } };
        private Mock mock2 = new Mock() { Num = 12, Label = "TAPANG" };
        private List<Mock> mockList;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            mock1.Mocks.Add("Last", new Mock() { Num = 3 });
            mock2.Mocks.Add("Last", new Mock() { Num = 7 });
            mockList = new List<Mock>() { mock1, mock2 };
        }

        [Test]
        public void TestSingleParameterFromSingleObject()
        {
            Test("Single Parameter", "$0.Num 1 +", 3, new { Num = 2 });
            Test("Single Parameter from Object", "$0.Num", 5, new Mock { Num = 5 });
            Test("Single Parameter from Multiple Objects", "$0.Num $1.Num +", 8, new Mock { Num = 5 }, new Mock { Num = 3 });
        }
        [Test]
        public void TestMultipleParametersFromMultipleObjects()
        {
            Test("Multiple Parameters", "$0.Num $1.Num min", 2, new { Num = 2 }, new { Num = 5 });
        }
        [Test]
        public void TestMultipleParametersFromMultipleValue()
        {
            Test("Multiple Parameters", "$0 $1 +", 5, 2, 3);
        }
        [Test]
        public void TestArrays()
        {
            Test("Array Item", "$0[0] $0[1] +", 5, new int[] { 2, 3 });
            Test("Array Property", "$0.Length", 4, new int[] { 2, 3, 4, 5 });
            Test("Array of Objects", "$0[0].Num $0[1].Num $1 + +", 8, new Mock[] { new Mock() { Num = 3 }, new Mock() { Num = 2 } }, 3);
        }
        [Test]
        public void TestOtherCollections()
        {
            Test("List of Objects", "$0[0].Num $0[1].Num $1 + +", 8, new List<Mock> { new Mock() { Num = 3 }, new Mock() { Num = 2 } }, 3);
            Test("Dictionary", "$0[abc] $0[def] +", 5, new Dictionary<string, int> { { "abc", 3 }, { "def", 2 } });
            Test("Dictionary of Objects", "$0[abc].Num $0[def].Num +", 5, new Dictionary<string, Mock> { { "abc", new Mock() { Num = 3 } }, { "def", new Mock() { Num = 2 } } });


        }
        [Test]
        public void TestComplexObject()
        {
            Test("Complex", "$1.Mocks.Last.Num $0.Mocks.Last.Num +", 10, mock1, mock2);
            Test("Collection", "$0", mockList, mockList);
        }
        [Test]
        public void TestSpread()
        {
            Test("Spread", "$0 Num ... sum", 17, mockList);
        }
        private class Mock
        {
            public int Num { get; set; }
            public string Label { get; set; }
            public Mock SubMock { get; set; }
            public Dictionary<string, Mock> Mocks { get; set; } = new Dictionary<string, Mock>();
        }
    }
}