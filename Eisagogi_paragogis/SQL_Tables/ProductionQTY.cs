using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "ProductionQTY")]
    public class ProductionQTY
    {
        [Column(Name = "TOTAL ID", IsPrimaryKey = true)] public Nullable<int> TOTAL_ID { get; set; }
        [Column] public Nullable<int> TotalProduction { get; set; }
        [Column] public Nullable<int> Produced { get; set; }
        [Column] public Nullable<int> Rest { get; set; }
    }
}
