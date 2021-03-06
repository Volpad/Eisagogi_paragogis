﻿using System;
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
    /// Interaction logic for productionView.xaml
    /// </summary>
    public partial class productionView : Window
    {

        private static Production18 production18 = new Production18();

        private static IEnumerable<DELTIO_FINISH_SUPER> Deltio_Finish_Super = from DELTIO_FINISH_SUPER in production18.DELTIO_FINISH_SUPER
                                                                              orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                                                                              select DELTIO_FINISH_SUPER;

        List<DELTIO_FINISH_SUPER> deltio_finish_super = Deltio_Finish_Super.ToList();


        private static IEnumerable<DELTIO_FINISH_SUPER1> Deltio_Finish_Super1 = from DELTIO_FINISH_SUPER1 in production18.DELTIO_FINISH_SUPER1
                                                                                orderby DELTIO_FINISH_SUPER1.TOTAL_ID descending
                                                                                select DELTIO_FINISH_SUPER1;

        List<DELTIO_FINISH_SUPER1> deltio_finish_super1 = Deltio_Finish_Super1.ToList();

        private static IEnumerable<eisagogiParagogis> Eisagogioaragogis = from eisagogiParagogis in production18.eisagogiParagogis
                                                                          orderby eisagogiParagogis.Total_Id descending
                                                                          where  eisagogiParagogis.barcode != null// && eisagogiParagogis.date > DateTime.Now.AddMonths(-10) 
                                                                          select eisagogiParagogis;

        List<eisagogiParagogis> eisagogiparagogis = Eisagogioaragogis.ToList();

        private static IEnumerable<Semi_finished_warehouse> Semifinished = from Semi_finished_warehouse in production18.Semi_finished_warehouse
                                                                                orderby Semi_finished_warehouse.shipmentNo descending
                                                                                select Semi_finished_warehouse;

        List<Semi_finished_warehouse> semifinished = Semifinished.ToList();

        private static IEnumerable<shipment> Shipment= from shipment in production18.shipment
                                                       orderby shipment.shipmentNo descending
                                                       select shipment;

        List<shipment> shipments = Shipment.ToList();



        public productionView()
        {
            InitializeComponent();

            if (!Static_Variables.adminRights)
            {
                ekkremotites.Visibility = Visibility.Collapsed;
            }

            product.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.FINISHAP_ID).FirstOrDefault();
            date.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.PROD_DATE).FirstOrDefault().ToString();
            orderref.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.OrderNo).FirstOrDefault();
            deliverydate.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.DeliveryDate).FirstOrDefault();

            this.Title = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.FINISHAP_ID).FirstOrDefault() + " - " + Static_Variables.prodviewtotalid.ToString();

            using (var context = new Production18())
            {
                var of = context.Orders_Fin;
                var findOtherName = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.FINISHAP_ID).FirstOrDefault().ToString();

                var findOtherAccessNo = context.DELTIO_FINISH_SUPER.Where(c => c.FINISHAP_ID.Equals(findOtherName)).Select(d =>  new { d.TOTAL_ID, name = (d.TOTAL_ID + " - Order: " + (d.OrderNo == null ? " " : d.OrderNo) + (of.Any(f => f.TOTAL_ID == d.TOTAL_ID) == false ? "" : " Completed"))}).ToList();

                var Memo = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.ΜΕΜΟ).FirstOrDefault();

                var orderfinishdate = context.Orders_Fin.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.DATE_ENTRY).FirstOrDefault();
                var orderfinishnumber = context.Orders_Fin.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.id).FirstOrDefault();
                if (orderfinishdate < DateTime.Now.AddMonths(-1000))
                {
                    OrderFinish.Text = "";
                    ycanvas.Background = new SolidColorBrush(Colors.LightYellow);
                    product.Foreground = new SolidColorBrush(Colors.Black);
                    date.Foreground = new SolidColorBrush(Colors.Black);
                    date.Foreground = new SolidColorBrush(Colors.Black);
                    orderref.Foreground = new SolidColorBrush(Colors.Black);
                    deliverydate.Foreground = new SolidColorBrush(Colors.Black);
                    OrderFinish.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    OrderFinish.Text = orderfinishnumber.ToString() + " - " + orderfinishdate.ToString("dd/MM/yy");
                    ycanvas.Background = new SolidColorBrush(Colors.Green);
                    product.Foreground = new SolidColorBrush(Colors.White);
                    date.Foreground = new SolidColorBrush(Colors.White);
                    date.Foreground = new SolidColorBrush(Colors.White);
                    orderref.Foreground = new SolidColorBrush(Colors.White);
                    deliverydate.Foreground = new SolidColorBrush(Colors.White);
                    OrderFinish.Foreground = new SolidColorBrush(Colors.White);
                }


                accessno.ItemsSource = findOtherAccessNo;
                accessno.DisplayMemberPath = "name";
                accessno.SelectedValuePath = "TOTAL_ID";
                accessno.SelectedValue = Static_Variables.prodviewtotalid;
                memo.Text = Memo;
            }

            //var query = from c in Deltio_Finish_Super1
            //            join ct in eisagogiparagogis
            //            on c.COL_ID equals ct.Col_Id into g
            //            from ct in g.DefaultIfEmpty()
            //            where c.TOTAL_ID == Static_Variables.prodviewtotalid
            //            select new
            //            {
            //                c.COL_ID,
            //                c.COLORID,
            //                c.COLOR,
            //                c.SIZE,
            //                c.Production,
            //                pdozen = ct == null ? 0 : (ct.dozen == null? 0 : ct.dozen),
            //                psock = ct == null ? 0 : (ct.socks == null ? 0 : ct.socks),
            //            };


            var query = from c in production18.DELTIO_FINISH_SUPER1
                        join ct in production18.eisagogiParagogis
                        on c.COL_ID equals ct.Col_Id into g
                        from ct in g.DefaultIfEmpty()
                        where c.TOTAL_ID == Static_Variables.prodviewtotalid
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

            var qeisagogi1 = from c in production18.eisagogiParagogis
                             join ct in production18.Semi_finished_warehouse
                             on c.barcode equals ct.AccessNo into g
                             from ct in g.DefaultIfEmpty()
                             join ctc in production18.shipment
                             on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
                             from ctc in f.DefaultIfEmpty()
                             join bo in production18.Boading
                             on c.barcode equals bo.AccessNo into z
                             from bo in z.DefaultIfEmpty()
                                 //where c.Col_Id == colid
                             select new
                             {
                                 c.AutoNo,
                                 c.date,
                                 c.barcode,
                                 c.Machine,
                                 c.dozen,
                                 c.socks,
                                 c.user,
                                 c.Col_Id,
                                 shipmentno = ct == null ? null : ct.shipmentNo,
                                 shipmentdate = ctc == null ? null : ctc.ShipmentDate,
                                 Finished = bo == null ? null : bo.Warehouse,
                                 findate = bo == null ? null : bo.BoardingDate,
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
                restsocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) - (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24) * 24,
                GR = qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.socks) / 24),
                BG = qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24),

                //Rest = (g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24) == null ? (g.Key.Production / 24 == 0 ? null : g.Key.Production / 24) :
                //       (((qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.socks) / 24)) == null ? 0 : (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.socks) / 24))) +
                //       ((qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24)) == null ? 0 : (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24))) +
                //       (g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? 0 : ((((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24)) <= 0 ? 0 : (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24)))

            });

            var query3 = query1.Select(s => new
            {
                s.Cno,
                s.Cid,
                s.Cnam,
                s.size,
                s.productiondosens,
                s.productionsocks,
                s.producedDosen,
                s.producedSocks,
                s.restdosens,
                s.restsocks,
                s.GR,
                s.BG,
                Rest = (s.productiondosens + s.productionsocks == 0) ? null :
                s.restdosens + s.restsocks == null ? s.productiondosens :
                (s.BG == null ? 0 : s.BG) + (s.GR == null ? 0 : s.GR) + s.restdosens

            });

            productiondata.IsReadOnly = true;
            productiondata.HeadersVisibility = DataGridHeadersVisibility.None;
            productiondata.ItemsSource = query3; //.OrderBy(c => c.Cno).ThenBy(d => d.Cid).ThenBy(f => f.size) ;
            TotalGr.Text = query1.Sum(c => c.GR).ToString();
            TotalBG.Text = query1.Sum(c => c.BG).ToString();

            eisagogiparagogisgrid.IsReadOnly = true;
            eisagogiparagogisgrid.HeadersVisibility = DataGridHeadersVisibility.None;

            using (var context = new Production18()) {

                var query2 = from DELTIO_FINISH_SUPER in production18.DELTIO_FINISH_SUPER
                            where DELTIO_FINISH_SUPER.TOTAL_ID == Static_Variables.prodviewtotalid
                            select DELTIO_FINISH_SUPER;


                foreach (DELTIO_FINISH_SUPER Query2 in query2)
                {
                    var fid = Query2.FINISHAP_ID;

                    var images = from IMAGES in production18.IMAGES
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

        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var context = new Production18())
            {


                var grid = sender as DataGrid;
                try
                {

                    int? colid = Convert.ToInt32(grid.SelectedValue.ToString().Substring(8, 6));

                    var qeisagogi1 = from c in context.eisagogiParagogis
                                     join ct in context.Semi_finished_warehouse
                                     on c.barcode equals ct.AccessNo into g
                                     from ct in g.DefaultIfEmpty()
                                     join ctc in context.shipment
                                     on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
                                     from ctc in f.DefaultIfEmpty()
                                     join bo in context.Boading
                                     on c.barcode equals bo.AccessNo into z
                                     from bo in z.DefaultIfEmpty()
                                     where c.Col_Id == colid
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
                                         shipmentdate = ctc == null ? null : ctc.ShipmentDate,
                                         Finished = bo == null ? null : bo.Warehouse,
                                         findate = bo == null ? null : bo.BoardingDate,
                                     };

                    eisagogiparagogisgrid.ItemsSource = qeisagogi1;
                }
                catch
                {

                }
            }

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
            productiondata.Columns[10].Width = 30;
            productiondata.Columns[11].Width = 30;
            productiondata.Columns[12].Width = 30;

        }

        private void eisagogiparagogisgrid_Loaded(object sender, RoutedEventArgs e)
        {
            eisagogiparagogisgrid.Columns[0].Width = 60;
            eisagogiparagogisgrid.Columns[1].Width = 120;
            eisagogiparagogisgrid.Columns[2].Width = 78;
            eisagogiparagogisgrid.Columns[3].Width = 70;
            eisagogiparagogisgrid.Columns[4].Width = 35;
            eisagogiparagogisgrid.Columns[5].Width = 25;
            eisagogiparagogisgrid.Columns[6].Width = 70;
            eisagogiparagogisgrid.Columns[7].Width = 35;
            eisagogiparagogisgrid.Columns[8].Width = 80;
            eisagogiparagogisgrid.Columns[9].Width = 50;
            eisagogiparagogisgrid.Columns[10].Width = 80 ;


        }

        private void accessno_DropDownClosed(object sender, EventArgs e)
        {
            using (var context = new Production18())
            {
                var query = from c in context.DELTIO_FINISH_SUPER1
                            join ct in context.eisagogiParagogis
                            on c.COL_ID equals ct.Col_Id into g
                            from ct in g.DefaultIfEmpty()
                            where c.TOTAL_ID.Equals(accessno.SelectedValue)
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



                var qeisagogi1 = from c in context.eisagogiParagogis
                                 join ct in context.Semi_finished_warehouse
                                 on c.barcode equals ct.AccessNo into g
                                 from ct in g.DefaultIfEmpty()
                                 join ctc in context.shipment
                                 on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
                                 from ctc in f.DefaultIfEmpty()
                                 join bo in context.Boading
                                 on c.barcode equals bo.AccessNo into z
                                 from bo in z.DefaultIfEmpty()
                                 //where c.Col_Id == colid
                                 select new
                                 {
                                     c.AutoNo,
                                     c.date,
                                     c.barcode,
                                     c.Machine,
                                     c.dozen,
                                     c.socks,
                                     c.user,
                                     c.Col_Id,
                                     shipmentno = ct == null ? null : ct.shipmentNo,
                                     shipmentdate = ctc == null ? null : ctc.ShipmentDate,
                                     Finished = bo == null ? null : bo.Warehouse,
                                     findate = bo == null ? null : bo.BoardingDate,
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
                    restsocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) - (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24) * 24,
                    GR = qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.socks) / 24),
                    BG = qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24),
                     //Rest = (g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24) == null ? (g.Key.Production / 24 == 0 ? null : g.Key.Production / 24) :
                     //  (((qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.socks) / 24)) == null ? 0 : (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null && c.findate == null).Sum(d => d.socks) / 24))) +
                     //  ((qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24)) == null ? 0 : (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi1.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24))) +
                     //  (g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? 0 : ((((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24)) <= 0 ? 0 : (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24)))


                });

                var query3 = query1.Select(s => new
                {
                    s.Cno,
                    s.Cid,
                    s.Cnam,
                    s.size,
                    s.productiondosens,
                    s.productionsocks,
                    s.producedDosen,
                    s.producedSocks,
                    s.restdosens,
                    s.restsocks,
                    s.GR,
                    s.BG,
                    Rest = (s.productiondosens + s.productionsocks == 0) ? null :
                            s.restdosens + s.restsocks == null ? s.productiondosens :
                            (s.BG == null ? 0 : s.BG) + (s.GR == null ? 0 : s.GR) + s.restdosens

                });



                productiondata.ItemsSource = query3;
                TotalGr.Text = query1.Sum(c => c.GR).ToString();
                TotalBG.Text = query1.Sum(c => c.BG).ToString();


                Static_Variables.prodviewtotalid = Convert.ToInt32(accessno.SelectedValue);
                date.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.PROD_DATE).FirstOrDefault().ToString();
                orderref.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.OrderNo).FirstOrDefault();
                deliverydate.Text = Deltio_Finish_Super.Where(i => i.TOTAL_ID == Static_Variables.prodviewtotalid).Select(x => x.DeliveryDate).FirstOrDefault();
              //  var Memo = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.ΜΕΜΟ).FirstOrDefault();
                memo.Text = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.ΜΕΜΟ).FirstOrDefault();

                var orderfinishdate = context.Orders_Fin.Where(c => c.TOTAL_ID == Static_Variables.prodviewtotalid).Select(d => d.DATE_ENTRY).FirstOrDefault();
                if (orderfinishdate < DateTime.Now.AddMonths(-1000))
                {
                    OrderFinish.Text = "";
                    ycanvas.Background = new SolidColorBrush(Colors.LightYellow);
                    product.Foreground = new SolidColorBrush(Colors.Black);
                    date.Foreground = new SolidColorBrush(Colors.Black);
                    date.Foreground = new SolidColorBrush(Colors.Black);
                    orderref.Foreground = new SolidColorBrush(Colors.Black);
                    deliverydate.Foreground = new SolidColorBrush(Colors.Black);
                    OrderFinish.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    OrderFinish.Text = orderfinishdate.ToString();
                    ycanvas.Background = new SolidColorBrush(Colors.Green);
                    product.Foreground = new SolidColorBrush(Colors.White);
                    date.Foreground = new SolidColorBrush(Colors.White);
                    date.Foreground = new SolidColorBrush(Colors.White);
                    orderref.Foreground = new SolidColorBrush(Colors.White);
                    deliverydate.Foreground = new SolidColorBrush(Colors.White);
                    OrderFinish.Foreground = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }

        }

        private void whcheck_Click(object sender, RoutedEventArgs e)
        {
            Static_Variables.finishid = product.Text;

            Window balance = new Balance();
            balance.Show();

        }

        private void Specs_Click(object sender, RoutedEventArgs e)
        {
            Static_Variables.finishid = product.Text;
            Window ps = new Product_Specification();
            ps.Show();

        }

        private void Ekremotites_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                //int? colid = Convert.ToInt32(grid.SelectedValue.ToString().Substring(8, 6));

                var fid = Convert.ToInt32(accessno.SelectedValue.ToString());
                var prodname = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == fid).Select(d => d.FINISHAP_ID).FirstOrDefault();
                var listtotid = context.DELTIO_FINISH_SUPER.Where(c => c.FINISHAP_ID == prodname && c.PROD_DATE < DateTime.Now.AddYears(-1)).Select(d => d.TOTAL_ID);


                var qeisagogi1 = from c in context.eisagogiParagogis
                                 join ct in context.Semi_finished_warehouse
                                 on c.barcode equals ct.AccessNo into g
                                 from ct in g.DefaultIfEmpty()
                                 join ctc in context.shipment
                                 on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
                                 from ctc in f.DefaultIfEmpty()
                                 join bo in context.Boading
                                 on c.barcode equals bo.AccessNo into z
                                 from bo in z.DefaultIfEmpty()
                                 select new
                                 {
                                     c.Total_Id,
                                     c.AutoNo,
                                     c.date,
                                     c.barcode,
                                     c.Machine,
                                     c.dozen,
                                     c.socks,
                                     c.user,
                                     shipmentno = ct == null ? null : ct.shipmentNo,
                                     shipmentdate = ctc == null ? null : ctc.ShipmentDate,
                                     Finished = bo == null ? null : bo.Warehouse,
                                     findate = bo == null ? null : bo.BoardingDate,
                                 };


                var q3 = from f in qeisagogi1
                         join lst in context.DELTIO_FINISH_SUPER
                         on f.Total_Id equals lst.TOTAL_ID into g
                         from lst in g.DefaultIfEmpty()
                         where lst.FINISHAP_ID == prodname && f.shipmentdate.HasValue && DateTime.Compare(f.shipmentdate.HasValue ? f.shipmentdate.Value : DateTime.Now, DateTime.Now.AddYears(-1)) > 0 && !f.findate.HasValue 
                         select new
                         {
                            // lst.FINISHAP_ID,
                             f.Total_Id,
                             f.AutoNo,
                             f.date,
                             f.barcode,
                             f.Machine,
                             f.dozen,
                             f.socks,
                             f.shipmentno,
                             f.shipmentdate,
                             f.Finished,
                             f.findate
                         };

                eisagogiparagogisgrid.ItemsSource = q3;









                var query = from c in context.DELTIO_FINISH_SUPER1
                            join ct in context.eisagogiParagogis
                            on c.COL_ID equals ct.Col_Id into g
                            from ct in g.DefaultIfEmpty()
                            join f in context.Semi_finished_warehouse
                            on ct.barcode equals f.AccessNo into z
                            from f in z.DefaultIfEmpty()
                            join ctc in context.shipment
                            on f == null ? null : f.shipmentNo equals ctc.shipmentNo into h
                            from ctc in h.DefaultIfEmpty()
                            join bo in context.Boading
                            on ct.barcode equals bo.AccessNo into o
                            from bo in o.DefaultIfEmpty()
                            where c.FINISHAP_ID == prodname && ctc.ShipmentDate.HasValue && DateTime.Compare(ctc.ShipmentDate.HasValue?ctc.ShipmentDate.Value:DateTime.Now, DateTime.Now.AddYears(-1))>0 && !bo.BoardingDate.HasValue
                            select new
                            {
                                c.SIZESORTING,
                                c.COL_ID,
                                c.COLORID,
                                c.COLOR,
                                c.SIZE,
                                c.Production,
                                pdozen = ct == null ? 0 : (ct.dozen == null ? 0 : ct.dozen),
                                psock = ct == null ? 0 : (ct.socks == null ? 0 : ct.socks),
                            };



                var qeisagogi5 = from c in context.eisagogiParagogis
                                 join ct in context.Semi_finished_warehouse
                                 on c.barcode equals ct.AccessNo into g
                                 from ct in g.DefaultIfEmpty()
                                 join ctc in context.shipment
                                 on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
                                 from ctc in f.DefaultIfEmpty()
                                 join bo in context.Boading
                                 on c.barcode equals bo.AccessNo into z
                                 from bo in z.DefaultIfEmpty()
                                 where ct.ItemCode == prodname && ctc.ShipmentDate.HasValue && DateTime.Compare(ctc.ShipmentDate.HasValue ? ctc.ShipmentDate.Value : DateTime.Now, DateTime.Now.AddYears(-1)) > 0 && !bo.BoardingDate.HasValue
                                 //where c.Col_Id == colid
                                 select new
                                 {
                                     ct.SizeDesc,
                                    ct.ColorCode,
                                     c.AutoNo,
                                     c.date,
                                     c.barcode,
                                     c.Machine,
                                     c.dozen,
                                     c.socks,
                                     c.user,
                                     c.Col_Id,
                                     shipmentno = ct == null ? null : ct.shipmentNo,
                                     shipmentdate = ctc == null ? null : ctc.ShipmentDate,
                                     Finished = bo == null ? null : bo.Warehouse,
                                     findate = bo == null ? null : bo.BoardingDate,
                                 };




                var query1 = query.GroupBy(s => new { s.COLORID, s.COLOR, s.Production, s.SIZE, s.SIZESORTING }).Select(g => new
                {
                   // Cno = g.Key.COL_ID,
                    Cid = g.Key.COLORID,
                    Cnam = g.Key.COLOR,
                    size = g.Key.SIZE,
                    sizesorting = g.Key.SIZESORTING,
                   // productiondosens = g.Key.Production / 24,
                 //   productionsocks = (g.Key.Production) - ((g.Key.Production / 24) * 24),
                  //  producedDosen = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24,
                 //   producedSocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => (x.pdozen * 24) + x.psock) - (g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24) * 24,
                  //  restdosens = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24,
                   // restsocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) - (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24) * 24,
                   // GR = qeisagogi5.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null).Sum(d => d.dozen) + (qeisagogi5.Where(c => c.Col_Id.Equals(g.Key.COL_ID) && c.shipmentdate == null).Sum(d => d.socks) / 24),
                    BG = qeisagogi5.Where(c => c.ColorCode.Equals(g.Key.COLORID) && c.SizeDesc.Equals(g.Key.SIZE) && c.shipmentdate != null && c.findate == null).Sum(d => d.dozen) + (qeisagogi5.Where(c => c.ColorCode.Equals(g.Key.COLORID) && c.SizeDesc.Equals(g.Key.SIZE) && c.shipmentdate != null && c.findate == null).Sum(d => d.socks) / 24)
                }).OrderBy(z => z.Cid).ThenBy(n => n.sizesorting);

                TotalGr.Text = "0";
                TotalBG.Text = query1.Sum(c => c.BG).ToString();

                productiondata.ItemsSource = query1;

            }

        }
    }
}
