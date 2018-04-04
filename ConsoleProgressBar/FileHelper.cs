namespace AaronLuna.Common.IO
{
    public static class FileHelper
    {
		public const double OneKB = 1024;
		public const double OneMB = 1024 * 1024;
		public const double OneGB = 1024 * 1024 * 1024;

        public static string FileSizeToString(long fileSizeInBytes)
        {
            if (fileSizeInBytes > OneGB)
            {
                return $"{fileSizeInBytes / OneGB:F2} GB";
            }

            if (fileSizeInBytes > OneMB)
            {
                return $"{fileSizeInBytes / OneMB:F2} MB";
            }

            return fileSizeInBytes > OneKB
                ? $"{fileSizeInBytes / OneKB:F2} KB"
                : $"{fileSizeInBytes} bytes";
        }
    }
}
