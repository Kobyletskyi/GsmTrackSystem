using System;

namespace BusinessLayer.Models
{
    public class CellTower
    {
        public Int32 Arfcn { get; set; }//Absolute radio frequency channel number
        public Byte RxLevel { get; set; }//Receive level
        public Byte Bsic { get; set; }//Base station identy code
        public Int32 CellId { get; set; }//Cell ID
        public Int32 Mcc { get; set; }//Mobile country code
        public Int32 Mnc { get; set; }//Mobile network code
        public Int32 Lac { get; set; }//Location area code
    }
}