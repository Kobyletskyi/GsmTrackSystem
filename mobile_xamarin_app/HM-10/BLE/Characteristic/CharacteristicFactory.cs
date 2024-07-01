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
    public class CharacteristicFactory
    {
        private BluetoothGatt gatt;
        private IGattEvents gattEvents;
        public CharacteristicFactory(BluetoothGatt btGatt, IGattEvents btGattEvents) {
            gatt = btGatt;
            gattEvents = btGattEvents;
        }
        public INotityCharacteristic GetNotityCharacteristic(string uuid)
        {
            BluetoothGattCharacteristic characteristic = gatt.GetCharacteristic(uuid);
            if (characteristic != null && (characteristic.Properties & GattProperty.Notify) > 0)
            {
                return new NotityCharacteristic(gatt, characteristic, gattEvents);
            }
            return null;
        }
        public IReadCharacteristic GetReadCharacteristic(string uuid)
        {
            BluetoothGattCharacteristic characteristic = gatt.GetCharacteristic(uuid);
            if (characteristic != null && (characteristic.Properties & GattProperty.Read) > 0)
            {
                return new ReadCharacteristic(gatt, characteristic, gattEvents);
            }
            return null;
        }
        public IWriteCharacteristic GetWriteCharacteristic(string uuid)
        {
            BluetoothGattCharacteristic characteristic = gatt.GetCharacteristic(uuid);
            if (characteristic != null 
                && (((characteristic.Properties & GattProperty.Write) > 0) 
                || ((characteristic.Properties & GattProperty.WriteNoResponse) > 0)))
            {
                return new WriteCharacteristic(gatt, characteristic, gattEvents);
            }
            return null;
        }
    }
}