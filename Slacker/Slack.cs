using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

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

        public byte[] DownloadBytes(string Url)
        {
            HttpClient DownloadClient = new HttpClient();
            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, Url);
            Request.Headers.Add("Authorization", "Bearer " + Token);

            HttpResponseMessage Responce = DownloadClient.SendAsync(Request).Result;
            return Responce.Content.ReadAsByteArrayAsync().Result;
        }

        public void DownloadToFile(string Url, string Path)
        {
            byte[] Bytes = DownloadBytes(Url);
            File.WriteAllBytes(Path, Bytes);
        }

        public JArray GetFiles()
        {
            string Responce = SendGetApiRequest("files.list");
            JObject ListObj = JObject.Parse(Responce);
            return ListObj.Value<JArray>("files");
        }

        public JArray GetFilesFrom(string ChannelID)
        {
            string Responce = SendGetApiRequest("files.list", "channel=" + ChannelID);
            JObject ListObj = JObject.Parse(Responce);
            return ListObj.Value<JArray>("files");
        }

        public JArray GetChannels()
        {
            string Responce = SendGetApiRequest("channels.list");
            JObject ListObj = JObject.Parse(Responce);
            return ListObj.Value<JArray>("channels");
        }

        public string GetChannelId(string ChannelName)
        {
            JArray Channeles = GetChannels();
            foreach (JObject Channel in Channeles)
            {
                string Name = Channel.Value<string>("name");
                string Id = Channel.Value<string>("id");
                if (Name == ChannelName)
                {
                    return Id;
                }
            }
            return null;
        }
    }
}
