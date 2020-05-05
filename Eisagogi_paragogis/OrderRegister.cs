using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "OrderRegister")]
    public class OrderRegister
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int idn { get; set; }
        [Column] public DateTime OrderDate { get; set; }
        [Column] public string ItemCode { get; set; }
        [Column] public string Qty { get; set; }
        [Column] public string Machine { get; set; }
        [Column] public Nullable<int> TotalID { get; set; }
        [Column] public string Production { get; set; }

    }
}
