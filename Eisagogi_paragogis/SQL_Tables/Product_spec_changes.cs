using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "Product_Specs_Changes")]
    public class Product_spec_changes
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public string user_modified { get; set; }
        [Column] public Nullable<DateTime> date_modified { get; set; }
        [Column] public string specification_changed { get; set; }
        [Column] public string old_value { get; set; }
        [Column] public string new_value { get; set; }
        [Column] public string ProductName { get; set; }


    }
}
