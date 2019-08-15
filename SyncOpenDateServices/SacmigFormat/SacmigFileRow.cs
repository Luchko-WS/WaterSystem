using System;
using System.Collections.Generic;

namespace SyncOpenDateServices.SacmigFormat
{
    public class SacmigFileRow
    {
        public SacmigFileRow()
        {
            Characteristics = new Dictionary<string, double>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<string, double> Characteristics { get; set; }
    }
}
