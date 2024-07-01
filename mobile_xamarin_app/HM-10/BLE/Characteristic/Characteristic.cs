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
    public class Characteristic
    {
        public Characteristic(BluetoothGatt btGatt, string uuid, IGattEvents btGattEvents)
        {
            gatt = btGatt;
            characteristic = btGatt.GetCharacteristic(uuid);
            gattEvents = btGattEvents;
        }
        public Characteristic(BluetoothGatt btGatt, BluetoothGattCharacteristic btCharacteristic, IGattEvents btGattEvents)
        {
            gatt = btGatt;
            characteristic = btCharacteristic;
            gattEvents = btGattEvents;
        }

        protected BluetoothGatt gatt;
        protected BluetoothGattCharacteristic characteristic;
        protected IGattEvents gattEvents;

        public virtual string Uuid => characteristic.Uuid.ToString();
        public byte[] GetValue() => characteristic.GetValue();
    }
}