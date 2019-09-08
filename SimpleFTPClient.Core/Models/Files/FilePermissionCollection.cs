using System;

namespace SimpleFTPClient.Core.Models.Files
{
    public class FilePermissionCollection
    {
        internal FilePermissionCollection()
        {
            User = new FilePermissionSet();
            Group = new FilePermissionSet();
            Other = new FilePermissionSet();
        }

        public static FilePermissionCollection FromString(string permissions)
        {
            permissions = permissions?.Trim() ?? "---------";

            var fp = new FilePermissionCollection();

            if (permissions.Length != 9)
            {
                throw new ArgumentException("Permission string must be 9 characters long");
            }

            if (string.IsNullOrWhiteSpace(permissions))
            {
                return new FilePermissionCollection();
            }

            fp.User = FilePermissionSet.FromString(permissions.Substring(0, 3));
            fp.Group = FilePermissionSet.FromString(permissions.Substring(3, 3));
            fp.Other = FilePermissionSet.FromString(permissions.Substring(6, 3));

            return fp;
        }

        public FilePermissionSet User { get; set; }
        public FilePermissionSet Group { get; set; }
        public FilePermissionSet Other { get; set; }

        public int NumericValue => (User.NumericValue * 100) + (Group.NumericValue * 10) + Other.NumericValue;

        public override string ToString()
        {
            return $"{User.ToString()}{Group.ToString()}{Other.ToString()}";
        }
    }
}
