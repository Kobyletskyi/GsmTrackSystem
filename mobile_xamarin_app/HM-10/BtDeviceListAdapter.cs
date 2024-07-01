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

namespace HM_10
{
    public class BtDeviceListAdapter : BaseAdapter<BtDevice>
    {
        List<BtDevice> mLeDevices;
        Context context;

        public BtDeviceListAdapter(Context c)
        {
            context = c;
            mLeDevices = new List<BtDevice>();
        }

        public void AddDevice(BtDevice device)
        {
            if (!mLeDevices.Contains(device))
            {
                mLeDevices.Add(device);
            }
        }

        //public BtDevice GetDevice(int position)
        //{
        //    return mLeDevices[position];
        //}

        public void Clear()
        {
            mLeDevices.Clear();
        }

        public override int Count
        {
            get
            {
                return mLeDevices.Count;
            }
        }

        public override BtDevice this[int position]
        {
            get { return mLeDevices[position]; }
        }
        
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.ListItem, null, false);
            }

            TextView deviceName = row.FindViewById<TextView>(Resource.Id.device_name);
            TextView deviceRssi = row.FindViewById<TextView>(Resource.Id.device_rssi);

            BtDevice device = mLeDevices[position];
            deviceName.Text = device.Name ?? device.Address;
            //deviceAddress.Text = device.Address;
            deviceRssi.Text = device.Rssi.ToString();
            
            return row;
        }
    }
}