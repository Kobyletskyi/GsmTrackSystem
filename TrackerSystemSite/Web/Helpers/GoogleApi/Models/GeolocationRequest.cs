using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers.GoogleApi
{
    public class GeolocationRequest
    {
        /// <summary>
        /// The mobile country code (MCC) for the device's home network
        /// </summary>
        public int homeMobileCountryCode { get; set; }
        /// <summary>
        /// The mobile network code (MNC) for the device's home network.
        /// </summary>
        public int homeMobileNetworkCode { get; set; }
        /// <summary>
        /// The mobile radio type. Supported values are lte, gsm, cdma, and wcdma. While this field is optional, it should be included if a value is available, for more accurate results.
        /// </summary>
        public string radioType { get; set; } //"gsm",
                                              /// <summary>
                                              /// The carrier name.
                                              /// </summary>
        public string carrier { get; set; }// "Vodafone",
                                           /// <summary>
                                           /// Specifies whether to fall back to IP geolocation if wifi and cell tower signals are not available. Note that the IP address in the request header may not be the IP of the device. Defaults to true. Set considerIp to false to disable fall back.
                                           /// </summary>
        public bool considerIp { get; set; }//"true",
                                            /// <summary>
                                            /// An array of cell tower objects. See the Cell Tower Objects section below.
                                            /// </summary>
        public CellTower[] cellTowers { get; set; }
        /// <summary>
        /// An array of WiFi access point objects. See the WiFi Access Point Objects section below.
        /// </summary>
        public WiFiAccessPoint[] wifiAccessPoints { get; set; }

        public class CellTower
        {
            /// <summary>
            /// (required): Unique identifier of the cell. On GSM, this is the Cell ID (CID); CDMA networks use the Base Station ID (BID). WCDMA networks use the UTRAN/GERAN Cell Identity (UC-Id), which is a 32-bit value concatenating the Radio Network Controller (RNC) and Cell ID. Specifying only the 16-bit Cell ID value in WCDMA networks may return inaccurate results.
            /// </summary>
            public int cellId { get; set; }//42,
                                           /// <summary>
                                           /// (required): The Location Area Code (LAC) for GSM and WCDMAnetworks. The Network ID (NID) for CDMA networks.
                                           /// </summary>
            public int locationAreaCode { get; set; }//415,
                                                     /// <summary>
                                                     /// (required): The cell tower's Mobile Country Code (MCC).
                                                     /// </summary>
            public int mobileCountryCode { get; set; }// 310,
                                                      /// <summary>
                                                      /// (required): The cell tower's Mobile Network Code. This is the MNC for GSM and WCDMA; CDMA uses the System ID (SID).
                                                      /// </summary>
            public int mobileNetworkCode { get; set; }//410,
                                                      /// <summary>
                                                      /// The number of milliseconds since this cell was primary. If age is 0, the cellId represents a current measurement.
                                                      /// </summary>
            public int age { get; set; }//0,
                                        /// <summary>
                                        /// Radio signal strength measured in dBm.
                                        /// </summary>
            public int signalStrength { get; set; }//-60,
                                                   /// <summary>
                                                   /// The timing advance value.
                                                   /// </summary>
            public int timingAdvance { get; set; }// 15
        }

        public class WiFiAccessPoint
        {
            /// <summary>
            /// (required) The MAC address of the WiFi node. Separators must be : (colon) and hex digits must use uppercase.
            /// </summary>
            public string macAddress { get; set; }//"01:23:45:67:89:AB",
                                                  /// <summary>
                                                  /// The current signal strength measured in dBm.
                                                  /// </summary>
            public int signalStrength { get; set; }// -65,
                                                   /// <summary>
                                                   /// The number of milliseconds since this access point was detected.
                                                   /// </summary>
            public int age { get; set; }// 0,
                                        /// <summary>
                                        /// The channel over which the client is communicating with the access point.
                                        /// </summary>
            public int channel { get; set; }// 11,
                                            /// <summary>
                                            /// The current signal to noise ratio measured in dB.
                                            /// </summary>
            public int signalToNoiseRatio { get; set; }// 40
        }
    }
}