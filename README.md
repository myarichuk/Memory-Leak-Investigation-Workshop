# Memory-Leak-Investigation-Workshop
A repository with code for hands-on excercises of memory leak investigation workshop.
Here you will find "mystery" apps that contain some sort of memory management related issue.

### Getting started

1. Install Visual Studio 2019 Community or any IDE capable of compiling and running .Net 5.0
(If you don't have any installed, get Visual Studio for free from [here](https://visualstudio.microsoft.com/downloads)
2. Ensure .Net 5.0 SDK is installed. Get it from [here](https://dotnet.microsoft.com/download/dotnet/5.0)
3. Install [**WinDbg Preview**](https://www.microsoft.com/en-us/p/windbg-preview/9pgjgd53tn86?activetab=pivot:overviewtab) from Windows Store.
4. WinDbg requires a bit of configuring. Take a look at the following or use [this blog post](http://www.graymatterdeveloper.com/2020/02/12/setting-up-windbg/) as a guide  
  Set up symbol resolution string. The format of the symbol string is **cache*[local cache folder 1]*[local cache folder 2];srv*[local cache folder]*[symbol server path]**  
  Note 1: This will enable WinDbg to resolve symbols so proper stack traces can be displayed.  
  Note 2: *!sym noisy on* command will enable debug output to see what kind of symbols WinDbg looks for and why the resolution is missing.
  ![](https://github.com/myarichuk/Memory-Leak-Investigation-Workshop/blob/master/Images/SymbolsInWinDBG.PNG) 
5. Download **Process Explorer** from [here](https://docs.microsoft.com/en-us/sysinternals/downloads/process-explorer). It will be useful for taking dumps of processes.
6. Download and install Windows 10 SDK (you can [download it from here](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk/)). Make sure to tick **Debugging Tools for Windows** when you install
