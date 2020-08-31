using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using DemoMef.Core;

namespace DemoMef.CLI
{
    class Program
    {
        static void Main()
        { 
            var p = new Program();
            
            p.ConfigureMef();
            p.PrintWelcome();
            p.PrintHelp();
            while (true)
            {
                Console.WriteLine("\r\nEnter path:");
                var path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("ERROR: File does not exist");
                    continue;
                }

                Console.WriteLine(p.ReadFile(path));
            }
        }

        private void PrintWelcome()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("--                                             --");
            Console.WriteLine("--                 File Reader                 --");
            Console.WriteLine("--                                             --");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }
        private void PrintHelp()
        {
            Console.WriteLine("This is a file reader. Enter the path of a supported file type to read its content.");
            PrintSupportedFormats();
        }
        private void PrintSupportedFormats()
        {
            var formats = FormatReaders.Select(x => x.Format);
            Console.WriteLine("Supported formats are: " + string.Join(' ', formats));
        }





        /*
         *
         * Here's the interesting part :
         *
        */



        /// <summary>
        /// Here we import all the exported IFormatReader we will find in our catalog.
        /// See next method for details
        ///
        /// What happens here is when the CompositionContainer compose the parts in the catalog,
        ///     it will match any class having the attribute [Export(typeof(IFormatReader))] with
        ///     this property, because this one is looking for Imports (Many, so any that will match).
        /// The CompositionContainers know this one is looking for typeof(IFormatReader) because
        /// the ImportMany     implies an IEnumerable and the definition of the property states
        /// the interface to populate it with.
        /// </summary>
        [ImportMany] 
        public IEnumerable<IFormatReader> FormatReaders;

        /// <summary>
        /// The composition container is the class that is going to match our Imports and Exports.
        /// In the current example, whan we will call _containerComposeParts() in the
        ///     next method, it will actually populate FormatReaders with all the classes
        ///     using the decorator [Export(IFormatReader)].
        /// </summary>
        private CompositionContainer _container;

        /// <summary>
        /// There are a few things happening down there. This here is all the configuration
        ///     that needs to be done to use MEF. First thing is to create an aggregate catalog,
        ///     it will contain all the other catalogs we will define next.
        ///     You can see the AggregateCatalog as a collection of Catalogs if you will.
        /// Then we have to define an actual catalog. In this example we use a Directory
        ///     Catalog and ask it to filter .DLL files. What this means is it will look
        ///     at all the DLL it finds in the given directory and note all the properties
        ///     that use either an [Import] or [Export].
        /// Finally we instanciate a Composition Container and ask it to compose the parts.
        ///     Parts are all the [Import] or [Export] found and stored in the catalog,
        ///     and composing the parts means match all imports with corresponding exports.
        ///
        /// Note that you can have more than one Catalog, in this instance we are using a
        ///     Directory Catalog but the basic tutorial also suggests using an Assembly Catalog.
        /// For more info : https://docs.microsoft.com/en-us/dotnet/framework/mef/
        /// </summary>
        private void ConfigureMef()
        {
            //We create the catalog that will contain all the parts we want to compose (I.E. our plugins)
            var catalog = new AggregateCatalog();

            //We are looking in a directory for any exported parts
            //For this demo it will make our life easier to have the plugins at the app root.
            //What we'd typically do is have a "Plugins" folder at the app root that would
            //contain the plugins' DLL.
            var pluginsDir = AppDomain.CurrentDomain.BaseDirectory;
            catalog.Catalogs.Add(new DirectoryCatalog(pluginsDir, "*.dll"));

            //We create the CompositionContainer with the parts in the catalog.
            _container = new CompositionContainer(catalog);

            try
            {
                //We fill the current context's import with all the exports we found in the
                //catalog's parts. In this example, the property FormatReaders will be filled
                //with an instance of every class using the decorator [Export(IFormatReader)].
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
                throw;
            }
        }

        /// <summary>
        /// In this method, we will be checking the extension of the file. We are using
        ///     that to check if there is any compatible Reader in the catalog.
        /// In our base interface, IFormatReader, we define a Read method, and a Format
        ///     property. This property contains the supported extension.
        /// By looking for a match between any Reader's format in the catalog and the file
        ///     extension, we will be able to read the file.
        /// If there is no match, it means the format is not yet supported and we need a
        ///     new plugin to read it.
        /// </summary>
        private string ReadFile(string path)
        {
            //We search in our list of readers if there is any matching the current extension.
            //In this example we consider the format of a file by its extension.
            //I.E. a Microsoft word file would be a .docx file.
            var extension = Path.GetExtension(path);
            var reader = FormatReaders.FirstOrDefault(x => x.Format == extension);
            
            //If we have a reader for that format then we call its Read method and return the result.
            return reader == null ? $"Unsupported format \"{extension}\"" : reader.Read(path); 
        }
    }
}
