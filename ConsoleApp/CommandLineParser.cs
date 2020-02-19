using System;
using System.Linq;

namespace Sunsets.ConsoleApp
{
    public class CommandLineParser
    {
        public bool ParseFlag(string flag, string[] args)
        {
            return args.Contains(flag);
        }

        public string ParseSetting(string setting, string[] args)
        {
            string versionArg;
            if ((versionArg = args.FirstOrDefault(arg => arg.Contains($"{setting}="))) != null)
            {
                return versionArg.Split('=')[1].Replace('~', ' ');
            }
            else
            {
                return null;
            }
        }

        
    }
}