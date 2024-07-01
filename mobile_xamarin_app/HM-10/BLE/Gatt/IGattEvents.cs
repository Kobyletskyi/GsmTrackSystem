using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLE
{
    public interface IGattEvents
    {
        event Action<GattStatus, string, byte[]> OnReseiveData;
        event Action<string> OnCharacteristicUpdate;
        event Action<GattStatus> OnServicesReady;
        event Action<ProfileState> OnConnectionChange;
        event Action<GattStatus, string> OnCharacteristicWrote;
    }
}
