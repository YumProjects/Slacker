using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Net.Http;

namespace Slacker
{
    class Program
    {
        static string Token = "xoxp-166756780596-250867385107-368693279941-b9c4e0600d550b80f91a5eded5f1c4cc";
        static Slack slack = new Slack(Token);

        static void Main(string[] args)
        {
            Console.WriteLine("Retrieving files list...");
            JArray Files = GetFilesList();

            foreach(JObject FileObj in Files)
            {
                string ID = FileObj.Value<string>("id");
                string Name = FileObj.Value<string>("name");
                string Url = FileObj.Value<string>("url_private");

                Console.WriteLine("Downloading \"" + Name + "\"...");

                string FilePath = "C:\\Users\\Benny\\Desktop\\Slack\\" + MakeValidFileName(Name);

                byte[] Data = slack.DownloadSlackUrl(Url);

                File.WriteAllBytes(FilePath, Data);
            }

            Console.ReadLine();
        }

        static JArray GetFilesList()
        {
            string Responce = slack.SendPostApiRequest("files.list");

            JObject ListObj = JObject.Parse(Responce);

            return ListObj.Value<JArray>("files");
        }

        static string MakeValidFileName(string FileName)
        {
            string ValidFileName = FileName;

            foreach(char c in Path.GetInvalidFileNameChars())
            {
                ValidFileName = ValidFileName.Replace(c, '_');
            }

            return ValidFileName;
        }
    }
}