using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "COLOR")]
    public class ColoursView
    {
        [Column(Name = "Color ID", IsPrimaryKey = true, IsDbGenerated = false)] public string ColorId { get; set; }
        [Column] public string COLOR { get; set; }
        [Column] public string ColorFRdesc { get; set; }
        [Column] public string ColorFRdesc2 { get; set; }
        [Column] public string ColorENdesc { get; set; }

    }
}
