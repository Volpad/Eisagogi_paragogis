using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "Semi-finished_warehouse")]
    public class Semi_finished_warehouse
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public Nullable<int> shipmentNo { get; set; }
        [Column] public Nullable<DateTime> Date { get; set; }
        [Column] public string AccessNo { get; set; }
        [Column] public string Linking { get; set; }
        [Column] public string ItemCode { get; set; }
        [Column] public string ColorCode { get; set; }
        [Column] public string ColorGRdesc { get; set; }
        [Column] public string SizeDesc { get; set; }
        [Column] public Nullable<int> D_qty { get; set; }
        [Column] public Nullable<int> P_qty { get; set; }
        [Column] public Nullable<int> Sequence_No { get; set; }
        [Column] public string Col_ID2 { get; set; }
        [Column] public string Syskeuastika { get; set; }
        [Column] public Nullable<int> Col_ID { get; set; }

    }
}
