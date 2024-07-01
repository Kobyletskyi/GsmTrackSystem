using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLE
{
    public class GattEvents : BluetoothGattCallback, IGattEvents
    {
        public event Action<GattStatus, string, byte[]> OnReseiveData;
        public event Action<string> OnCharacteristicUpdate;
        public event Action<GattStatus, string> OnCharacteristicWrote;
        public event Action<GattStatus> OnServicesReady;
        public event Action<ProfileState> OnConnectionChange;

        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            UUID uuid = characteristic.Uuid;
            OnCharacteristicUpdate?.Invoke(uuid.ToString());
        }
        public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            UUID uuid = characteristic.Uuid;
            byte[] data = (status == GattStatus.Success) ? characteristic.GetValue() : null;
            OnReseiveData?.Invoke(status, uuid.ToString(), data);
        }
        public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            OnCharacteristicWrote?.Invoke(status, characteristic.Uuid.ToString());
        }
        public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            OnConnectionChange?.Invoke(newState);
        }
        public override void OnDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
        {

        }
        public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
        {

        }
        public override void OnMtuChanged(BluetoothGatt gatt, int mtu, GattStatus status)
        {

        }
        public override void OnReadRemoteRssi(BluetoothGatt gatt, int rssi, GattStatus status)
        {

        }
        public override void OnReliableWriteCompleted(BluetoothGatt gatt, GattStatus status)
        {

        }
        public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
        {
            OnServicesReady?.Invoke(status);
        }
    }
}
