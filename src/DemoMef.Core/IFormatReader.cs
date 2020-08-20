using System;

namespace DemoMef.Core
{
    public interface IFormatReader
    {
        string Format { get; }
        string Read(string path);
    }
}
