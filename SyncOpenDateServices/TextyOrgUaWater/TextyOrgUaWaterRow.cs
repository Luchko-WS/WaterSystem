using System;

namespace SyncOpenDateServices.TextyOrgUaWater
{
    public class TextyOrgUaWaterRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string River { get; set; }
        public string Laboratory { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public double Value { get; set; }
        //and other
    }
}
