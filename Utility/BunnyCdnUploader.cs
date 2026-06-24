using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Utilities;

namespace Utilities
{
    public static class BunnyCdnUploader
    {
        private static readonly HttpClient _http = new HttpClient();
        private static string uploadUrl = ConfigurationManager.AppSettings["UploadUrl"];
        private static string accessKey = ConfigurationManager.AppSettings["AccessKey"];
        //public BunnyCdnUploader()
        //{
        //    accessKey = ConfigurationManager.AppSettings["AccessKey"];
        //    string uploadUrl = ConfigurationManager.AppSettings["UploadUrl"];
        //    _uploadUrl = uploadUrl.Replace("${FolderName}", folder);
        //}
        public static async Task<string> UploadAsync(HttpPostedFile file, string folder)
        {
            uploadUrl = uploadUrl.Replace("${FolderName}", folder);

            var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var targetUrl = uploadUrl.TrimEnd('/') + "/" + folder + "/" + uniqueName;
            var bytes = new byte[file.ContentLength];
            file.InputStream.Read(bytes, 0, bytes.Length);
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var request = new HttpRequestMessage(HttpMethod.Put, targetUrl);
            request.Content = content;
            request.Headers.Add("AccessKey", accessKey);
            var res = await _http.SendAsync(request);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    "CDN upload failed: " + (int)res.StatusCode + " " + res.ReasonPhrase + " | " + body);
            }
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            return baseUrl.TrimEnd('/') + "/" + folder + "/" + uniqueName;
        }
    }
}
