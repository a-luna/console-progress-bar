namespace AaronLuna.Common.Extensions
{
	using System;

    public static class TimeSpanExtensions
    {
        public static string ToFormattedString(this TimeSpan timeSpan)
        {         
            var s = string.Empty;

            var numYears = timeSpan.Days / 365;
            if (numYears > 0)
            {
                s += $"{numYears}y ";
                timeSpan -= new TimeSpan(numYears * 365, 0, 0, 0);
            }

            var numWeeks = timeSpan.Days / 7;
            if (numWeeks > 0)
            {
                s += $"{numWeeks}w ";
                timeSpan -= new TimeSpan(numWeeks * 7, 0, 0, 0);
            }

            if (timeSpan.Days > 0)
            {
                s += $"{timeSpan.Days}d ";
            }

            if (timeSpan.Hours > 0)
            {
                s += $"{timeSpan.Hours}h ";
            }

            if (timeSpan.Minutes > 0)
            {
                s += $"{timeSpan.Minutes}m ";
            }

            if (!string.IsNullOrEmpty(s))
            {
                if (timeSpan.Seconds > 0)
                {
                    s += $"{timeSpan.Seconds}s";
                }

                return s.Trim();
            }

            if (timeSpan.Seconds > 0)
            {
                s += $"{timeSpan.Seconds}s ";
            }

			s = s.TrimStart();

            var remainingTicks = timeSpan.Ticks - (timeSpan.Seconds * 10_000_000);
            var milliseconds = remainingTicks / 10_000;

			if (milliseconds <= 0)
            {
                s += "\u00a0\u00a00ms";
            }

            if (milliseconds > 0)
            {
				if (milliseconds < 100)
				{
					s += $"\u00a0{milliseconds}ms";
				}
				else if (milliseconds < 10)
				{
					s += $"\u00a0\u00a0{milliseconds}ms";
				}
				else
				{
					s += $"{milliseconds}ms";
				}
            }

            return s;
        }
    }
}
