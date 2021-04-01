using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Eisagogi_paragogis.Windows
{
    /// <summary>
    /// Interaction logic for Days_Calculation.xaml
    /// </summary>
    public partial class Days_Calculation : Window
    {
        public Days_Calculation()
        {
            InitializeComponent();


            using(var context = new Production18())
            {
                var data = from c in context.DELTIO_FINISH_SUPER1
                           where c.TOTAL_ID.Equals(16187) || c.TOTAL_ID.Equals(16086) || c.TOTAL_ID.Equals(16085)
                           select c;

                //var te = context.DELTIO_FINISH_SUPER1.Where(b => b.TOTAL_ID.Equals(16187) && b.TOTAL_ID.Equals(16187) && b.TOTAL_ID.Equals(16187)).GroupBy(c => c.TOTAL_ID).Select(d => new {Remaining = d.Sum( b => b.Production/24)});
                var te = data.GroupBy(c => c.TOTAL_ID).Select(d => new {Remaining = d.Sum( b => b.Production/24)});

                dg.ItemsSource = te;
            }
        }
    }
}
