using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Kataxwrisi_Deltiwn_Paragogis.xaml
    /// </summary>
    public partial class Kataxwrisi_Deltiwn_Paragogis : Window
    {
        public string finishap { get; set; }

        public Kataxwrisi_Deltiwn_Paragogis()
        {
            InitializeComponent();
            this.DataContext = this;

            refresh_Grid();
            date.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            date.Focusable = false;
            refference.Focusable = false;
            deltioNumber.Text = "";
            deltioNumber.Focus();

            finishap = "";
    
            
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void refresh_Grid()
        {
            using (var context = new Production18())
            {
                var of = from c in context.Orders_Fin
                         orderby c.id descending
                         select new
                         {
                             AA = c.id,
                             Ημερομηνία = c.DATE_ENTRY.Day + "/" + c.DATE_ENTRY.Month + "/" + c.DATE_ENTRY.Year,
                             Δελτίο = c.TOTAL_ID,
                             Κωδικός = c.FINISHAP_ID
                         };

                grid.ItemsSource = of;

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
                int index = data.IndexOf("AA = ");
                int index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 5, index2 - (index + 5));
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
            var id = Static_Variables.rowid;

            using (var context = new Production18())
            {
                var rowToBeDeleted = context.Orders_Fin.Where(c => c.id == id).ToList();

                foreach (var rows in rowToBeDeleted)
                {
                    context.Orders_Fin.DeleteOnSubmit(rows);
                }
                context.SubmitChanges();
            }
            Refresh_Datagrid_eisagogiApoParagogi.NotifyDataChanged();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh_Datagrid_eisagogiApoParagogi.DataChanged += new EventHandler(Refresh_Datagrid_DataChanged);
        }

        private void Refresh_Datagrid_DataChanged(object sender, EventArgs e)
        {
            refresh_Grid();
        }

        private void deltioNumber_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            using (var context = new Production18())
            {
                int number = 0;
                var test = int.TryParse(deltioNumber.Text, out number) ? Convert.ToInt32(deltioNumber.Text) : 0;
                bool x = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == number);

                if (x)
                {
                    refference.Text = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == number).Select(f => f.FINISHAP_ID).FirstOrDefault();
                    
                }
                else
                {
                    if (deltioNumber.Text != "")
                    {
                        refference.Text = "";
                        MessageBox.Show("Λάθος αριθμός καταχώρησης", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void deltioNumber_KeyDown(object sender, KeyEventArgs e)
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

        private void okbutton_GotFocus(object sender, RoutedEventArgs e)
        {
            if (refference.Text == "")
            {
                deltioNumber.Focus();
            }
            else
            {
                using (var context = new Production18())
                {
                    bool exists = context.Orders_Fin.Any(c => c.TOTAL_ID == Convert.ToInt32(deltioNumber.Text));

                    if (exists)
                    {
                        var id = context.Orders_Fin.Where(c => c.TOTAL_ID == Convert.ToInt32(deltioNumber.Text)).Select(x => x.id).FirstOrDefault();
                        MessageBox.Show("Έχει γίνει καταχώρηση του δελτίου με αριθμό: " + id);
                        deltioNumber.Focus();
                    }
                }
            }
        }

        private void okbutton_Click(object sender, RoutedEventArgs e)
        {
            if (deltioNumber.Text != "")
            {
                using (var context = new Production18())
                {
                    Orders_Fin input = new Orders_Fin
                    {
                        FINISHAP_ID = refference.Text,
                        TOTAL_ID = Convert.ToInt32(deltioNumber.Text),
                        DATE_ENTRY = DateTime.Now
                    };
                    context.Orders_Fin.InsertOnSubmit(input);
                    context.SubmitChanges();
                }
                Refresh_Datagrid_eisagogiApoParagogi.NotifyDataChanged();
                refference.Text = "";
                deltioNumber.Text = "";
            }
            deltioNumber.Focus();
        }
    }
    
}
