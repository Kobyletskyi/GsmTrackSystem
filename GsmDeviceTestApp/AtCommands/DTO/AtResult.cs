using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim900.DTO
{
    public class AtResult
    {
        public Status Status { get; set; }
        public string Data { get; set; }
    }
    public enum Status
    {
        OK, ERROR
    }
}
