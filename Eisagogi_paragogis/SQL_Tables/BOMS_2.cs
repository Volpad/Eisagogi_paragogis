using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "BOMS 2")]
    public class BOMS_2
    {
        [Column(Name = "FINISH ID")] public string FINISHAP_ID { get; set; }
        [Column] public string COLOR { get; set; }
        [Column] public string PART { get; set; }
        [Column] public Nullable<int> ΠΟΣΟΤΗΤΑ { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }


    }
}
