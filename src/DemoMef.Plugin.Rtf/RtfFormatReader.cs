using DemoMef.Core;
using System.ComponentModel.Composition;
using System.Linq;
using SautinSoft.Document;

namespace DemoMef.Plugin.Rtf
{
    [Export(typeof(IFormatReader))]
    public class RtfFormatReader : IFormatReader
    {
        public string Format => ".rtf";

        public string Read(string path)
        {
            var dc = DocumentCore.Load(path);
            var runList = dc.GetChildElements(true, ElementType.Run).Select(x => x.Content.ToString());
            return string.Concat(runList);
        }
    }
}
