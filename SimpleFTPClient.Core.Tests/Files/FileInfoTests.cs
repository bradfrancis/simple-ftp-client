using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTPClient.Core.Models.Files;
using System;
using System.Globalization;

namespace SimpleFTPClient.Core.Tests
{
    [TestClass]
    [TestCategory("Files")]
    public class FileInfoTests
    {
        [TestMethod]
        public void Test_FromString_ValidFileInput()
        {
            var inputString = "-rwxrwxrwx 1 dev dev    31 Dec 23  18:05 bashrc";
            var fileInfo = FileInfo.FromString(inputString);
            
            // Correct file type
            Assert.IsTrue(fileInfo.IsFile);
            Assert.IsFalse(fileInfo.IsDirectory);
            Assert.IsFalse(fileInfo.IsSymbolicLink);

            // File permissions
            Assert.IsTrue(fileInfo.FilePermissions.User.CanRead);
            Assert.IsTrue(fileInfo.FilePermissions.User.CanWrite);
            Assert.IsTrue(fileInfo.FilePermissions.User.CanExecute);
            Assert.IsTrue(fileInfo.FilePermissions.Group.CanRead);
            Assert.IsTrue(fileInfo.FilePermissions.Group.CanWrite);
            Assert.IsTrue(fileInfo.FilePermissions.Group.CanExecute);
            Assert.IsTrue(fileInfo.FilePermissions.Other.CanRead);
            Assert.IsTrue(fileInfo.FilePermissions.Other.CanWrite);
            Assert.IsTrue(fileInfo.FilePermissions.Other.CanExecute);

            // Owner/group
            Assert.AreEqual("dev", fileInfo.Owner);
            Assert.AreEqual("dev", fileInfo.Group);

            // Modification time
            Assert.AreEqual(DateTime.ParseExact("Dec 23 18:05", "MMM dd HH:mm", CultureInfo.InvariantCulture), fileInfo.ModificationDate);

            // Filename
            Assert.AreEqual("bashrc", fileInfo.FileName);
        }

        [TestMethod]
        public void Test_FromString_ValidDirectoryInput()
        {
            var inputString = "drwxrwxrwx 1 dev dev    31 Dec 23  2017 folderName";
            var fileInfo = FileInfo.FromString(inputString);

            // Correct file type
            Assert.IsFalse(fileInfo.IsFile);
            Assert.IsTrue(fileInfo.IsDirectory);
            Assert.IsFalse(fileInfo.IsSymbolicLink);

            // File permissions
            Assert.IsTrue(fileInfo.FilePermissions.User.CanRead);
            Assert.IsTrue(fileInfo.FilePermissions.User.CanWrite);
            Assert.IsTrue(fileInfo.FilePermissions.User.CanExecute);
            Assert.IsTrue(fileInfo.FilePermissions.Group.CanRead);
            Assert.IsTrue(fileInfo.FilePermissions.Group.CanWrite);
            Assert.IsTrue(fileInfo.FilePermissions.Group.CanExecute);
            Assert.IsTrue(fileInfo.FilePermissions.Other.CanRead);
            Assert.IsTrue(fileInfo.FilePermissions.Other.CanWrite);
            Assert.IsTrue(fileInfo.FilePermissions.Other.CanExecute);

            // Owner/group
            Assert.AreEqual("dev", fileInfo.Owner);
            Assert.AreEqual("dev", fileInfo.Group);

            // Modification time
            Assert.AreEqual(DateTime.ParseExact("Dec 23 2017", "MMM dd yyyy", CultureInfo.InvariantCulture), fileInfo.ModificationDate);

            // Filename
            Assert.AreEqual("folderName", fileInfo.FileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_FromString_InvalidInput()
        {
            FilePermissionCollection.FromString("------abc123");
        }
    }
}
