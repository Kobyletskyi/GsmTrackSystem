using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLE
{
    public interface IBleDevice
    {
        bool Connected { get; }
        bool ServicesReady { get; }
        string Address { get; }
        string Name { get; }
        int Rssi { get; }

        CharacteristicFactory CharacteristicFactory { get; }

        event Action<bool> OnServicesReady;
        event Action<ProfileState> OnStateChanged;

        bool Connect(bool reconnect = false);
        void Disconnect();
    }
}
