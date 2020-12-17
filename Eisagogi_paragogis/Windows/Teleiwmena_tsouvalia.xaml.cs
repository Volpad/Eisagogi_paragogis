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

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for Teleiwmena_tsouvalia.xaml
    /// </summary>
    public partial class Teleiwmena_tsouvalia : Window
    {
        public Teleiwmena_tsouvalia()
        {
            InitializeComponent();
            this.DataContext = this;

            refreshGrid();
            reference.Focusable = false;
            colour.Focusable = false;
            size.Focusable = false;
            dialoges.Focusable = false;
            access_no.Text = "";
            reference.Text = "";
            colour.Text = "";
            size.Text = "";
            warehouse.Text = "";
            dialoges.Text = "";
            access_no.Focus();

            using (var context = new Production18())
            {
                warehouse.ItemsSource = context.Boading.GroupBy(x => x.Warehouse).Select(c => c.Key);
            }


        }

        private void Refresh_Datagrid_DataChanged(object sender, EventArgs e)
        {
            refreshGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh_Datagrid_eisagogiApoParagogi.DataChanged += new EventHandler(Refresh_Datagrid_DataChanged);
        }

        private void refreshGrid()
        {
            using (var context = new Production18())
            {
                grid.ItemsSource = from c in context.Boading
                                   orderby c.ID descending
                                   select new
                                   {
                                       Id = c.ID,
                                       Ημερομηνία = c.BoardingDate,
                                       Αποθήκη = c.Warehouse,
                                       Access_No = c.AccessNo,
                                       Κωδικός = c.ItemCode,
                                       Χρώμα = c.ColorCode + "." + c.ColorGRdesc,
                                       Μέγεθος = c.SizeDesc,
                                       Ποσότητα_παραγωγής = c.D_qty + " Δωδ. και " + c.S_qty + " κάλ."
                                   };
            }
        }

        private void grid_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
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
                int index = data.IndexOf("Id = ");
                int index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 5, index2 - (index + 5));
                    int id = Convert.ToInt32(temp);
                    Static_Variables.rowid = id;
                    ContextMenu cm = this.FindResource("cmenu") as ContextMenu;
                    cm.IsOpen = true;
                }
            }

        }

        private void access_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                //TraversalRequest request = new TraversalRequest(focusDirection);
                //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                //if (elementWithFocus != null)
                //{
                //    if (elementWithFocus.MoveFocus(request)) e.Handled = true;
                //}

                save.Focus();

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var id = Static_Variables.rowid;

            using (var context = new Production18())
            {
                var rowToBeDeleted = context.Boading.Where(c => c.ID == id).ToList();

                foreach (var rows in rowToBeDeleted)
                {
                    context.Boading.DeleteOnSubmit(rows);
                }
                context.SubmitChanges();
            }
            Refresh_Datagrid_eisagogiApoParagogi.NotifyDataChanged();
        }

        private void warehouse_GotFocus(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                //  var colid = context.eisagogiParagogis.Where(c => c.barcode == access_no.Text).Select(x => x.Col_Id).FirstOrDefault();
                int index = access_no.Text.IndexOf("-");
                int backSlashIndex = access_no.Text.IndexOf("-");

                var colid = (backSlashIndex > 0) ? int.Parse(access_no.Text.Substring(0, backSlashIndex)) : 0;
                if (context.eisagogiParagogis.Any(c => c.barcode == access_no.Text))
                {
                    reference.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.FINISHAP_ID).FirstOrDefault();
                    colour.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.COLORID).FirstOrDefault() + "." + context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.COLOR).FirstOrDefault();
                    size.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.SIZE).FirstOrDefault();
                }
                else
                {

                    if (context.DELTIO_FINISH_SUPER1.Any(c => c.COL_ID == colid))
                    {
                        MessageBox.Show("Το δελτίο δεν έχει περαστεί απο τη παραγωγή.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        access_no.Text = "";
                        access_no.Focus();
                    }
                    else
                    {
                        if (access_no.Text != "")
                        {
                            MessageBox.Show("Λάθος αριθμός καταχώρησης", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            access_no.Text = "";
                            access_no.Focus();
                        }
                    }
                }
            }

        }

        private void access_no_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void warehouse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                TraversalRequest request = new TraversalRequest(focusDirection);
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                if (elementWithFocus != null)
                {
                    if (elementWithFocus.MoveFocus(request)) e.Handled = true;
                }

            }

        }

        private void save_GotFocus(object sender, RoutedEventArgs e)
        {

            using (var context = new Production18())
            {
                //  var colid = context.eisagogiParagogis.Where(c => c.barcode == access_no.Text).Select(x => x.Col_Id).FirstOrDefault();
                int index = access_no.Text.IndexOf("-");
                int backSlashIndex = access_no.Text.IndexOf("-");

                var colid = (backSlashIndex > 0) ? int.Parse(access_no.Text.Substring(0, backSlashIndex)) : 0;
                if (context.eisagogiParagogis.Any(c => c.barcode == access_no.Text))
                {
                    reference.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.FINISHAP_ID).FirstOrDefault();
                    colour.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.COLORID).FirstOrDefault() + "." + context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.COLOR).FirstOrDefault();
                    size.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.SIZE).FirstOrDefault();
                }
                else
                {

                    if (context.DELTIO_FINISH_SUPER1.Any(c => c.COL_ID == colid))
                    {
                        MessageBox.Show("Το δελτίο δεν έχει περαστεί απο τη παραγωγή.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        access_no.Text = "";
                        access_no.Focus();
                    }
                    else
                    {
                        if (access_no.Text != "")
                        {
                            MessageBox.Show("Λάθος αριθμός καταχώρησης", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            access_no.Text = "";
                            access_no.Focus();
                        }
                    }
                }
            }

            if (access_no.Text == "")
            {
                access_no.Focus();
            }
            else
            {
                using (var context = new Production18())
                {
                    bool exists = context.Boading.Any(c => c.AccessNo == access_no.Text);
                    if (exists)
                    {
                        MessageBox.Show("Έχει γίνει καταχώρηση του δελτίου, με αριθμό " + context.Boading.Where(c => c.AccessNo == access_no.Text).Select(x => x.ID).FirstOrDefault(), "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        access_no.Focus();
                    }
                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if(warehouse.Text == "")
            {
                MessageBox.Show("Πρέπει να είναι συμπληρωμένο το πεδίο 'Από'", "", MessageBoxButton.OK, MessageBoxImage.Error);
                warehouse.Focus();
            }
            else
            {
                if(access_no.Text != "")
                {
                    using (var context = new Production18())
                    {
                        var colid = context.eisagogiParagogis.Where(c => c.barcode == access_no.Text).Select(x => x.Col_Id).FirstOrDefault();

                        Boarding input = new Boarding
                        {
                            BoardingDate = DateTime.Now,
                            Warehouse = warehouse.Text,
                            AccessNo = access_no.Text,
                            ItemCode = reference.Text,
                            ColorCode = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.COLORID).FirstOrDefault(),
                            ColorGRdesc = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.COLOR).FirstOrDefault(),
                            SizeDesc = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == colid).Select(x => x.SIZE).FirstOrDefault(),
                            D_qty = context.eisagogiParagogis.Where(c => c.barcode == access_no.Text).Select(x => x.dozen).FirstOrDefault(),
                            S_qty = context.eisagogiParagogis.Where(c => c.barcode == access_no.Text).Select(x => x.socks).FirstOrDefault(),
                            Df_qty = 0,
                            Sf_qty = 0,
                            notes = "",
                            Col_ID = colid
                        };
                        context.Boading.InsertOnSubmit(input);
                        context.SubmitChanges();
                    }
                    Refresh_Datagrid_eisagogiApoParagogi.NotifyDataChanged();
                    reference.Text = "";
                    colour.Text = "";
                    size.Text = "";
                    access_no.Text = "";
                    dialoges.Text = "";

                }
                access_no.Focus();
            }
        }


    }
}
