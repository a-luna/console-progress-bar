namespace TestConsole
{
    using System;
    using System.Threading.Tasks;

    using AaronLuna.Common.IO;
    using AaronLuna.ConsoleProgressBar;

    static class Program
    {
        // async Main is a C# 7.1 feature, change your project settings to the 
        // new version if this is flagged as an error
        static async Task Main()
        {
            Console.Clear();
            await Task.Delay(2000);
            await ConsoleProgressBars();
            Console.WriteLine();

            await FileTransferProgressBars();
            Console.WriteLine();
        }

	static async Task ConsoleProgressBars()
        {
            var pb1 = new ConsoleProgressBar();
            await TestProgressBar(pb1, 1);
	
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

            var pb3 = new ConsoleProgressBar
            {
                DisplayBar = false,
                AnimationSequence = ProgressAnimations.RotatingTriangle
            };
            await TestProgressBar(pb3, 3);
        }

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

        static async Task FileTransferProgressBars()
        {
            const long fileSize = (long)(8 * FileHelper.OneKB);
	    var pb4 = new FileTransferProgressBar(fileSize, TimeSpan.FromSeconds(5))
	    {
		NumberOfBlocks = 15,
		StartBracket = "|",
		EndBracket = "|",
		CompletedBlock = "|",
		IncompleteBlock = "\u00a0",
                AnimationSequence = ProgressAnimations.PulsingLine
	    };
	    await TestFileTransferProgressBar(pb4, fileSize, 4);

            const long fileSize2 = (long)(100 * 36 * FileHelper.OneMB);
	    var pb5 = new FileTransferProgressBar(fileSize2, TimeSpan.FromSeconds(5))
	    {
		DisplayBar = false,
                DisplayAnimation = false
            };
            pb5.FileTransferStalled += HandleFileTransferStalled;
            await TestFileTransferStalled(pb5, fileSize2, 5);
        }

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

        static void HandleFileTransferStalled(object sender, ProgressEventArgs eventArgs)
        {
	    var pb = (FileTransferProgressBar)sender;
	    pb.Dispose();

	    Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}File transfer stalled!");
	    Console.WriteLine($"{pb.TimeSpanFileStalled.Seconds} seconds elapsed since last data received");
        }
    }
}
