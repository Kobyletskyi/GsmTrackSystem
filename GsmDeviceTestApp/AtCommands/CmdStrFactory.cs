using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim900.AtCommand
{
    public static class CmdStrFactory
    {
        public static string EchoDisable = "ATE0";
        public static string EchoEnable = "ATE1";
        public static string HangUp = "ATH";
        public static string AnswerIncCall = "ATA";
        //readonly AT+CMD

        public static CmdStrBuilder IMEI = new CmdStrBuilder("GSN");
        public static CmdStrBuilder ID = new CmdStrBuilder("GMM");
        public static CmdStrBuilder Revision = new CmdStrBuilder("GMR");
        public static CmdStrBuilder Status = new CmdStrBuilder("CPAS");
        //рівень сигналу
        public static CmdStrBuilder NetScan = new CmdStrBuilder("CNETSCAN");
        public static CmdStrBuilder CENG = new CmdStrBuilder("CENG");
        public static CmdStrBuilder SignalStrength = new CmdStrBuilder("CSQ");
                                                                              /// <summary>
                                                                              /// монітор напруги живлення
                                                                              /// </summary>
                                                                              /// <param name="1"> 0 – не заряжается 1 – заряжается 2 – зарядка окончена</param>
                                                                              /// <param name="2"> 1-100 % — уровень заряда батареи</param>
                                                                              /// <param name="3"> Напряжение питание модуля (VBAT), мВ</param>
        public static CmdStrBuilder Voltage = new CmdStrBuilder("CBC");
        //public static CmdStrBuilder AnswerIncCall = new CmdStrBuilder("ATA");

        public static CmdStrBuilder Provider = new CmdStrBuilder("COPS");
        public static CmdStrBuilder ADN = new CmdStrBuilder("CLIP");//автовизначенння номера 1 – вкл / 0 – викл
                                                                    //SMS
        public static CmdStrBuilder SmsMode = new CmdStrBuilder("CMGF");//Текстовий режим 1 – вкл / 0 – викл
        public static CmdStrBuilder SmsTextCoding = new CmdStrBuilder("CSCS");//Кодування текстового режиму IRA, GSM, UCS2, HEX, PCCP, PCDN, 8859-1
        public static CmdStrBuilder DisableBrcstSms = new CmdStrBuilder("CSCB");//1 disable 0 enable
        public static CmdStrBuilder SimSmsStorage = new CmdStrBuilder("CPMS");//SM - simcart
        public static CmdStrBuilder CSCB = new CmdStrBuilder("CSCB");//Прийом спец повідомлень 0 - дозволенно / 1 - забороненно
        public static CmdStrBuilder DTMF = new CmdStrBuilder("DDET");//DTMF 1 - дозволенно / 0 - забороненно
                                                                     /// <summary>
                                                                     /// New SMS Message Indication Setting
                                                                     /// 1>mode 0, 1, 2!, 3
                                                                     /// 2>mt 0, 1!, 2, 3
                                                                     /// 3> bm 0!, 2
                                                                     /// 4> ds 0!, 1
                                                                     /// 5>bfr 0!, 1
                                                                     /// </summary>
                                                                     /// <param name="1"> mode 0, 1, 2!, 3</param>
                                                                     /// <param name="2"> mt 0, 1!, 2, 3</param>
                                                                     /// <param name="3"> bm 0!, 2</param>
                                                                     /// <param name="4"> ds 0!, 1</param>
                                                                     /// <param name="5"> bfr 0!, 1</param>
        public static CmdStrBuilder IncSmsSetting = new CmdStrBuilder("CNMI");
        /// <summary>
        ///  уровень информации об ошибке. Может быть от 0 до 2.
        ///0 - отключено.Будет просто писать ERROR.
        ///1 -  код ошибки. Будет возвращать цифровой код ошибки.
        ///2 -  описание ошибки. Напишет что именно ему не нравится в команде.
        /// </summary>
        public static CmdStrBuilder ErrorReport = new CmdStrBuilder("CMEE");
        /// <summary>
        /// GSM Location and Time
        /// AT+CIPGSMLOC=type,cid
        /// type - 1 (View the longitude, latitude and time), 2 (View time)
        /// cid - network parameters, refer to AT+SAPBR
        /// response
        /// +CIPGSMLOC:<locationcode>,<longitude>,<latitude>,<date>,<time>
        /// +CIPGSMLOC:<locationcode>,<date>,<time>
        /// 0 Success
        /// If the operation failed, the location code is not 0, such as:
        /// 601 Network Error
        /// 602 No memory
        /// 603 DNS Error
        /// 604 Stack Busy
        /// 65535 Other Error
        /// </summary>
        public static CmdStrBuilder CellLocation = new CmdStrBuilder("CIPGSMLOC");
        /// <summary>
        /// 
        /// </summary>
        public static CmdStrBuilder Ussd = new CmdStrBuilder("CUSD");
        #region File System
        /// <summary>
        /// Play AMR File \r
        /// AT+CPAMR=fileName,audioLocationStatus>\r
        /// AT+CPAMR  -  stop playing
        /// fileName - less than 50 characters\r
        /// audioLocationStatus - 0!(remoute user can hear the playing audio), 1(local user can hear the playing audio)
        /// </summary>
        public static CmdStrBuilder PlayAMR = new CmdStrBuilder("CPAMR");
        /// <summary>
        /// Get Flash Data Buffer
        /// AT+CFSINIT
        /// </summary>
        public static CmdStrBuilder GetFlashBuffer = new CmdStrBuilder("CFSINIT");
        /// <summary>
        /// Write File to the Flash Buffer Allocated by CFSINIT
        /// AT+CFSWFILE=fileName,mode,fileSize,inputTime
        /// fileName - File name length should less or equal 50 characters
        /// mode - 0 (If the file already existed, write the data at the beginning of the file) 1 (If the file already existed, add the data at the end of the file)
        /// fileSize - File size should be less than 65536 bytes
        /// inputTime - Millisecond, should send file during this period or you can't send file when timeout
        /// </summary>
        public static CmdStrBuilder WriteFileToFlash = new CmdStrBuilder("CFSWFILE");
        /// <summary>
        /// Get File Size
        /// AT+CFSGFIS=fileName
        /// fileName - File name length should less or equal 50 characters
        /// </summary>
        public static CmdStrBuilder GetFileSize = new CmdStrBuilder("CFSGFIS");
        /// <summary>
        /// Read File from Flash
        /// AT+CFSRFILE=fileName,mode,fileSize,position
        /// fileName - File name length should less or equal 50 characters
        /// mode - 0 (Read data at the beginning of the file) 1 (Read data at the position of the file)
        /// fileSize - The size of the file that you want to read should be less than 65536
        /// position - The starting position that will be read in the file. When mode=0, position is invalid. Read data from the beginning to the end of the file. When mode=1, position is valid. Read data from the position to the end of the file
        /// </summary>
        public static CmdStrBuilder ReadFileFromFlash = new CmdStrBuilder("CFSRFILE");
        /// <summary>
        /// Delete File from Flash
        /// AT+CFSDFILE=fileName
        /// fileName - File name length should less or equal 50 characters
        /// </summary>
        public static CmdStrBuilder DelFileFromFlash = new CmdStrBuilder("CFSDFILE");
        /// <summary>
        /// Rename File
        /// AT+CFSREN=fileName,newFileName
        /// fileName - File name length should less or equal 50 characters
        /// newFileName - File name length should less or equal 50 characters
        /// </summary>
        public static CmdStrBuilder RenameFile = new CmdStrBuilder("CFSREN");
        /// <summary>
        /// List the Files in Flash
        /// AT+CFSLIST
        /// </summary>
        public static CmdStrBuilder FilesList = new CmdStrBuilder("CFSLIST");
        /// <summary>
        /// Free the Flash Buffer Allocated by CFSINIT
        /// AT+CFSTERM
        /// </summary>
        public static CmdStrBuilder FreeFlash = new CmdStrBuilder("CFSTERM");

        #endregion

        #region Internet
        /// <summary>
        /// SIMCOM APPLICATION BEARER
        /// Response +SAPBR: (0-4), (1-3), “ConParamTag”, “ConParamValue”
        /// cmd_type - 0:  close bearer 1:  open bearer 2:  query bearer 3:  set bearer parameters 4:  get bearer parameters
        /// </summary>
        public static CmdStrBuilder SetupGPRS = new CmdStrBuilder("SAPBR");
        public static CmdStrBuilder HttpInit = new CmdStrBuilder("HTTPINIT");
        public static CmdStrBuilder HttpTerm = new CmdStrBuilder("HTTPTERM");
        public static CmdStrBuilder HttpConfig = new CmdStrBuilder("HTTPPARA");
        public static CmdStrBuilder HttpAction = new CmdStrBuilder("HTTPACTION");
        public static CmdStrBuilder HTTPDATA = new CmdStrBuilder("HTTPDATA");
        public static CmdStrBuilder HTTPREAD = new CmdStrBuilder("HTTPREAD");


        public static CmdStrBuilder GsmBusy = new CmdStrBuilder("GSMBUSY");
        public static CmdStrBuilder Sleep = new CmdStrBuilder("CSCLK");
        #endregion
    }
}
