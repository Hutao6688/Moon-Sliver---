using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Native.XQ.SDK.Event.EventArgs;
using Native.XQ.SDK.Interfaces;

namespace MoonSliver.Code
{
    public class Event_GroupMessage : IXQGroupMessage
    {
        public static bool isOn = false;

        public void GroupMessage(object sender, XQAppGroupMsgEventArgs e)
        {

            if (e.FromQQ.Id == e.RobotQQ)
            {
                return;
            }
            //手动的全体12s禁言策略
            if (e.Message.Text == "苏醒吧，冰霜的主人!")
            {
                isOn = true;
                if (isOn)
                {
                    e.FromGroup.SendMessage(e.RobotQQ, "...力量，请借我一用！");
                    Task.Factory.StartNew(async () =>
                    {
                        await Task.Delay(1000);
                        e.FromGroup.SendMessage(e.RobotQQ, "显现吧，白银之月！");
                        e.FromGroup.ShutUpAll(e.RobotQQ);
                        await Task.Delay(12000);
                        e.FromGroup.SendMessage(e.RobotQQ, "极寒风暴！");
                        e.FromGroup.UnShutUpAll(e.RobotQQ);
                        isOn = false;
                    });
                }
            }

            if (MoonSliver.Settings.FliterOn)
            {
                //检测是否有符合的敏感词
                foreach (var item in MoonSliver.KeyWordsDic)
                {
                    var word = item.Value.FindAll(w => e.Message.Text.Contains(w));
                    if ( word !=null)
                    {
                        if (word.Count < 4 )
                        {
                            return;
                        }
                        isOn = true;
                        //符合的违禁词
                        e.XQAPI.Log($"{e.FromQQ.Id}触发了敏感词");

                        if (MoonSliver.Settings.WithdrawAll)
                        {
                            if (!MoonSliver.Settings.WithdrawDelay)
                            {
                                e.Message.Withdraw(e.RobotQQ, e.FromGroup.Id);//立刻撤回
                            }
                            else
                            {
                                Task.Factory.StartNew(async () =>
                                {
                                    e.FromGroup.SendMessage(e.RobotQQ, "苏醒吧，冰" + (new Random().NextDouble() <= 0.5 ? "霜" : "箱") + "的主人！");
                                    await Task.Delay(2000);
                                    //随机处理
                                    var r = new Random().NextDouble();
                                    if (r < 0.1)
                                    {
                                        e.FromGroup.SendMessage(e.RobotQQ, "德莉莎其实是个很可爱...");
                                        await Task.Delay(400);
                                        e.FromGroup.SendMessage(e.RobotQQ, "哦不...抱歉我失态了");
                                    }
                                    else if (r < 0.2)
                                    {
                                        e.FromGroup.SendMessage(e.RobotQQ, "万能的白银之月...你说我要怎么对付刚刚那个人呢...");
                                    }
                                    else if (r < 0.3)
                                    {
                                        e.FromGroup.SendMessage(e.RobotQQ, "我刚刚说的是冰霜..还是冰箱来着");
                                    }
                                    await Task.Delay(1000);
                                    e.FromGroup.SendMessage(e.RobotQQ, "上吧，白银之月！");
                                    e.Message.Withdraw(e.RobotQQ, e.FromGroup.Id);
                                    await Task.Delay(1000);
                                });
                            }
                        }

                        if (MoonSliver.Settings.ShutUp)
                        {
                            e.FromGroup.SendMessage(e.RobotQQ, "[@" + e.FromQQ.Id + "] 陷入寒冰之中吧！");
                            e.FromGroup.ShutUpMember(e.RobotQQ, e.FromQQ.Id, MoonSliver.Settings.ShutUpTime);
                        }

                        //记录频率
                        if (MoonSliver.Settings.ShutUpAll) {
                            if (!MoonSliver.Frequency.ContainsKey(e.FromGroup.Id))
                            {
                                //处理不存在key
                                List<TimeSpan> times = new List<TimeSpan>();
                                MoonSliver.Frequency.Add(e.FromGroup.Id,times);
                            }

                            MoonSliver.Frequency[e.FromGroup.Id].Add(DateTime.Now.TimeOfDay);
                            if(MoonSliver.Frequency[e.FromGroup.Id].Count(s=> DateTime.Now.TimeOfDay - s <= TimeSpan.FromSeconds(60)) >= 10)
                            {
                                //1分钟之内出现十条违禁词，直接触发

                                Task.Factory.StartNew(async () =>
                                {
                                    e.FromGroup.SendMessage(e.RobotQQ, "苏醒吧，冰" + (new Random().NextDouble() <= 0.5 ? "霜" : "箱") + "的主人！");
                                    await Task.Delay(2000);
                                    //随机处理
                                    var r = new Random().NextDouble();

                                    if (r < 0.1)
                                    {
                                        e.FromGroup.SendMessage(e.RobotQQ, "恒霜之斯卡蒂...来了吗");
                                        await Task.Delay(400);
                                        e.FromGroup.SendMessage(e.RobotQQ, "哈！协助我吧，白银之月");
                                    }
                                    else if (r < 0.2)
                                    {
                                        e.FromGroup.SendMessage(e.RobotQQ, "K423的行踪...唔...现在似乎并不是考虑这个问题的时候");
                                    }
                                    else if (r < 0.3)
                                    {
                                        e.FromGroup.SendMessage(e.RobotQQ, "放风时间到了!哦不对...好像不是那个装甲...");
                                    }

                                    await Task.Delay(1000);
                                    e.FromGroup.SendMessage(e.RobotQQ, "极寒风暴！");
                                    e.FromGroup.ShutUpAll(e.RobotQQ);
                                    await Task.Delay((int)TimeSpan.FromMinutes(5).TotalMilliseconds);//禁言5分钟
                                    e.FromGroup.UnShutUpAll(e.RobotQQ);
                                });

                            }

                            //清理赘余
                            if(MoonSliver.Frequency[e.FromGroup.Id].Count >= 100)
                            {
                                var c = MoonSliver.Frequency[e.FromGroup.Id].RemoveAll(s=> DateTime.Now.TimeOfDay - s >= TimeSpan.FromSeconds(120));//清理超过120s的记录
                                e.XQAPI.Info("清理了"+c.ToString()+"个赘余记录");
                            }
                        } 

                        isOn = false;
                        return;
                    }
                }
            }
        }
    }
}