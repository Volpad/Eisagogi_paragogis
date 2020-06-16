using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "GMP")]
    public class GMP
    {
        [Column(Name = "FINISHSAP ID")] public string FINISHAP_ID { get; set; }
        [Column] public string PROGRAM { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
    }
}
