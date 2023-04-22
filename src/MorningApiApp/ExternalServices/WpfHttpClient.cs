using System;
using System.Net.Http;
using System.Threading;

namespace MorningApiApp.ExternalServices
{
    internal abstract class WpfHttpClient : IWpfHttpClient
    {
        internal virtual string GetHttpResponse(string url)
        {
            string result = string.Empty;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                    _ = response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    break;
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                }
            }

            return result;
        }
    }
}
