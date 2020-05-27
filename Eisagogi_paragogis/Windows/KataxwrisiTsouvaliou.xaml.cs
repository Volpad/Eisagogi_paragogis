using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for KataxwrisiTsouvaliou.xaml
    /// </summary>
    public partial class KataxwrisiTsouvaliou : Window
    {
        public KataxwrisiTsouvaliou()
        {
            InitializeComponent();
            //product.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.FINISHAP_ID).FirstOrDefault();
            //date.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.PROD_DATE).FirstOrDefault().ToString();
            //orderref.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.OrderNo).FirstOrDefault();
            //deliverydate.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.DeliveryDate).FirstOrDefault();

            List<int> list = new List<int>();
            for (var i=0; i<25; i++)
            {
                list.Add(i);
            }
            dosens.ItemsSource = list;
            socks.ItemsSource = list;

            List<string> users = new List<string>();
            users.Add("Khan");
            users.Add("Awais");
            users.Add("Afie");
            users.Add("Μαρία");
            users.Add("Agha");
            users.Add("Imran");
            users.Add("Νάντια");
            users.Add("Τάσος");
            users.Add("Ανδρέας");

            user.ItemsSource = users;

            using (var context = new Production18())
            {
                this.Title = context.DELTIO_FINISH_SUPER.Where(i => i.TOTAL_ID == Static_Variables.kattotalid).Select(x => x.FINISHAP_ID).FirstOrDefault() + " - " + Static_Variables.kattotalid.ToString();
                prodname.Text = context.DELTIO_FINISH_SUPER.Where(i => i.TOTAL_ID == Static_Variables.kattotalid).Select(x => x.FINISHAP_ID).FirstOrDefault();
                prodtotid.Text = Static_Variables.kattotalid.ToString();
                machine.Text = context.EXOPLISMOS1.Where(c => c.DESCR.StartsWith(Static_Variables.katmachineno)).Select(d => d.DESCR).FirstOrDefault();// + " " + context.EXOPLISMOS1.Where(c => c.DESCR.StartsWith(Static_Variables.katmachineno)).Select(d => d.MHXANH).FirstOrDefault();

                // var of = context.Orders_Fin;
                // var findOtherName = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Static_Variables.kattotalid).Select(d => d.FINISHAP_ID).FirstOrDefault().ToString();

                //  var findOtherAccessNo = context.DELTIO_FINISH_SUPER.Where(c => c.FINISHAP_ID.Equals(findOtherName)).Select(d => new { d.TOTAL_ID, name = (d.TOTAL_ID + " - Order: " + (d.OrderNo == null ? " " : d.OrderNo) + (of.Any(f => f.TOTAL_ID == d.TOTAL_ID) == false ? "" : " Completed")) }).ToList();


                //    var orderfinishdate = context.Orders_Fin.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.DATE_ENTRY).FirstOrDefault();
                //    if (orderfinishdate < DateTime.Now.AddMonths(-1000))
                //    {
                //        OrderFinish.Text = "";
                //        ycanvas.Background = new SolidColorBrush(Colors.LightYellow);
                //        product.Foreground = new SolidColorBrush(Colors.Black);
                //        date.Foreground = new SolidColorBrush(Colors.Black);
                //        date.Foreground = new SolidColorBrush(Colors.Black);
                //        orderref.Foreground = new SolidColorBrush(Colors.Black);
                //        deliverydate.Foreground = new SolidColorBrush(Colors.Black);
                //        OrderFinish.Foreground = new SolidColorBrush(Colors.Black);
                //    }
                //    else
                //    {
                //        OrderFinish.Text = orderfinishdate.ToString();
                //        ycanvas.Background = new SolidColorBrush(Colors.Green);
                //        product.Foreground = new SolidColorBrush(Colors.White);
                //        date.Foreground = new SolidColorBrush(Colors.White);
                //        date.Foreground = new SolidColorBrush(Colors.White);
                //        orderref.Foreground = new SolidColorBrush(Colors.White);
                //        deliverydate.Foreground = new SolidColorBrush(Colors.White);
                //        OrderFinish.Foreground = new SolidColorBrush(Colors.White);
                //    }


                //    accessno.ItemsSource = findOtherAccessNo;
                //    accessno.DisplayMemberPath = "name";
                //    accessno.SelectedValuePath = "TOTAL_ID";
                //    accessno.SelectedValue = Static_Variables.prodviewtotalid;
                //}

                var query = from c in context.DELTIO_FINISH_SUPER1
                            join ct in context.eisagogiParagogis
                            on c.COL_ID equals ct.Col_Id into g
                            from ct in g.DefaultIfEmpty()
                            where c.TOTAL_ID == Static_Variables.kattotalid
                            select new
                            {
                                c.COL_ID,
                                c.COLORID,
                                c.COLOR,
                                c.SIZE,
                                c.Production,
                                pdozen = ct == null ? 0 : (ct.dozen == null ? 0 : ct.dozen),
                                psock = ct == null ? 0 : (ct.socks == null ? 0 : ct.socks),
                            };

                var query1 = query.GroupBy(s => new { s.COL_ID, s.COLORID, s.COLOR, s.Production, s.SIZE }).Select(g => new
                {
                    Cno = g.Key.COL_ID,
                    Cid = g.Key.COLORID,
                    Cnam = g.Key.COLOR,
                    size = g.Key.SIZE,
                    productiondosens = g.Key.Production / 24,
                    productionsocks = (g.Key.Production) - ((g.Key.Production / 24) * 24),
                    producedDosen = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24,
                    producedSocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => (x.pdozen * 24) + x.psock) - (g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24) * 24,
                    restdosens = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24,
                    restsocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) - (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24) * 24
                });

                productiondata.IsReadOnly = true;
                productiondata.HeadersVisibility = DataGridHeadersVisibility.None;
                productiondata.ItemsSource = query1;

                //eisagogiparagogisgrid.IsReadOnly = true;
               // eisagogiparagogisgrid.HeadersVisibility = DataGridHeadersVisibility.None;



                var query2 = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                             where DELTIO_FINISH_SUPER.TOTAL_ID == Static_Variables.kattotalid
                             select DELTIO_FINISH_SUPER;


                foreach (DELTIO_FINISH_SUPER Query2 in query2)
                {
                    var fid = Query2.FINISHAP_ID;

                    var images = from IMAGES in context.IMAGES
                                 where IMAGES.GUIDE.Equals(fid)
                                 select IMAGES;


                    foreach (IMAGES IMAGE in images)
                    {
                        if (IMAGE.PICTURE != null)
                        {
                            BitmapImage bmp = new BitmapImage();
                            MemoryStream strmImg = new MemoryStream(IMAGE.PICTURE);
                            bmp.BeginInit();
                            bmp.StreamSource = strmImg;
                            bmp.EndInit();
                            image.Source = bmp;
                        }
                    }
                }

            }

            xroma.Visibility = Visibility.Collapsed;
            megethos.Visibility = Visibility.Collapsed;
            dosenstitle.Visibility = Visibility.Collapsed;
            dosens.Visibility = Visibility.Collapsed;
            sockstitle.Visibility = Visibility.Collapsed;
            socks.Visibility = Visibility.Collapsed;
            usertitle.Visibility = Visibility.Collapsed;
            user.Visibility = Visibility.Collapsed;
            save.Visibility = Visibility.Collapsed;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {

            var grid = sender as DataGrid;

            //var test = grid.SelectedValue.ToString();

            //int index = test.LastIndexOf("Cid =");
            //if (index > 0)
            //    test = test.Substring(index, index);
            using (var context = new Production18())
            {


                try
                {

                    int? colid = Convert.ToInt32(grid.SelectedValue.ToString().Substring(8, 6));

                    xroma.Visibility = Visibility.Visible;
                    megethos.Visibility = Visibility.Visible;
                    dosenstitle.Visibility = Visibility.Visible;
                    dosens.Visibility = Visibility.Visible;
                    sockstitle.Visibility = Visibility.Visible;
                    socks.Visibility = Visibility.Visible;
                    usertitle.Visibility = Visibility.Visible;
                    user.Visibility = Visibility.Visible;
                    save.Visibility = Visibility.Visible;
                    xroma.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID.Equals(colid)).Select(d => d.COLORID).FirstOrDefault() + "." + context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID.Equals(colid)).Select(d => d.COLOR).FirstOrDefault();
                    megethos.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID.Equals(colid)).Select(d => d.SIZE).FirstOrDefault();

                    var qeisagogi1 = from c in context.eisagogiParagogis
                                     join ct in context.Semi_finished_warehouse
                                     on c.barcode equals ct.AccessNo into g
                                     from ct in g.DefaultIfEmpty()
                                     join ctc in context.shipment
                                     on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
                                     from ctc in f.DefaultIfEmpty()
                                     where c.Total_Id == Static_Variables.kattotalid && c.Col_Id == colid
                                     select new
                                     {
                                         c.AutoNo,
                                         c.date,
                                         c.barcode,
                                         c.Machine,
                                         c.dozen,
                                         c.socks,
                                         c.user,
                                         shipmentno = ct == null ? null : ct.shipmentNo,
                                         shipmentdate = ctc == null ? null : ctc.ShipmentDate
                                     };


                }
                catch
                {

                }
            }
            this.Focus();
        }

        private void productionDataOnLoad(object sender, RoutedEventArgs e)
        {
            productiondata.Columns[0].Width = 67;
            productiondata.Columns[1].Width = 63;
            productiondata.Columns[2].Width = 150;
            productiondata.Columns[3].Width = 70;
            productiondata.Columns[4].Width = 35;
            productiondata.Columns[5].Width = 25;
            productiondata.Columns[6].Width = 35;
            productiondata.Columns[7].Width = 25;
            productiondata.Columns[8].Width = 35;
            productiondata.Columns[9].Width = 25;

        }

        //private void eisagogiparagogisgrid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    eisagogiparagogisgrid.Columns[0].Width = 60;
        //    eisagogiparagogisgrid.Columns[1].Width = 120;
        //    eisagogiparagogisgrid.Columns[2].Width = 78;
        //    eisagogiparagogisgrid.Columns[3].Width = 70;
        //    eisagogiparagogisgrid.Columns[4].Width = 35;
        //    eisagogiparagogisgrid.Columns[5].Width = 25;
        //    eisagogiparagogisgrid.Columns[6].Width = 70;
        //    eisagogiparagogisgrid.Columns[7].Width = 70;
        //    eisagogiparagogisgrid.Columns[8].Width = 120;

        //}



        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}

