using DemoMef.Core;
using System.ComponentModel.Composition;
using System.Linq;
using SautinSoft.Document;

namespace DemoMef.Plugin.Doc
{
    [Export(typeof(IFormatReader))]
    public class PdfFormatReader : IFormatReader
    {
        public string Format => ".pdf";

        public string Read(string path)
        {
            var dc = DocumentCore.Load(path);
            var runList = dc.GetChildElements(true, ElementType.Run).Select(x => x.Content.ToString());
            return string.Concat(runList);
        }
    }
}