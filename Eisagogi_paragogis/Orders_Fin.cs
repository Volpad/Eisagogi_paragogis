﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "Orders_Fin")]
    public class Orders_Fin
    {
        [Column(Name = "FINISHAP ID")] public string FINISHAP_ID { get; set; }
        [Column(Name = "TOTAL ID", IsPrimaryKey = true, IsDbGenerated = true)] public Nullable<int> TOTAL_ID { get; set; }
        [Column(Name = "DATE ENTRY")] public DateTime DATE_ENTRY { get; set; }
        [Column] public int id { get; set; }
    }
}
