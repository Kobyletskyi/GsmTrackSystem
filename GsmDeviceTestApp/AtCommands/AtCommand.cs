using ComPort_Library;
using Sim900.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sim900.AtCommand
{
    public class AtCmd
    {
        private string command;
        private const string successText = "OK";
        private const string errorText = "ERROR";
        string buffer = string.Empty;
        bool waitingForResponse = false;
        CommPort port = CommPort.Instance;

        public AtCmd(string comm)
        {
            command = comm;
        }

        public AtResult Run(bool handleResponse = true)
        {

            //if port is closed 
            //port.Open();
            //open port

            //send commant to the port
            port.Send(command);
            var result = new AtResult();
            if (handleResponse)
            {
                //waiting for the response
                buffer = string.Empty;
                waitingForResponse = true;
                Action<string> OnDataReceived = (data) => { };
                OnDataReceived = (dataIn) => {
                    //port can send piece of bytes

                    //making full command response 
                    buffer += dataIn;

                    //If response has the end of answer 'OK'
                    if (buffer.Contains(successText))
                    {
                        result.Status = Status.OK;
                        //stop listening
                        port.DataReceived -= OnDataReceived;
                        waitingForResponse = false;
                    }
                    if (buffer.Contains(errorText))
                    {
                        result.Status = Status.ERROR;
                        //stop listening
                        port.DataReceived -= OnDataReceived;
                        waitingForResponse = false;
                    }
                };
                port.DataReceived += OnDataReceived;

                while (waitingForResponse)
                {
                    Thread.Sleep(new TimeSpan(0, 0, 1));
                }

                var tmp = new string(buffer.ToCharArray().Where(c => !Char.IsControl(c)).ToArray());
                tmp = tmp.Trim();
                tmp = Regex.Replace(tmp, "^" + Regex.Escape(command), String.Empty, RegexOptions.IgnoreCase);
                tmp = Regex.Replace(tmp, @"\s*" + successText + @"\s*$", String.Empty, RegexOptions.IgnoreCase);
                result.Data = tmp.Trim();
            }
            return result;
        }

    }
}
