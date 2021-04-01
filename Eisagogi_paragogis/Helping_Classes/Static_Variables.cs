using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Eisagogi_paragogis
{
    static class Static_Variables
    {
       
        public static int prodviewtotalid;
        public static bool isWalk;
        public static int rowid;
        public static int totalid;
        public static string finishid;
#pragma warning disable CS0649 // Field 'Static_Variables.sales' is never assigned to, and will always have its default value null
        public static IEnumerable<walk_prd_FullItemLinesList> sales;
#pragma warning restore CS0649 // Field 'Static_Variables.sales' is never assigned to, and will always have its default value null
        public static int kattotalid;
        public static string katmachineno;
        public static string printer;
        public static int colourid;
        public static int sequence;
        public static bool iswalkopen = false;
        public static bool isothersopen = false;
#pragma warning disable CS0649 // Field 'Static_Variables.document' is never assigned to, and will always have its default value null
        public static FixedDocumentSequence document;
#pragma warning restore CS0649 // Field 'Static_Variables.document' is never assigned to, and will always have its default value null
        public static FixedDocument fixedDoc;
        public static bool tsouvali_click = false;
        public static bool ProductMachinesChanged = false;
        public static bool print_form = false;
        public static bool adminRights = false;
    }
}
