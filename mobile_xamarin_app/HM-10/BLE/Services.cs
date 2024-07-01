using Java.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLE
{
    public sealed class Services
    {
        public const string DataNotify = "0000ffe0-0000-1000-8000-00805f9b34fb";
        public const string DataWrite = "0000ffe5-0000-1000-8000-00805f9b34fb";
    }
    //RF-BM-S02
    public sealed class DataCharacteristic
    {
        //public const string Service = Services.DataNotify;

        public const string Write = "0000ffe1-0000-1000-8000-00805f9b34fb";//"0000ffe9-0000-1000-8000-00805f9b34fb";
        public const string Notify = "0000ffe1-0000-1000-8000-00805f9b34fb";//"0000ffe4-0000-1000-8000-00805f9b34fb";
        public const string Read = "0000ffe1-0000-1000-8000-00805f9b34fb";
    }

}