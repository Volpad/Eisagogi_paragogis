using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "item_warehouse")]
    public class ItemBalances
    {
        [Column] public string ItemCode { get; set; }
        [Column] public byte Inactive { get; set; }
        [Column] public string ColorCode { get; set; }
        [Column] public string ColorGRdesc { get; set; }
        [Column] public string SizeCode { get; set; }
        [Column] public string SizeDesc { get; set; }
        [Column] public string WarehouseCode { get; set; }
        [Column] public int Warehouse { get; set; }
        [Column] public Nullable<int> Balance { get; set; }
        [Column] public Nullable<int> Remain { get; set; }
        [Column] public Nullable<int> OrderQty { get; set; }   

    }
}
