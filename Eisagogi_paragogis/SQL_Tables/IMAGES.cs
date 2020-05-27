using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "IMAGES")]
    public class IMAGES
    {
        [Column] public string GUIDE { get; set; }
        [Column] public byte[] PICTURE { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
    }
}
