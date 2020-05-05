using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "BOMS3")]
    public class BOMS3
    {
        [Column(Name = "FINISHAP ID")] public string FINISHAP_ID { get; set; }
        [Column] public string NO { get; set; }
        [Column] public string PAT { get; set; }
        [Column] public string MKAL { get; set; }
        [Column] public string PKAL { get; set; }
        [Column] public string YLAS { get; set; }
        [Column] public string PLAS { get; set; }
        [Column] public string WEIG { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }


    }
}
