using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm
{
    public class Test<TResult>
    {
        private Action action;
        private Func<TResult> function;
        public Test(Func<TResult> func)
        {
            function = func;
            ResultList = new List<TResult>();
            RunTimeList = new List<TimeSpan>();
            haveResult = true;
        }
        bool haveResult = false;
        public Test(Action act)
        {
            action = act;
            RunTimeList = new List<TimeSpan>();
        }

        public object Result { get; private set; }

        public List<TResult> ResultList { get; private set; }

        public TimeSpan RunTime { get; private set; }

        public List<TimeSpan> RunTimeList { get; private set; }

        public bool Start()
        {
            Stopwatch sw = new Stopwatch();
            if (haveResult)
            {
                try
                {
                    sw.Start();
                    Result = function();
                    sw.Stop();
                    RunTime = sw.Elapsed;
                    return true;
                }
                catch (Exception ex)
                {
                    CausedException = ex;
                    sw.Stop();
                    RunTime = sw.Elapsed;
                    return false;
                }
            }
            else
            {
                try
                {
                    sw.Start();
                    action();
                    sw.Stop();
                    RunTime = sw.Elapsed;
                    return true;
                }
                catch (Exception ex)
                {
                    CausedException = ex;
                    sw.Stop();
                    RunTime = sw.Elapsed;
                    return false;
                }
            }

        }

        public bool Start(int times)
        {
            Stopwatch sw = new Stopwatch();
            if (haveResult)
            {
                for (int i = 0; i < times; i++)
                {
                    try
                    {
                        sw.Start();
                        ResultList.Add(function());
                        sw.Stop();
                        RunTimeList.Add(sw.Elapsed);
                        sw.Reset();
                    }
                    catch (Exception ex)
                    {
                        CausedException = ex;
                        sw.Stop();
                        RunTime = sw.Elapsed;
                        return false;
                    }
                }
                return true;
            }
            else
            {
                for (int i = 0; i < times; i++)
                {
                    try
                    {
                        sw.Start();
                        action();
                        sw.Stop();
                        RunTimeList.Add(sw.Elapsed);
                        sw.Reset();
                    }
                    catch (Exception ex)
                    {
                        CausedException = ex;
                        sw.Stop();
                        RunTime = sw.Elapsed;
                        return false;
                    }
                }
                return true;
            }
        }

        public Exception CausedException { get; private set; }

        public override string ToString()
        {
            string str = "";
            if (RunTime == TimeSpan.Zero && RunTimeList.Count == 0)
            {
                return "未进行过测试";
            }
            else
            {
                if (RunTime != TimeSpan.Zero)
                {
                    str = "单次测试结果：" + Environment.NewLine;
                    if (haveResult)
                    {
                        str += "返回值：" + Result + Environment.NewLine;
                    }
                    str += "       执行时间：" + RunTime.ToString() + Environment.NewLine;
                }
                if (RunTimeList.Count > 0)
                {
                    str += "多次测试结果：" + Environment.NewLine;
                    for (int i = 0; i < RunTimeList.Count; i++)
                    {
                        str += string.Format("{0:000}", i) + "：    ";
                        if (haveResult)
                        {
                            str += "返回" + string.Format("{0,36}", ResultList[i]);
                        }
                        str += "       执行时间             " + RunTimeList[i].ToString() + Environment.NewLine;
                    }
                }
                if (CausedException != null)
                {
                    str += "发生过异常：" + Environment.NewLine + CausedException.ToString();
                }
                return str;
            }
        }

    }
    public class Test : Test<object>
    {
        public Test(Func<object> func) : base(func)
        {
        }
        public Test(Action act) : base(act)
        {
        }
    }
}
