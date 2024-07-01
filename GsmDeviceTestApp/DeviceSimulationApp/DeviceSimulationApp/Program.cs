using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComPort_Library;
using Sim900.AtCommand;
using System.IO.Ports;
using Sim900.DTO;
using System.Threading;
using System.Collections;
using System.Timers;

namespace DeviceSimulationApp
{

    class Program
    {
        #region Definitions
        static int trackFlag = 0;
        static int ringFlag = 1;
        static int callFlag = 2;
        static int smsFlag = 3;
        static BitArray flags = new BitArray(8);
        #endregion

        private static CommPort port = CommPort.Instance;

        static TimeSpan? timer = null;

        static void Main(string[] args)
        {
            port.DataReceived += (data) => Console.WriteLine(data);


            Stopwatch watch = new Stopwatch();


            port.Open();

            //initComPort(9600)
            initModule();
            //main cicle
            port.PortPinChanged += pin =>
            {
                if (pin == SerialPinChange.Ring)
                {
                    if (!flags[ringFlag])
                    {
                        watch.Start();
                        flags[ringFlag] = true;
                    }
                    else
                    {
                        if (watch.IsRunning)
                        {
                            watch.Stop();
                            if (watch.ElapsedMilliseconds > 110 && watch.ElapsedMilliseconds < 130)
                            {
                                flags[smsFlag] = true;
                            }
                            else
                            {
                                flags[callFlag] = true;
                            }
                        }
                        flags[ringFlag] = false;
                    }
                }
            };
            while (true)
            {
                if (flags[ringFlag])
                {
                    if (watch.IsRunning && watch.ElapsedMilliseconds > 130)
                    {
                        watch.Stop();
                        flags[callFlag] = true;
                    }
                    sleepSim900(false);
                }
                if (flags[smsFlag])
                {
                    //resend sms
                    flags[smsFlag] = false;
                }
                if (flags[callFlag])
                {
                    //runCmd(CmdStrFactory.HangUp);
                    //runCmd(CmdStrFactory.GsmBusy.Set(1));
                    //ProccedLocation();
                    //runCmd(CmdStrFactory.GsmBusy.Set(0));
                    runCmd(CmdStrFactory.AnswerIncCall);
                    playFile("2.amr");
                    var statCode = getStatusCode();
                    while (statCode == 4)
                    {
                        //    //if(repeatFlag){playFile("hello.amr");}
                        var key = waitDtmfKey(ref statCode);
                        switch (key.Trim())
                        {
                            case "1"://Enable tracking
                                sleepSim900(false);
                                runCmd(CmdStrFactory.HangUp.ToString());
                                Thread.Sleep(3000);
                                flags[trackFlag] = true;
                                ProccedLocation();
                                break;
                            case "2":
                                runCmd(CmdStrFactory.HangUp.ToString());
                                if (flags[trackFlag])
                                {
                                    flags[trackFlag] = false;
                                }
                                sleepSim900(true);
                                break;
                            case "3":
                                //send location one time
                                runCmd(CmdStrFactory.HangUp.ToString());
                                Thread.Sleep(3000);
                                ProccedLocation();                                
                                break;
                            case "4":

                                break;
                            case "5":
                                //send sms with balance
                                break;
                            case "6":
                                //send sms about batarea
                                break;
                            case "7":

                                break;
                            case "8":
                                break;
                            case "9":
                                break;
                            case "0":
                                //set repeat flag
                                break;
                            case "*":
                                break;
                            case "#":
                                break;
                        }

                    }
                    flags[callFlag] = false;
                }
                if (flags[trackFlag])
                {
                    if (timer.HasValue && timer > new TimeSpan(0, 1, 0))
                    {
                        sleepSim900(false);
                        stopTimer();
                        Thread.Sleep(3000);
                        ProccedLocation();
                    }
                    startTimer();
                }
                if (!flags[ringFlag] && !flags[smsFlag] && !flags[callFlag])
                {
                    //sleepSim900(true);
                }
            }
        }
        static int getStatusCode()
        {
            var resp = runCmd(CmdStrFactory.Status.ToString()).Data;
            var index = resp.IndexOf(CmdStrFactory.Status.Prefix) + CmdStrFactory.Status.Prefix.Length;
            var code = resp.Substring(index);
            return int.Parse(code);
        }
        static void playFile(string fileName)
        {
            //start play file 
            // return result dont waut for end playing

            runCmd(CmdStrFactory.PlayAMR.Set(fileName, 0));
        }

