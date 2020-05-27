using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddTsouvali.xaml
    /// </summary>
    public partial class AddTsouvali : Window
    {
        public AddTsouvali()
        {
            InitializeComponent();
            dozen.PreviewTextInput += new TextCompositionEventHandler(NumberValidationTextBox);
            socks.PreviewTextInput += new TextCompositionEventHandler(NumberValidationTextBox);

            using (var context = new Production18())
            {
                var available = context.EXOPLISMOS1.OrderBy(d => d.DESCR).Where(c => c.Available == true);
                machc.ItemsSource = available.Select(c => c.DESCR).ToList();
                if (Static_Variables.tsouvali_click)
                {
                    barcode.Text = context.eisagogiParagogis.Where(c => c.AutoNo == Static_Variables.rowid).Select(x => x.barcode).FirstOrDefault().ToString();
                }
            }

            if (Static_Variables.tsouvali_click)
            {
                
                fill_data();
                machc.Focus();
                barcode.Focusable = true;
                barcode.IsReadOnly = false;
            }
            else
            {
                Auto_no.Text = "";
                date.Text = "";
                barcode.Text = "";
                reference.Text = "";
                colour.Text = "";
                size.Text = "";
                machc.Text = "";
                dozen.Text = "";
                socks.Text = "";
                user.Text = "";

                barcode.Focus();
            }

         

        }

        private void fill_data()
        {
            using (var context = new Production18())
            {
                var ep = context.eisagogiParagogis;
                var dfs1 = context.DELTIO_FINISH_SUPER1;

                var bar = barcode.Text;
                int index = bar.IndexOf("-");
                var colidt = bar.Substring(0, index);
                var colid = Convert.ToInt32(colidt);


                reference.Text = dfs1.Where(c => c.COL_ID == colid).Select(d => d.FINISHAP_ID).FirstOrDefault();
                colour.Text = dfs1.Where(c => c.COL_ID == colid).Select(d => d.COLORID).FirstOrDefault() + "." + dfs1.Where(c => c.COL_ID == colid).Select(d => d.COLOR).FirstOrDefault();
                size.Text = dfs1.Where(c => c.COL_ID == colid).Select(d => d.SIZE).FirstOrDefault();

                if (Auto_no.Text == "")
                {



                    if (Static_Variables.tsouvali_click || ep.Any(c => c.barcode == barcode.Text))
                    {
                        Static_Variables.tsouvali_click = true;
                        var dat = ep.Where(c => c.barcode == bar).Select(d => d.date).FirstOrDefault().ToString();
                        index = dat.IndexOf(" ");
                        dat = dat.Substring(0, index);

                        Auto_no.Text = ep.Where(c => c.barcode == bar).Select(d => d.AutoNo).FirstOrDefault().ToString();
                        date.Text = dat;
                        machc.Text = ep.Where(c => c.barcode == bar).Select(d => d.Machine).FirstOrDefault();
                        dozen.Text = ep.Where(c => c.barcode == bar).Select(d => d.dozen).FirstOrDefault().ToString();
                        socks.Text = ep.Where(c => c.barcode == bar).Select(d => d.socks).FirstOrDefault().ToString();
                        user.Text = ep.Where(c => c.barcode == bar).Select(d => d.user).FirstOrDefault();

                    }
                    else
                    {
                        Auto_no.Text = "";
                        date.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        machc.Text = "";
                        dozen.Text = "";
                        socks.Text = "";
                        user.Text = "";

                    }
                }
                else
                {

                }

            }
        }

        private void barcode_KeyDown(object sender, KeyEventArgs e)
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
        
        private void barcode_LostFocus(object sender, RoutedEventArgs e)
        {
            
            using (var context = new Production18())
            {
                var bar = barcode.Text;
                int index = bar.IndexOf("-");

                if (index > 0)
                {
                    var colidt = bar.Substring(0, index);
                    var colid = Convert.ToInt32(colidt);
                    var dfs1 = context.DELTIO_FINISH_SUPER1;
                    Static_Variables.tsouvali_click = false;

                    if (dfs1.Any(c => c.COL_ID == colid))
                    {
                        if (Auto_no.Text != "")
                        {
                            bool doubleDelt = context.eisagogiParagogis.Any(c => c.barcode == barcode.Text);
                            if (doubleDelt)
                            {
                                MessageBox.Show("Το δελτίο είναι καταχωρημένο." + Environment.NewLine + " Δεν μπορεί να περαστεί 2 φορές");
                                this.Close();
                            }
                            else
                            {
                                fill_data();
                            }
                        }
                        else
                        {
                            fill_data();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Λάθος barcode. Ξανα σκανάρετε!!");
                        barcode.Text = "";
                    }
                    
                    

                }
                else
                {

                }



            }
        }

        private void mach_GotFocus(object sender, RoutedEventArgs e)
        {
            if (barcode.Text == "")
            {
                barcode.Focus();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void machc_LostFocus(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                var gm = context.GM;
                var available = context.EXOPLISMOS1.OrderBy(d => d.DESCR).Where(c => c.Available == true);


                if (!available.Any(c => c.DESCR == machc.Text) && barcode.Text != "" && machc.Text != "")
                {
                    MessageBox.Show("Λάθος Mηχανή!!");
                    machc.Text = "";

                }

                bool correct = gm.Where(c => c.FINISHAP_ID == reference.Text).Any(x => x.MHXANH == available.Where(c => c.DESCR == machc.Text).Select(f => f.MHXANH).FirstOrDefault());
                if (!correct && machc.Text != "")
                {
                    MessageBox.Show("Ο κωδικός " + reference.Text + " δεν μπορεί να βγεί στη μηχανή " + machc.Text + Environment.NewLine + "Βάλτε σωστή μηχανή");
                    machc.Text = "";
                }

            }
        }

        private void dozen_GotFocus(object sender, RoutedEventArgs e)
        {
            if (machc.Text == "")
            {
                machc.Focus();
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (dozen.Text == "")
            {
                dozen.Text = "0";
            }
            if (socks.Text == "")
            {
                socks.Text = "0";
            }

            if (machc.Text == "")
            {
                MessageBox.Show("Το πεδίο ΜΗΧΑΝΗ είναι υποχρεωτικό");
                machc.Focus();

            }

            if (user.Text == "")
            {
                MessageBox.Show("Το πεδίο ΧΡΗΣΤΗΣ είναι υποχρεωτικό");
                user.Focus();

            }

            using (var context = new Production18())
            {

                var bk = barcode.Text;
                int index = bk.IndexOf("-");
                var colid = bk.Substring(0, index);
                var totalid = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == Convert.ToInt32(colid)).Select(x => x.TOTAL_ID).FirstOrDefault();

                var check = context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid));

                int socksi = context.eisagogiParagogis.Any(c => c.Total_Id == Convert.ToInt32(totalid)) ? context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid)).Sum(x => x.socks ?? 0) : 0;
                int dozeni = context.eisagogiParagogis.Any(c => c.Total_Id == Convert.ToInt32(totalid)) ? context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid)).Sum(x => x.dozen ?? 0) : 0 ;
                int produced = socksi + (dozeni * 24);
                int orderqty = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == Convert.ToInt32(totalid)).Sum(x => x.Production ?? 0);
                int rest = (orderqty - produced) / 24;
                var onMachine = context.Machineqty.Where(c => c.MachineNo == machc.Text && c.queueNo != "deleted").Any(x => x.AccessNo == Convert.ToInt32(totalid));
                var mqty = context.Machineqty.Where(x => x.AccessNo == Convert.ToInt32(totalid) && x.queueNo != "deleted");

                if (Auto_no.Text != "")
                {
                    eisagogiParagogis ep = (from c in context.eisagogiParagogis
                                            where c.AutoNo == Convert.ToInt32(Auto_no.Text)
                                            select c).SingleOrDefault();
                    ep.user = user.Text;
                    ep.dozen = Convert.ToInt32(dozen.Text);
                    ep.socks = Convert.ToInt32(socks.Text);
                    ep.Machine = machc.Text;
                    ep.barcode = barcode.Text;
                    ep.Total_Id = totalid;
                    ep.Col_Id = Convert.ToInt32(colid);
                    context.SubmitChanges();


                     socksi = context.eisagogiParagogis.Any(c => c.Total_Id == Convert.ToInt32(totalid)) ? context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid)).Sum(x => x.socks ?? 0) : 0;
                     dozeni = context.eisagogiParagogis.Any(c => c.Total_Id == Convert.ToInt32(totalid)) ? context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid)).Sum(x => x.dozen ?? 0) : 0;
                     produced = socksi + (dozeni * 24);
                     orderqty = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == Convert.ToInt32(totalid)).Sum(x => x.Production ?? 0);
                     rest = (orderqty - produced) / 24;

                    orderqty = orderqty / 24;
                    produced = produced / 24;
                    foreach (var c in mqty)
                    {
                        c.Rest = rest;
                        c.Productionqty = produced;
                    }
                    if (!onMachine)
                    {
                        int max = Convert.ToInt32(context.Machineqty.Where(c => c.MachineNo == machc.Text && c.queueNo != "deleted").OrderByDescending(f => f.queueNo).Select(x => x.queueNo).FirstOrDefault());
                        max = max + 1;

                        Machineqty inputm = new Machineqty
                        {
                            AccessNo = totalid,
                            MachineNo = machc.Text,
                            queueNo = max.ToString(),
                            Orderqty = orderqty,
                            Productionqty = produced,
                            Rest = rest,
                            DailyQty = 0,
                            Status = false

                        };
                        context.Machineqty.InsertOnSubmit(inputm);

                    }
                }
                else
                {
                    eisagogiParagogis input = new eisagogiParagogis
                    {
                        barcode = barcode.Text,
                        date = DateTime.Now,
                        user = user.Text,
                        dozen = Convert.ToInt32(dozen.Text),
                        socks = Convert.ToInt32(socks.Text),
                        Machine = machc.Text,
                        Col_Id = Convert.ToInt32(colid),
                        Total_Id = totalid

                    };
                    context.eisagogiParagogis.InsertOnSubmit(input);
                    context.SubmitChanges();

                    socksi = context.eisagogiParagogis.Any(c => c.Total_Id == Convert.ToInt32(totalid)) ? context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid)).Sum(x => x.socks ?? 0) : 0;
                    dozeni = context.eisagogiParagogis.Any(c => c.Total_Id == Convert.ToInt32(totalid)) ? context.eisagogiParagogis.Where(c => c.Total_Id == Convert.ToInt32(totalid)).Sum(x => x.dozen ?? 0) : 0;
                    produced = socksi + (dozeni * 24);
                    orderqty = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == Convert.ToInt32(totalid)).Sum(x => x.Production ?? 0);
                    rest = (orderqty - produced) / 24;
                    orderqty = orderqty / 24;
                    produced = produced / 24;

                    foreach (var c in mqty)
                    {
                        c.Rest = rest;
                        c.Productionqty = produced;
                    }

                    if (!onMachine)
                    {
                        int max = Convert.ToInt32(context.Machineqty.Where(c => c.MachineNo == machc.Text && c.queueNo != "deleted").OrderByDescending(f => f.queueNo).Select(x => x.queueNo).FirstOrDefault());
                        max = max + 1;

                        Machineqty inputm = new Machineqty
                        {
                            AccessNo = totalid,
                            MachineNo = machc.Text,
                            queueNo = max.ToString(),
                            Orderqty = orderqty,
                            Productionqty = produced,
                            Rest = rest,
                            DailyQty = 0,
                            Status = false

                        };
                        context.Machineqty.InsertOnSubmit(inputm);

                    }


                    
                }
                context.SubmitChanges();
                Refresh_Datagrid_eisagogiApoParagogi.NotifyDataChanged();
                Static_Variables.tsouvali_click = false;
                this.Close();

            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Static_Variables.rowid = 0;
            Static_Variables.tsouvali_click = false;
        }
    }
}
