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
    public class WriteCharacteristic : Characteristic, IWriteCharacteristic
    {
        const int maxBulkSize = 20;
        internal WriteCharacteristic(BluetoothGatt btGatt, BluetoothGattCharacteristic btCharacteristic, IGattEvents btGattEvents)
            : base(btGatt, btCharacteristic, btGattEvents)
        {
            writeClb = (status, characteristicUuid) =>
            {
                if (characteristicUuid == Uuid.ToString())
                {
                    OnWrite?.Invoke(status);
                    //gattEvents.OnReseiveData -= dataClb;
                }
            };
        }

        public event Action<GattStatus> OnWrite;
        private Action<GattStatus, string> writeClb;

        public bool Write(byte[] data)
        {

            gattEvents.OnCharacteristicWrote += writeClb;
            bool result = false;
            for (int skip = 0; skip < data.Length; skip += maxBulkSize)
            {
                int size = (skip + maxBulkSize > data.Length - 1) ? data.Length - skip : maxBulkSize;
                byte[] chunk = data.Skip(skip).Take(size).ToArray();
                result &= writeCharacteristicChunk(chunk);
            }
            return result;
        }
        public bool Write(string data)
        {

            gattEvents.OnCharacteristicWrote += writeClb;
            bool result = false;
            for (int skip = 0; skip < data.Length; skip += maxBulkSize)
            {
                int size = (skip + maxBulkSize > data.Length - 1) ? data.Length - skip : maxBulkSize;
                string chunk = data.Substring(skip, size);
                result &= writeCharacteristicChunk(chunk);
            }
            return result;
        }

        private bool writeCharacteristicChunk(byte[] chunk)
        {
            if (chunk.Length > maxBulkSize)
            {
                throw new ArgumentNullException("Data block is too big");
            }

            characteristic.SetValue(chunk);
            return gatt.WriteCharacteristic(characteristic);
        }
        private bool writeCharacteristicChunk(string chunk)
        {
            if (chunk.Length > maxBulkSize)
            {
                throw new ArgumentNullException("String block is too big");
            }

            characteristic.SetValue(chunk);
            return gatt.WriteCharacteristic(characteristic);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    gattEvents.OnCharacteristicWrote -= writeClb;
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