        static string waitDtmfKey(ref int statCode)
        {
            string buffer = String.Empty;
            string key = String.Empty;
            Action<string> OnDataReceived = (data) => { };
            OnDataReceived = (dataIn) =>
            {
                //making full command response 
                buffer += dataIn;
                //If response has the end of answer 'OK'
                bool dtmfDetected = buffer.Contains("+DTMF:");
                if (dtmfDetected)
                {
                    var index = buffer.IndexOf("+DTMF:") + 6;
                    var tmp = buffer.Substring(index);
                    var endInd = tmp.IndexOf("\r");
                    key = tmp.Substring(0, endInd);
                    port.DataReceived -= OnDataReceived;
                }
            };
            port.DataReceived += OnDataReceived;
            while (String.IsNullOrEmpty(key) && statCode == 4)
            {
                statCode = getStatusCode();
            }
            return key;
        }
        static string getDtmfCommand()
        {
            string buffer = String.Empty;
            string key = String.Empty;
            Action<string> OnDataReceived = (data) => { };
            OnDataReceived = (dataIn) =>
            {
                //making full command response 
                buffer += dataIn;
                //If response has the end of answer 'OK'
                bool dtmfDetected = buffer.Contains("+DTMF:");
                if (dtmfDetected)
                {
                    var index = buffer.IndexOf("+DTMF:") + 6;
                    var tmp = buffer.Substring(index);
                    var endInd = tmp.IndexOf("\r");
                    key = tmp.Substring(0, endInd);
                    port.DataReceived -= OnDataReceived;
                }
            };
            port.DataReceived += OnDataReceived;
            return key;
        }
        static void initModule()
        {
            sleepSim900(false);
#if DEBUG
            runCmd(CmdStrFactory.EchoEnable);
#else
            runCmd(CmdStrFactory.EchoDisable);
#endif
            runCmd(CmdStrFactory.GsmBusy.Set(1));
            runCmd(CmdStrFactory.SmsMode.Set(1));
            runCmd(CmdStrFactory.SmsTextCoding.Set("GSM"));
            runCmd(CmdStrFactory.DisableBrcstSms.Set(1));
            runCmd(CmdStrFactory.SimSmsStorage.Set("SM"));
            runCmd(CmdStrFactory.IncSmsSetting.Set(1, 1, 0, 0, 0));
            runCmd(CmdStrFactory.GsmBusy.Set(0));
            runCmd(CmdStrFactory.DTMF.Set(1));

        }
        private static void sleepSim900(bool enable)
        {
            if (port.DtrPin != enable)
            {
                port.DtrPin = enable;
                wait(200);
                port.Send(CmdStrFactory.Sleep.Set(enable ? 1 : 0));
                wait(100);
            }
        }
        static void wait(int ms)
        {
            Thread.Sleep(ms);
        }
        static bool waitForResponse()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            bool okFlag = false;
            bool errorFlag = false;
            string buffer = String.Empty;

            Action<string> OnDataReceived = (data) => { };
            OnDataReceived = (dataIn) =>
            {
                //making full command response 
                buffer += dataIn;

                //If response has the end of answer 'OK'
                okFlag = buffer.Contains("OK");
                errorFlag = buffer.Contains("ERROR");
                if (okFlag || errorFlag)
                {
                    port.DataReceived -= OnDataReceived;
                }
            };
            port.DataReceived += OnDataReceived;
            while (!okFlag || !errorFlag)
            {
                if (timer.ElapsedMilliseconds > 30000)
                {
                    break;
                }
            }

