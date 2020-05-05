using System;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "Production_Plan_Changes")]
    public class Production_Plan_Changes
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int id { get; set; }
        [Column] public Nullable<int> Value_set { get; set; }
        [Column] public Nullable<DateTime> date { get; set; }
        [Column] public string user { get; set; }
        [Column] public string control { get; set; }
        [Column] public string Machine { get; set; }
        [Column] public Boolean isChanged { get; set; }
        [Column] public string Change_Type { get; set; }
        


    }
}