using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "Staff")]
    public class Staff
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int NoName { get; set; }
        [Column] public string Name { get; set; }
        [Column] public string Surname { get; set; }
    }
}
