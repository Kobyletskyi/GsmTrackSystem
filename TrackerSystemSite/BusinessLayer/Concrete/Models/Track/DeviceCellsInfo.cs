using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Models
{
    public class DeviceCellsInfo
    {
        public string carrier { get; set; }
        public Int32 Arfcn { get; set; }
        public byte RxLevel { get; set; }
        public byte Ber { get; set; }
        public byte Bsic { get; set; }
        public Int32 CellID { get; set; }
        public Int32 Mcc { get; set; }
        public Int32 Mnc { get; set; }
        public Int32 Lac { get; set; }
        public byte RxLevAccessMin { get; set; }
        public byte MsTxpwrMaxCch { get; set; }
        public byte Ta { get; set; }
        public List<CellTower> cells { get; set; }
    }
}