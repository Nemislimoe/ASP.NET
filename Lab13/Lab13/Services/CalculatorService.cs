using System;

namespace Lab13.Services
{
    public class CalculatorService : ICalculatorService
    {
        public int Add(int a, int b) => a + b;

        public (bool Success, string Message, double? Result) Divide(int a, int b)
        {
            if (b == 0)
                return (false, "Ділення на нуль неможливе", null);

            return (true, "OK", (double)a / b);
        }
    }
}
