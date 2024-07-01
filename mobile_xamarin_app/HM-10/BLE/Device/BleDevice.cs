using Android.Bluetooth;
using Android.Content;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Android.Runtime;
using System.Linq;

namespace BLE
{
    public class BleDevice : IBleDevice
    {

        public string Address { get; private set; }
        public string Name { get; private set; }
        public int Rssi { get; private set; }
        public bool Connected => _gatt != null;
        public bool ServicesReady { get; private set; }

        public event Action<bool> OnServicesReady;
        public event Action<ProfileState> OnStateChanged;

        public CharacteristicFactory CharacteristicFactory { get; private set; }

        protected Dictionary<string, BluetoothGattCharacteristic> characteristics
            => services.SelectMany(s => s.Value.Characteristics).ToDictionary(c => c.Uuid.ToString(), c => c);

        protected Dictionary<string, BluetoothGattService> services
            => gatt.Services.ToDictionary(s => s.Uuid.ToString(), s => s);
            

        private BluetoothGatt _gatt;
        protected BluetoothGatt gatt
        {
            get
            {
                if (_gatt == null)
                {
                    _gatt = ConnectGatt();
                }
                return _gatt;
            }
            set
            {
                if (_gatt != null)
                {
                    //dispose prev gatt
                    _gatt.Dispose();
                }
                _gatt = value;
            }
        }

        private IGattEvents gattEvents = new GattEvents();
        private Context _context;

        public BleDevice(Context context, string address, string name, int rssi = 0)
        {
            Address = address;
            Name = name;
            Rssi = rssi;
            _context = context;
        }
        public bool Connect(bool reconnect = false)
        {
            gatt = ConnectGatt(reconnect);
            return Connected;
        }
        private BluetoothGatt ConnectGatt(bool reconnect = false)
        {
            BluetoothDevice device = BluetoothAdapter.DefaultAdapter?.GetRemoteDevice(Address);
            var gatt = device?.ConnectGatt(_context, reconnect, gattEvents as GattEvents);
            gattEvents.OnConnectionChange += connectionChanged;
            return gatt;
        }

        private void connectionChanged(ProfileState state)
        {
            switch (state)
            {
                case ProfileState.Connecting:
                    break;
                case ProfileState.Connected:
                    startDiscoverServices();
                    break;
                case ProfileState.Disconnecting:
                    break;
                case ProfileState.Disconnected:
                    break;
                default:
                    break;
            }
            OnStateChanged?.Invoke(state);
        }

        public void Disconnect()
        {
            gatt?.Disconnect();
        }

        protected void startDiscoverServices()
        {

            Action<GattStatus> clb = null;
            clb = (status) =>
            {
                ServicesReady = status == GattStatus.Success;
                gattEvents.OnServicesReady -= clb;
                OnServicesReady?.Invoke(ServicesReady);
            };
            gattEvents.OnServicesReady += clb;
            CharacteristicFactory = CharacteristicFactory ?? new CharacteristicFactory(gatt, gattEvents);

            gatt.DiscoverServices();
        }

    }
}
