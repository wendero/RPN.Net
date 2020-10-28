namespace RPN.Exceptions
{
    public class NullExpressionException : RPNException
    {
        public NullExpressionException() : base("RPN expression is null")
        { }
    }
}