namespace TestConsole
{
    using System;
    using System.Threading.Tasks;
    using AaronLuna.Common.Console;
    using AaronLuna.Common.IO;

    class Program
    {
        // async Main is a C# 7.1 feature, change your project settings to the 
        // new version if this is flagged as an error
        static async Task Main()
        {
            await Task.Delay(5000);
            await ConsoleProgressBars();
            await FileTransferProgressBars();
        }

        static async Task FileTransferProgressBars()
        {
            var pb1desc = $"File transfer with bytes received/total bytes displayed:{Environment.NewLine}{Environment.NewLine}" +
                $"var ftpb1 = new FileTransferProgressBar({Environment.NewLine}" +
                "{" + $"{Environment.NewLine}" +
                "    NumberOfBlocks = 10," + $"{Environment.NewLine}" +
                "    StartBracket = \"|\"," + $"{Environment.NewLine}" +
                "    EndBracket = \"|\"," + $"{Environment.NewLine}" +
                "    CompletedBlock = \"|\"," + $"{Environment.NewLine}" +
                "    IncompleteBlock = \"\\u00a0\",\\\\ non-breaking space" + $"{Environment.NewLine}" +
                "    DisplayLastRxTime = false" + $"{Environment.NewLine}" +
                "    AnimationSequence = ProgressAnimations.RotatingCircle" + $"{Environment.NewLine}" +
                "};" + $"{Environment.NewLine}";

            Console.WriteLine(pb1desc);

            var fileSize1 = 100 * 48 * FileHelper.OneMB;
            var pb1 = new FileTransferProgressBar((long)fileSize1, TimeSpan.FromSeconds(10))
            {
                NumberOfBlocks = 10,
                StartBracket = "|",
                EndBracket = "|",
                CompletedBlock = "|",
                IncompleteBlock = "\u00a0",
                DisplayLastRxTime = false,
                AnimationSequence = ProgressAnimations.RotatingCircle
            };
            await TestFileTransferProgressBar(pb1);
            Console.Clear();

            var pb2desc = $"File transfer with time since last Rx and bytes received/total bytes hidden:{Environment.NewLine}{Environment.NewLine}" +
                $"var ftpb2 = new FileTransferProgressBar({Environment.NewLine}" +
                "{" + $"{Environment.NewLine}" +
                "    NumberOfBlocks = 20," + $"{Environment.NewLine}" +
                "    StartBracket = string.Empty," + $"{Environment.NewLine}" +
                "    EndBracket = string.Empty," + $"{Environment.NewLine}" +
                "    CompletedBlock = \"\\u00bb\"," + $"{Environment.NewLine}" +
                "    IncompleteBlock = \"\\u00bb\"," + $"{Environment.NewLine}" +
                "    DisplayLastRxTime = false" + $"{Environment.NewLine}" +
                "    AnimationSequence = ProgressAnimations.BouncingBall" + $"{Environment.NewLine}" +
                "};" + $"{Environment.NewLine}";

            Console.WriteLine(pb2desc);

            var fileSize2 = 100 * 81 * FileHelper.OneKB;
            var pb2 = new FileTransferProgressBar((long)fileSize2, TimeSpan.FromSeconds(10))
            {
                NumberOfBlocks = 20,
                StartBracket = string.Empty,
                EndBracket = string.Empty,
                CompletedBlock = "\u00bb",
                IncompleteBlock = "\u00a0",
                DisplayLastRxTime = false,
                DisplayBytes = false,
                AnimationSequence = ProgressAnimations.BouncingBall
            };
            await TestFileTransferProgressBar(pb2);
            Console.Clear();

            var pb3desc = $"File transfer with time since last Rx shown, percent complete hidden and bytes received/total bytes hidden:{Environment.NewLine}{Environment.NewLine}" +
                $"var ftpb3 = new FileTransferProgressBar({Environment.NewLine}" +
                "{" + $"{Environment.NewLine}" +
                "    NumberOfBlocks = 15," + $"{Environment.NewLine}" +
                "    StartBracket = \"\\u00a0\",\\\\ non-breaking space" + $"{Environment.NewLine}" +
                "    EndBracket = \"\\u00a0\",\\\\ non-breaking space" + $"{Environment.NewLine}" +
                "    CompletedBlock = \"=\"," + $"{Environment.NewLine}" +
                "    IncompleteBlock = \"-\"," + $"{Environment.NewLine}" +
                "    DisplayLastRxTime = true" + $"{Environment.NewLine}" +
                "    DisplayPercentComplete = false" + $"{Environment.NewLine}" +
                "    DisplayBytes = false" + $"{Environment.NewLine}" +
                "    AnimationSequence = ProgressAnimations.RotatingPipe" + $"{Environment.NewLine}" +
                "};" + $"{Environment.NewLine}";

            Console.WriteLine(pb3desc);

            var fileSize3 = 100 * 59 * 1024;
            var pb3 = new FileTransferProgressBar((long)fileSize3, TimeSpan.FromSeconds(10))
            {
                NumberOfBlocks = 15,
                StartBracket = "\u00a0",
                EndBracket = "\u00a0",
                CompletedBlock = "=",
                IncompleteBlock = "-",
                DisplayLastRxTime = true,
                DisplayPercentComplete = false,
                DisplayBytes = false,
                AnimationSequence = ProgressAnimations.RotatingPipe
            };
            await TestFileTransferProgressBar(pb3);

        }

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

