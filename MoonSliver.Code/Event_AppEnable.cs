using System;
using Native.XQ.SDK.Event.EventArgs;
using Native.XQ.SDK.Interfaces;

namespace MoonSliver.Code
{
    public class Event_AppEnable : IXQAppEnable
    {
        public void AppEnable(object sender, XQEventArgs e)
        {
            try
            {
                e.XQAPI.Log("MoonSliver正在加载敏感词库...");
                MoonSliver.LoadDic(e.XQAPI.AppDirectory);
                MoonSliver.LoadSetting(e.XQAPI.AppDirectory);
                e.XQAPI.Log($"MoonSliver成功加载了{MoonSliver.KeyWordsDic.Count}个词库文件");
            }
            catch (Exception ex)
            {
                e.XQAPI.Info("[错误]","MoonSliver加载词库失败...");

            }
        }
    }
}