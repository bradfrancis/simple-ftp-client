using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTPClient.Core.Models.Files;
using System;

namespace SimpleFTPClient.Core.Tests
{
    [TestClass]
    [TestCategory("Files")]
    public class FilePermissionCollectionTests
    {
        [TestMethod]
        public void Test_FromString_FullPermissions()
        {
            var inputString = "rwxrwxrwx";
            var collection = FilePermissionCollection.FromString(inputString);

            Assert.IsTrue(collection.User.CanRead);
            Assert.IsTrue(collection.User.CanWrite);
            Assert.IsTrue(collection.User.CanExecute);
            Assert.AreEqual(7, collection.User.NumericValue);

            Assert.IsTrue(collection.Group.CanRead);
            Assert.IsTrue(collection.Group.CanWrite);
            Assert.IsTrue(collection.Group.CanExecute);
            Assert.AreEqual(7, collection.Group.NumericValue);

            Assert.IsTrue(collection.Other.CanRead);
            Assert.IsTrue(collection.Other.CanWrite);
            Assert.IsTrue(collection.Other.CanExecute);
            Assert.AreEqual(7, collection.Other.NumericValue);

            Assert.AreEqual(777, collection.NumericValue);
            Assert.AreEqual(inputString, collection.ToString());
        }

        [TestMethod]
        public void Test_FromString_NullInput()
        {
            var collection = FilePermissionCollection.FromString(null);

            Assert.IsFalse(collection.User.CanRead);
            Assert.IsFalse(collection.User.CanWrite);
            Assert.IsFalse(collection.User.CanExecute);
            Assert.AreEqual(0, collection.User.NumericValue);

            Assert.IsFalse(collection.Group.CanRead);
            Assert.IsFalse(collection.Group.CanWrite);
            Assert.IsFalse(collection.Group.CanExecute);
            Assert.AreEqual(0, collection.Group.NumericValue);

            Assert.IsFalse(collection.Other.CanRead);
            Assert.IsFalse(collection.Other.CanWrite);
            Assert.IsFalse(collection.Other.CanExecute);
            Assert.AreEqual(0, collection.Other.NumericValue);

            Assert.AreEqual(0, collection.NumericValue);
            Assert.AreEqual("---------", collection.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_FromString_InvalidInput()
        {
            FilePermissionCollection.FromString("------abc123");
        }
    }
}
