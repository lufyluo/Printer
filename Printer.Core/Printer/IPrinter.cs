namespace Printer.Core.Printer
{
    public interface IPrinter<T>
    {
        void SetPrinter(string printerName);
        void Print(T value);
    }
}