        static async Task ConsoleProgressBars()
        {
            var pb1desc = $"Default behavior:{Environment.NewLine}{Environment.NewLine}" +
                $"var pb1 = new ConsoleProgressBar();{Environment.NewLine}";

            Console.WriteLine(pb1desc);

            var pb1 = new ConsoleProgressBar();
            await TestProgressBar(pb1);
            Console.Clear();

            var pb2desc = $"Customized progress bar and animation:{Environment.NewLine}{Environment.NewLine}" +
                $"var pb2 = new ConsoleProgressBar{Environment.NewLine}" +
                "{" + $"{Environment.NewLine}" +
                "    NumberOfBlocks = 13," + $"{Environment.NewLine}" +
                "    StartBracket = \"|\"," + $"{Environment.NewLine}" +
                "    EndBracket = \"|\"," + $"{Environment.NewLine}" +
                "    CompletedBlock = \"|\"," + $"{Environment.NewLine}" +
                "    IncompleteBlock = \"\\u00a0\",\\\\ non-breaking space" + $"{Environment.NewLine}" +
                "    AnimationSequence = ProgressAnimations.GrowingBarVertical" + $"{Environment.NewLine}" +
                "};" + $"{Environment.NewLine}";

            Console.WriteLine(pb2desc);

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
            Console.Clear();

            var pb3desc = $"No amimation:{Environment.NewLine}{Environment.NewLine}" +
                $"var pb3 = new ConsoleProgressBar{Environment.NewLine}" +
                "{" + $"{Environment.NewLine}" +
                "    StartBracket = string.Empty," + $"{Environment.NewLine}" +
                "    EndBracket = string.Empty," + $"{Environment.NewLine}" +
                "    CompletedBlock = \"\u00bb\"," + $"{Environment.NewLine}" +
                "    IncompleteBlock = \"-\"," + $"{Environment.NewLine}" +
                "    DisplayAnimation = false" + $"{Environment.NewLine}" +
                "};" + $"{Environment.NewLine}";

            Console.WriteLine(pb3desc);

            var pb3 = new ConsoleProgressBar
            {
                StartBracket = string.Empty,
                EndBracket = string.Empty,
                CompletedBlock = "\u00bb",
                IncompleteBlock = "-",
                DisplayAnimation = false
            };
            await TestProgressBar(pb3);
            Console.Clear();

            var pb4desc = $"No progress bar, custom animation:{Environment.NewLine}{Environment.NewLine}" +
                $"var pb4 = new ConsoleProgressBar{Environment.NewLine}" +
                "{" + $"{Environment.NewLine}" +
                "    DisplayBar = false," + $"{Environment.NewLine}" +
                "    AnimationSequence = ProgressAnimations.RotatingArrow" + $"{Environment.NewLine}" +
                "};" + $"{Environment.NewLine}";

            Console.WriteLine(pb4desc);

            var pb4 = new ConsoleProgressBar
            {
                DisplayBar = false,
                AnimationSequence = ProgressAnimations.RotatingArrow
            };
            await TestProgressBar(pb4);
            Console.Clear();
        }

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
            await Task.Delay(2000);
        }
    }
}
