using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "shipment")]
    public class shipment
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public Nullable<int> shipmentNo { get; set; }
        [Column] public string truck { get; set; }
        [Column] public DateTime Date { get; set; }
        [Column] public Nullable<DateTime> ShipmentDate { get; set; }


    }
}
