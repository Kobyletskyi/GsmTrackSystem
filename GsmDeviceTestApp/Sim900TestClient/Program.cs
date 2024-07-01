using Sim900;
using Sim900.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        const char endOfDtmfCmd = '#';
        const char repeatDtmfCmd = '*';
        static Sim900Module module = new Sim900Module();

        static void Main(string[] args)
        {
            module.OnPortNewData += (data) => Console.WriteLine(data);

            module.Init();

            module.OnIncommingCall += handleIncCall;
            //while (true)
            //{
            //    module.ScanNetwork();
            //    Thread.Sleep(5000);
            //}
            

            //module.ScanNetwork();

            string revision = module.GetRevision();

            string imei = module.GetIMEI();

            // var files = module.GetFilesList();

            //var size = module.GetFileSize(files.First().Name);

            //var fileBytes = module.ReadFile(files.First().Name);

            //string ussd = module.USSD("*101#");

            //module.SendGetRequest(@"http://gsmtracker.somee.com/api");

            //var coord = module.GetLocation();

            //System.Web.HttpUtility.ParseQueryString

            //HttpValueCollection

            //NameValueCollection requestData = new NameValueCollection();
            //requestData.Add("imei", "imei");
            //requestData.Add("longitude", "4");
            //requestData.Add("latitude", "4");
            //var url = string.Format(@"http://gsmtracker.somee.com/api/Home/Post?imei={0}&longitude={1}&latitude={2}", imei, 5, 5);
            //module.SendPostRequest(url, requestData);

            //module.SendSMS(" + 380963636642", "SMS3");
            //module.GetLocation();


            //byte[] buff = null;
            //FileStream fs = new FileStream(@"C:\Users\www\a.amr", FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fs);
            //long numBytes = new System.IO.FileInfo(@"C:\Users\www\a.amr").Length;
            //buff = br.ReadBytes((int)numBytes);
            //module.WriteFile("w1.amr", buff);


            //var result = module.Call("+380665242642");
            //result.OnDTMF += (key, duration) => Console.WriteLine("OnDTMF =>>> " + key);
            //result.OnFinish += (data) => Console.WriteLine("OnFinish =>>> " + data);


        }
        private static void handleIncCall(IIncCallResponse response)
        {
            Console.WriteLine("IncommingCall =>>> " + response.Phone);
            if (module.AnswerIncCall())
            {
                long? commandNumber = null;
                bool repeat = true;
                bool finish = false;
                string playingFile = string.Empty;
                Action<char, TimeSpan> onDTMF = (key, duration) =>
                {
                    switch (key)
                    {
                        case endOfDtmfCmd:
                            if (commandNumber.HasValue)
                            {
                        //runComand(commandNumber.Value)
                        commandNumber = 0;
                                Console.WriteLine("DTMF CMD: " + commandNumber.Value);
                            }
                            break;
                        case repeatDtmfCmd:
                            repeat = true;
                            break;
                        default:
                            commandNumber = (commandNumber ?? 0) * 10 + Int16.Parse(key.ToString());
                            break;
                    }
                    //module.runCmd("AT+DDET=3");
                };
                Action onAudioStop = () => { playingFile = string.Empty; };
                Action<string> onFinish = (message) => { };
                onFinish = (message) =>
                            {
                                response.OnDTMF -= onDTMF;
                                response.OnFinish -= onFinish;
                                response.OnAudioStop -= onAudioStop;
                                response.Dispose();
                                finish = true;
                            };
                response.OnDTMF += onDTMF;
                response.OnFinish += onFinish;
                response.OnAudioStop += onAudioStop;
                string[] files = { "0.amr", "2.amr" };

                Thread.Sleep(3000);
                while (!finish)
                {
                    if (repeat)
                    {
                        foreach (string file in files)
                        {
                            playingFile = file;
                            module.PlayFile(file);
                            while (!string.IsNullOrWhiteSpace(playingFile))
                            {
                                Thread.Sleep(500);
                            }
                        }
                        repeat = false;
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
