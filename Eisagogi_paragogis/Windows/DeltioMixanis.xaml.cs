using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for DeltioMixanis.xaml
    /// </summary>
    public partial class DeltioMixanis : Window
    {
        //FixedDocument fixedDoc = new FixedDocument();
        FixedDocument fixedDoc =Static_Variables.fixedDoc;

       
        public DeltioMixanis()
        {

            int totalidt = Static_Variables.totalid;

            InitializeComponent();


           

            using (var context = new Production18())
            {

                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          where DELTIO_FINISH_SUPER.TOTAL_ID == totalidt
                          select DELTIO_FINISH_SUPER;

                var fin1 = from Finish1 in context.Finish1
                           where Finish1.FINISHAP_ID == dfs.Select(c => c.FINISHAP_ID).FirstOrDefault()
                           select Finish1;

                totalidbarcode.Text = "*" + totalidt + "*";
                totalid.Text = totalidt.ToString();
                var datee = dfs.Select(c => c.PROD_DATE.Value).FirstOrDefault().ToString("dd/MM/yy");

                date.Text = datee.ToString();
 
                productRef.Text = dfs.Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();
                mach_Type.Text = dfs.Select(c => c.MHXANH).FirstOrDefault();
                memo.Text = dfs.Select(c => c.ΜΕΜΟ).FirstOrDefault();
                orderNo.Text = dfs.Select(c => c.OrderNo).FirstOrDefault();
                deliveryDate.Text = dfs.Select(c => c.DeliveryDate).FirstOrDefault();
                CustomerName.Text = fin1.Select(c => c.COUNTRY).FirstOrDefault();

            }

            using (var context = new Production18())
            {
                var dsf1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                           where DELTIO_FINISH_SUPER1.TOTAL_ID == totalidt
                           orderby DELTIO_FINISH_SUPER1.QueueNo, DELTIO_FINISH_SUPER1.SIZESORTING
                           select DELTIO_FINISH_SUPER1;

                yarn.Text = dsf1.Select(x => x.YarnTitle).FirstOrDefault();
                dosencount.Text = (dsf1.Sum(c => c.Production) / 24).ToString() + " Δωδεκάδες";

                var countsizes = dsf1.GroupBy(c => c.MACH_SIZE).Count();
                WidthCalc(countsizes);

                //puts sizes in textblocks
                var sizes = dsf1.GroupBy(c => c.MACH_SIZE);
                int i = 1;
                foreach (var s in sizes)
                {
                    TextBlock size = (TextBlock)FindName("tbs" + i);
                    TextBlock sizem = (TextBlock)FindName("sbs" + i);
                    TextBlock ar = (TextBlock)FindName("ar" + i);
                    size.Text = dsf1.Where(c => c.MACH_SIZE == s.Key.ToString()).Select(x => x.SIZE).FirstOrDefault();
                    sizem.Text = s.Key.ToString();
                    ar.Text = context.boms3a.Where(c => c.FINISHAP_ID == productRef.Text && c.NO == size.Text).Select(x => x.PKAL).FirstOrDefault();

                    i++;
                }

                // 110 X 7 σύνολο κάθε γραμμή είναι +25. Σύνολο 14 γραμμές, 3 ανοιχτές.
                // puts colours and production in textblocks
                var colours = dsf1.GroupBy(c => c.QueueNo).Select(p => p.OrderBy(x => x.QueueNo).FirstOrDefault());
                i = 1;
                int z = 1;
                int totalheight = 0;

                grid1.Height = 0;
                grid2.Height = 0;
                grid3.Height = 0;
                grid4.Height = 0;
                grid5.Height = 0;
                grid6.Height = 0;
                grid7.Height = 0;


                foreach (var col in colours)
                {

                    int? maxprod = dsf1.Where(c => c.QueueNo == col.QueueNo).Max(x => x.Production);
                    int colourheight = 0;

                    if (maxprod != 0)
                    {



                        // Grid grid = (Grid)FindName("grid" + i);

                        if (maxprod < 1500)
                        {
                            //grid.Height = 110;
                            colourheight = 110;
                        }
                        else
                        {


                            //grid.Height = 110;
                            colourheight = 110;

                            if (maxprod < 6001)
                            {
                                var counter = (maxprod - 1500) / 500;

                                //grid.Height = grid.Height + Convert.ToInt32(((counter+1) * 25));
                                colourheight = colourheight + Convert.ToInt32(((counter + 1) * 25));

                            }
                            else
                            {
                                //grid.Height = 365;
                                colourheight = 365;
                            }
                        }
                        // colourheight = Convert.ToInt32(grid.Height);


                        if (totalheight + colourheight > 771)
                        {
                            i = 1;
                            Grid grid = (Grid)FindName("grid" + i);
                            // grid.Height = 0;
                            //print here and change page
                            this.panel.InvalidateVisual();
                            this.panel.UpdateLayout();
                            print();
                            grid1.Height = colourheight;
                            grid2.Height = 0;
                            grid3.Height = 0;
                            grid4.Height = 0;
                            grid5.Height = 0;
                            grid6.Height = 0;
                            grid7.Height = 0;

                            totalheight = colourheight;
                        }
                        else
                        {
                            Grid grid = (Grid)FindName("grid" + i);
                            grid.Height = colourheight;
                            totalheight = totalheight + colourheight;
                        }

                        foreach (var size in sizes)
                        {
                            int? production = dsf1.Where(c => c.COLORID == col.COL_ID.ToString() && c.MACH_SIZE == size.Key.ToString()).Select(x => x.Production).FirstOrDefault();


                            TextBlock qty = (TextBlock)FindName("tbc" + i + z);
                            TextBlock cid = (TextBlock)FindName("sbc" + i + z);

                            qty.Text = dsf1.Where(c => c.QueueNo == col.QueueNo && c.MACH_SIZE == size.Key.ToString()).Select(x => x.Production).FirstOrDefault().ToString();
                            cid.Text = dsf1.Where(c => c.QueueNo == col.QueueNo && c.MACH_SIZE == size.Key.ToString()).Select(x => x.COL_ID).FirstOrDefault().ToString();

                            z++;
                        }


                        TextBlock colour = (TextBlock)FindName("c" + i);
                        TextBlock prog = (TextBlock)FindName("program" + i);
                        TextBlock yarns = (TextBlock)FindName("yarns" + i);

                        colour.Text = "(" + col.COLORID.ToString() + ")";
                        prog.Text = dsf1.Where(c => c.COLORID == col.COLORID.ToString()).Select(x => x.PROGRAM).FirstOrDefault();
                        yarns.Text = dsf1.Where(c => c.COLORID == col.COLORID.ToString()).Select(x => x.YarnTitle).FirstOrDefault();

                        z = 1;
                        i++;
                    }
                }
            }
            this.panel.InvalidateVisual();
            this.panel.UpdateLayout();
            print();
            //ShowPrintPreview(fixedDoc);
            Static_Variables.fixedDoc = fixedDoc;
            this.Close();

        }

        private void WidthCalc(int countsizes)
        {
            switch (countsizes)
            {
                case 1:
                    bs1.Width = 535;
                    bs2.Width = 0;
                    bs3.Width = 0;
                    bs4.Width = 0;
                    bs5.Width = 0;
                    bs6.Width = 0;

                    s11.Width = 531;
                    s12.Width = 0;
                    s13.Width = 0;
                    s14.Width = 0;
                    s15.Width = 0;
                    s16.Width = 0;

                    s21.Width = 531;
                    s22.Width = 0;
                    s23.Width = 0;
                    s24.Width = 0;
                    s25.Width = 0;
                    s26.Width = 0;

                    s31.Width = 531;
                    s32.Width = 0;
                    s33.Width = 0;
                    s34.Width = 0;
                    s35.Width = 0;
                    s36.Width = 0;

                    s41.Width = 531;
                    s42.Width = 0;
                    s43.Width = 0;
                    s44.Width = 0;
                    s45.Width = 0;
                    s46.Width = 0;

                    s51.Width = 531;
                    s52.Width = 0;
                    s53.Width = 0;
                    s54.Width = 0;
                    s55.Width = 0;
                    s56.Width = 0;

                    s61.Width = 531;
                    s62.Width = 0;
                    s63.Width = 0;
                    s64.Width = 0;
                    s65.Width = 0;
                    s66.Width = 0;

                    s71.Width = 531;
                    s72.Width = 0;
                    s73.Width = 0;
                    s74.Width = 0;
                    s75.Width = 0;
                    s76.Width = 0;
                    break;
                case 2:
                    bs1.Width = 267;
                    bs2.Width = 267;
                    bs3.Width = 0;
                    bs4.Width = 0;
                    bs5.Width = 0;
                    bs6.Width = 0;

                    s11.Width = 266;
                    s12.Width = 265;
                    s13.Width = 0;
                    s14.Width = 0;
                    s15.Width = 0;
                    s16.Width = 0;

                    s21.Width = 266;
                    s22.Width = 265;
                    s23.Width = 0;
                    s24.Width = 0;
                    s25.Width = 0;
                    s26.Width = 0;

                    s31.Width = 266;
                    s32.Width = 265;
                    s33.Width = 0;
                    s34.Width = 0;
                    s35.Width = 0;
                    s36.Width = 0;

                    s41.Width = 266;
                    s42.Width = 265;
                    s43.Width = 0;
                    s44.Width = 0;
                    s45.Width = 0;
                    s46.Width = 0;

                    s51.Width = 266;
                    s52.Width = 265;
                    s53.Width = 0;
                    s54.Width = 0;
                    s55.Width = 0;
                    s56.Width = 0;

                    s61.Width = 266;
                    s62.Width = 265;
                    s63.Width = 0;
                    s64.Width = 0;
                    s65.Width = 0;
                    s66.Width = 0;

                    s71.Width = 266;
                    s72.Width = 265;
                    s73.Width = 0;
                    s74.Width = 0;
                    s75.Width = 0;
                    s76.Width = 0;
                    break;
                case 3:
                    bs1.Width = 178;
                    bs2.Width = 178;
                    bs3.Width = 178;
                    bs4.Width = 0;
                    bs5.Width = 0;
                    bs6.Width = 0;

                    s11.Width = 177;
                    s12.Width = 177;
                    s13.Width = 177;
                    s14.Width = 0;
                    s15.Width = 0;
                    s16.Width = 0;

                    s21.Width = 177;
                    s22.Width = 177;
                    s23.Width = 177;
                    s24.Width = 0;
                    s25.Width = 0;
                    s26.Width = 0;

                    s31.Width = 177;
                    s32.Width = 177;
                    s33.Width = 177;
                    s34.Width = 0;
                    s35.Width = 0;
                    s36.Width = 0;

                    s41.Width = 177;
                    s42.Width = 177;
                    s43.Width = 177;
                    s44.Width = 0;
                    s45.Width = 0;
                    s46.Width = 0;

                    s51.Width = 177;
                    s52.Width = 177;
                    s53.Width = 177;
                    s54.Width = 0;
                    s55.Width = 0;
                    s56.Width = 0;

                    s61.Width = 177;
                    s62.Width = 177;
                    s63.Width = 177;
                    s64.Width = 0;
                    s65.Width = 0;
                    s66.Width = 0;

                    s71.Width = 177;
                    s72.Width = 177;
                    s73.Width = 177;
                    s74.Width = 0;
                    s75.Width = 0;
                    s76.Width = 0;
                    break;
                case 4:
                    bs1.Width = 134;
                    bs2.Width = 134;
                    bs3.Width = 134;
                    bs4.Width = 134;
                    bs5.Width = 0;
                    bs6.Width = 0;

                    s11.Width = 133;
                    s12.Width = 133;
                    s13.Width = 133;
                    s14.Width = 132;
                    s15.Width = 0;
                    s16.Width = 0;

                    s21.Width = 133;
                    s22.Width = 133;
                    s23.Width = 133;
                    s24.Width = 132;
                    s25.Width = 0;
                    s26.Width = 0;

                    s31.Width = 133;
                    s32.Width = 133;
                    s33.Width = 133;
                    s34.Width = 132;
                    s35.Width = 0;
                    s36.Width = 0;

                    s41.Width = 133;
                    s42.Width = 133;
                    s43.Width = 133;
                    s44.Width = 132;
                    s45.Width = 0;
                    s46.Width = 0;

                    s51.Width = 133;
                    s52.Width = 133;
                    s53.Width = 133;
                    s54.Width = 132;
                    s55.Width = 0;
                    s56.Width = 0;

                    s61.Width = 133;
                    s62.Width = 133;
                    s63.Width = 133;
                    s64.Width = 132;
                    s65.Width = 0;
                    s66.Width = 0;

                    s71.Width = 133;
                    s72.Width = 133;
                    s73.Width = 133;
                    s74.Width = 132;
                    s75.Width = 0;
                    s76.Width = 0;
                    break;
                case 5:
                    bs1.Width = 107;
                    bs2.Width = 107;
                    bs3.Width = 107;
                    bs4.Width = 107;
                    bs5.Width = 107;
                    bs6.Width = 0;

                    s11.Width = 106;
                    s12.Width = 106;
                    s13.Width = 107;
                    s14.Width = 106;
                    s15.Width = 106;
                    s16.Width = 0;

                    s21.Width = 106;
                    s22.Width = 106;
                    s23.Width = 107;
                    s24.Width = 106;
                    s25.Width = 106;
                    s26.Width = 0;

                    s31.Width = 106;
                    s32.Width = 106;
                    s33.Width = 107;
                    s34.Width = 106;
                    s35.Width = 106;
                    s36.Width = 0;

                    s41.Width = 106;
                    s42.Width = 106;
                    s43.Width = 107;
                    s44.Width = 106;
                    s45.Width = 106;
                    s46.Width = 0;

                    s51.Width = 106;
                    s52.Width = 106;
                    s53.Width = 107;
                    s54.Width = 106;
                    s55.Width = 106;
                    s56.Width = 0;

                    s61.Width = 106;
                    s62.Width = 106;
                    s63.Width = 107;
                    s64.Width = 106;
                    s65.Width = 106;
                    s66.Width = 0;

                    s71.Width = 106;
                    s72.Width = 106;
                    s73.Width = 107;
                    s74.Width = 106;
                    s75.Width = 106;
                    s76.Width = 0;
                    break;
                case 6:
                    bs1.Width = 89;
                    bs2.Width = 89;
                    bs3.Width = 89;
                    bs4.Width = 89;
                    bs5.Width = 89;
                    bs6.Width = 89;

                    s11.Width = 88;
                    s12.Width = 89;
                    s13.Width = 88;
                    s14.Width = 89;
                    s15.Width = 88;
                    s16.Width = 89;

                    s21.Width = 88;
                    s22.Width = 89;
                    s23.Width = 88;
                    s24.Width = 89;
                    s25.Width = 88;
                    s26.Width = 89;

                    s31.Width = 88;
                    s32.Width = 89;
                    s33.Width = 88;
                    s34.Width = 89;
                    s35.Width = 88;
                    s36.Width = 89;

                    s41.Width = 88;
                    s42.Width = 89;
                    s43.Width = 88;
                    s44.Width = 89;
                    s45.Width = 88;
                    s46.Width = 89;

                    s51.Width = 88;
                    s52.Width = 89;
                    s53.Width = 88;
                    s54.Width = 89;
                    s55.Width = 88;
                    s56.Width = 89;

                    s61.Width = 88;
                    s62.Width = 89;
                    s63.Width = 88;
                    s64.Width = 89;
                    s65.Width = 88;
                    s66.Width = 89;

                    s71.Width = 88;
                    s72.Width = 89;
                    s73.Width = 88;
                    s74.Width = 89;
                    s75.Width = 88;
                    s76.Width = 89;
                    break;
                default:

                    throw new Exception("Invalid Size Counter");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            print();
        }

        private void print()
        {

            //var cid = colidsetter.Text;
           // List<UIElement> l = new List<UIElement>();
            using (var context = new Production18())
            {
                this.panel.InvalidateVisual();
                this.panel.UpdateLayout();



                PrintDialog printDlg = new PrintDialog();
                Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                PrintTicket pt = printDlg.PrintTicket;
                pt.PageOrientation = PageOrientation.Portrait;

                // var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                //             where DELTIO_FINISH_SUPER1.COL_ID == Convert.ToInt32(cid)
                //            select DELTIO_FINISH_SUPER1;

                // List<DELTIO_FINISH_SUPER1> dfs1l = dfs1.ToList();

                //try
                PrintQueue pq;

                if (Static_Variables.printer.ToString().StartsWith("\\"))
                {
                    var pname = Static_Variables.printer.Substring(2, Static_Variables.printer.Length - 2);
                    var end = pname.IndexOf("\\", 0);
                    var path = "\\\\" + pname.Substring(0, end);
                    pname = pname.Substring(end + 1, pname.Length - end - 1);


                    PrintServer printServer = new PrintServer(path);
                    pq = printServer.GetPrintQueue(pname);

                }
                else
                {

                    LocalPrintServer printServer = new LocalPrintServer();
                    pq = printServer.GetPrintQueue(Static_Variables.printer);
                    //  var pq = LocalPrintServer.GetDefaultPrintQueue();
                }
                var writer = PrintQueue.CreateXpsDocumentWriter(pq);


                for (var i = 0; i < 1; i++)
                {
                   // writer = PrintQueue.CreateXpsDocumentWriter(pq);


                    // printed++;

                    //    dfs1l = dfs1l.Select(c => { c.GetType().GetProperties().Single(x => x.Name.Equals("sequence")).SetValue(c, printed.ToString(), null); return c; }).ToList();


                    //     pnum.Text = printed.ToString();
                    //      time.Text = DateTime.Now.ToString();
                    //      unnum.Text = cid + "-" + printed;
                    //      bk.Text = "*" + cid + "-" + printed + "*";


                    

                    //var bitmap = new RenderTargetBitmap(Convert.ToInt32(pan1.Width), Convert.ToInt32(pan1.Height), 96, 96, PixelFormats.Default);

                    panel.Measure(pageSize);
                    panel.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));



                    Panel copy = XamlReader.Parse(XamlWriter.Save(panel)) as Panel;
                    PageContent pageContent = new PageContent();
                    FixedPage page = new FixedPage();
                    page.Width = pageSize.Width;
                    page.Height = pageSize.Height;
                    ((IAddChild)pageContent).AddChild(page);
                    fixedDoc.Pages.Add(pageContent);
                    page.Children.Add(copy);
                    

                    




                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (Action)delegate ()
                    {

         //               writer.Write(panel);
                     
                    });

                }
            }
        }

        public static void ShowPrintPreview(FixedDocument fixedDoc)
        {
            Window wnd = new Window();
            DocumentViewer viewer = new DocumentViewer();
            viewer.Document = fixedDoc;
            wnd.Content = viewer;
            wnd.ShowDialog();
        }

       // public FixedDocumentSequence Document { get; set; }
    }
}
