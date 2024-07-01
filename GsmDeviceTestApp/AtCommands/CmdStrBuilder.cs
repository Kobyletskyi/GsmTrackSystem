using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim900.AtCommand
{
    public class CmdStrBuilder
    {
        public readonly string Text;

        public CmdStrBuilder(string comm)
        {
            Text = comm;
        }
        public string Read { get { return string.Format("{0}?", getCommandStr()); } }
        public string Test { get { return string.Format("{0}=?", getCommandStr()); } }
        public string Prefix { get { return string.Format("+{0}:", Text); } }
        public string Set(params object[] args)
        {
            return string.Format("{0}={1}", getCommandStr(), getParamsStr(args));
        }
        public override string ToString()
        {
            return getCommandStr();
        }

        private string getCommandStr()
        {
            return string.Format("AT+{0}", Text);
        }
        private string getParamsStr(params object[] args)
        {
            string str = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is String)
                {
                    str += string.Format("\"{0}\"", args[i]);
                }
                else
                {
                    str += args[i];
                }
                if (i != args.Length - 1)
                {
                    str += ",";
                }

            }
            return str;
        }
    }
}
