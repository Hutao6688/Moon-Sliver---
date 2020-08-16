using System;
using Newtonsoft.Json;

namespace MoonSliver.Code
{
    [Serializable]
    public class Setting
    {
        public Setting()
        {
            FliterOn = true;
            SuperMode = false;
            WithdrawAll = true;
            WithdrawDelay = true;
            ShutUp = true;
            ShutUpTime = 600;
            ShutUpAll = true;
            ShutUpAllDelay = true;
        }

        public bool FliterOn { get; set; }

        public bool SuperMode { get; set; }

        public bool WithdrawAll { get; set; }

        public bool WithdrawDelay { get; set; }

        public bool ShutUp { get; set; }

        public int ShutUpTime { get; set; }

        public bool ShutUpAll { get; set; }

        public bool ShutUpAllDelay { get; set; }

        /// <summary>
        /// 利用JSON获取Setting
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Setting GetSetting(string json)
        {
            try
            {
                var set = JsonConvert.DeserializeObject<Setting>(json);
                if (set == null)
                {
                    set = new Setting();
                }
                return set;
            }
            catch (Exception ex)
            {
                return new Setting();
            }
        }
    }
}