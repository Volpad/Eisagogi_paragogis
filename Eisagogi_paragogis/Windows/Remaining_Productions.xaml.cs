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
    /// Interaction logic for Remaining_Productions.xaml
    /// </summary>
    public partial class Remaining_Productions : Window
    {
        public bool iswalk = Static_Variables.isWalk;
        

        public Remaining_Productions()
        {
            if (iswalk)
            {
                Static_Variables.isothersopen = true;
            }
            else
            {
                Static_Variables.isothersopen = true;
            }

            InitializeComponent();

            tbl1.Text = "Lonati L1 (240)";
            tbl2.Text = "Lonati L2 (220)\nLonati L3 (220k)\nSangiacomo S1 (216)";
            tbb1.Text = "Busi B1 (240)";
            tbb2.Text = "Busi B2 (220)";
            tbm6.Text = "Matec M6 (168)";
            tbm4.Text = "Matec M4 (240)";
            tbm5.Text = "Matec M5 (200)";
            tbl8.Text = "Lonati L8 (132 παιδική)";
            tbl9.Text = "Lonati L9 (132 Χοντρή)\nΌλες οι Komet";
            tbl6.Text = "Lonati L6 (160)\nSangiacomo S2 (168)";
            tbb4.Text = "Busi B4 (168)\nLonati L4 (168)";

            Check_orders();

            setGrids(null,null);
        }

        private static void Check_orders()
        {
            using (var context = new Production18())
            {
                var tbp = from ToBePlanned in context.ToBePlanned
                          orderby ToBePlanned.LastCheck descending
                          // where ToBePlanned.Deleted == false && ToBePlanned.OnMachine == false
                          select ToBePlanned;

                DateTime lastdate = tbp.Max(c => c.LastCheck).GetValueOrDefault();

                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                          where DELTIO_FINISH_SUPER.PROD_DATE > lastdate
                          select DELTIO_FINISH_SUPER;

                var mqty = from Machineqty in context.Machineqty
                           orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                           where Machineqty.MachineNo != null && Machineqty.queueNo != "deleted" && Machineqty.queueNo != "returned"
                           select Machineqty;

                var mqtyfull = from Machineqty in context.Machineqty
                               orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                               where Machineqty.MachineNo != null && Machineqty.queueNo != "returned"
                               select Machineqty;

                var fns1 = from Finish1 in context.Finish1
                           select Finish1;

                foreach (var d in dfs)
                {
                    DateTime deldate = DateTime.Now;
                    var ContainsPermissionIDFive = mqty.Any(p => p.AccessNo == d.TOTAL_ID);
                    var isWalk = fns1.Where(c => c.FINISHAP_ID.ToUpper().Equals(d.FINISHAP_ID.ToUpper())).Any(p => p.COUNTRY.ToUpper() == "WALK" || p.COUNTRY.ToUpper() == "VENNI" || p.COUNTRY.ToUpper() == "MARCO VENNI");
                    DateTime.TryParse(d.DeliveryDate, out deldate);
                                       

                    if (!ContainsPermissionIDFive)
                    {
                        if (isWalk)
                        {
                            ToBePlanned input = new ToBePlanned
                            {
                                AccessNo = d.TOTAL_ID,
                                Machine = d.MHXANH.Substring(d.MHXANH.Length - 2),
                                CDate = d.PROD_DATE,
                                LastCheck = DateTime.Now,
                                isWalk = true,
                                DDate = deldate
                        };
                            context.ToBePlanned.InsertOnSubmit(input);
                        }
                        else
                        {
                            ToBePlanned input = new ToBePlanned
                            {

                                AccessNo = d.TOTAL_ID,
                                Machine = d.MHXANH.Substring(d.MHXANH.Length - 2),
                                CDate = d.PROD_DATE,
                                LastCheck = DateTime.Now,
                                isWalk = false,
                                DDate = deldate

                            };
                            context.ToBePlanned.InsertOnSubmit(input);
                        }
                    }
                }

                List<ToBePlanned> list = context.ToBePlanned.ToList();

                foreach (var t in tbp)
                {
                    var onmachine = mqtyfull.Any(c => c.AccessNo == t.AccessNo);

                    if (onmachine)
                    {
                        list = tbp.AsEnumerable().Where(f => f.ID == t.ID).Select(c => { c.GetType().GetProperties().Single(x => x.Name.Equals("OnMachine")).SetValue(c, true, null); return c; }).ToList();
                    }
                }
                context.SubmitChanges();
            }
        }

        private void setGrids(DateTime? d1, DateTime? d2)
        {
            if (d2 >= d1 || (d1 == null && d2 == null))
            {
                using (var context = new Production18())
                {


                    var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                              orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                              //where DELTIO_FINISH_SUPER.TOTAL_ID >15600
                              //where Convert.ToDateTime(DELTIO_FINISH_SUPER.DeliveryDate) > d1 && Convert.ToDateTime( DELTIO_FINISH_SUPER.DeliveryDate) < d2
                              select DELTIO_FINISH_SUPER;
                    var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                               orderby DELTIO_FINISH_SUPER1.TOTAL_ID descending
                               select DELTIO_FINISH_SUPER1;

                    var eispar = from eisagogiParagogis in context.eisagogiParagogis
                                 orderby eisagogiParagogis.Total_Id descending
                                 where eisagogiParagogis.date > DateTime.Now.AddMonths(-10) && eisagogiParagogis.barcode != null
                                 select eisagogiParagogis;

                    var mqty = from Machineqty in context.Machineqty
                               orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                               where Machineqty.MachineNo != null
                               select Machineqty;

                    var tbp = from ToBePlanned in context.ToBePlanned
                              orderby ToBePlanned.CDate descending
                              where ToBePlanned.Deleted == false && ToBePlanned.OnMachine == false// && ToBePlanned.DDate.Value.Date >= d1.Value.Date && ToBePlanned.DDate.Value.Date <= d2.Value.Date
                              select ToBePlanned; 

                    if (d1 == null || d2 == null)
                    {
                         //tbp = from ToBePlanned in context.ToBePlanned
                         //         orderby ToBePlanned.CDate descending
                         //         where ToBePlanned.Deleted == false && ToBePlanned.OnMachine == false && ToBePlanned.DDate.Value.Date >= d1.Value.Date && ToBePlanned.DDate.Value.Date <= d2.Value.Date
                         //         select ToBePlanned;
                    }
                    else
                    {
                         tbp = from ToBePlanned in context.ToBePlanned
                                  orderby ToBePlanned.CDate descending
                                  where ToBePlanned.Deleted == false && ToBePlanned.OnMachine == false && ToBePlanned.DDate.Value.Date >= d1.Value.Date && ToBePlanned.DDate.Value.Date <= d2.Value.Date
                                  select ToBePlanned;
                    }

                    var query = from c in dfs1
                                join ct in eispar
                                on c.COL_ID equals ct.Col_Id into g
                                from ct in g.DefaultIfEmpty()
                                select new
                                {
                                    c.TOTAL_ID,
                                    c.COL_ID,
                                    c.COLORID,
                                    c.COLOR,
                                    c.SIZE,
                                    c.Production,
                                    pdozen = ct == null ? 0 : (ct.dozen == null ? 0 : ct.dozen),
                                    psock = ct == null ? 0 : (ct.socks == null ? 0 : ct.socks),
                                };



                    //Vazei tis hmeromhnies paradoshs an den exoun perastei apo thn arxh h an allaxei afoy exei perastei.

                    foreach (var c in context.ToBePlanned.Where(n => n.Deleted == false && n.OnMachine == false))
                    {
                        DateTime dt = DateTime.Now;
                        var test2 = (dfs.Where(d => d.TOTAL_ID == c.AccessNo).Select(f => f.DeliveryDate)).FirstOrDefault();
                        var test = DateTime.TryParse((dfs.Where(d => d.TOTAL_ID == c.AccessNo).Select(f => f.DeliveryDate)).FirstOrDefault(), out dt);

                        if (test)
                        {
                            c.DDate = dt;
                        }
                    }
                    context.SubmitChanges();

                    var query1 = query.GroupBy(s => new { s.COL_ID, s.COLORID, s.COLOR, s.Production, s.SIZE, s.TOTAL_ID }).Select(g => new
                    {
                        AccessNo = g.Key.TOTAL_ID,
                        Cno = g.Key.COL_ID,
                        Cid = g.Key.COLORID,
                        Cnam = g.Key.COLOR,
                        size = g.Key.SIZE,
                        productiondosens = g.Key.Production / 24,
                        productionsocks = (g.Key.Production) - ((g.Key.Production / 24) * 24),
                        producedDosen = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24,
                        producedSocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : g.Sum(x => (x.pdozen * 24) + x.psock) - (g.Sum(x => ((x.pdozen * 24) + x.psock)) / 24) * 24,
                        restdosens = ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24,
                        restsocks = g.Sum(x => ((x.pdozen * 24) + x.psock)) == 0 ? null : ((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) - (((g.Key.Production) - (g.Sum(x => ((x.pdozen * 24) + x.psock)))) / 24) * 24
                    });


                    var l1 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("L1") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var l2 = tbp.Where(s => s.Deleted == false && (s.Machine.StartsWith("L2") || s.Machine.StartsWith("L3") || s.Machine.StartsWith("S1")) && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var b1 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("B1") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var b2 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("B2") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var m6 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("M6") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var m4 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("M4") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var m5 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("M5") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var l8 = tbp.Where(s => s.Deleted == false && s.Machine.StartsWith("L8") && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var l9 = tbp.Where(s => s.Deleted == false && (s.Machine.StartsWith("L9") || s.Machine.StartsWith("K")) && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var l6 = tbp.Where(s => s.Deleted == false && (s.Machine.StartsWith("L6") || s.Machine.StartsWith("S2")) && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    var b4 = tbp.Where(s => s.Deleted == false && (s.Machine.StartsWith("B4") || s.Machine.StartsWith("L4")) && s.OnMachine == false && s.isWalk == iswalk).Select(c => new
                    {
                        c.ID,
                        Date = c.CDate,
                        Name = dfs.Where(f => f.TOTAL_ID.Equals(c.AccessNo)).Select(k => k.FINISHAP_ID).FirstOrDefault(),
                        AccessNo = c.AccessNo,
                        Rest = query1.Where(k => k.AccessNo == c.AccessNo).Sum(g => g.restdosens)
                    });

                    L1.IsReadOnly = true;
                    L2.IsReadOnly = true;
                    B1.IsReadOnly = true;
                    B2.IsReadOnly = true;
                    M6.IsReadOnly = true;
                    M4.IsReadOnly = true;
                    M5.IsReadOnly = true;
                    L8.IsReadOnly = true;
                    L9.IsReadOnly = true;
                    L6.IsReadOnly = true;
                    B4.IsReadOnly = true;
                    //L1.HeadersVisibility = DataGridHeadersVisibility.None;
                    L1.ItemsSource = l1;
                    L2.ItemsSource = l2;
                    B1.ItemsSource = b1;
                    B2.ItemsSource = b2;
                    M6.ItemsSource = m6;
                    M4.ItemsSource = m4;
                    M5.ItemsSource = m5;
                    L8.ItemsSource = l8;
                    L9.ItemsSource = l9;
                    L6.ItemsSource = l6;
                    B4.ItemsSource = b4;

                    BrushConverter bc = new BrushConverter();

                    //L1.ColumnHeaderStyle = Style.Setters.Add(new Setter(DataGrid.BackgroundProperty, (System.Windows.Media.Brush)bc.ConvertFrom("#03729e"))) ;


                    //var Machinenamecreate = from Machineqty in context.Machineqty
                    //                        orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                    //                        select Machineqty;
                    var Eisagogioaragogis = from eisagogiParagogis in context.eisagogiParagogis
                                            orderby eisagogiParagogis.Total_Id descending
                                            where eisagogiParagogis.date > DateTime.Now.AddMonths(-10) && eisagogiParagogis.barcode != null
                                            select eisagogiParagogis;


                    //string machinename2 = Machinenamecreate.Where(i => i.MachineNo.StartsWith(machinename)).Select(k => k.MachineNo).FirstOrDefault().ToString();

                    mol1.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("L1")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("L1") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mol2.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("L2") && i.Machine.Contains("L3") && i.Machine.Contains("S1")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => (i.Machine.Contains("L2") || i.Machine.Contains("L3") || i.Machine.Contains("S1")) && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mob1.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("B1")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("B1") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mob2.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("B2")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("B2") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mob4.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("B4") && i.Machine.Contains("L4")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => (i.Machine.Contains("B4") || i.Machine.Contains("L4")) && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mol6.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("L6") && i.Machine.Contains("S2")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => (i.Machine.Contains("L1") || i.Machine.Contains("S2")) && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mom4.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("M4")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("M4") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mom5.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("M5")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("M5") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mom6.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("M6") && i.Machine.Contains("KOM")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => (i.Machine.Contains("M6") || i.Machine.Contains("KOM")) && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mol9.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("L9")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("L9") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();
                    mol8.Text = Eisagogioaragogis.Where(i => i.Machine.Contains("L8")).Equals(null) ? "" : "MO: " + (Decimal.Round(Convert.ToDecimal(Eisagogioaragogis.Where(i => i.Machine.Contains("L8") && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / 21.32) / 24), 1)).ToString();





                    L1.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#03729e");
                    L2.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#51b3d2");
                    B1.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#4b889a");
                    B2.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#4eaec7");
                    M6.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#85bd5f");
                    M4.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#b7d8a1");
                    M5.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#76b54c");
                    L8.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#67b5cc");
                    L9.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#7cc0da");
                    L6.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#366988");
                    B4.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#735183");

                    //mol1.Text = "MO: " + ;



                    restl1.Text = l1.Sum(c => c.Rest) == null ? "" : "Sum: " + l1.Sum(c => c.Rest).ToString();
                    restl2.Text = l2.Sum(c => c.Rest) == null ? "" : "Sum: " + l2.Sum(c => c.Rest).ToString();
                    restb1.Text = b1.Sum(c => c.Rest) == null ? "" : "Sum: " + b1.Sum(c => c.Rest).ToString();
                    restb2.Text = b2.Sum(c => c.Rest) == null ? "" : "Sum: " + b2.Sum(c => c.Rest).ToString();
                    restb4.Text = b4.Sum(c => c.Rest) == null ? "" : "Sum: " + b4.Sum(c => c.Rest).ToString();
                    restl6.Text = l6.Sum(c => c.Rest) == null ? "" : "Sum: " + l6.Sum(c => c.Rest).ToString();
                    restm4.Text = m4.Sum(c => c.Rest) == null ? "" : "Sum: " + m4.Sum(c => c.Rest).ToString();
                    restm5.Text = m5.Sum(c => c.Rest) == null ? "" : "Sum: " + m5.Sum(c => c.Rest).ToString();
                    restm6.Text = m6.Sum(c => c.Rest) == null ? "" : "Sum: " + m6.Sum(c => c.Rest).ToString();
                    restl8.Text = l8.Sum(c => c.Rest) == null ? "" : "Sum: " + l8.Sum(c => c.Rest).ToString();
                    restl9.Text = l9.Sum(c => c.Rest) == null ? "" : "Sum: " + l9.Sum(c => c.Rest).ToString();


                }
            }
            else
            {
                MessageBox.Show("Λάθος ημερομηνίες");
            }

        }

        private void gridloaded(object sender, RoutedEventArgs e)
        {
            DataGrid machTypeGrid = sender as DataGrid;

            ColumnDefinition cd = new ColumnDefinition();


            //machTypeGrid.Columns[0].Width = 70;
            //machTypeGrid.Columns[1].Width = 67;
            //machTypeGrid.Columns[2].Width = 67;
            //machTypeGrid.Columns[3].Width = 40;







        }

        private void RowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;

                var test = grid.SelectedValue.ToString();

                int index = test.LastIndexOf("AccessNo =");
                if (index > 0)
                    test = test.Substring(index + 11, 5);

                //int? Accessno = Convert.ToInt32(test);

                Static_Variables.prodviewtotalid = Convert.ToInt32(test);

                productionView prodview = new productionView();

                prodview.Show();
            }
            catch
            {

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
                int index = data.IndexOf("ID =");
                int index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 5, index2 - (index + 5));
                    int id = Convert.ToInt32(temp);
                    Static_Variables.rowid = id;
                    ContextMenu cm = this.FindResource("cmenu") as ContextMenu;
                    cm.IsOpen = true;
                }
                index = data.LastIndexOf("AccessNo =");
                if (index > 0)
                {
                    var temp = data.Substring(index + 11, 5);
                    int totalid = Convert.ToInt32(temp);

                    Static_Variables.totalid = totalid;

                }
            }
        }


        private void Click_OnMachine(object sender, RoutedEventArgs e)
        {

        }

        private void Click_Delete(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Static_Variables.rowid.ToString());

            using (var context = new Production18())
            {
                List<ToBePlanned> tbpl = context.ToBePlanned.ToList();
                tbpl = context.ToBePlanned.AsEnumerable().Where(f => f.ID == Static_Variables.rowid).Select(c => { c.GetType().GetProperties().Single(x => x.Name.Equals("Deleted")).SetValue(c, true, null); return c; }).ToList();
                context.SubmitChanges();
                setGrids(null,null);
            }
        }

        private void Click_Copy_Access_Number(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Static_Variables.totalid.ToString());
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }

        }

        private void Balance_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          where DELTIO_FINISH_SUPER.TOTAL_ID == Static_Variables.totalid
                          select DELTIO_FINISH_SUPER;

                Static_Variables.finishid = dfs.Select(c => c.FINISHAP_ID).FirstOrDefault();

                Window balance = new Balance();
                balance.Show();

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (iswalk)
            {
                Static_Variables.isothersopen = false;
            }
            else
            {
                Static_Variables.isothersopen = false;
            }
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            setGrids(date1.SelectedDate, date2.SelectedDate);
        }
    }
}
