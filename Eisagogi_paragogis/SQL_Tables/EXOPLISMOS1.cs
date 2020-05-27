using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "ΕΧΟΠΛΙΣΜΟΣ1")]
    public class EXOPLISMOS1
    {
        [Column] public string MHXANH { get; set; }
        [Column] public string DESCR { get; set; }
        [Column] public string TYPE { get; set; }
        [Column] public string SERNO { get; set; }
        [Column] public string DATE2 { get; set; }
        [Column] public string ShortName { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public bool Available { get; set; }

    }
}
