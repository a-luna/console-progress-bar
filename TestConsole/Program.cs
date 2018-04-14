﻿namespace TestConsole
{
	using System;
	using System.Threading.Tasks;
    using AaronLuna.Common.Extensions;
	using AaronLuna.Common.IO;
	using AaronLuna.ConsoleProgressBar;

    static class Program
    {
        // async Main is a C# 7.1 feature, change your project settings to the 
        // new version if this is flagged as an error
        static async Task Main()
        {
            await ConsoleProgressBars();
			Console.WriteLine();

            await FileTransferProgressBars();
        }

		static async Task ConsoleProgressBars()
        {
            var pb1 = new ConsoleProgressBar();
            await TestProgressBar(pb1);

			var pb2 = new ConsoleProgressBar
			{
				NumberOfBlocks = 15,
				StartBracket = "|",
				EndBracket = "|",
				CompletedBlock = "|",
				IncompleteBlock = "\u00a0",
                AnimationSequence = ProgressAnimations.PulsingLine
            };
            await TestProgressBar(pb2);

            var pb3 = new ConsoleProgressBar
            {
                DisplayBar = false,
                AnimationSequence = ProgressAnimations.RotatingTriangle
            };
            await TestProgressBar(pb3);
        }

        static async Task TestProgressBar(ConsoleProgressBar progress)
        {
            Console.Write("Performing some task... ");
            using (progress)
            {
                for (var i = 0; i <= 100; i++)
                {
                    progress.Report((double)i / 100);
                    await Task.Delay(40);
                }

                progress.Report(1);
                await Task.Delay(100);
            }

            Console.WriteLine();
        }

        static async Task FileTransferProgressBars()
        {
            const long fileSize2 = (long)(100 * 81 * FileHelper.OneKB);
			var pb2 = new FileTransferProgressBar(fileSize2, TimeSpan.FromSeconds(10))
			{
				NumberOfBlocks = 20,
				StartBracket = string.Empty,
				EndBracket = string.Empty,
				CompletedBlock = "\u00bb",
				IncompleteBlock = "\u00a0",
                AnimationSequence = ProgressAnimations.RotatingDot
			};
			await TestFileTransferProgressBar(pb2, fileSize2);

            const long fileSize4 = (long)(10 * 36 * FileHelper.OneMB);
			var pb4 = new FileTransferProgressBar(fileSize4, TimeSpan.FromSeconds(2))
			{
				DisplayBar = false,
                DisplayAnimation = false
            };
            pb4.FileTransferStalled += HandleFileTransferStalled;

            await TestFileTransferStalled(pb4, fileSize4);
        }

        static async Task TestFileTransferProgressBar(FileTransferProgressBar progress, long fileSize)
        {
            Console.Write("File transfer in progress... ");
            using (progress)
            {
				var onePercent = fileSize / 100;
                for (int i = 0; i <= 100; i++)
                {
                    progress.BytesReceived = onePercent * i;
                    progress.Report((double)i / 100);
                    await Task.Delay(40);
                }

                progress.BytesReceived = fileSize;
                progress.Report(1);
                await Task.Delay(100);
            }

            Console.WriteLine();
        }

        static async Task TestFileTransferStalled(FileTransferProgressBar progress, long fileSize)
        {
        	Console.Write("File transfer in progress... ");
            using (progress)
            {
				var onePercent = fileSize / 100;
                for (int i = 0; i <= 63; i++)
                {
					progress.BytesReceived = onePercent * i;
                    progress.Report((double)i / 100);
                    await Task.Delay(40);
                }

        		await Task.Delay(3000);
            }
        }

        static void HandleFileTransferStalled(object sender, ProgressEventArgs eventArgs)
        {
        	var pb = (FileTransferProgressBar)sender;
        	pb.Dispose();

        	Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}File transfer stalled!");
        	Console.WriteLine($"No data received for {pb.TimeSpanFileStalled.Seconds} seconds");
        }
    }
}
