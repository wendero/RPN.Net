using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace RPN.Evaluators
{
    internal class ControlEvaluator
    {
        private static string[] OPERATORS = new string[] { "pop", "popx", "clr", "ret", "retif", "if", "ife", "case", "end", "stack", "swap", "rot", "dup" };
        private static string[] DATA_OPERATORS = new string[] { "dpush", "dpop", "dclr", "data", "fromindex" };

        internal static bool Evaluate<T>(RPNContext context)
        {
            return EvaluateOperators<T>(context) || EvaluateDataOperators(context);
        }
        private static bool EvaluateOperators<T>(RPNContext context)
        {
            if (OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "pop":
                        {
                            context.Stack.Pop();
                            break;
                        }
                    case "popx":
                        {
                            Stack<dynamic> tempStack = new Stack<dynamic>();
                            var x = context.Stack.Pop();
                            for (int ix = 0; ix < x; ix++)
                            {
                                tempStack.Push(context.Stack.Pop());
                            }

                            context.Stack.Pop();

                            while (tempStack.Count > 0)
                            {
                                context.Stack.Push(tempStack.Pop());
                            }
                            break;
                        }
                    case "clr":
                        {
                            context.Stack.Clear();
                            break;
                        }
                    case "ret":
                        {
                            context.SetResult<T>((T)context.Stack.Pop());
                            break;
                        }
                    case "retif":
                        {
                            var condition = Convert.ToBoolean(context.Stack.Pop());
                            if (condition)
                                context.SetResult<T>((T)context.Stack.Pop());
                            break;
                        }
                    case "if":
                        {
                            var value = context.Stack.Pop();
                            var condition = Convert.ToBoolean(context.Stack.Pop());
                            if (condition)
                                context.Stack.Push(value);
                            break;
                        }
                    case "ife":
                        {
                            var falseValue = context.Stack.Pop();
                            var trueValue = context.Stack.Pop();
                            var condition = Convert.ToBoolean(context.Stack.Pop());
                            if (condition)
                                context.Stack.Push(trueValue);
                            else
                                context.Stack.Push(falseValue);
                            break;
                        }
                    case "case":
                        {
                            var caseTrue = context.Stack.Pop();
                            var caseValue = context.Stack.Pop();
                            var switchValue = context.Stack.Peek();
                            if (caseValue == switchValue)
                            {
                                context.Stack.Pop();
                                context.Stack.Push(caseTrue);
                                do
                                {
                                    context.MoveNext();
                                } while (context.Current != "end");
                            }
                            break;
                        }
                    case "end":
                        break;
                    case "stack":
                        context.Stack.Push(JsonSerializer.Serialize(context.Stack.Reverse()));
                        break;
                    case "swap":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            context.Stack.Push(x);
                            context.Stack.Push(y);
                            break;
                        }
                    case "rot":
                        {
                            var x = context.Stack.Pop();
                            var y = context.Stack.Pop();
                            var z = context.Stack.Pop();
                            context.Stack.Push(x);
                            context.Stack.Push(z);
                            context.Stack.Push(y);
                            break;
                        }
                    case "dup":
                        {
                            var x = context.Stack.Pop();
                            context.Stack.Push(x);
                            context.Stack.Push(x);
                            break;
                        }

                }
                return true;
            }
            return false;
        }
        private static bool EvaluateDataOperators(RPNContext context)
        {
            if (DATA_OPERATORS.Contains(context.Current))
            {
                switch (context.Current)
                {
                    case "dpush":
                        context.Data.Add(context.Stack.Pop());
                        break;
                    case "dpop":
                        var last = context.Data.Last();
                        context.Stack.Push(last);
                        context.Data.Remove(last);
                        break;
                    case "dclr":
                        context.Data.Clear();
                        break;
                    case "data":
                        context.Stack.Push(JsonSerializer.Serialize(context.Data));
                        break;
                    case "fromindex":
                        {
                            var index = Convert.ToInt32(context.Stack.Pop());
                            var len = Convert.ToInt32(context.Stack.Pop());
                            var array = new ArrayList();
                            for (var ix = 1; ix <= len; ix++)
                                array.Add(context.Stack.Pop());

                            context.Stack.Push(array[index]);
                        }
                        break;
                }
                return true;
            }
            return false;
        }
    }
}