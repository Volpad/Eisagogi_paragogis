using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "eisagogiParagogis")]
    public class eisagogiParagogis
    {

        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int AutoNo { get; set; }
        [Column] public string barcode { get; set; }
        [Column] public Nullable<DateTime> date { get; set; }
        [Column] public string user { get; set; }
        [Column] public Nullable<int> dozen { get; set; }
        [Column] public Nullable<int> socks { get; set; }
        [Column] public string Machine { get; set; }
        [Column] public string Defective { get; set; }
        [Column(Name = "TOTAL ID")] public int? Total_Id { get; set; }
        [Column(Name = "COL ID")] public int Col_Id { get; set; }

    }
}
