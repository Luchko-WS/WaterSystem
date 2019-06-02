using System;

namespace OpenDataStorage.ViewModels.ReportsViewModel
{
    public class ReportViewModel
    {
        public Guid? ObjectId { get; set; }
        public Guid? CharacterisitcId { get; set; }
        public Guid? TypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}