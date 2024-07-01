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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BLE;

namespace HM_10
{
    [Activity(Label = "LogsActivity")]
    public class LogsActivity : Activity
    {
        public static readonly String EXTRAS_DEVICE_NAME = "DEVICE_NAME";
        public static readonly String EXTRAS_DEVICE_ADDRESS = "DEVICE_ADDRESS";

        String mDeviceName;
        String mDeviceAddress;

        IBleDevice bt;
        IWriteCharacteristic write;
        IReadCharacteristic read;
        BluetoothGattCharacteristic characteristic;

        TextView dataView;
        Button sendBtn;
        Button readBtn;
        EditText cmd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.logView);

            Intent intent = Intent;
            mDeviceName = intent.GetStringExtra(EXTRAS_DEVICE_NAME);
            mDeviceAddress = intent.GetStringExtra(EXTRAS_DEVICE_ADDRESS);

            this.Title = mDeviceName;
            TextView addr = FindViewById<TextView>(Resource.Id.device_address);
            addr.Text = mDeviceAddress;
            dataView = FindViewById<TextView>(Resource.Id.logs);

            sendBtn = FindViewById<Button>(Resource.Id.btn_send);
            //sendBtn.Enabled = false;
            readBtn = FindViewById<Button>(Resource.Id.btn_read);
            //readBtn.Enabled = false;
            cmd = FindViewById<EditText>(Resource.Id.edit_cmd);

            sendBtn.Click += send;
            readBtn.Click += readChara;

            if (!BtManager.SupportBtLE(this))
            {
                Toast.MakeText(this, "Resource.String.ble_not_supported", ToastLength.Short).Show();
                Finish();
                return;
            }

            bt = new BleDevice(this, mDeviceAddress, mDeviceName);

            //bt.OnReseiveData += setData;
            //bt.OnConnectionChange += connectionStateChange;
            bt.OnServicesReady += btServicesReady;
            bt.OnStateChanged += connectionStateChange;
            bt.Connect(true);
        }

        private StringBuilder buffer = new StringBuilder();
        private void setData(string data)
        {
            buffer.Append(data);
            string msg = buffer.ToString();
            if (data.Contains("\r\n"))
            {
                try
                {
                    MdlEnvelopeBase baseEnv = JsonConvert.DeserializeObject<MdlEnvelopeBase>(msg);
                    switch (baseEnv.type)
                    {
                        case "cnfg":
                            {
                                MdlEnvelopeCnfgData envData = JsonConvert.DeserializeObject<MdlEnvelopeCnfgData>(baseEnv.data.ToString());
                                break;
                            }
                        case "log":
                            {
                                MdlEnvelopeLogData envData = JsonConvert.DeserializeObject<MdlEnvelopeLogData>(baseEnv.data.ToString());
                                break;
                            }
                        default:
                            break;
                    }
                }
                catch
                {

                }
                RunOnUiThread(new Action(delegate
                {
                    dataView.Text += msg;
                }));
                buffer.Clear();
            }
        }

        private void connectionStateChange(ProfileState newState)
        {
            switch (newState)
            {
                case ProfileState.Connecting:
                    setData(string.Format("[{1}]\r\n", bt.Address, "Connecting"));
                    break;
                case ProfileState.Connected:
                    setData(string.Format("[{1}]\r\n", bt.Address, "Connected"));
                    break;
                case ProfileState.Disconnecting:
                    setData(string.Format("{1}]\r\n", bt.Address, "Disconnecting"));
                    break;
                case ProfileState.Disconnected:
                    setData(string.Format("[{1}]\r\n", bt.Address, "Disconnected"));
                    break;
                default:
                    break;
            }
        }

        private void btServicesReady(bool ready)
        {
            if (ready)
            {
                write = bt.CharacteristicFactory?.GetWriteCharacteristic(DataCharacteristic.Write);                
                read = bt.CharacteristicFactory?.GetReadCharacteristic(DataCharacteristic.Read);
                read.OnData += (a, b) =>
                {

                };
                
                //RunOnUiThread(new Action(delegate
                //{
                //    sendBtn.Enabled = write != null;
                //    readBtn.Enabled = read != null;
                //}));
                INotityCharacteristic notify = bt.CharacteristicFactory?.GetNotityCharacteristic(DataCharacteristic.Notify);
                notify.SetNotification(true);
                notify.OnNotification += (data) => setData(Encoding.UTF8.GetString(data));
            }

            RunOnUiThread(new Action(delegate
            {
                TextView addr = FindViewById<TextView>(Resource.Id.device_status);
                addr.Text = ready.ToString();
            }));
        }

        private void send(object sender, EventArgs e)
        {
            write.Write(cmd.Text);
        }
        private void readChara(object sender, EventArgs e)
        {
            read.Read();
        }

    }

    public class MdlEnvelopeBase
    {
        public string type { get; set; }
        public object data { get; set; }
    }
    public class MdlEnvelopeCnfgData
    {
        public UInt16 network { get; set; }
        public UInt16 mode { get; set; }
        public UInt16 trackInterval { get; set; }
        public Int16 timeZone { get; set; }

    }
    public class MdlEnvelopeLogData
    {
        public string msg { get; set; }
    }
}
