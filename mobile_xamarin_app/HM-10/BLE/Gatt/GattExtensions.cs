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

namespace BLE
{
    public static class GattExtensions
    {
        public static BluetoothGattCharacteristic GetCharacteristic( this BluetoothGatt gatt, string uuid)
        {
            var a = gatt.Services.Select(s => s.Uuid.ToString()).ToArray();
            return gatt.Services
                .Where(s => s.GetCharacteristic(UUID.FromString(uuid)) != null)
                .Select(s => s.GetCharacteristic(UUID.FromString(uuid)))
                .FirstOrDefault();
        }
    }
}