# MEF Demo - Managed Extensibility Framework

This project is meant to demonstrate [MEF](https://docs.microsoft.com/en-us/dotnet/framework/mef/)'s basic functionnalities with a simple application.
The projet runs fully on Docker. The easiest way to launch it is to use the `start.sh` script at the root of this directory.

MEF (Managed Extensibility Framework) is a standard library for creating extensible applications, where additional features can be added by plugins without recompiling the original application. In this demo, we will show a basic file reader as example. 
Since there are multiple possible formats to store text (plain, PDF, Word files, etc), this is a perfect example to demonstrate MEF's basic extensibility capabalities.

The solutions contains multiple projects, two of them are important :
* The 'CLI' executing a basic file reader
* The 'Core' providing an interface and an example class to develop additional extensions.
Aside from these two, there are currently 3 extensions to demonstrate their implementation

The way this works is the CLI will provide no way to read any form of text, not a single format will be supported by the base application. 
All formats will be supported by extensions of the application. To develop extensions, their project will need a reference to the 'Core' project.
Once the extension is build, its DLL simply needs to be placed in the CLI's folder.

# How to run the demo
The demo is 'out of the box', all you need to do is run the `start.sh` script to build and run the container with Docker.
If you run it with Docker, you will see that the CLI supports 4 formats (txt, rtf, docx and pdf). This is because the extensions are included in the container.
If you wish to experiement with this yourself, open the solution in Visual Studio and try building the CLI. You will now see only the basic 'txt' format is supported.
If you build any of the demo plugins and copy its DLL file in the CLI's application root, then launch it again, you'll now see the new format is available in the app.

# How it works
The code is pretty detailed about this, but the basic idea is this : 

The projet `DemoMef.Core` contains an interface, [`IFormatReader`](src/DemoMef.Core/IFormatReader.cs). This interface defines two items. A `Read` method, and a `Format` property which serves to define the supported file format.

`DemoMef.Core` also contains a basic plain text reader as an implementation example, `TxtFormatReader.cs`. You can see its `Format` property marks it as being a `.txt` file reader. Its `Read` method does just that.


From there on it's easy to develop an extension to support any other format. Just add `DemoMef.Core` as a dependancy to the new project and implement [`IFormatReader`](src/DemoMef.Core/IFormatReader.cs) in your custom reader.
Once your reader is built, add it's DLL to the CLI. Done.

# Documentation
You can access MEF's documentation [here](https://docs.microsoft.com/en-us/dotnet/framework/mef/).