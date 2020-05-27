using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "MachinePosition")]
    public class MachinePosition
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public string MachineNo { get; set; }
        [Column] public string MachinePos { get; set; }
    }
}
