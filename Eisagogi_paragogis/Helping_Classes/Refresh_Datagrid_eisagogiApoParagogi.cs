using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    public static class Refresh_Datagrid_eisagogiApoParagogi
    {

        public static event EventHandler DataChanged;

        public static void NotifyDataChanged()
        {
            EventHandler temp = DataChanged;
            if (temp != null)
            {
                temp(null, EventArgs.Empty);
            }
        }
    }
}
