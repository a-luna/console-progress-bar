# Progress Bar for Console Applications (.NET Core 2.0)
A simple way to represent the progress of a long-running job in a C# console app. Targets .NET Core 2.0.

## Features
- **ConsoleProgressBar**
  - **Implements `IProgress<double>`** The `IProgress` interface greatly simplifies the effort to report progress from an `async` method to the UI, clunky boilerplate code to ensure thread-safety is not required since the `SynchronizationContext` is captured when the progress bar is instantiated.
  - **Efficient and light-weight** `Console` can become sluggish and unresponsive when called frequently, this progress bar only performs 8 calls/second regardless of how often progress is reported.
  - **Customizable** Each component of the progress bar (start/end brackets, completed/incomplete blocks, progress animation) can be set to any string value through public properties and each item displayed (the progress bar itself, percentage complete, animation) can be shown or hidden.

- **FileTransferProgressBar**
  - **Extends** ConsoleProgressBar and tracks the time when progress was last reported. If the time since the last report exceeds the FileStalledInterval value, the FileTransferStalled event fires.
  - **Provides further customization** of the progress bar with the addition of bytes received/total bytes and time since last Rx display options
  
## Usage
### FileTransferProgressBar
```csharp
// Progress bar is setup to trigger FileTransferStalled after 2 seconds of inactivity
var pb4 = new FileTransferProgressBar((long)fileSize, TimeSpan.FromSeconds(2))
{
	NumberOfBlocks = 10,
	StartBracket = "{",
	EndBracket = "}",
	CompletedBlock = "-",
	IncompleteBlock = "\u00a0",
	DisplayLastRxTime = true,
	DisplayAnimation = false
};
pb4.FileTransferStalled += HandleFileTransferStalled;

await TestFileTransferStalled(pb4);
```
![File Transfer Progress Bar-4](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/FileTransferProgressBar-4.gif)
```csharp
var pb1 = new FileTransferProgressBar((long)fileSize, TimeSpan.FromSeconds(10))
{
    NumberOfBlocks = 10,
    StartBracket = "|",
    EndBracket = "|",
    CompletedBlock = "|",
    IncompleteBlock = "\u00a0",
    DisplayLastRxTime = false,	// Time since last progress reported not displayed
    AnimationSequence = ProgressAnimations.RotatingCircle
};
await TestFileTransferProgressBar(pb1);
```
![File Transfer Progress Bar-1](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/FileTransferProgressBar-1.gif)
```csharp
var pb2 = new FileTransferProgressBar((long)fileSize, TimeSpan.FromSeconds(10))
{
    NumberOfBlocks = 20,
    StartBracket = string.Empty,
    EndBracket = string.Empty,
    CompletedBlock = "\u00bb",
    IncompleteBlock = "\u00a0",
    DisplayLastRxTime = false,	// Time since last progress reported not displayed
    DisplayBytes = false,	// Bytes received/total bytes not displayed
    AnimationSequence = ProgressAnimations.BouncingBall
};
await TestFileTransferProgressBar(pb2);
```
![File Transfer Progress Bar-2](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/FileTransferProgressBar-2.gif)
```csharp
 var pb3 = new FileTransferProgressBar((long)fileSize, TimeSpan.FromSeconds(10))
 {
     NumberOfBlocks = 15,
     StartBracket = "\u00a0",
     EndBracket = "\u00a0",
     CompletedBlock = "=",
     IncompleteBlock = "-",
     DisplayLastRxTime = true,
     DisplayPercentComplete = false,	// Percent complete not displayed
     DisplayBytes = false,
     AnimationSequence = ProgressAnimations.RotatingPipe
 };
 await TestFileTransferProgressBar(pb3);
```
![File Transfer Progress Bar-3](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/FileTransferProgressBar-3.gif)

### ConsoleProgressBar
```csharp
var pb1 = new ConsoleProgressBar(); // Default behavior
await TestProgressBar(pb1);
```
![Console Progress Bar Default Behavior](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ConsoleProgressBar-1.gif)
```csharp
var pb2 = new ConsoleProgressBar
{   
    // Customized all parts of the progress bar
    NumberOfBlocks = 13,
    StartBracket = "|",
    EndBracket = "|",
    CompletedBlock = "|",
    IncompleteBlock = "\u00a0",
    AnimationSequence = ProgressAnimations.GrowingBarVertical
};
await TestProgressBar(pb2);
```
![Console Progress Bar Custom-1](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ConsoleProgressBar-2.gif)
```csharp
var pb3 = new ConsoleProgressBar
{
    StartBracket = string.Empty,
    EndBracket = string.Empty,
    CompletedBlock = "\u00bb",
    IncompleteBlock = "-",
    DisplayAnimation = false      // Animation is not displayed
};
await TestProgressBar(pb3);
```
![Console Progress Bar Custom-2](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ConsoleProgressBar-3.gif)
```csharp
var pb4 = new ConsoleProgressBar
{
    DisplayBar = false, // Progress bar is not displayed
    AnimationSequence = ProgressAnimations.RotatingArrow
};
await TestProgressBar(pb4);
```
![Console Progress Bar Custom-3](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ConsoleProgressBar-4.gif)
### Test Methods Referenced in Usage Examples
```csharp
static async Task TestProgressBar(ConsoleProgressBar progress)
{
    Console.Write("Performing some task... ");
    using (progress)
    {
        for (int i = 0; i <= 100; i++)
        {
            progress.Report((double)i / 100);
            await Task.Delay(50);
        }

        progress.Report(1);
        await Task.Delay(100);
    }

    Console.WriteLine("Done.");
}
```
```csharp
static async Task TestFileTransferProgressBar(FileTransferProgressBar progress)
{
    Console.Write("File transfer in progress... ");
    using (progress)
    {
        for (int i = 0; i <= 100; i = i + 10)
        {
            progress.BytesReceived = (long)(48 * FileHelper.OneMB * i);
            progress.Report((double)i / 100);
            await Task.Delay(1000);
        }

        progress.BytesReceived = (long)(48 * FileHelper.OneMB * 100);
        progress.Report(1);
        await Task.Delay(100);
    }

    Console.WriteLine("Done.");
    await Task.Delay(2000);
}
```
```csharp
static async Task TestFileTransferStalled(FileTransferProgressBar progress)
{
	Console.Write("File transfer in progress... ");
    using (progress)
    {
        for (int i = 0; i <= 63; i++)
        {
            progress.BytesReceived = (long)(36 * FileHelper.OneKB * i);
            progress.Report((double)i / 100);
            await Task.Delay(50);
        }

		await Task.Delay(3000);
    }
}
```
```csharp
static void HandleFileTransferStalled(object sender, ProgressEventArgs eventArgs)
{
	var pb = (FileTransferProgressBar)sender;
	pb.Dispose();

	Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}File transfer stalled!");
	Console.WriteLine($"Last Progress Reported: {eventArgs.LastDataReceived}");
	Console.WriteLine($"Notified Transfer Stalled: {eventArgs.TimeOutTriggered}");
	Console.WriteLine($"{Environment.NewLine}Transfer Idle Timespan (Expected): {pb.FileStalledInterval.ToFormattedString()}");
	Console.WriteLine($"Transfer Idle Timespan (Actual): {eventArgs.Elapsed.ToFormattedString()}");
	Console.ReadLine();
}
```