            timer.Start();
            return okFlag && !errorFlag;
        }
        static private AtResult runCmd(string cmdStr, bool handleResponse = true)
        {
            AtCmd command = new AtCmd(cmdStr);
            AtResult result = command.Run(handleResponse);
            return result;
        }
        static void ProccedLocation()
        {
            runCmd(CmdStrFactory.SetupGPRS.Set(3, 1, "CONTYPE", "GPRS"));
            runCmd(CmdStrFactory.SetupGPRS.Set(3, 1, "APN", "internet"));
            //init GPRS
            runCmd(CmdStrFactory.SetupGPRS.Set(1, 1));
            string longitude, latitude, date, time;
            if (getLocation(out longitude, out latitude, out date, out time))
            {
                runCmd(CmdStrFactory.HttpInit.ToString());
                sendCoordinates("012207000000015", longitude, latitude);
                runCmd(CmdStrFactory.HttpTerm.ToString());
            }
            //close GPRS
            runCmd(CmdStrFactory.SetupGPRS.Set(0, 1));
        }
        static bool getLocation(out string longitude, out string latitude, out string date, out string time)
        {
            port.Send(CmdStrFactory.CellLocation.Set(1, 1));
            bool okFlag = false;
            bool resultFlag = false;
            string buffer = String.Empty;
            bool result = false;
            result = true;
            longitude = String.Empty;
            latitude = String.Empty;
            date = String.Empty;
            time = String.Empty;
            Action<string> OnDataReceived = (data) => { };
            OnDataReceived = (dataIn) =>
            {
                //making full command response 
                buffer += dataIn;

                //If response has the end of answer 'OK'
                if (buffer.Contains("CIPGSMLOC:"))
                {
                    resultFlag = true;
                }
                if (resultFlag && buffer.Contains("OK"))
                {
                    okFlag = true;
                    port.DataReceived -= OnDataReceived;
                }
            };
            port.DataReceived += OnDataReceived;
            while (!okFlag)
            {

            }
            var start = buffer.IndexOf("CIPGSMLOC:") + 10;
            string tmp = buffer.Substring(start);
            var end = tmp.IndexOf("\r");
            string response = tmp.Substring(0, end);
            var parts = response.Split(',').Select(x => x.Trim()).ToArray();
            if (parts.Length == 5 && parts[0] == "0")
            {
                result = true;
                longitude = parts[1];
                latitude = parts[2];
                date = parts[3];
                time = parts[4];
            }
            else
            {
                result = false;
                //error
                if (parts.Length > 1)
                {
                    //string result = parts[0];
                }
            }

            return result;
        }
        static void sendCoordinates(string imei, string longitude, string latitude)
        {
#if DEBUG
            runCmd(CmdStrFactory.EchoDisable);//disable echo for long commands
#endif
            wait(1000);
            runCmd("AT+HTTPPARA=\"URL\",\"http://gsmtracker.somee.com/api/Values/set?imei=" + imei + "&longitude=" + longitude + "&latitude=" + latitude + "\"\r");
#if DEBUG
            runCmd(CmdStrFactory.EchoEnable);
#endif
            runCmd(CmdStrFactory.HttpAction.Set(0));//Get
            string buffer = String.Empty;
            bool resultFlag = false;
            Action<string> OnDataReceived = (data) => { };
            OnDataReceived = (dataIn) =>
            {
                //making full command response 
                buffer += dataIn;

                //If response has the end of answer 'OK'
                if (buffer.Contains("HTTPACTION:"))
                {
                    resultFlag = true;
                }
            };
            port.DataReceived += OnDataReceived;
            while (!resultFlag)
            {

            }
            //waitForResponse();
        }
        static void startTimer()
        {
            if (!timer.HasValue)
            {
                timer = new TimeSpan(0, 0, 0);
                System.Timers.Timer t = new System.Timers.Timer(1000);
                t.Elapsed += (c, f) =>
                {
                    if (timer.HasValue)
                    {
                        timer += new TimeSpan(0, 0, 1);
                    }
                    else
                    {
                        //stop timer
                        t.Stop();
                    }
                };
                t.Start();
            }
        }
        static void stopTimer()
        {
            timer = null;
        }
    }
}
