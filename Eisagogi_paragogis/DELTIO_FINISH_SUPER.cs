using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "DELTIO FINISH SUPER")]
    public class DELTIO_FINISH_SUPER
    {
        [Column(Name = "PROD DATE")] public Nullable<DateTime> PROD_DATE { get; set; }
        [Column] public string BARDIA { get; set; }
        [Column] public string EGKRI { get; set; }
        [Column] public string comments { get; set; }
        [Column(Name = "FINISHSAP ID")] public string FINISHAP_ID { get; set; }
        [Column(Name = "TOTAL ID", IsPrimaryKey = true, IsDbGenerated = true)] public int TOTAL_ID { get; set; }
        [Column] public string MHXANH { get; set; }
        [Column(Name = "ΠΡΟΓ ΜΗΧ")] public string PROG_MIX { get; set; }
        [Column] public Boolean end { get; set; }
        [Column] public string mID { get; set; }
        [Column] public string USER { get; set; }
        [Column] public string OrderNo { get; set; }
        [Column] public string SPECIALPACK { get; set; }
        [Column] public string orderUnit { get; set; }
        [Column] public Nullable<int> multi { get; set; }
        [Column] public string DeliveryDate { get; set; }
        [Column] public string TotalQTY { get; set; }
        [Column] public string CONFIRMATION_DATE { get; set; }
        [Column] public string CONFIRMATION_NAME { get; set; }
        [Column] public string ΜΕΜΟ { get; set; }
        [Column] public string Status { get; set; }
        [Column] public string WalkOrder { get; set; }
        [Column] public Boolean ShowInProdPlan { get; set; }
        [Column] public Decimal waste { get; set; }

    }
}
