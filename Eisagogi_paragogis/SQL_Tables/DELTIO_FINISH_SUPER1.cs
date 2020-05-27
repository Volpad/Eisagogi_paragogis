using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "DELTIO SUPER FINISH1")]
    public class DELTIO_FINISH_SUPER1
    {
        [Column(Name = "TOTAL ID")] public Nullable<int> TOTAL_ID { get; set; }
        [Column(Name = "[FINISHSAP ID]")] public string FINISHAP_ID { get; set; }
        [Column] public string COLOR { get; set; }
        [Column] public string SIZE { get; set; }
        [Column(Name = "COL ID", IsPrimaryKey = true, IsDbGenerated = true)] public int COL_ID { get; set; }
        [Column] public Nullable<int> ΠΟΣΟΤΗΤΑ { get; set; }
        [Column(Name = "MACH SIZE")] public string MACH_SIZE { get; set; }
        [Column] public string FORMES { get; set; }
        [Column] public Nullable<int> QueueNo { get; set; }
        [Column] public Nullable<int> SIZESORTING { get; set; }
        [Column] public string PROGRAM { get; set; }
        [Column] public string COLORID { get; set; }
        [Column] public Nullable<int> InStock { get; set; }
        [Column] public string SpecialInstruction { get; set; }
        [Column] public string set { get; set; }
        [Column] public string YarnTitle { get; set; }
        [Column] public string SuplierColor { get; set; }
        [Column] public Nullable<int> Production { get; set; }
        [Column] public string mID { get; set; }
        [Column] public string sequence { get; set; }
        [Column] public string SuplierColorDesc { get; set; }






    }
}
