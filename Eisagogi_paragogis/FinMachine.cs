using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "FinMachine")]
    public class FinMachine
    {
        [Column(Name = "ΚΩΔ# ΜΗΧΑΝΗΣ")] public string KOD_MHXANHS { get; set; }
        [Column(Name = "FINISHAP ID")] public string FINISHAP_ID { get; set; }
        [Column(Name = "ΠΡΟΓΡΑΜΜΑ")] public string PROGRAMMA { get; set; }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }

    }
}
