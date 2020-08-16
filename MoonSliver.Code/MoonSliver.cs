using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MoonSliver.Code
{
    public class MoonSliver
    {
        public static Dictionary<string, List<string>> KeyWordsDic { get; set; } 
        public static Setting Settings { get; set; }

        public static Dictionary<string, List<TimeSpan>> Frequency;

        public static void LoadDic(string path)
        {
            
            KeyWordsDic = new Dictionary<string, List<string>>();
            Frequency = new Dictionary<string, List<TimeSpan>>();
            DirectoryInfo info = new DirectoryInfo(Path.Combine(path,"Keywords"));
            if (!info.Exists)
                info.Create();
            foreach (var item in info.GetFiles())
            {
                List<string> strs = new List<string>();
                strs.AddRange(File.ReadAllLines(item.FullName));
                KeyWordsDic.Add(Path.GetFileNameWithoutExtension(item.Name),strs);
            }
            //加载完成
        }

        public static void LoadSetting(string path)
        {
            var p = Path.Combine(path, "Setting.json");
            if (!File.Exists(p))
                File.Create(p).Close();
            Settings = Setting.GetSetting(File.ReadAllText(p));
        }

        public static void Save(string path)
        {
            var p = Path.Combine(path, "Setting.json");
            if (!File.Exists(p))
                File.Create(p).Close();
            File.WriteAllText(p, JsonConvert.SerializeObject(Settings));
        }
    }
}
