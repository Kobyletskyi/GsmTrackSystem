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
    public class NotityCharacteristic : Characteristic, INotityCharacteristic
    {
        internal NotityCharacteristic(BluetoothGatt btGatt, BluetoothGattCharacteristic btCharacteristic, IGattEvents btGattEvents)
            : base(btGatt, btCharacteristic, btGattEvents)
        {
            notificationClb = (characteristicUuid) =>
            {
                if (characteristicUuid == Uuid.ToString())
                {
                    OnNotification?.Invoke(characteristic.GetValue());
                }
            };
        }

        public event Action<byte[]> OnNotification;
        private Action<string> notificationClb;

        public bool SetNotification(bool enable)
        {
            if (enable)
            {
                gattEvents.OnCharacteristicUpdate += notificationClb;
            }
            else
            {
                gattEvents.OnCharacteristicUpdate -= notificationClb;
            }

            return gatt.SetCharacteristicNotification(characteristic, enable);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SetNotification(false);
                }

                disposedValue = true;
            }
        }
        //~ReadCharacteristic()
        //{
        //    Dispose(false);
        //}

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}