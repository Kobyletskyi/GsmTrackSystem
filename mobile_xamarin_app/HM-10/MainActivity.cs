using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace HM_10
{
    [Activity(Label = "HM_10", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly int REQUEST_ENABLE_BT = 1;
        static readonly long SCAN_PERIOD = 10000;

        BtManager btMng;
        BtDeviceListAdapter adapter;
        ListView list;
        Button button;
        Handler mHandler;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);



            mHandler = new Handler();
            list = FindViewById<ListView>(Resource.Id.DevicesList);
            list.ItemClick += onListItemClick;
            button = FindViewById<Button>(Resource.Id.StartBtn);
            button.Click += onRescanClick;

            // Use this check to determine whether BLE is supported on the device.  Then you can
            // selectively disable BLE-related features.
            if (!BtManager.SupportBtLE(this))
            {
                Toast.MakeText(this, "Resource.String.ble_not_supported", ToastLength.Short).Show();
                Finish();
            }

            btMng = new BtManager(this);

            if (!btMng.IsAvailable)
            {
                // Checks if Bluetooth is supported on the device.
                Toast.MakeText(this, "Resource.String.error_bluetooth_not_supported", ToastLength.Short).Show();
                Finish();
                return;
            }
            btMng.OnDeviceDisovered += onScan;
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Ensures Bluetooth is enabled on the device.  If Bluetooth is not currently enabled,
            // fire an intent to display a dialog asking the user to grant permission to enable it.
            if (!btMng.IsEnabled)
            {
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
            }

            adapter = new BtDeviceListAdapter(this);
            list.Adapter = adapter;
            ScanLeDevice(true);
        }

        void ScanLeDevice(bool enable)
        {
            if (enable)
            {
                // Stops scanning after a pre-defined scan period.
                mHandler.PostDelayed(new Action(delegate
                {
                    button.Text = "Scan";
                    btMng.StopScan();
                    InvalidateOptionsMenu();
                }), SCAN_PERIOD);

                button.Text = "Stop";
                adapter.Clear();
                btMng.StartScan();
            }
            else
            {
                button.Text = "Scan";
                btMng.StopScan();
            }
            InvalidateOptionsMenu();
        }
        private void onScan(BtDevice device)
        {

            adapter.AddDevice(device);
            RunOnUiThread(new Action(delegate
                {
                    adapter.NotifyDataSetChanged();
                }));
        }

        private void onRescanClick(Object sender, EventArgs e)
        {
            ScanLeDevice(!btMng.IsDiscovering);
        }

        private void onListItemClick(Object sender, ItemClickEventArgs e)
        {
            BtDevice device = adapter[e.Position];
            if (device == null)
                return;

            Intent intent = new Intent(this, typeof(LogsActivity));
            intent.PutExtra(LogsActivity.EXTRAS_DEVICE_NAME, device.Name);
            intent.PutExtra(LogsActivity.EXTRAS_DEVICE_ADDRESS, device.Address);

            if (btMng.IsDiscovering)
            {
                ScanLeDevice(false);
            }
            StartActivity(intent);
        }
    }
}

