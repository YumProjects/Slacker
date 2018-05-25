using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Slacker
{
    class Program
    {
        static Slack slack;

        static void Main(string[] args)
        {
            Properties.Settings settings = new Properties.Settings();

            slack = new Slack(settings.Token);
            JArray Files;

            if(args.Length > 0)
            {
                string ChannelID = slack.GetChannelId(args[0]);
                Console.WriteLine("Chanel Id of \"" + args[0] + "\" is " + ChannelID);
                Files = slack.GetFilesFrom(ChannelID);
            }
            else
            {
                Files = slack.GetFiles();
            }

            foreach(JObject FileObj in Files)
            {
                string ID = FileObj.Value<string>("id");
                string Name = FileObj.Value<string>("name");
                string Url = FileObj.Value<string>("url_private");

                Console.WriteLine("Downloading \"" + Name + "\"...");

                string FilePath = MakeValidPath("C:\\Users\\Benny\\Desktop\\Slack\\" + Name);

                slack.DownloadToFile(Url, FilePath);
            }

            Console.ReadLine();
        }

        static string MakeValidPath(string path)
        {
            string tmp = path;

            foreach (char c in Path.GetInvalidPathChars())
            {
                tmp = tmp.Replace(c, '_');
            }

            string FolderPath = Path.GetDirectoryName(tmp);
            string FullName = Path.GetFileName(tmp);
            string Name = Path.GetFileNameWithoutExtension(tmp);
            string Extention = Path.GetExtension(tmp);

            if (File.Exists(tmp))
            {
                int i = 1;
                while (File.Exists(tmp))
                {
                    tmp = FolderPath + "\\" + Name + "_" + i + Extention;
                    i++;
                }
            }
            return tmp;
        }
    }
}