using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN.Evaluators
{
    internal class MathEvaluator
    {
        private static string[] MATH_FUNCTIONS = new string[] { 
            "acos", "asin", "atan", "atan2", "ceiling", "cos",  "cosh", "floor", "sin", "tan", "sinh", "tanh", "truncate", "trunc", "ceil", "sqrt" };

        private static string[] OPERATORS = new string[] { 
            "pi", "^", "pow", "log", "round", "exp", "10x", "E", "logb", "log10", "abs", "random", "rnd", "btw", "sum", "sumx", "sumk", "+-", "-+", "++", "--", "max", "min" };


        internal static bool Evaluate(RPNContext context)
        {
            return EvaluateFunctions(context) || EvaluateOperators(context);
        }
        private static bool EvaluateFunctions(RPNContext context)
        {
            var current = context.Current;

            if (MATH_FUNCTIONS.Contains(current))
            {
                if (current == "ceil") current = "ceiling";
                else if (current == "trunc") current = "truncate";

                var mathMethods = typeof(Math).GetMethods();
                var method = mathMethods.FirstOrDefault(f => f.Name.ToLower() == current);
                var ceilingMethod = mathMethods.Where(w => w.Name.ToLower() == "ceiling");

                if (new string[] { "ceiling", "floor", "truncate" }.Contains(current))
                    method = mathMethods.First(f => f.Name.ToLower() == current && f.ToString().ToLower().Contains("double"));

                if (method != null)
                {
                    var methodParameters = new List<double>();
                    for (int j = 0; j < method.GetParameters().Count(); j++)
                        methodParameters.Add(Convert.ToDouble(context.Stack.Pop()));
                    methodParameters.Reverse();

                    var parametersArray = method.Invoke(null, methodParameters.Cast<object>().ToArray());
                    context.Stack.Push(parametersArray);
                }
                return true;
            }
            return false;
        }
        private static bool EvaluateOperators(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "pi":
                        context.Stack.Push(Math.PI);
                        break;
                    case "^":
                    case "pow":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            context.Stack.Push(Math.Pow(y, x));
                        }
                        break;
                    case "log":
                        context.Stack.Push(Math.Log(context.Stack.Pop()));
                        break;
                    case "round":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            context.Stack.Push(Math.Round(y, Convert.ToInt32(x)));
                        }
                        break;
                    case "exp":
                        context.Stack.Push(Math.Exp(context.Stack.Pop()));
                        break;
                    case "10x":
                    case "E":
                        context.Stack.Push(Math.Pow(10, context.Stack.Pop()) * context.Stack.Pop());
                        break;
                    case "logb":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            context.Stack.Push(Math.Log(y, x));
                        }
                        break;
                    case "log10":
                        {
                            context.Stack.Push(Math.Log10(context.Stack.Pop()));
                        }
                        break;
                    case "abs":
                        context.Stack.Push(Math.Abs(context.Stack.Pop()));
                        break;
                    case "random":
                    case "rnd":
                        {
                            context.Stack.Push(new Random().NextDouble());
                            break;
                        }
                    case "btw":
                        {
                            var x = Convert.ToInt32(context.Stack.Pop());
                            var y = Convert.ToInt32(context.Stack.Pop());
                            var randomNumber = new Random().Next(y, x + 1);
                            context.Stack.Push(randomNumber);
                            break;
                        }
                    case "sum":
                        {
                            double sum = 0;
                            while (context.Stack.Count > 0)
                            {
                                var value = context.Stack.Pop();
                                sum += value;
                            }
                            context.Stack.Push(sum);
                        }
                        break;
                    case "sumx":
                    case "sumk":
                        {
                            double sum = 0;
                            int x = (int)context.Stack.Pop();
                            for (int ik = 0; ik < x; ik++)
                            {
                                var value = context.Stack.Pop();
                                sum += value;
                            }
                            context.Stack.Push(sum);
                        }
                        break;
                    case "-+":
                    case "+-":
                        {
                            context.Stack.Push(context.Stack.Pop() * (-1));
                            break;
                        }
                    case "++":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(x + 1);
                            break;
                        }
                    case "--":
                        {
                            var x = Convert.ToDouble(context.Stack.Pop());
                            context.Stack.Push(x - 1);
                            break;
                        }
                    case "max":
                        {
                            List<double> numbers = new List<double>();
                            while (context.Stack.Count > 0)
                            {
                                numbers.Add(Convert.ToDouble(context.Stack.Pop()));
                            }
                            context.Stack.Push(numbers.Max());
                            break;
                        }
                    case "min":
                        {
                            List<double> numbers = new List<double>();
                            while (context.Stack.Count > 0)
                            {
                                numbers.Add(Convert.ToDouble(context.Stack.Pop()));
                            }
                            context.Stack.Push(numbers.Min());
                            break;
                        }

                }
                return true;
            }
            return false;
        }
    }
}