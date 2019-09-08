using System;
using System.Linq;

namespace SimpleFTPClient.Core.Models.Files
{
    public class FilePermissionSet
    {
        private char[] _charVal;

        internal FilePermissionSet()
        {
            NumericValue = 0;
            _charVal = "---".ToCharArray();
        }

        public static FilePermissionSet FromString(string permissions)
        {
            var set = new FilePermissionSet();

            permissions = permissions?.Trim() ?? "---";

            if (permissions.Length != 3)
            {
                throw new ArgumentException("Permission set must be 3 characters long");
            }

            if (string.IsNullOrWhiteSpace(permissions))
            {
                return set;
            }
           
            set.NumericValue = permissions
                .ToLower()
                .ToCharArray()
                .Distinct()
                .Sum(y =>
                {
                    switch (y)
                    {
                        case 'r':
                            set._charVal[0] = 'r';
                            set.CanRead = true;
                            return 4;
                        case 'w':
                            set._charVal[1] = 'w';
                            set.CanWrite = true;
                            return 2;
                        case 'x':
                            set._charVal[2] = 'x';
                            set.CanExecute = true;
                            return 1;
                        default: return 0;
                    }
                });

            return set;
        }

        public int NumericValue { get; private set; }

        public bool CanRead { get; private set; }
        public bool CanWrite { get; private set; }
        public bool CanExecute { get; private set; }

        public override string ToString()
        {
            return string.Join("", _charVal);
        }
    }
}
