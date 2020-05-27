using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "BOMS4")]
    public class BOMS4
    {

        [Column(Name = "FINISHSAP ID", IsPrimaryKey = true)] public string FINISHAP_ID { get; set; }
        [Column] public string LABEL { get; set; }
        [Column] public string BOX { get; set; }
        [Column] public string PAPER { get; set; }
        [Column] public string IRON { get; set; }
        [Column] public string STICKER { get; set; }
        [Column] public string FORM { get; set; }
        [Column] public string ΡΙΖΟΧΑΡΤΟ { get; set; }
        [Column] public string ΖΩΝΑΚΙ { get; set; }



    }
}
