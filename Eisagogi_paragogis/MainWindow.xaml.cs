using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Production18 production18 = new Production18();

        private static IEnumerable<eisagogiParagogis> eisagogiParagogis = from eisagogiParagogis in production18.eisagogiParagogis
                                                          orderby eisagogiParagogis.date descending
                                                         // where Vamvakera.Inactive == true
                                                          select eisagogiParagogis;

        private static IEnumerable<DELTIO_FINISH_SUPER1> DELTIO_FINISH_SUPER1 = from DELTIO_FINISH_SUPER1 in production18.DELTIO_FINISH_SUPER1
                                                                          //orderby eisagogiParagogis.date descending
                                                                          // where Vamvakera.Inactive == true
                                                                          select DELTIO_FINISH_SUPER1;


        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        //List<eisagogiParagogis> listeisagogiParagogis = eisagogiParagogis.ToList();

        //string test = ;

        //   List<eisagogiParagogis> EisagogiParagogis = eisagogiParagogis.ToList();


        public MainWindow()
        {
            InitializeComponent();

            Grid1.ItemsSource = DELTIO_FINISH_SUPER1;
         
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Production_Plan production_Plan = new Production_Plan();
            production_Plan.Show();
            this.Close();
        }
    }
}
