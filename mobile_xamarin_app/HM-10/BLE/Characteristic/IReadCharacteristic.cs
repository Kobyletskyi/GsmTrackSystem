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

namespace BLE
{
    public interface IReadCharacteristic : IDisposable
    {
        string Uuid { get; }
        bool Read();
        event Action<GattStatus, byte[]> OnData;
    }
}