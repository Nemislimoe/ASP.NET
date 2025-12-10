namespace Lab13.Services
{
    public interface ICalculatorService
    {
        int Add(int a, int b);
        (bool Success, string Message, double? Result) Divide(int a, int b);
    }
}
