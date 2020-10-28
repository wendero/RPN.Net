namespace RPN.Exceptions
{
    public class EmptyExpressionException : RPNException
    {
        public EmptyExpressionException() : base("RPN expression is empty")
        { }
    }
}