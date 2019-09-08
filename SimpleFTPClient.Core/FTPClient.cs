using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleFTPClient.Core
{

    public class FTPClient
    {
        private static Dictionary<Enums.RequestType, string> requestMap = new Dictionary<Enums.RequestType, string>()
        {
            { Enums.RequestType.DOWNLOAD_FILE, WebRequestMethods.Ftp.DownloadFile },
            { Enums.RequestType.UPLOAD_FILE, WebRequestMethods.Ftp.UploadFile },
            { Enums.RequestType.LIST_CONTENTS, WebRequestMethods.Ftp.ListDirectoryDetails }
        };
        private Uri _baseUri;

        public FTPClient()
        {
            Credentials = InitCredentials();
        }

        public FTPClient(string url)
        {
            url = url?.Trim() ?? String.Empty;

            if (!url.StartsWith("ftp://"))
            {
                url = "ftp://" + url;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out _baseUri))
            {
                throw new ArgumentException($"{url} is not a valid FTP hostname");
            }

            // If username/password were included in url then set network credentials accordingly
            Credentials = InitCredentials(_baseUri.UserInfo);
            
        }

        private NetworkCredential InitCredentials(string userInfo = null)
        {
            if (string.IsNullOrEmpty(userInfo))
            {
                return new NetworkCredential("anonymous", "anonymous@domain.com");
            }
            else
            {
                var userInfoParts = userInfo.Split(':');
                return new NetworkCredential
                {
                    UserName = userInfoParts.ElementAt(0),
                    Password = userInfoParts.ElementAtOrDefault(1)
                };
            }
        }

        internal FtpWebResponse MakeRequest(Enums.RequestType requestType, string path = null)
        {
            var uri = new Uri(BaseUri, path ?? string.Empty);
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uri);

            req.Method = requestMap[requestType];
            req.UsePassive = UsePassiveMode;
            req.UseBinary = UseBinaryTransferMode;
            req.Credentials = Credentials ?? new NetworkCredential();
            req.KeepAlive = false;

            return (FtpWebResponse) req.GetResponse();

        }

        public async Task<MemoryStream> DownloadFileAsync(string relativePathToFile)
        {
            MemoryStream ms = new MemoryStream();

            if (string.IsNullOrWhiteSpace(relativePathToFile))
            {
                throw new ArgumentNullException("File path cannot be null or empty string");
            }

            var res = MakeRequest(Enums.RequestType.DOWNLOAD_FILE, relativePathToFile);

            using (var stream = res.GetResponseStream())
            {
                await stream.CopyToAsync(ms);
            }

            return ms;
        }
        
        public string HostName => BaseUri.Host;
        public Uri BaseUri => _baseUri;
        public bool UsePassiveMode { get; set; }
        public bool UseBinaryTransferMode { get; set; }
        public NetworkCredential Credentials { get; set; }
    }
}
