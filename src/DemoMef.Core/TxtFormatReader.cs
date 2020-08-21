using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;

namespace DemoMef.Core
{
    /// <summary>
    /// Reads .txt file formats.
    /// This FormatReader is provided with the IFormatInterface to show an concrete implementation.
    /// </summary>
    [Export(typeof(IFormatReader))]
    class TxtFormatReader : IFormatReader
    {
        public string Format => ".txt";

        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
