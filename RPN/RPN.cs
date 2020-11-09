using System;
using RPN.Exceptions;
using RPN.Evaluators;

namespace RPN
{
    public static class RPN
    {
        public static T Evaluate<T>(RPNExpression expression)
        {
            try
            {
                var context = new RPNContext(expression);
                while (context.CanMove)
                {
                    context.MoveNext();

                    if (DefaultEvaluator.Evaluate(context)) continue;
                    if (BasicEvaluator.Evaluate(context)) continue;
                    if (MathEvaluator.Evaluate(context)) continue;
                    if (LogicEvaluator.Evaluate(context)) continue;
                    if (StringEvaluator.Evaluate(context)) continue;
                    if (DateTimeEvaluator.Evaluate(context)) continue;
                    if (ControlEvaluator.Evaluate<T>(context)) continue;

                    context.Stack.Push(context.Current);
                }
                return context.GetResult<T>();
            }
            catch(RPNException)
            {
                throw;
            }
            catch
            {
                throw new ParsingException(expression.Expression);
            }
        }
        public static dynamic Evaluate(RPNExpression expression)
        {
            return Evaluate<dynamic>(expression);
        }

        [ObsoleteAttribute("This method has been deprecated. Use Evaluate method instead.", false)]
        public static T Eval<T>(string expression, params object[] parameters)
        {
            return Evaluate<T>(expression, parameters);
        }
        public static T Evaluate<T>(string expression, params object[] parameters)
        {
            var rpnExpression = new RPNExpression(expression, parameters);
            return Evaluate<T>(rpnExpression);
        }

        [ObsoleteAttribute("This method has been deprecated. Use Evaluate method instead.", false)]
        public static dynamic Eval(string expression, params object[] parameters)
        {
            return Evaluate(expression, parameters);
        }
        public static dynamic Evaluate(string expression, params object[] parameters)
        {
            var rpnExpression = new RPNExpression(expression, parameters);
            return Evaluate<dynamic>(rpnExpression);
        }
    }
}