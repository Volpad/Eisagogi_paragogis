using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "DailyProduction")]
    public class DailyProduction
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int id { get; set; }
        [Column] public DateTime time { get; set; }
        [Column] public string username { get; set; }
        [Column] public Nullable<int> dozens { get; set; }
        [Column] public Nullable<int> restonmac { get; set; }
        [Column] public Nullable<int> prevrestmac { get; set; }

    }
}
