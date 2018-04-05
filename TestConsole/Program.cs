namespace TestConsole
{
	using System;
	using System.Threading.Tasks;
	using AaronLuna.Common.Console;
	using AaronLuna.Common.Extensions;
	using AaronLuna.Common.IO;

	class Program
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
				NumberOfBlocks = 13,
				StartBracket = "|",
				EndBracket = "|",
				CompletedBlock = "|",
				IncompleteBlock = "\u00a0",
                AnimationSequence = ProgressAnimations.GrowingBarVertical
            };
            await TestProgressBar(pb2);

            var pb3 = new ConsoleProgressBar
            {
                StartBracket = string.Empty,
                EndBracket = string.Empty,
                CompletedBlock = "\u00bb",
                IncompleteBlock = "-",               
                DisplayPercentComplete = false,
                AnimationSequence = ProgressAnimations.RotatingTriangle
            };
            await TestProgressBar(pb3);

            var pb4 = new ConsoleProgressBar
            {
                DisplayBar = false,
                AnimationSequence = ProgressAnimations.RotatingArrow
            };
            await TestProgressBar(pb4);
        }

        static async Task TestProgressBar(ConsoleProgressBar progress)
        {
            Console.Write("Performing some task... ");
            using (progress)
            {
                for (int i = 0; i <= 100; i++)
                {
                    progress.Report((double)i / 100);
                    await Task.Delay(30);
                }

                progress.Report(1);
                await Task.Delay(100);
            }

            Console.WriteLine("Done.");
        }

        static async Task FileTransferProgressBars()
        {
			var fileSize1 = (long)(100 * 48 * FileHelper.OneMB);
			var pb1 = new FileTransferProgressBar(fileSize1, TimeSpan.FromSeconds(10))
			{
			    NumberOfBlocks = 10,
			    StartBracket = "|",
			    EndBracket = "|",
			    CompletedBlock = "|",
			    IncompleteBlock = "\u00a0",
			    DisplayLastRxTime = false,
			    AnimationSequence = ProgressAnimations.RotatingCircle
			};
			await TestFileTransferProgressBar(pb1, fileSize1);
            
			var fileSize2 = (long)(100 * 81 * FileHelper.OneKB);
			var pb2 = new FileTransferProgressBar(fileSize2, TimeSpan.FromSeconds(10))
			{
			    NumberOfBlocks = 20,
			    StartBracket = string.Empty,
			    EndBracket = string.Empty,
			    CompletedBlock = "\u00bb",
			    IncompleteBlock = "\u00a0",
			    DisplayLastRxTime = false,
			    DisplayBytes = false,
				DisplayAnimation = false
			};
			await TestFileTransferProgressBar(pb2, fileSize2);
   

             var fileSize3 = 100 * 59 * 1024;
             var pb3 = new FileTransferProgressBar(fileSize3, TimeSpan.FromSeconds(10))
             {
                 NumberOfBlocks = 15,
                 StartBracket = "\u00a0",
                 EndBracket = "\u00a0",
                 CompletedBlock = "/",
				 IncompleteBlock = "\u00a0",
                 DisplayLastRxTime = true,
                 DisplayPercentComplete = false,
                 DisplayBytes = false,
                 AnimationSequence = ProgressAnimations.RotatingPipe
             };
             await TestFileTransferProgressBar(pb3, fileSize3);

			var fileSize4 = (long)(100 * 36 * FileHelper.OneKB);
			var pb4 = new FileTransferProgressBar(fileSize4, TimeSpan.FromSeconds(2))
			{
				NumberOfBlocks = 10,
				StartBracket = "\u00a0",
				EndBracket = "\u00a0",
				CompletedBlock = "\\",
				IncompleteBlock = "\u00a0",
				DisplayLastRxTime = true,
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
                    await Task.Delay(30);
                }

                progress.BytesReceived = fileSize;
                progress.Report(1);
                await Task.Delay(100);
            }

            Console.WriteLine("Done.");
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
                    await Task.Delay(15);
                }

        		await Task.Delay(3000);
            }
        }

        static void HandleFileTransferStalled(object sender, ProgressEventArgs eventArgs)
        {
        	var pb = (FileTransferProgressBar)sender;
        	pb.Dispose();

        	Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}File transfer stalled!");
        	Console.WriteLine($"Last Progress Reported: {eventArgs.LastDataReceived}");
        	Console.WriteLine($"Notified Transfer Stalled: {eventArgs.TimeOutTriggered}");
        	Console.WriteLine($"Transfer Idle Timespan (Expected): {pb.FileStalledInterval.ToFormattedString()}");
        	Console.WriteLine($"Transfer Idle Timespan (Actual): {eventArgs.Elapsed.ToFormattedString()}");
        }
    }
}
