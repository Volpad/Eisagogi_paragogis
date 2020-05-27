using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "EXOPLISMOS")]
    public class EXOPLISMOS
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public string MHXANH { get; set; }
        [Column] public string DESCR { get; set; }
        [Column] public string TYPE { get; set; }
        [Column] public string SERNO { get; set; }
        [Column] public string DATE2 { get; set; }

    }
}
