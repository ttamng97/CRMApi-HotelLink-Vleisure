using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace CRMapi.Cls
{
    public static class LogWriter
    {
        public static void Log(string action, string request, string response, string error)
        {
            DateTime now = DateTime.Now;
            try
            {
                string logfile = Directory.GetCurrentDirectory() + "\\Logs\\" + action.ToString() + "_" + now.ToString("yyyy-MM-dd") + ".txt";
                using (StreamWriter w = System.IO.File.AppendText(logfile))
                {
                    try
                    {
                        dynamic param = new System.Dynamic.ExpandoObject();
                        param.time = now.ToString("yyyy-MM-dd HHmmss \"GMT\"zzz");
                        if (!string.IsNullOrEmpty(request)) { param.rq = request; }
                        if (!string.IsNullOrEmpty(response)) { param.rs = response; }
                        if (!string.IsNullOrEmpty(error)) { param.er = error; }
                        w.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(param));
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
