using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsAPI.Data.Entities
{
    public class asset
    {
        public long id { get; set; }
        [ForeignKey("currency")]
        public long base_currency_id { get; set; }
        [ForeignKey("currency")]
        public long quote_currency_id { get; set; }
        [ForeignKey("provider")]
        public long provider_id { get; set; }
    }
}
