using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;

namespace HM_10
{
    public class BtDevice
    {

        public string Address { get; private set; }
        public string Name { get; private set; }
        public int Rssi { get; private set; }

        public BtDevice(string address, string name, int rssi)
        {
            Address = address;
            Name = name;
            Rssi = rssi;
        }
    }
}