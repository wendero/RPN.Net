using System;
using System.Linq;

namespace RPN.Evaluators
{
    internal class LogicEvaluator
    {
        private static string[] OPERATORS = new string[] { ">", "<", "=", "==", "!=", "<>", ">=", "<=", "not", "!", "&", "&&", "and", "|", "||", "or" };

        internal static bool Evaluate(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "=":
                    case "==":
                        {
                            var x = context.Stack.Pop().ToString();
                            var y = context.Stack.Pop().ToString();
                            context.Stack.Push(y == x);
                            break;
                        }
                    case "!=":
                    case "<>":
                        {
                            var x = context.Stack.Pop().ToString();
                            var y = context.Stack.Pop().ToString();
                            context.Stack.Push(y != x);
                            break;
                        }
                    case "!":
                    case "not":
                        {
                            var x = Convert.ToBoolean(context.Stack.Pop());
                            context.Stack.Push(!x);
                            break;
                        }
                    case "&":
                    case "&&":
                    case "and":
                        {
                            var x = Convert.ToBoolean(context.Stack.Pop());
                            var y = Convert.ToBoolean(context.Stack.Pop());
                            context.Stack.Push(y && x);
                            break;
                        }
                    case "|":
                    case "||":
                    case "or":
                        {
                            var x = Convert.ToBoolean(context.Stack.Pop());
                            var y = Convert.ToBoolean(context.Stack.Pop());
                            context.Stack.Push(y || x);
                            break;
                        }
                    case ">":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y > x);
                            break;
                        }
                    case "<":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y < x);
                            break;
                        }
                    case ">=":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y >= x);
                            break;
                        }
                    case "<=":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            var y = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(y <= x);
                            break;
                        }

                }
                return true;
            }
            return false;
        }
    }
}