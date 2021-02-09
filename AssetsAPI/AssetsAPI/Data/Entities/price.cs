using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsAPI.Data.Entities
{
    public class price
    {
        public long id { get; set; }
        [ForeignKey("asset")]
        public long asset_id { get; set; }
        public decimal value { get; set; }
        public DateTime timestamp { get; set; }
    }
}