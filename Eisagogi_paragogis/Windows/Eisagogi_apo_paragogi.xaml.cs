using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for Eisagogi_apo_paragogi.xaml
    /// </summary>
    public partial class Eisagogi_apo_paragogi : Window
    {
        public Eisagogi_apo_paragogi()
        {
            InitializeComponent();
            kataxwriseis.IsReadOnly = true;
            kataxwriseis.AutoGenerateColumns = false;
          //  kataxwriseis.HeadersVisibility = DataGridHeadersVisibility.Row;

            refresh_Grid();

        }

        private void refresh_Grid()
        {
            using (var context = new Production18())
            {
                var ep = context.eisagogiParagogis;
                var dfs1 = context.DELTIO_FINISH_SUPER1;

                var gridData = from e in ep
                               join d in dfs1
                               on e.Col_Id equals d.COL_ID into g
                               from d in g.DefaultIfEmpty()
                               orderby e.AutoNo descending
                               select new
                               {
                                   Auto_no = e.AutoNo,
                                   Barcode = e.barcode,
                                   Ημερομηνία = e.date,
                                   Μηχανή = e.Machine,
                                   Κωδικός = d.FINISHAP_ID,
                                   Χρώμα = d.COLORID + " . " + d.COLOR,
                                   Μέγεθος = d.SIZE,
                                   Δωδεκάδες = e.dozen,
                                   Κάλτσες = e.socks,
                                   Υπεύθυνος = e.user
                               };
                kataxwriseis.ItemsSource = gridData;
            }
        }

        private void newProd_Click(object sender, RoutedEventArgs e)
        {
            Window tsouvali = new AddTsouvali();
            tsouvali.Show();
            refresh_Grid();
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            refresh_Grid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh_Datagrid_eisagogiApoParagogi.DataChanged += new EventHandler(Refresh_Datagrid_eisagogiApoParagogi_DataChanged);
        }

        private void Refresh_Datagrid_eisagogiApoParagogi_DataChanged(object sender, EventArgs e)
        {
            refresh_Grid();
        }

        private void kart_print_Click(object sender, RoutedEventArgs e)
        {
            kartelaBox dbox = new kartelaBox("Εισάγετε κωδικό καρτέλας", "");
            if (dbox.ShowDialog() == true)
            {
                var temp = Convert.ToInt32(dbox.Answer);
            }

            if (!dbox.Answer.Equals(null))
            {

                using (var context = new Production18())
                {
                    if (dbox.Answer != "")
                    {

                        bool correct = context.DELTIO_FINISH_SUPER1.Any(c => c.COL_ID == Convert.ToInt32(dbox.Answer));
                        //  Static_Variables.colourid
                        if (context.DELTIO_FINISH_SUPER1.Any(c => c.COL_ID == Convert.ToInt32(dbox.Answer)))
                        {
                            var seq = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == Convert.ToInt32(dbox.Answer)).Select(x => x.sequence).FirstOrDefault();

                            Static_Variables.sequence = Convert.ToInt32(seq) + 1;

                            Static_Variables.colourid = Convert.ToInt32(dbox.Answer);
                            PrinterSettings settings = new PrinterSettings();
                            Static_Variables.printer = settings.PrinterName;
                            Static_Variables.fixedDoc = new FixedDocument();


                            KartaProiontos karta = new KartaProiontos();


                            PrintDialog dialog = new PrintDialog();
                            var doc = Static_Variables.fixedDoc.DocumentPaginator;
                            for (int i = 0; i < doc.PageCount; i++)
                            {
                                dialog.PrintVisual(doc.GetPage(i).Visual, "Page " + i);
                            }

                            var dfs1 = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == Convert.ToInt32(dbox.Answer));
                            foreach (var c in dfs1)
                            {
                                c.sequence = Static_Variables.sequence.ToString();
                            }
                            context.SubmitChanges();
                            //karta.Show();
                            MessageBox.Show("Η καρτέλα εκτυπώθηκε", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        else
                        {
                            MessageBox.Show("Ο αριθμός καρτέλας δεν υπάρχει", "Πρόβλημα!", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Λάθος αριθμός", "Πρόβλημα!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


            }

            }

        private void RowMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // DataGridRow row = sender as DataGridRow;

            var hit = VisualTreeHelper.HitTest((Visual)sender, e.GetPosition((IInputElement)sender));
            DependencyObject cell = VisualTreeHelper.GetParent(hit.VisualHit);
            while (cell != null && !(cell is System.Windows.Controls.DataGridCell)) cell = VisualTreeHelper.GetParent(cell);
            System.Windows.Controls.DataGridCell targetCell = cell as System.Windows.Controls.DataGridCell;



            if (cell is DataGridCell)
            {
                while (cell != null && !(cell is DataGridRow))
                {
                    cell = VisualTreeHelper.GetParent(cell);
                }

                DataGridRow row = cell as DataGridRow;

                var data = row.Item.ToString();
                int index = data.IndexOf("Auto_no = ");
                int index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 10, index2 - (index + 10));
                    int id = Convert.ToInt32(temp);
                    Static_Variables.rowid = id;
                    ContextMenu cm = this.FindResource("cmenu") as ContextMenu;
                    cm.IsOpen = true;
                }
                //index = data.LastIndexOf("AccessNo =");
                //if (index > 0)
                //{
                //    var temp = data.Substring(index + 11, 5);
                //    int totalid = Convert.ToInt32(temp);

                //    Static_Variables.totalid = totalid;

                //}
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Static_Variables.tsouvali_click = true;
            Window tsouvali = new AddTsouvali();
            tsouvali.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var id = Static_Variables.rowid;

            using (var context = new Production18())
            {
                var rowToBeDeleted = context.eisagogiParagogis.Where(c => c.AutoNo == id).ToList();
                
                foreach (var rows in rowToBeDeleted)
                {
                    context.eisagogiParagogis.DeleteOnSubmit(rows);
                }
                context.SubmitChanges();  
            }
            Refresh_Datagrid_eisagogiApoParagogi.NotifyDataChanged();
        }
    }
}
