namespace Printer.Framework.Printer
{
    public interface IPrinter<T>
    {
        void SetPrinter(string printerName);
        void Print(T value);
    }
}
