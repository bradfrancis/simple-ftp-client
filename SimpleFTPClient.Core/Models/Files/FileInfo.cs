using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleFTPClient.Core.Models.Files
{
    public class FileInfo
    {
        private readonly static Regex statLineRegex = new Regex(@"^([-dl])([rwx-]{9})\s+(\d)\s+(\w+)\s+(\w+)\s+(\d+)\s+((?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+\d+\s+(?:\d{4}|\d{2}:\d{2}))\s+([\w\.]+)$");

        internal FileInfo()
        {

        }
        
        public static FileInfo FromString(string s)
        {
            FileInfo fileInfo;

            var match = statLineRegex.Match(s);

            if (!match.Success)
            {
                throw new ArgumentException("Invalid stat string");
            }

            fileInfo = new FileInfo();

            // Determine if file is file, directory or symlink
            switch(match.Groups[1].Value)
            {
                case "-": fileInfo.IsFile = true;
                    break;
                case "d": fileInfo.IsDirectory = true;
                    break;
                case "l": fileInfo.IsSymbolicLink = true;
                    break;
            }

            // Set permissions
            fileInfo.FilePermissions = FilePermissionCollection.FromString(match.Groups[2].Value);

            // Hard link count
            fileInfo.HardLinkCount = Convert.ToInt32(match.Groups[3].Value);

            // Owner/group
            fileInfo.Owner = match.Groups[4].Value;
            fileInfo.Group = match.Groups[5].Value;

            // File size
            fileInfo.FileSize = Convert.ToInt32(match.Groups[6].Value);

            // Modification date
            DateTime modificationDate;

            if (!DateTime.TryParseExact(match.Groups[7].Value, "MMM dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out modificationDate))
            {
                DateTime.TryParseExact(match.Groups[7].Value, "MMM dd yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out modificationDate);
            }

            fileInfo.ModificationDate = modificationDate;

            // Filename
            fileInfo.FileName = match.Groups[8].Value;

            return fileInfo;
        }

        public string Owner { get; private set; }
        public string Group { get; private set; }
        public bool IsFile { get; private set; }
        public bool IsDirectory { get; private set; }
        public bool IsSymbolicLink { get; private set; }
        public FilePermissionCollection FilePermissions { get; private set; }
        public int FileSize { get; private set; }
        public int HardLinkCount { get; private set; }
        public DateTime ModificationDate { get; private set; }
        public string FileName { get; private set; }
    }
}
