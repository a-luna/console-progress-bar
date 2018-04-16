namespace AaronLuna.ConsoleProgressBar
{
    using System;

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs()
        {
            LastDataReceived = DateTime.MinValue;
            TimeOutTriggered = DateTime.MinValue;
        }

        public DateTime LastDataReceived { get; set; }
        public DateTime TimeOutTriggered { get; set; }
        public TimeSpan Elapsed => TimeOutTriggered - LastDataReceived;
    }
}
