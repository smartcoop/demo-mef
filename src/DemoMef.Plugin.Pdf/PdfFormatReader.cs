using System.Collections.Generic;
using DemoMef.Core;
using System.ComponentModel.Composition;
using SautinSoft.Document;

namespace DemoMef.Plugin.Doc
{
    [Export(typeof(IFormatReader))]
    public class PdfFormatReader : IFormatReader
    {
        public string Format => ".pdf";

        public string Read(string path)
        {
            var output = "";

            var dc = DocumentCore.Load(path);
            var list = new List<string>();
            foreach (var run in dc.GetChildElements(true, ElementType.Run))
            {
                output += run.Content.ToString();
            }
            return output;
        }
    }
}