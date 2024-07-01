using System.Collections.Specialized;
using ComPort_Library;
using Sim900.AtCommand;
using Sim900.DTO;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sim900
{
    public class Sim900Module : ISim900, IDisposable
    {
        private char comaSeparator = ',';
        private char quotes = '"';
        private string AMR_STOP = "AMR_STOP";

        public event Action<string> OnPortNewData;
        public event Action<string> OnSMS;
        public event Action<IIncCallResponse> OnIncommingCall;


        public bool IsReady
        {
            get { return ready; }
        }
        public bool Sleepping
        {
            get { return sleep; }
        }


        #region public interface

        #region File System

        public IList<FileInfo> GetFilesList()
        {
            var cmd = CmdStrFactory.FilesList;
            string[] listCmdEcho = new[] { cmd.Prefix };
            Func<string, FileInfo> parseFileInfo = (fileInfo) =>
            {
                var parts = fileInfo.Split(comaSeparator);
                var res = new FileInfo() { Name = parts[0].Trim(quotes), Size = Int64.Parse(parts[1]) };
                return res;
            };
            var result = runCmd(CmdStrFactory.GetFlashBuffer.ToString());
            var success = (result.Status == Status.OK);
            List<FileInfo> files = null;
            if (success)
            {
                result = runCmd(cmd.ToString());
                if (result.Status == Status.OK)
                {
                    var tmp = result.Data
                        .Split(listCmdEcho, StringSplitOptions.None);
                    files = tmp
                        .Where(fi => !string.IsNullOrWhiteSpace(fi))
                        .Select(parseFileInfo)
                        .ToList();
                }
            }
            runCmd(CmdStrFactory.FreeFlash.ToString());
            return files;
        }
        public bool WriteFile(string name, byte[] data)
        {
            bool result = false;
            var answer = runCmd(CmdStrFactory.GetFlashBuffer.ToString());
            if (answer.Status == Status.OK)
            {
                var timeout = 300000;//data.Length*2
                var res = runCmd(CmdStrFactory.WriteFileToFlash.Set(name, 0, data.Length, timeout), false);
                port.DataReceived += (dataIn) =>
                {
                    if (dataIn.Contains("CONNECT"))
                    {
                        port.Send(data);
                    }
                };

                Thread.Sleep(timeout);
                runCmd(CmdStrFactory.FreeFlash.ToString());
            }
            return result;
        }

        public byte[] ReadFile(string name)
        {
            byte[] result = null;
            var fileSize = GetFileSize(name);
            if (fileSize.HasValue)
            {
                var answer = runCmd(CmdStrFactory.GetFlashBuffer.ToString());
                var success = (answer.Status == Status.OK);
                if (success)
                {
                    var cmd = CmdStrFactory.ReadFileFromFlash;
                    answer = runCmd(cmd.Set(name, 0, fileSize, 0));
                    if (answer.Status == Status.OK)
                    {
                        var fileStr = answer.Data.Replace(cmd.Prefix + " " + fileSize, string.Empty).Trim();
                        result = Encoding.ASCII.GetBytes(fileStr);

                        //byte[] bytes = new byte[fileStr.Length * sizeof(char)];
                        //System.Buffer.BlockCopy(fileStr.ToCharArray(), 0, bytes, 0, bytes.Length);
                    }
                }
                runCmd(CmdStrFactory.FreeFlash.ToString());
            }
            return result;
        }
        public long? GetFileSize(string name)
        {
            long? result = null;
            var answer = runCmd(CmdStrFactory.GetFlashBuffer.ToString());
            var success = (answer.Status == Status.OK);
            if (success)
            {
                var cmd = CmdStrFactory.GetFileSize;
                answer = runCmd(cmd.Set(name));
                if (answer.Status == Status.OK)
                {
                    var size = answer.Data
                        .Replace(cmd.Prefix, string.Empty)
                        .Trim();
                    result = long.Parse(size);
                }
            }
            runCmd(CmdStrFactory.FreeFlash.ToString());
            return result;
        }
        public bool RenameFile(string name, string newName)
        {
            var result = true;
            if (name != newName)
            {
                var answer = runCmd(CmdStrFactory.RenameFile.Set(name, newName));
                result = answer.Status == Status.OK;
            }
            return result;
        }
        public bool DeleteFile(string name)
        {
            var answer = runCmd(CmdStrFactory.DelFileFromFlash.Set(name));
            var success = answer.Status == Status.OK;
            return success;
        }
        public bool PlayFile(string name)
        {
            var answer = runCmd(CmdStrFactory.PlayAMR.Set(name, 0));
            var success = answer.Status == Status.OK;
            return success;
        }

        #endregion

        #region Network

        public List<string> GetProviders()
        {
            var answer = runCmd((CmdStrFactory.Provider.Test));
            var parts = answer.Data.Split(comaSeparator);
            return parts.Select(p => p.Trim(new[] { '(', ')' })).ToList();
        }

        public void StartGPRS()
        {
            //Получаем сведения о соединении и IP адрес
            var res = runCmd(CmdStrFactory.SetupGPRS.Set(2, 1));
            if (res.Data.Contains("SAPBR: 1,3"))
            {
                //res = runCmd(CmdStrFactory.GPRS.Set(3, 1,"CONTYPE", "GPRS"));
                //res = runCmd(CmdStrFactory.GPRS.Set(3, 1,"APN", "internet"));
                //Устанавливаем GPRS соединение
                res = runCmd(CmdStrFactory.SetupGPRS.Set(1, 1));
                res = runCmd(CmdStrFactory.SetupGPRS.Set(2, 1));
            }
        }

        public void StopGPRS()
        {
            var res = runCmd(CmdStrFactory.SetupGPRS.Set(2, 1));
            if (res.Data.Contains("SAPBR: 1,1"))
            {
                //Закрываем GPRS соединение
                res = runCmd(CmdStrFactory.SetupGPRS.Set(0, 1));
            }
        }

        public GeoCoordinate GetLocation()
        {
            StartGPRS();
            var cmd = CmdStrFactory.CellLocation;
            var answer = runCmd(cmd.Set(1, 1));//+CIPGSMLOC: 0,60.603438, 56.838486,2013/10/03,16:34:38
            GeoCoordinate coord = null;
            if (answer.Status == Status.OK)
            {
                coord = new GeoCoordinate();
                var parts = answer.Data
                    .Replace(cmd.Prefix, string.Empty)
                    .Split(comaSeparator);
                int status = Int16.Parse(parts[0]);
                if (status == 0 && parts.Length == 5)
                {
                    double longitude = double.Parse(parts[1]);
                    double latitude = double.Parse(parts[1]);
                    DateTime date = DateTime.Parse(parts[3]);
                    TimeSpan time = TimeSpan.Parse(parts[4]);
                    coord = new GeoCoordinate(latitude, longitude);
                }
            }
            StopGPRS();

            return coord;
        }

        public string USSD(string ussd, long timeout = 60000, bool ignoreResponse = false)
        {
            var cmd = CmdStrFactory.Ussd;
            var result = string.Empty;
            var answer = runCmd(cmd.Set(Convert.ToInt16(!ignoreResponse), ussd));
            if (answer.Status == Status.OK && !ignoreResponse)
            {
                var startTime = DateTime.Now;
                Action<string> handleUssdResult = (dataIn) => { };
                handleUssdResult = (dataIn) =>
                {
                    if (dataIn.Contains(cmd.Prefix))
                    {
                        result = dataIn
                        .Replace(cmd.Prefix, string.Empty)
                        .Trim(); ;
                        port.DataReceived -= handleUssdResult;
                    }
                };
                port.DataReceived += handleUssdResult;
                while (string.IsNullOrWhiteSpace(result))
                {
                    if (DateTime.Now.Subtract(startTime).TotalMilliseconds < timeout)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        throw new TimeoutException(string.Format("Ussd timeout {0}", timeout));
                    }
                }
            }
            else
            {
                throw new Exception(answer.Data);
            }
            return result;
        }

        public ICallResponse Call(string number)
        {
            //validate number to +380961234567
            CallResponse result = new CallResponse();
            string dialCmd = string.Format("{0}{1};", "ATD", number);
            var res = runCmd(dialCmd);
            Action waitAnswer = () =>
            {
                var statusCmd = CmdStrFactory.Status;
                while (true)
                {
                    var r = runCmd(statusCmd.ToString());
                    var statCode = r.Data.Replace(statusCmd.Prefix, string.Empty).Trim();
                    if (statCode == "4")
                    {
                        result.FireOnAnswer();
                    }
                }
            };
            Task.Factory.StartNew(waitAnswer);


            Action<string> onDtmf = (dataIn) =>
            {
                var message = string.Empty;
                var prefixDTMF = "+DTMF:";
                if (dataIn.Contains(prefixDTMF))
                {
                    message = dataIn.Replace(prefixDTMF, "").Trim();
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    result.FireOnDTMF(message[0], new TimeSpan());
                }
            };

            Action<string> onDialResult = (dataIn) => { };

            onDialResult = (dataIn) =>
            {
                var message = string.Empty;

                if (dataIn.Contains("NO DIALTONE"))
                {
                    message = "NO DIALTONE";
                }
                if (dataIn.Contains("BUSY"))
                {
                    message = "BUSY";
                }
                if (dataIn.Contains("NO CARRIER"))
                {
                    message = "NO CARRIER";
                }
                if (dataIn.Contains("NO ANSWER"))
                {
                    message = "NO ANSWER";
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    port.DataReceived -= onDialResult;
                    port.DataReceived -= onDtmf;
                    result.FireOnFinish(message);
                }
            };

            port.DataReceived += onDialResult;
            port.DataReceived += onDtmf;

            return result;
        }

        public bool AnswerIncCall()
        {
            //runCmd(CmdStrFactory.DTMF.Set(1,0));

            runCmd("ATA", false);

            //runCmd(CmdStrFactory.DTMF.Set(2,0));
            //check status

            return true;
        }

        public void SendSMS(string number, string text)
        {
            //block port

            var str = string.Format("AT+CMGS=\"{0}\"", number, text, (char)26);
            runCmd(str, false);
            //some dalay
            str = text + (char)26;
            runCmd(str);


            //unblock port
        }

        public void ScanNetwork()
        {
            runCmd(CmdStrFactory.SignalStrength.ToString());
            //runCmd(CmdStrFactory.CENG.Set(3, 1));
            //runCmd(CmdStrFactory.CENG.Read);
            //runCmd(CmdStrFactory.NetScan.Set(1));
           // runCmd(CmdStrFactory.CENG.Set(1, 1));
            //runCmd(CmdStrFactory.CENG.Read);
            //runCmd(CmdStrFactory.NetScan.Set(1));
            runCmd(CmdStrFactory.CENG.Set(3, 1));
            runCmd(CmdStrFactory.CENG.Read);
            runCmd(CmdStrFactory.NetScan.Set(1));
            //runCmd(CmdStrFactory.CENG.Set(1, 0));
            //runCmd(CmdStrFactory.CENG.Read);
            //runCmd(CmdStrFactory.NetScan.Set(1));
            //runCmd(CmdStrFactory.CENG.Set(2, 0));
            //runCmd(CmdStrFactory.CENG.Read);
            //runCmd(CmdStrFactory.NetScan.Set(1));
            //runCmd(CmdStrFactory.CENG.Set(3, 0));
            //runCmd(CmdStrFactory.CENG.Read);
            //runCmd(CmdStrFactory.NetScan.Set(1));
            
        }

        #endregion

        #region Internet

        public string SendPostRequest(string host, string uri, NameValueCollection data)
        {
            string result = null;
            StartGPRS();
            var answer = runCmd(CmdStrFactory.HttpInit.ToString());
            if (answer.Status == Status.OK)
            {
                answer = runCmd(CmdStrFactory.HttpConfig.Set("CID", 1));
                answer = runCmd(CmdStrFactory.HttpConfig.Set("URL", uri));

                var timeout = 300000;//data.Length*2
                //runCmd(CmdStrFactory.HTTPDATA.Set(data.ToString().Length, timeout), false);
                //port.DataReceived += (dataIn) =>
                //{
                //    if (dataIn.Contains("DOWNLOAD"))
                //    {
                //        port.Send(data.ToString());
                //    }
                //};
                answer = runCmd(CmdStrFactory.HttpAction.Set(1));
                Thread.Sleep(timeout);

                answer = runCmd(CmdStrFactory.HttpTerm.ToString());
            }
            StopGPRS();
            return result;
        }

        public string SendGetRequest(string host, string uri)
        {
            string result = null;
            StartGPRS();
            var answer = runCmd(CmdStrFactory.HttpInit.ToString());
            if (answer.Status == Status.OK)
            {
                answer = runCmd(CmdStrFactory.HttpConfig.Set("CID", 1));
                answer = runCmd(CmdStrFactory.HttpConfig.Set("URL", uri));
                var cmd = CmdStrFactory.HttpAction;
                answer = runCmd(cmd.Set(0));
                if (answer.Status == Status.OK)
                {
                    long? dataSize = null;
                    port.DataReceived += (dataIn) =>
                    {
                        if (dataIn.Contains(cmd.Prefix))
                        {
                            dataIn = dataIn.Replace(cmd.Prefix, string.Empty);
                            var parts = dataIn.Split(',');
                            var status = parts[1].Trim();
                            dataSize = Int64.Parse( parts[2].Trim());
                        }
                    };

                    while (!dataSize.HasValue)
                    {
                        
                        Thread.Sleep(100);
                    }
                    var cmd2 = CmdStrFactory.HTTPREAD;
                    answer = runCmd(cmd2.ToString());
                    if (answer.Status == Status.OK)
                    {
                        result = answer.Data
                            .Replace(cmd2.Prefix, string.Empty).Trim()
                            .Replace(dataSize.ToString(), string.Empty).Trim();
                        //result = Encoding.ASCII.GetBytes(dataStr);

                        //byte[] bytes = new byte[fileStr.Length * sizeof(char)];
                        //System.Buffer.BlockCopy(fileStr.ToCharArray(), 0, bytes, 0, bytes.Length);
                    }
                }
                
                
                answer = runCmd(CmdStrFactory.HttpTerm.ToString());
            }
            StopGPRS();

            return result;
        }

        #endregion
        public string GetRevision()
        {
            var answ = runCmd(CmdStrFactory.Revision.ToString());
            return answ.Data;
        }

        public string GetIMEI()
        {
            var answ = runCmd(CmdStrFactory.IMEI.ToString());
            return answ.Data;
        }
        public void GotoSleep()
        {
            //runCommand("GotoSleep");
        }

        public void WakeUp()
        {
            //runCommand("WakeUp");
        }

        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private event Action<string> _onDialResult;

        private object _lock = new object();
        private bool ready = false;
        private bool sleep = false;
        private IncCallResponse incCallResult = null;
        private bool waitForNumber = true;
        private CommPort port;


        public void Init()
        {
            port = CommPort.Instance;
            port.Open();
            port.DataReceived += listener;
            var success = runCmd("AT").Status == Status.OK;
            if (!success)
            {
                //wait some time
                //and try again
            }
            else
            {
                runCmd(CmdStrFactory.DTMF.Read);
                //success = success && runCmd("AT+SIDET=0,10").Status == Status.OK;
                success = success && runCmd(CmdStrFactory.DTMF.Set(2)).Status == Status.OK;
                runCmd(CmdStrFactory.DTMF.Read);
                /*
                By default when you make a voice call Sim900 doesn’t send any response if the called party picks the call. Sometime its important to know if the other side is getting ring or has picked the call. In order to show the state we have to configure the module using the following AT Command

AT+MORING=1
After sending the is command if a number is dialed, a URC string “MO RING” will be received if the other mobile is alerted and “MO CONNECTED” will be received if the call is answered.
                */
                
                success = success && runCmd("AT+MORING=1").Status == Status.OK;
                success = success && runCmd(CmdStrFactory.ErrorReport.Set(2)).Status == Status.OK;
                //success = success && runCmd("ATE0").Status == Status.OK;//Disable Echo
                success = success && runCmd("AT+CIPSPRT=0").Status == Status.OK;
                //success = success && runCmd(CmdStrFactory.DTMF.Set(1,0)).Status == Status.OK;
                success = success && runCmd(CmdStrFactory.ADN.Set(1)).Status == Status.OK;//автовизначенння номера
                //success = success && runCmd(CmdStrFactory.CMGF.Set(1)).Status == Status.OK;//текстовый режим sms
                success = success && runCmd("AT+IFC=1,1").Status == Status.OK;// устанавливает программный контроль потоком передачи данных
                success = success && runCmd("AT+CPBS=\"SM\"").Status == Status.OK;//открывает доступ к данным телефонной книги SIM-карты
                //runCmd(Cmds.IncSmsSetting.SetStr(1, 1, 2, 1, 0));//включает оповещение о новых сообщениях, новые сообщения приходят в следующем формате: +CMT: "<номер телефона>", "", "<дата, время>", а на следующей строчке с первого символа идёт содержимое сообщения
                success = success && runCmd(CmdStrFactory.IncSmsSetting.Set(1, 2, 2, 1, 0)).Status == Status.OK;
                //success = success && runCmd(CmdStrFactory.CSCS.Set("GSM")).Status == Status.OK;//кодування
                
            }
            ready = success;
        }
        private void listener(string dataIn)
        {
            lock (_lock)
            {
                if (OnPortNewData != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        OnPortNewData(dataIn);
                    });
                }
            }
            #region incomming call

            if (dataIn.Contains("+CLIP:") && incCallResult == null)//only for first ring
            {
                incCallResult = new IncCallResponse();
                incCallResult.Phone = dataIn;
                lock (_lock)
                {
                    if (OnIncommingCall != null)
                    {
                        Task.Factory.StartNew(() => OnIncommingCall(incCallResult));
                    }
                }
            }
            if (incCallResult != null && dataIn.Contains("NO CARRIER"))
            {

                incCallResult.FireOnFinish("NO CARRIER");
                incCallResult.Dispose();
                incCallResult = null;
            }
            #endregion


            #region DTMF

            var key = string.Empty;
            var prefixDTMF = "+DTMF:";

            if (incCallResult != null && dataIn.Contains(prefixDTMF))
            {
                key = dataIn.Replace(prefixDTMF, "");
                if (!string.IsNullOrWhiteSpace(key))
                {
                    incCallResult.FireOnDTMF(key.Trim()[0], new TimeSpan());
                }
            }


            #endregion

            #region Audio

            if (dataIn.Contains(AMR_STOP))
            {
                incCallResult.FireOnAudioStop();
            }
            #endregion
        }

        public AtResult runCmd(string cmdStr, bool handleResponse = true)
        {
            AtCmd command = new AtCmd(cmdStr);
            AtResult result = command.Run(handleResponse);
            return result;
        }

        ~Sim900Module()
        {
            runCmd(CmdStrFactory.GetFlashBuffer.ToString());
            //disable GPRS
            //end Calls
        }

    }
}
