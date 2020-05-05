using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "RAW MATE")]
    public class RAW_MATE
    {
        [Column(Name = "Raw ID", IsPrimaryKey = true, IsDbGenerated = true)] public string Raw_ID { get; set; }
        [Column] public string Name { get; set; }
        [Column] public string Descr { get; set; }
        [Column] public string pack { get; set; }

    }
}
