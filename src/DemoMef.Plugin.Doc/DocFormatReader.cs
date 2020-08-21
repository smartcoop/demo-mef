using System.Collections.Generic;
using DemoMef.Core;
using System.ComponentModel.Composition;
using SautinSoft.Document;

namespace DemoMef.Plugin.Doc
{
    [Export(typeof(IFormatReader))]
    public class DocFormatReader : IFormatReader
    {
        public string Format => ".docx";

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