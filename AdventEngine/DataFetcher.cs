using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace AdventEngine
{
    public static class DataFetcher
    {
        private static async Task<string> GetWebContentAsync(string url, string session_val = "53616c7465645f5fab056406237e596303fd2e83cb037e3b7a7f0f7a7a2db9c6f0479afbf715fc75d6815afba87765d5fb571c2aae8d13c127425774cfbe468a")
        {
            using (HttpClient client = new HttpClient())
            {
                // Set request headers here
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                client.DefaultRequestHeaders.Add("Cookie", $"_ga=GA1.2.246773438.1701412137; session={session_val}; _gid=GA1.2.1356576185.1702755277; _ga_MHSNPJKWC7=GS1.2.1702755277.2.1.1702755326.0.0.0");
                // Add other headers as needed

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // If the response content is already decompressed by HttpClient, just read it as string
                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    using (var streamReader = new StreamReader(decompressedStream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
                else
                {
                    // If the content is not gzip-encoded, just read it as a normal string
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private static void WriteContentToFile(string content, string filePath)
        {
            File.WriteAllText(filePath, content);
        }


        public static async Task Fetch(string[]? args=null)
        {
            string url = "https://adventofcode.com/2023/day/1/input";
            string content = await GetWebContentAsync(url);

            string filePath = "file.txt"; // Replace with your desired file path
            WriteContentToFile(content, filePath);

            Console.WriteLine($"Content written to {filePath}");
        }
    }
}