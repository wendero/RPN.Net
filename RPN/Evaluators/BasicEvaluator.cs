using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN.Evaluators
{
    internal class BasicEvaluator
    {
        private static string[] OPERATORS = new string[] { "+", "-", "*", "/", "%", "perc", "percf", "percd", "diff", "diffd", "difff" };

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
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            var perc = GetPercentage(x, y);
                            context.Stack.Push(Math.Round(Convert.ToDouble(100 * perc), 0));
                        }
                        break;
                    case "percd":
                        {
                            var d = Convert.ToInt32(context.Stack.Pop());
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            var perc = GetPercentage(x, y);
                            context.Stack.Push(Math.Round(100 * perc, d));
                        }
                        break;
                    case "percf":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            var perc = GetPercentage(x, y);
                            context.Stack.Push(perc);
                        }
                        break;
                    case "diff":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());

                            var diff = GetDifference(x, y) * 100;

                            context.Stack.Push(Math.Round(diff, 0));
                        }
                        break;
                    case "difff":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());

                            var diff = GetDifference(x, y);

                            context.Stack.Push(diff);
                        }
                        break;
                    case "diffd":
                        {
                            var d = Convert.ToInt32(context.Stack.Pop());
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());

                            var diff = GetDifference(x, y) * 100;

                            context.Stack.Push(Math.Round(diff, d));
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
        private static double GetDifference(double current, double previous)
        {
            if (previous == current)
            {
                return 0;
            }

            if (previous == 0)
            {
                return (current > 0 ? 1 : -1);
            }

            return ((current - previous) / previous);
        }
    }
}