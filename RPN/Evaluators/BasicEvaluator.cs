using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN.Evaluators
{
    internal class BasicEvaluator
    {
        private static string[] OPERATORS = new string[] { "+", "-", "*", "/", "%", "perc" };

        internal static bool Evaluate(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
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
                            context.Stack.Push(Math.Round(Convert.ToDouble(100 * x / y), 0));
                        }
                        break;
                }
                return true;
            }
            return false;
        }
    }
}