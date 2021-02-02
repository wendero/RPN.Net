using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace RPN.Examples
{
    class Program
    {
        private static int _total = 0;
        private static int _success = 0;
        static void Main(string[] args)
        {
            //var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddConsole().SetMinimumLevel(LogLevel.Debug);
            //});
            //ILogger logger = loggerFactory.CreateLogger<Program>();

            //logger.LogError("This is log message.");
            //Thread.Sleep(1000);
            //return;

            if (args.Length == 0)
            {
                ShowExamples();
                return;
            }
            var expression = string.Join(' ', args);

            Console.WriteLine(RPN.Evaluate(expression, null));
        }
        static void ShowExamples()
        {
            Mock mock1 = new Mock() { Num = 5, Label = "bazinga", SubMock = new Mock() { Num = 11 } };
            mock1.Mocks.Add("Last", new Mock() { Num = 3 });

            Mock mock2 = new Mock() { Num = 12, Label = "TAPANG" };
            mock2.Mocks.Add("Last", new Mock() { Num = 7 });

            List<Mock> mockList = new List<Mock>() { mock1, mock2 };

            /******************************************/
            /** BASIC OPERATIONS
            /******************************************/
            Test("Add", "1 1 + 2 2 2 + + +", 8);
            Test("Subtract", "1 1 - 2 2 2 - - -", -2);
            Test("Multiplication", "2 5 * 2 3 * *", 60);
            Test("Division", "20 2 / 2 /", 5);
            Test("Remainder", "11 3 %", 2);
            Test("Percent", "500 10% +", 550);
            Test("Percent", "500 20% -", 400);
            Test("Percentage", "1000 670 perc", 67);
            Test("Floating Percentage", "1000 670 percf", 0.67);
            Test("Percentage and Round", "1000 675.38 2 percd", 67.54);


            /******************************************/
            /** MATH OPERATIONS
            /******************************************/
            Test("PI", "pi", Math.PI);
            Test("Exponentiation", "3 2 pow", Math.Pow(3, 2));
            Test("Powers of 10", "3 3 E", 3000);
            Test("Abs", "-1 abs", 1);
            Test("Sign Change", "5 +-", -5);
            Test("Round", "pi 2 round", 3.14);
            Test("Ceiling", "3.12534 ceiling", Math.Ceiling(3.12534));
            Test("Floor", "pi floor", Math.Floor(Math.PI));
            Test("Random", "rnd rnd 0 >= swap 1 <= &", true);
            Test("Between", "100 200 btw dup 100 >= swap 200 <= &", true);
            Test("Truncate", "pi truncate", Math.Truncate(Math.PI));
            Test("Sqrt", "230 230 * sqrt", 230);
            Test("Sum", "1 2 3 5 8 13 21 sum", 53);
            Test("Sum x items", "1 2 3 5 8 13 21 3 sumx", 42);
            Test("Log base b", "100 10 logb", 2);
            Test("Lob base 10", "pi log10", Math.Log10(Math.PI));
            Test("Log", "pi log", Math.Log(Math.PI));
            Test("Exp", "pi exp", Math.Exp(Math.PI));

            /******************************************/
            /** LOGICAL OPERATIONS
            /******************************************/
            Test("Greater than", "5 3 >", true);
            Test("Less than", "5 3 <", false);
            Test("Equals Integer", "3 3 =", true);
            Test("Equals Decimal True", "3.5 3.5 =", true);
            Test("Equals Decimal False", "3.5 3.6 =", false);
            Test("Equals Boolean", "true true =", true);
            Test("Equals Text", "`abc` `abc` =", true);
            Test("Different False", "3 3 !=", false);
            Test("Different True", "3 4 !=", true);
            Test("Different <>", "3 3 <>", false);
            Test("Logical And", "true true &", true);
            Test("Logical Or", "true false |", true);
            Test("Logical Not", "true !", false);
            Test("Greater or equals to", "5 3 >= 3 3 >= &", true);
            Test("Less or equals to", "3 5 <= 3 3 <= &", true);

            /******************************************/
            /** STRING OPERATIONS
            /******************************************/
            Test("Uppercase", "bazinga ucase", "BAZINGA");
            Test("Lowercase", "BAZINGA lcase", "bazinga");
            Test("String Format", "hel wor lo ld `{0}{2} {1}{3}` strfmt", "hello world");
            Test("Date Default Format", "2020 12 31 15 16 17 6 date default datefmt", "2020-12-31 15:16:17");
            Test("Date Custom Format", "2020 12 31 15 16 17 6 date `dd/MM/yyyy HH:mm:ss` datefmt", "31/12/2020 15:16:17");
            Test("Date System Format", "2018 12 31 23 59 58 123 7 date str", new DateTime(2018, 12, 31, 23, 59, 58).ToString());
            Test("Parse JSON", "`{\"Name\": \"Bazinga\"}` parse $0.Name", "Bazinga");
            Test("Parse JSON", "`{\"Key\":123}` parse $0.Key", 123);
            Test("Stringify", "$0 stringify", JsonSerializer.Serialize(new { Name = "Bazinga" }), new { Name = "Bazinga" });

            var builder = new StringBuilder();
            builder.AppendLine("line 1");
            builder.AppendLine("line 2");
            builder.AppendLine("line 3");
            builder.AppendLine("line 4");
            builder.AppendLine("line 5");

            Test("Test Line Count", "$0 lines", 6, builder.ToString());
            Test("Test Line Get", "$0 3 line", "line 3", builder.ToString());

            /******************************************/
            /** DATETIME OPERATIONS
            /******************************************/
            Test("Date Year", "2020 1 date default datefmt", "2020-01-01 00:00:00", null);
            Test("Date Year and Month", "2020 12 2 date default datefmt", "2020-12-01 00:00:00", null);
            Test("Date Simple", "2020 12 31 3 date default datefmt", "2020-12-31 00:00:00", null);
            Test("Date", "2020 12 31 15 16 17 6 date", new DateTime(2020, 12, 31, 15, 16, 17), null);
            Test("TimeSpan 3 days", "3 days", new TimeSpan(3, 0, 0, 0), null);
            Test("TimeSpan 5 hours", "5 hours", new TimeSpan(0, 5, 0, 0), null);
            Test("TimeSpan 7 minutes", "7 minutes", new TimeSpan(0, 0, 7, 0), null);
            Test("TimeSpan 9 seconds", "9 seconds", new TimeSpan(0, 0, 0, 9), null);
            Test("TimeSpan full", "3 5 7 9 ts", new TimeSpan(3, 5, 7, 9), null);
            Test("Add 1 hour", "2020 1 date 1 hour +", new DateTime(2020, 1, 1, 1, 0, 0), null);
            Test("Add 1 day", "2020 1 date 1 day +", new DateTime(2020, 1, 2), null);
            Test("Subtract 1 hour", "2020 1 date 1 hour -", new DateTime(2019, 12, 31, 23, 0, 0), null);
            Test("Subtract 1 day", "2020 1 date 1 day -", new DateTime(2019, 12, 31), null);
            Test("Add 1 month", "2020 1 date 1 month +", new DateTime(2020, 2, 1), null);
            Test("Add 1 year", "2020 1 date 1 year +", new DateTime(2021, 1, 1), null);
            Test("Add 2 months", "2020 1 date 2 months +", new DateTime(2020, 3, 1), null);
            Test("Add 2 years", "2020 1 date 2 years +", new DateTime(2022, 1, 1), null);
            Test("Subtract 1 month", "2020 1 date 1 month -", new DateTime(2019, 12, 1), null);
            Test("Subtract 1 year", "2020 1 date 1 year -", new DateTime(2019, 1, 1), null);

            /******************************************/
            /** TRIGONOMETRIC OPERATIONS
            /******************************************/
            Test("Sine", "pi sin", Math.Sin(Math.PI));
            Test("Hyperbolic Sine", "pi sinh", Math.Sinh(Math.PI));
            Test("Arcsine", "0.5 asin", Math.Asin(0.5));
            Test("Cosine", "pi cos", Math.Cos(Math.PI));
            Test("Hyperbolic Cosine", "pi cosh", Math.Cosh(Math.PI));
            Test("Arccosine", "0.5 acos", Math.Acos(0.5));
            Test("Tangent", "0.5 tan", Math.Tan(0.5));
            Test("Hyperbolic Tangent", "0.5 tanh", Math.Tanh(0.5));
            Test("Arctangent", "0.5 atan", Math.Atan(0.5));
            Test("Arctangent Of x And y", "0.6 0.7 atan2", Math.Atan2(0.6, 0.7));

            /******************************************/
            /** OTHER OPERATIONS
            /******************************************/
            Test("Increment 1", "5 ++", 6);
            Test("Decrement 1", "5 --", 4);
            Test("Minimum", "10 20 30 40 50 5 3 2 min", 2);
            Test("Maximum", "10 20 30 40 50 5 3 2 max", 50);

            /******************************************/
            /** CONTROL OPERATIONS
            /******************************************/
            Test("Stack", "5 2 3 stack", "[5,2,3]");
            Test("Swap", "1 5 swap stack", "[5,1]");
            Test("Rotate", "1 2 3 rot stack", "[3,1,2]");
            Test("Duplicate", "100 dup +", 200);
            Test("Pop (discard entry)", "3 5 pop", 3);
            Test("Clear stack", "10 20 30 40 50 5 3 2 clr 1", 1);
            Test("Pop X", "1 2 4 8 50 16 32 64 3 popx sum", 127);
            Test("Data push", "5 dpush data", "[5]");
            Test("Data pop", "5 dpush dpop", 5);
            Test("Data clear", "5 dpush dclr data", "[]");
            Test("Return", "10 20 30 40 50 5 3 20 ret 30", 20);
            Test("Return ", "10 20 30 40 50 5 3 20 Bazinga ret", "Bazinga");
            Test("Return If", "tapang bazinga false ! retif", "bazinga");
            Test("Function and Call", "@a 10 20 + @a 3 @@a + 11 /", 3);
            Test("If", "true 10 if", 10);
            Test("If", "5 false 10 if", 5);
            Test("If-then-Else", "5 true 10 20 ife", 10);
            Test("If-then-Else", "5 false 10 20 ife", 20);
            Test("Case", "3 1 um case 2 dois case 3 tres case end", "tres");
            Test("Case", "1 1 um case 2 dois case 3 tres case end", "um");
            Test("Case", "5 1 um case 2 dois case 3 tres case outro end", "outro");
            Test("From Index", "10 20 30 40 3 1 fromindex", 30);

            /******************************************/
            /** COMPLEX OBJECTS
            /******************************************/
            Test("Single Parameter", "$0.Num 1 +", 3, new { Num = 2 });
            Test("Single Parameter from Object", "$0.Num", 5, new Mock { Num = 5 });
            Test("Single Parameter from Multiple Objects", "$0.Num $1.Num +", 8, new Mock { Num = 5 }, new Mock { Num = 3 });
            Test("Single Json String Parameter", "$0.Num 1 +", 3, "{ \"Num\": 2 }");
            Test("Multiple Parameters", "$0.Num $1.Num min", 2, new { Num = 2 }, new { Num = 5 });
            Test("Multiple Parameters", "$0 $1 +", 5, 2, 3);
            Test("Array Item", "$0[0] $0[1] +", 5, new int[] { 2, 3 });
            Test("Array Property", "$0.Length", 4, new int[] { 2, 3, 4, 5 });
            Test("Array of Objects", "$0[0].Num $0[1].Num $1 + +", 8, new Mock[] { new Mock() { Num = 3 }, new Mock() { Num = 2 } }, 3);
            Test("List of Objects", "$0[0].Num $0[1].Num $1 + +", 8, new List<Mock> { new Mock() { Num = 3 }, new Mock() { Num = 2 } }, 3);
            Test("Dictionary", "$0[abc] $0[def] +", 5, new Dictionary<string, int> { { "abc", 3 }, { "def", 2 } });
            Test("Dictionary of Objects", "$0[abc].Num $0[def].Num +", 5, new Dictionary<string, Mock> { { "abc", new Mock() { Num = 3 } }, { "def", new Mock() { Num = 2 } } });
            Test("Complex", "$1.Mocks.Last.Num $0.Mocks.Last.Num +", 10, mock1, mock2);
            Test("Collection", "$0", mockList, mockList);
            Test("Spread", "$0 Num ... sum", 17, mockList);

            /******************************************/
            /** REGEX OPERATIONS
            /******************************************/
            Test("Regex Match", @"`telefone: 371-8333 555-8888 321-4001 bazinga` rx/\d{3}\-\d{4}/ match $0[0].Value", "371-8333");

            Summary();
        }
        static void Test(string name, string exp, dynamic expected)
        {
            Test(name, exp, expected, null);
        }
        static void Test(string name, string exp, dynamic expected, params object[] objects)
        {
            _total++;
            string strVal = "";
            bool status = false;

            try
            {
                var val = RPN.Evaluate(exp, objects);
                strVal = val.ToString();
                status = expected == val;

                if (status) _success++;

                Console.WriteLine($"{name} => RPN: {exp} :: Expected: {expected} :: Value: {strVal} :: Test: {(status ? "PASS" : "FAIL")}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{name} => RPN: {exp} :: Expected: {expected} :: Value: EXCEPTION :: Test: FAIL");
                Console.WriteLine($"          {e.Message}");
            }

        }
        static void Summary()
        {
            var rate = RPN.Evaluate("$0 $1 perc", _total, _success);
            var summary = $"Summary: Total: {_total}, Success: {_success}, Failures: {_total - _success}, Success Rate: {rate}%";

            Console.WriteLine("".PadLeft(80, '*'));
            Console.WriteLine(summary);
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
