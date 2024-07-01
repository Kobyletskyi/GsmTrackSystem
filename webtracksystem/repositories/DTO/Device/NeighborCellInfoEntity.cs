using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto
{
    public class NeighborCellInfoEntity : BaseEntity<int>
    {        
        //[Required]
        public Int32 Arfcn { get; set; }//Absolute radio frequency channel number
        //[Required]
        public Byte RxLevel { get; set; }//Receive level
        //[Required]
        public Byte Bsic { get; set; }//Base station identy code
        //[Required]
        public Int32 CellId { get; set; }//Cell ID
        //[Required]
        public Int32 Mcc { get; set; }//Mobile country code
        //[Required]
        public Int32 Mnc { get; set; }//Mobile network code
        //[Required]
        public Int32 Lac { get; set; }//Location area code
        
        public byte? Ber { get; set; }
        public byte? RxLevAccessMin { get; set; }
        public byte? MsTxpwrMaxCch { get; set; }
        public byte? Ta { get; set; }

        //[ForeignKey("GeoLocation")]
        public int LocationId { get; set; }
        public virtual GeoLocationByCellsEntity GeoLocation { get; set; }
    }
}
