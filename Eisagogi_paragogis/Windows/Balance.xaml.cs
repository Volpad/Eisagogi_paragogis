using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Balance.xaml
    /// </summary>
    public partial class Balance : Window
    {
        
        public Balance()
        {
            InitializeComponent();

            //fromdate.SelectedDate = DateTime.Now.Date;
            //todate.SelectedDate = DateTime.Now.AddDays(1).Date;

            if(Static_Variables.finishid != "")
            {
                using (var context = new Production18())
                {
                    setGrid(context, Static_Variables.finishid);
                    productName.Text = Static_Variables.finishid;
                }
            }

        }


        private void productName_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void productName_GotFocus(object sender, RoutedEventArgs e)
        {
           

        }

        private void productName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            

        }

        private void productName_GotMouseCapture(object sender, MouseEventArgs e)
        {
           
        }

        private void productName_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (s.Text == "Αναζήτηση Κωδικού")
            {

                s.Text = "";
            }
            else
            {
                s.SelectAll();
            }

        }

        private void productName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ResetFocusArea.Focus();

                using (var context = new Production18())
                {
                    var product = productName.Text;

                    setGrid(context, product);
                }
            }
            if (e.Key == Key.Escape)
            {
                ResetFocusArea.Focus();
                
            }

        }

        private void setGrid(Production18 context, string product)
        {
            var ep = from eisagogiParagogis in context.eisagogiParagogis
                     select eisagogiParagogis;

            var boarding = from Boarding in context.Boading
                           where Boarding.ItemCode.Equals(product)
                           select Boarding;

            var sfw = from Semi_finished_warehouse in context.Semi_finished_warehouse
                      select Semi_finished_warehouse;

            var ship = from shipment in context.shipment
                       select shipment;

            var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          // where DELTIO_FINISH_SUPER.FINISHAP_ID == product
                      select DELTIO_FINISH_SUPER;

            //var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
            //               //where DELTIO_FINISH_SUPER1.FINISHAP_ID == product
            //           select DELTIO_FINISH_SUPER1;

            var ib = from ItemBalances in context.ItemBalances
                         //where ItemBalances.ItemCode == product
                     orderby ItemBalances.ColorCode
                     select new
                     {
                         ItemBalances.ItemCode,
                         Χρώμα = ItemBalances.ColorCode + "." + ItemBalances.ColorGRdesc,
                         ItemBalances.SizeDesc,
                         Αποθήκη = ItemBalances.Warehouse,
                         Παραγγελίες = ItemBalances.OrderQty,
                     };






            //var test = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
            //           where walk_prd_FullItemLinesList.ItemCode.Equals(product) && 
            //           walk_prd_FullItemLinesList.RegistrationDate >  DateTime.Parse(fromdate.ToString()) && 
            //           walk_prd_FullItemLinesList.RegistrationDate < DateTime.Parse(todate.ToString()) && 
            //          // walk_prd_FullItemLinesList.CompWRHCode == "1" && 
            //           (walk_prd_FullItemLinesList.EntryTypeDescription.Contains("Stock - Sales (VALUE/QTY)") || walk_prd_FullItemLinesList.DocTypeCode.Equals("ΔΝΓΠ")) && 
            //           !walk_prd_FullItemLinesList.EntryTypeDescription.Contains("return")
            //           select walk_prd_FullItemLinesList;


            //var deltia = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
            //             where walk_prd_FullItemLinesList.ItemCode.Equals(product) &&
            //             walk_prd_FullItemLinesList.RegistrationDate > DateTime.Parse(fromdate.ToString()) &&
            //             walk_prd_FullItemLinesList.RegistrationDate < DateTime.Parse(todate.ToString()) &&
            //            // walk_prd_FullItemLinesList.CompWRHCode == "1" &&
            //             (walk_prd_FullItemLinesList.DocTypeCode.Equals("ΔΝΓΠ") || walk_prd_FullItemLinesList.DocTypeCode.Equals("ΔΑΠ") || walk_prd_FullItemLinesList.DocTypeCode.Equals("ΤΔΑ") || walk_prd_FullItemLinesList.DocTypeCode.Equals("ΑΠΛ") || walk_prd_FullItemLinesList.DocTypeCode.Equals("ΑΠΛ-ES") || walk_prd_FullItemLinesList.DocTypeCode.Equals("ΔΕΠεπ"))
            //             select walk_prd_FullItemLinesList;



            var test2 = from ItemBalances in context.ItemBalances
                        //join wpfll in test
                       // on new { ItemBalances.ColorCode, ItemBalances.SizeCode } equals new { wpfll.ColorCode, wpfll.SizeCode } into g
                        orderby ItemBalances.ColorCode
                        where ItemBalances.ItemCode.Equals(product)
                        select new
                        {
                            ItemBalances.ItemCode,
                            Χρώμα = ItemBalances.ColorCode + "." + ItemBalances.ColorGRdesc,
                            ItemBalances.SizeDesc,
                            Αποθήκη = ItemBalances.Warehouse,
                            Παραγγελίες = ItemBalances.OrderQty,
                           // Πωλήσεις = g.Sum(d => (Nullable<int>)d.Quantity),
                        };

            //var unfinished = from eisagogiParagogis in context.eisagogiParagogis
            //                 join c in boarding on eisagogiParagogis.Col_Id equals c.Col_ID into g
            //                 from f in g.DefaultIfEmpty()
            //                 where f.ItemCode.Equals(product)
            //                 select new
            //                 {
            //                     f.ItemCode,
            //                     f.ColorCode,
            //                     f.SizeDesc,
            //                     f.D_qty,
            //                     f.S_qty
            //                 };


            var unfinished = from dfs1 in context.DELTIO_FINISH_SUPER1
                             join f in ep on dfs1.COL_ID equals f.Col_Id into g
                             from f in g.DefaultIfEmpty()
                             where dfs1.FINISHAP_ID.Equals(product)
                            // group s by s.Col_Id into k
                             select new
                             {
                                 st=dfs1,
                                 f.Col_Id
                                 
                             };

            var unfinished2 = context.DELTIO_FINISH_SUPER1.GroupJoin(ep, dfs1 => dfs1.COL_ID, EP => EP.Col_Id, (x, y) => new { DFS1 = x, ep = y }).SelectMany(x => x.ep.DefaultIfEmpty(), (x, y) => new { Col_Id = x.DFS1.COL_ID, AccessNo = y.barcode, Finishap_Id = x.DFS1.FINISHAP_ID, Colour = x.DFS1.COLORID, Size = x.DFS1.SIZE, produced = (y.dozen*12 +(y.socks/2)) });

            //  var unfinished3 = unfinished2.GroupBy(c => c.Col_Id).Select(f => new { });

            var unfinished3 = from c in unfinished2
                              group c by c.Col_Id into g
                              where g.Select(c => c.Finishap_Id).FirstOrDefault().Equals(product)
                              select new { Col_Id = g.Key, Finishap_Id = g.Select( x => x.Finishap_Id).FirstOrDefault(), produced = g.Sum(x => x.produced) };

            if (test2.Any(c => c.ItemCode.Equals(product)))
            {

                var sizes = ib.Where(f => f.ItemCode.Equals(product)).GroupBy(c => new { c.SizeDesc, c.Αποθήκη }).Select(d => new { d.Key.SizeDesc });

                // var colors = ib.Where(f => f.ItemCode.Equals(product)).GroupBy(c => new { c.ColorCode, c.ColorGRdesc }).Select(d => new { d.Key.ColorCode, d.Key.ColorGRdesc });


                balance.IsReadOnly = true;
                balance.ItemsSource = test2;//.Where(c => c.Finishap_Id.Equals(product));//.Where(c => c.ItemCode.Equals(product));
                //balance.ItemsSource = test.Where(c => c.ItemCode.StartsWith("W333"));
            }
            //else
            //{
            //    MessageBox.Show("Λάθος όνομα προιόντος");
            //}
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox s = (TextBox)FindName("productName");
            if (e.Key == Key.Escape)
            {
               
                if (s.IsFocused != true)
                {
                    this.Close();
                }
            }

            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                s.Focus();
            }
        }

        private void bal_Closed(object sender, EventArgs e)
        {
            Static_Variables.finishid = "";
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {

            using(var context = new Production18())
            {



                var deltiaapost = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
                                  where walk_prd_FullItemLinesList.ItemCode.Equals(productName.Text) &&
                                  walk_prd_FullItemLinesList.RegistrationDate > DateTime.Parse(fromdate.ToString()).AddDays(-1) &&
                                  walk_prd_FullItemLinesList.RegistrationDate < DateTime.Parse(todate.ToString()).AddDays(1) &&
                                  walk_prd_FullItemLinesList.CompWRHCode.Equals("1")// &&
                                  // (walk_prd_FullItemLinesList.EntryTypeDescription.Equals("Stock - Sales (QTY)") ||
                                    // walk_prd_FullItemLinesList.EntryTypeDescription.Equals("Stock - Sales (VALUE/QTY)") ||
                                    //walk_prd_FullItemLinesList.EntryTypeDescription.Equals("Stock - Dispense (VALUE/QTY)"))
                                  select walk_prd_FullItemLinesList;

                //var deltiaepistr = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
                //             where walk_prd_FullItemLinesList.ItemCode.Equals(productName.Text) &&
                //             walk_prd_FullItemLinesList.RegistrationDate > DateTime.Parse(fromdate.ToString()).AddDays(-1) &&
                //             walk_prd_FullItemLinesList.RegistrationDate < DateTime.Parse(todate.ToString()).AddDays(1) &&
                //                  walk_prd_FullItemLinesList.CompWRHCode.Equals("1") &&
                //               (walk_prd_FullItemLinesList.EntryTypeDescription.Equals("Stock - Dispense negative  (VALUE/QTY)") || 
                //               walk_prd_FullItemLinesList.EntryTypeDescription.Equals("Stock - Sales return (QTY)") || 
                //               walk_prd_FullItemLinesList.EntryTypeDescription.Equals("Stock - Sales return (VALUE/QTY)"))
                //            select walk_prd_FullItemLinesList;

                var test2 = from ItemBalances in context.ItemBalances
                            join wpfll in deltiaapost
                            on new { ItemBalances.ColorCode, ItemBalances.SizeCode } equals new { wpfll.ColorCode, wpfll.SizeCode } into g
                          //  join wpfll2 in deltiaepistr
                          //  on new { ItemBalances.ColorCode, ItemBalances.SizeCode } equals new { wpfll2.ColorCode, wpfll2.SizeCode } into f
                            orderby ItemBalances.ColorCode
                            where ItemBalances.ItemCode.Equals(productName.Text)
                            select new
                            {
                                ItemBalances.ItemCode,
                                Χρώμα = ItemBalances.ColorCode + "." + ItemBalances.ColorGRdesc,
                                ItemBalances.SizeDesc,
                                Αποθήκη = ItemBalances.Warehouse,
                                Παραγγελίες = ItemBalances.OrderQty,
                                Πωλήσεις = g.Sum(d => (Nullable<int>)d.Quantity)
                                //Πωλήσεις = (g.Sum(d => (Nullable<int>)d.Quantity) - f.Sum(d => d.Quantity)) == null ? g.Sum(d => (Nullable<int>)d.Quantity) : g.Sum(d => (Nullable<int>)d.Quantity) - f.Sum(d => (Nullable<int>)d.Quantity),
                               // Επιστροφές = f.Sum(d => (Nullable<int>)d.Quantity)
                              
                            };



                //var deltiaapost2 = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
                //                   where walk_prd_FullItemLinesList.ItemCode.Equals(productName.Text) &&
                //                   walk_prd_FullItemLinesList.RegistrationDate > DateTime.Parse(fromdate.ToString()).AddDays(-1) &&
                //                   walk_prd_FullItemLinesList.RegistrationDate < DateTime.Parse(todate.ToString()).AddDays(1) &&
                //                   walk_prd_FullItemLinesList.CompWRHCode == "1" &&
                //                    (walk_prd_FullItemLinesList.EntryTypeDescription == "Stock - Sales (QTY)" ||
                //                      walk_prd_FullItemLinesList.EntryTypeDescription == "Stock - Sales (VALUE/QTY)" ||
                //                     walk_prd_FullItemLinesList.EntryTypeDescription == "Stock - Dispense (VALUE/QTY)")
                //                   group walk_prd_FullItemLinesList by new
                //                   {
                //                       walk_prd_FullItemLinesList.ColorCode,
                //                       walk_prd_FullItemLinesList.SizeCode

                //                   } into g
                //                   select new
                //                   {
                //                        g.Key.ColorCode,
                //                        g.Key.SizeCode,
                //                       qty = g.Sum(c => c.Quantity)
                //                   };

                //var final = from ItemBalances in context.ItemBalances
                //            join wpfll in deltiaapost2
                //            on new { ItemBalances.ColorCode, ItemBalances.SizeCode } equals new { wpfll.ColorCode, wpfll.SizeCode } into g
                //            orderby ItemBalances.ColorCode
                //            where ItemBalances.ItemCode.Equals(productName.Text)
                //            select new
                //            {
                //                ItemBalances.ItemCode,
                //                Χρώμα = ItemBalances.ColorCode + "." + ItemBalances.ColorGRdesc,
                //                ItemBalances.SizeDesc,
                //                Αποθήκη = ItemBalances.Warehouse,
                //                Παραγγελίες = ItemBalances.OrderQty,
                //                Πωλήσεις2 = g.Sum(d => (Nullable<int>)d.qty),
                                
                //                //Πωλήσεις = (g.Sum(d => (Nullable<int>)d.Quantity) - f.Sum(d => d.Quantity)) == null ? g.Sum(d => (Nullable<int>)d.Quantity) : g.Sum(d => (Nullable<int>)d.Quantity) - f.Sum(d => (Nullable<int>)d.Quantity),
                //                //Επιστροφές = f.Sum(d => (Nullable<int>)d.Quantity)

                //            };



                if (test2.Any(c => c.ItemCode.Equals(productName.Text)))
                {

                    //var sizes = ib.Where(f => f.ItemCode.Equals(product)).GroupBy(c => new { c.SizeDesc, c.Αποθήκη }).Select(d => new { d.Key.SizeDesc });

                    // var colors = ib.Where(f => f.ItemCode.Equals(product)).GroupBy(c => new { c.ColorCode, c.ColorGRdesc }).Select(d => new { d.Key.ColorCode, d.Key.ColorGRdesc });


                    balance.IsReadOnly = true;
                    balance.ItemsSource = test2;//.Where(c => c.ItemCode.Equals(product));
                    totalsales.Text = test2.Sum(c => (Nullable<int>)c.Πωλήσεις).ToString();
                                                //balance.ItemsSource = test.Where(c => c.ItemCode.StartsWith("W333"));
                }
            }

        }
    }
}
