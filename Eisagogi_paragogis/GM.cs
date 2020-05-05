using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "GM")]
    public class GM
    {
        [Column] public string MHXANH { get; set; }
        [Column(Name = "FINISHAP ID")] public string FINISHAP_ID { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
    }
}
