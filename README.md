# Progress Bar for Console Applications (.NET Core 2.0)
A simple way to represent the progress of a long-running job in a C# console app. Targets .NET Core 2.0.

## Features
- **ConsoleProgressBar**
  - **Implements `IProgress<double>`** The `IProgress` interface greatly simplifies the effort to report progress from an `async` method to the UI, clunky boilerplate code to ensure thread-safety is not required since the `SynchronizationContext` is captured when the progress bar is instantiated.
  - **Efficient and light-weight** `Console` can easily become sluggish and unresponsive when called frequently, this progress bar only performs 8 calls/second regardless of how often progress is reported.
  - **Customizable** Each component of the progress bar (start/end brackets, completed/incomplete blocks, progress animation) can be set to any string value through public properties and each item displayed (the progress bar itself, percentage complete, animation) can be shown or hidden.

- **FileTransferProgressBar**
  - **Extends** ConsoleProgressBar and tracks the time when progress was last reported. If the time since the last report exceeds the FileStalledInterval value, the FileTransferStalled event fires.
  - **Provides further customization** of the progress bar with the addition of bytes received/total bytes and time since last Rx display options
  
## Usage
### ConsoleProgressBar
Default behavior:
```csharp
var pb1 = new ConsoleProgressBar();
await TestProgressBar(pb1);
```
![Console Progress Bar Default Behavior](https://s3-us-west-1.amazonaws.com/alunapublic/console_progress_bar/ConsoleProgressBar-1.gif)
