using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "ToBePlanned")]
    public class ToBePlanned
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public Nullable<int> AccessNo { get; set; }
        [Column] public Boolean OnMachine { get; set; }
        [Column] public Boolean Deleted { get; set; }
        [Column] public string Machine { get; set; }
        [Column] public Nullable<DateTime> CDate { get; set; }
        [Column] public Nullable<DateTime> DDate { get; set; }
        [Column] public string DUser { get; set; }
        [Column] public Nullable<DateTime> LastCheck { get; set; }
        [Column] public Boolean isWalk { get; set; }

    }
}
