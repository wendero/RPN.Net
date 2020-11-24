namespace RPN
{
    internal class ExtendedTimeSpan
    {
        internal int Months { get; private set; }
        internal int Years { get; private set; }

        internal ExtendedTimeSpan(int year, int month)
        {
            this.Months = month;
            this.Years = year;
        }
        internal void Invert()
        {
            this.Months = -this.Months;
            this.Years = -this.Years;
        }
    }
}