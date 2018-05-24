using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

namespace Slacker
{
    class Slack
    {
        public string Token;

        public Slack(string Token)
        {
            this.Token = Token;
        }

        public string SendGetApiRequest(string ApiCall, params string[] Args)
        {
            string Request;
            if (Args.Length == 0)
            {
                Request = "https://slack.com/api/" + ApiCall + "?token=" + Token;
            }
            else
            {
                Request = "https://slack.com/api/" + ApiCall + "?token=" + Token + "&" + string.Join("&", Args);
            }

            HttpClient Client = new HttpClient();
            HttpContent content = new StringContent(Request);
            content.Headers.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage Responce = Client.GetAsync(Request).Result;

            return Responce.Content.ReadAsStringAsync().Result;
        }

        public string SendPostApiRequest(string ApiCall, params string[] Args)
        {
            string UrlContent = "token=" + Token + "&" + string.Join("&", Args);

            HttpContent Content = new StringContent(UrlContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpClient Client = new HttpClient();
 
            HttpResponseMessage Responce = Client.PostAsync("https://slack.com/api/" + ApiCall, Content).Result;

            return Responce.Content.ReadAsStringAsync().Result;
        }

        public byte[] DownloadSlackUrl(string Url)
        {
            HttpClient DownloadClient = new HttpClient();

            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, Url);

            Request.Headers.Add("Authorization", "Bearer " + Token);

            HttpResponseMessage Responce = DownloadClient.SendAsync(Request).Result;

            return Responce.Content.ReadAsByteArrayAsync().Result;
        }
    }
}
