using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "boms3a")]
    public class boms3a
    {

        [Column(Name = "FINISHAP ID")] public string FINISHAP_ID { get; set; }
        [Column] public string NO { get; set; }
        [Column(Name = "MACH SIZE")] public string MACH_SIZE { get; set; }
        [Column] public string PAT { get; set; }
        [Column] public string MKAL { get; set; }
        [Column] public string PKAL { get; set; }
        [Column] public string YLAS { get; set; }
        [Column] public string PLAS { get; set; }
        [Column] public string WEIG { get; set; }
        [Column] public string FORMES { get; set; }
        [Column] public string SIZESORTING { get; set; }
        [Column] public string SizeHeadings { get; set; }
        [Column] public Nullable<int> ProductionTime { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }


    }
}
