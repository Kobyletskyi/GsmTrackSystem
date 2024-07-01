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

namespace BLE
{
    public interface INotityCharacteristic : IDisposable
    {
        string Uuid { get; }
        bool SetNotification(bool enable);
        event Action<byte[]> OnNotification;
    }
}