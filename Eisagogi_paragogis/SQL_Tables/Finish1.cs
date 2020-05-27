using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "Finish1")]
    public class Finish1
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = false, Name = "FINISHSAP ID")] public string FINISHAP_ID { get; set; }
        [Column] public string DESCRIPTION { get; set; }
        [Column] public string COUNTRY { get; set; }
        [Column] public DateTime DATE1 { get; set; }
        [Column] public string BARCODE { get; set; }
        [Column(Name = "TEM/KIB")] public string TEM_KIB { get; set; }
        [Column] public string COMMENTS_old { get; set; }
        [Column] public string EGKRI { get; set; }
        [Column] public string MHXANH { get; set; }
        [Column] public string PLEJH { get; set; }
        [Column] public string ENOSI { get; set; }
        [Column] public string ARITHMISI { get; set; }
        [Column] public string ΜΕΤΡΗΜΑ { get; set; }
        [Column] public Boolean cor { get; set; }
        [Column] public string SPI { get; set; }
        [Column] public string COMP { get; set; }
        [Column] public Boolean Inactive { get; set; }
        [Column] public string COMMENTS_Backup { get; set; }
        [Column] public string COMMENTS { get; set; }
        

    }
}
