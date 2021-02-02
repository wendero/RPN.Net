using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN.Evaluators
{
    internal class BasicEvaluator
    {
        private static string[] OPERATORS = new string[] { "+", "-", "*", "/", "%", "perc", "percf", "percd" };

        internal static bool Evaluate(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                if (context.Stack.First() is TimeSpan ||
                    context.Stack.First() is ExtendedTimeSpan)
                {
                    return false;
                }

                switch (context.Current)
                {
                    case "+":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y + x);
                            break;
                        }
                    case "-":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y - x);
                            break;
                        }
                    case "*":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y * x);
                            break;
                        }
                    case "/":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y / x);
                            break;
                        }
                    case "%":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var y = Convert.ToInt32(context.Stack.Pop());
                            context.Stack.Push(y % x);
                        }
                        break;
                    case "perc":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            var perc = GetPercentage(x, y);
                            context.Stack.Push(Math.Round(Convert.ToDouble(100 * perc), 0));
                        }
                        break;
                    case "percd":
                        {
                            int d = Convert.ToInt32(context.Stack.Pop());
                            double x = context.Stack.Pop();
                            double y = context.Stack.Pop();
                            double perc = GetPercentage(x, y);
                            context.Stack.Push(Math.Round(100 * perc, d));
                        }
                        break;
                    case "percf":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            var perc = GetPercentage(x, y);
                            context.Stack.Push(perc);
                        }
                        break;
                }
                return true;
            }
            return false;
        }
        private static double GetPercentage(double value, double totalValue)
        {
            return value / totalValue;
        }
    }
}