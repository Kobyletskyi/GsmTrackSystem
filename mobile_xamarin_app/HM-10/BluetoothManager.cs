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
using Android.Content.PM;
using Java.Util;

namespace HM_10
{
    public delegate void DeviceDisoveredHandler(BtDevice device);
    public class BtManager : Java.Lang.Object, BluetoothAdapter.ILeScanCallback
    {
        public static bool SupportBtLE(Context context)
        {
            return context.PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe);
        }

        public static BluetoothAdapter GetBtAdapter(Context context)
        {
            if (mBluetoothAdapter == null)
            {
                BluetoothManager bluetoothManager = (BluetoothManager)context?.GetSystemService(Context.BluetoothService);
                mBluetoothAdapter = bluetoothManager?.Adapter;
            }
            return mBluetoothAdapter;
        }

        static BluetoothAdapter mBluetoothAdapter;

        public BtManager(Context context)
        {
            BluetoothManager bluetoothManager = (BluetoothManager)context?.GetSystemService(Context.BluetoothService);
            mBluetoothAdapter = bluetoothManager?.Adapter;
        }

        public event DeviceDisoveredHandler OnDeviceDisovered;
        public bool IsAvailable
        {
            get
            {
                return mBluetoothAdapter != null;
            }
        }
        public bool IsEnabled
        {
            get
            {
                return mBluetoothAdapter != null && mBluetoothAdapter.IsEnabled;
            }
        }
        public bool IsDiscovering
        {
            get
            {
                return mBluetoothAdapter != null && mBluetoothAdapter.IsDiscovering;
            }
        }

        public bool StartScan()
        {
            if (mBluetoothAdapter != null)
            {
                if (mBluetoothAdapter.IsDiscovering)
                {
                    mBluetoothAdapter.StopLeScan(this);
                }
                return mBluetoothAdapter.StartLeScan(this);
            }
            else
            {
                return false;
            }
        }
        public void StopScan()
        {
            if (mBluetoothAdapter != null)
            {
                //if (mBluetoothAdapter.IsDiscovering)
                //{
                mBluetoothAdapter.StopLeScan(this);
                //}
            }
        }

        #region BluetoothAdapter.ILeScanCallback implementation
        public void OnLeScan(BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            OnDeviceDisovered?.Invoke(new BtDevice(device.Address, device.Name, rssi));
        }
        #endregion
    }

}