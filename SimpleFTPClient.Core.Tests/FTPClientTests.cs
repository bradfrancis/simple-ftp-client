using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFTPClient.Core.Tests
{
    [TestClass]
    [TestCategory("Client")]
    public class FTPClientTests
    {
        [TestMethod]
        public void CreateClient_WithUrlPlusFullCredentials()
        {
            var username = "username";
            var password = "password";
            var hostname = "test.ftp.com";

            var client = new FTPClient($"ftp://{username}:{password}@{hostname}/");

            Assert.AreEqual(hostname, client.HostName);
            Assert.AreEqual(username, client.Credentials.UserName);
            Assert.AreEqual(password, client.Credentials.Password);
        }

        [TestMethod]
        public void CreateClient_WithUrlPlusUsernameOnly()
        {
            var username = "username";
            var hostname = "test.ftp.com";

            var client = new FTPClient($"ftp://{username}@{hostname}/");

            Assert.AreEqual(hostname, client.HostName);
            Assert.AreEqual(username, client.Credentials.UserName);
        }

        [TestMethod]
        public void CreateClient_WithUrlNoCredentials()
        {
            var hostname = "test.ftp.com";

            var client = new FTPClient($"ftp://{hostname}/");

            Assert.AreEqual(hostname, client.HostName);
            Assert.AreEqual("anonymous", client.Credentials.UserName);
            Assert.AreEqual("anonymous@domain.com", client.Credentials.Password);
        }

        [TestMethod]
        public void CreateClient_WithUrlNoProtocol()
        {
            var hostname = "test.ftp.com";

            var client = new FTPClient(hostname);

            Assert.AreEqual(hostname, client.HostName);
        }

        [TestMethod]
        public async Task DownloadFileAsync_ValidURL()
        {
            var hostname = "ftp://ftp.bom.gov.au/";
            var filePath = "anon/gen/fwo/IDA00100.dat";
            var fileName = Path.GetFileName(filePath);

            var client = new FTPClient(hostname);

            using (var fs = await client.DownloadFileAsync(filePath))
            using (var outFile = File.Create(fileName))
            {
                fs.Seek(0, SeekOrigin.Begin);
                await fs.CopyToAsync(outFile);
            }

            var fileInfo = new FileInfo(fileName);

            Assert.IsTrue(fileInfo.Exists);
            Assert.IsTrue(fileInfo.Length > 0);
        }

        [TestMethod]
        public async Task GetFileDetailsFileAsync_ValidURL()
        {
            var hostname = "ftp://ftp.bom.gov.au/";
            var filePath = "anon/gen/fwo/IDA00100.dat";
            var fileName = Path.GetFileName(filePath);
            var expectedModificationDate = new DateTime(2019, 9, 8, 17, 10, 0);

            var client = new FTPClient(hostname);

            var fileInfo = await client.GetFileDetailsAsync(filePath);

            Assert.AreEqual(fileName, fileInfo.FileName);
            Assert.IsTrue(fileInfo.IsFile);
            Assert.IsFalse(fileInfo.IsDirectory);
            Assert.IsFalse(fileInfo.IsSymbolicLink);

            Assert.AreEqual(670, fileInfo.FileSize);
            Assert.AreEqual(expectedModificationDate, fileInfo.ModificationDate);
        }

        [TestMethod]
        public async Task ListDirectoryDetailsAsync_ValidURL()
        {
            var hostname = "ftp://ftp.bom.gov.au/";
            var filePath = "anon/gen/fwo/";

            var client = new FTPClient(hostname);

            var files = (await client.ListDirectoryDetailsAsync(filePath));

            Assert.IsTrue(files.Count() > 0);
        }
    }
}
