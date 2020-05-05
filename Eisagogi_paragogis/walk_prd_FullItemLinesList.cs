using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Table(Name = "walk_prd_FullItemLinesList")]
    public class walk_prd_FullItemLinesList
    {
        [Column] public int GID { get; set; }
       // [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int GID { get; set; }
        [Column] public Nullable<int> fItemGID { get; set; }
        [Column] public string ItemCode { get; set; }
        [Column] public string ItemDescription { get; set; }
        [Column] public byte ItemInactive { get; set; }
        [Column] public string ColorCode { get; set; }
        [Column] public string SizeCode { get; set; }
        [Column] public string SizeDescription { get; set; }
        [Column] public DateTime RegistrationDate { get; set; }
        [Column] public DateTime ESDCreated { get; set; }
        [Column] public Nullable<DateTime> ESDModified { get; set; }
        [Column] public Nullable<Decimal> Quantity { get; set; }
        [Column] public Nullable<Decimal> ESFIItemPeriodics_CreditQty { get; set; }
        [Column] public Nullable<Decimal> ESFIItemPeriodics_DebitQty { get; set; }
        [Column] public string EntryTypeCode { get; set; }
        [Column] public string EntryTypeDescription { get; set; }
        [Column] public string DocTypeCode { get; set; }
        [Column] public string DocTypeDescription { get; set; }
        [Column] public string DocumentCode { get; set; }
        [Column] public Nullable<int> LineNumber { get; set; }
        [Column] public Nullable<int> LineType { get; set; }
        [Column] public string Reasoning { get; set; }
        [Column] public string CompWRHCode { get; set; }
        [Column] public string CompWRHDescription { get; set; }

    }
}
