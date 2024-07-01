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
    public class ReadCharacteristic : Characteristic, IReadCharacteristic
    {
        internal ReadCharacteristic(BluetoothGatt btGatt, BluetoothGattCharacteristic btCharacteristic, IGattEvents btGattEvents)
            : base(btGatt, btCharacteristic, btGattEvents)
        {
            dataClb = (status, characteristicUuid, data) =>
            {
                if (characteristicUuid == Uuid.ToString())
                {
                    OnData?.Invoke(status, characteristic.GetValue());
                    //gattEvents.OnReseiveData -= dataClb;
                }
            };
        }

        public event Action<GattStatus, byte[]> OnData;
        private Action<GattStatus, string, byte[]> dataClb;

        public bool Read()
        {
            
            gattEvents.OnReseiveData += dataClb;
            return gatt.ReadCharacteristic(characteristic);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    gattEvents.OnReseiveData -= dataClb;
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