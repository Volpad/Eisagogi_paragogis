using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "Machineqty")]
    public class Machineqty
    {
        [Column] public string MachineNo { get; set; }
        [Column] public string queueNo { get; set; }
        [Column] public Boolean Status { get; set; }
        [Column] public string ItemCode { get; set; }
        [Column] public Nullable<int> AccessNo { get; set; }
        [Column] public Nullable<int> Orderqty { get; set; }
        [Column] public Nullable<int> Productionqty { get; set; }
        [Column] public Nullable<int> Rest { get; set; }
        [Column] public Nullable<int> DailyQty { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }

    }
}
