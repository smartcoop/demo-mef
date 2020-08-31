namespace DemoMef.Core
{
    /// <summary>
    /// This is the interface all FormatReader plugins will implement.
    /// It's a very simple one. Format is the file extension. (Example : .txt)
    /// Read is the method implementing how to read that specified format.
    /// </summary>
    public interface IFormatReader
    {
        string Format { get; }
        string Read(string path);
    }
}
