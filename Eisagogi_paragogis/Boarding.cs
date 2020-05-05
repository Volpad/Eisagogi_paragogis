using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "Boarding")]
    public class Boarding
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public Nullable<DateTime> BoardingDate { get; set; }
        [Column] public string Warehouse { get; set; }
        [Column] public string AccessNo { get; set; }
        [Column] public string ItemCode { get; set; }
        [Column] public string ColorCode { get; set; }
        [Column] public string ColorGRdesc { get; set; }
        [Column] public string SizeDesc { get; set; }
        [Column] public Nullable<int> D_qty { get; set; }
        [Column] public Nullable<int> S_qty { get; set; }
        [Column] public Nullable<int> Df_qty { get; set; }
        [Column] public Nullable<int> Sf_qty { get; set; }
        [Column] public string notes { get; set; }
        [Column] public Nullable<int> Col_ID { get; set; }

    }
}
