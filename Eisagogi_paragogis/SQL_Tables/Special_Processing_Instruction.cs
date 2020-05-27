using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "Special_Processing_Instruction")]
    public class special_Processing_Instruction
    {
        [Column(Name = "[Special processing instruction]")] public string Special_Processing_Instruction { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
    }
}
