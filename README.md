# Progress Bar for Console Applications (.NET Core 2.0)
A simple way to represent the progress of a long-running job in a C# console app. Targets .NET Core 2.0.

## Features
- **ConsoleProgressBar**
  - **Implements `IProgress<double>`** The `IProgress` interface greatly simplifies the effort to report progress from an `async` method to the UI, clunky boilerplate code to ensure thread-safety is not required since the `SynchronizationContext` is captured when the progress bar is instantiated.
  - **Efficient and light-weight** `Console` can become sluggish and unresponsive when called frequently, this progress bar only performs 8 calls/second regardless of how often progress is reported.
  - **Customizable** Each component of the progress bar (start/end brackets, completed/incomplete blocks, progress animation) can be set to any string value through public properties and each item displayed (the progress bar itself, percentage complete, animation) can be shown or hidden.

- **FileTransferProgressBar**
  - **Extends** ConsoleProgressBar and adds the ability to detect when a file transfer has stalled.
  - If the time since last progress reported exceeds the `TimeSpanFileStalled` value, the `FileTransferStalled` event fires.
  - **Provides further customization** of the display with the ability to show/hide the bytes received and file size in bytes
  
## Examples
### Windows
![File Transfer Progress Bar-4](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ProgressBar_Win.gif)
### Mac (VS Code)
![File Transfer Progress Bar-4](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ProgressBar_Mac.gif)
### Ubuntu
![File Transfer Progress Bar-4](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ProgressBar_Ubuntu.gif)
  
## Usage (Numbers correspond to those shown in the examples)
```csharp
// 1. Default behavior
var pb1 = new ConsoleProgressBar();
await TestProgressBar(pb1, 1);
```
```csharp
// 2. Customized all progress bar components
var pb2 = new ConsoleProgressBar
{
    NumberOfBlocks = 18,
    StartBracket = string.Empty,
    EndBracket = string.Empty,
    CompletedBlock = "\u2022",
    IncompleteBlock = "·",
    AnimationSequence = ProgressAnimations.RotatingPipe
};
await TestProgressBar(pb2, 2);
```
```csharp
// 3. Hide progress bar
var pb3 = new ConsoleProgressBar
{
    DisplayBar = false,
    AnimationSequence = ProgressAnimations.RotatingTriangle
};
await TestProgressBar(pb3, 3)
```
```csharp
// 4. Customized progress bar, successful file transfer
const long fileSize = (long)(8 * FileHelper.OneKB);
var pb4 = new FileTransferProgressBar(fileSize, TimeSpan.FromSeconds(5))
{
    NumberOfBlocks = 15,
    StartBracket = "|",
    EndBracket = "|",
    CompletedBlock = "|",
    IncompleteBlock = "\u00a0",   // Non-breaking space
    AnimationSequence = ProgressAnimations.PulsingLine
};
await TestFileTransferProgressBar(pb4, fileSize, 4);
```
```csharp
// 5. Hide progress bar and animation, unsuccessful file transfer
const long fileSize4 = (long)(100 * 36 * FileHelper.OneMB);
var pb4 = new FileTransferProgressBar(fileSize4, TimeSpan.FromSeconds(5))
{
    DisplayBar = false,
    DisplayAnimation = false
};
pb4.FileTransferStalled += HandleFileTransferStalled;
await TestFileTransferStalled(pb4, fileSize4, 5);
```
### Test Methods Referenced by Examples
```csharp
static async Task TestProgressBar(ConsoleProgressBar progress, int num)
{
    Console.Write($"{num}. Performing some task... ");
    using (progress)
    {
	for (var i = 0; i <= 150; i++)
	{
	    progress.Report((double)i / 150);
	    await Task.Delay(20);
	}

	progress.Report(1);
	await Task.Delay(200);
    }

    Console.WriteLine();
}
```
```csharp
static async Task TestFileTransferProgressBar(FileTransferProgressBar progress, long fileSize, int num)
{
    Console.Write($"{num}. File transfer in progress... ");
    using (progress)
    {
	for (int i = 0; i <= 150; i++)
	{
	    progress.BytesReceived = i * (fileSize / 150);
	    progress.Report((double)i / 150);
	    await Task.Delay(20);
	}

	progress.BytesReceived = fileSize;
	progress.Report(1);
	await Task.Delay(200);
    }

    Console.WriteLine();
}
```
```csharp
static async Task TestFileTransferStalled(FileTransferProgressBar progress, long fileSize, int num)
{
    Console.Write($"{num}. File transfer in progress... ");
    using (progress)
    {
	for (int i = 0; i <= 110; i++)
	{
	    progress.BytesReceived = i * (fileSize / 1000);
	    progress.Report((double)i / 1000);
	    await Task.Delay(2);
	}

	await Task.Delay(6000);
    }
}
```
```csharp
static void HandleFileTransferStalled(object sender, ProgressEventArgs eventArgs)
{
    var pb = (FileTransferProgressBar)sender;
    pb.Dispose();

    Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}File transfer stalled!");
    Console.WriteLine($"{pb.TimeSpanFileStalled.Seconds} seconds elapsed since last data received");
}
```
