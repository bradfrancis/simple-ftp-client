using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTPClient.Core.Models.Files;
using System;

namespace SimpleFTPClient.Core.Tests
{
    [TestClass]
    [TestCategory("Files")]
    public class FilePermissionSetTests
    {
        [TestMethod]
        public void Test_FromString_FullPermissions()
        {
            var set = FilePermissionSet.FromString("rwx");

            Assert.IsTrue(set.CanRead);
            Assert.IsTrue(set.CanWrite);
            Assert.IsTrue(set.CanExecute);

            Assert.AreEqual(7, set.NumericValue);
        }

        [TestMethod]
        public void Test_FromString_ReadOnly()
        {
            var set = FilePermissionSet.FromString("r--");

            Assert.IsTrue(set.CanRead);
            Assert.IsFalse(set.CanWrite);
            Assert.IsFalse(set.CanExecute);

            Assert.AreEqual(4, set.NumericValue);
            Assert.AreEqual("r--", set.ToString());
        }

        [TestMethod]
        public void Test_FromString_WriteOnly()
        {
            var set = FilePermissionSet.FromString("-w-");

            Assert.IsFalse(set.CanRead);
            Assert.IsTrue(set.CanWrite);
            Assert.IsFalse(set.CanExecute);

            Assert.AreEqual(2, set.NumericValue);
            Assert.AreEqual("-w-", set.ToString());
        }

        [TestMethod]
        public void Test_FromString_ExecuteOnly()
        {
            var set = FilePermissionSet.FromString("--x");

            Assert.IsFalse(set.CanRead);
            Assert.IsFalse(set.CanWrite);
            Assert.IsTrue(set.CanExecute);

            Assert.AreEqual(1, set.NumericValue);
            Assert.AreEqual("--x", set.ToString());
        }

        [TestMethod]
        public void Test_FromString_ReadAndWrite()
        {
            var set = FilePermissionSet.FromString("rw-");

            Assert.IsTrue(set.CanRead);
            Assert.IsTrue(set.CanWrite);
            Assert.IsFalse(set.CanExecute);

            Assert.AreEqual(6, set.NumericValue);
            Assert.AreEqual("rw-", set.ToString());
        }

        [TestMethod]
        public void Test_FromString_ReadAndExecute()
        {
            var set = FilePermissionSet.FromString("r-x");

            Assert.IsTrue(set.CanRead);
            Assert.IsFalse(set.CanWrite);
            Assert.IsTrue(set.CanExecute);

            Assert.AreEqual(5, set.NumericValue);
            Assert.AreEqual("r-x", set.ToString());
        }

        [TestMethod]
        public void Test_FromString_NullInput()
        {
            var set = FilePermissionSet.FromString(null);

            Assert.IsFalse(set.CanRead);
            Assert.IsFalse(set.CanWrite);
            Assert.IsFalse(set.CanExecute);

            Assert.AreEqual(0, set.NumericValue);
            Assert.AreEqual("---", set.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_FromString_InvalidInput()
        {
            var set = FilePermissionSet.FromString("------abc123");
        }
    }
}
