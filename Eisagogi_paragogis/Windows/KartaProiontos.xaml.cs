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
    /// Interaction logic for KartaProiontos.xaml
    /// </summary>
    public partial class KartaProiontos : Window
    {
        int printed = 0;

        public KartaProiontos()
        {
            InitializeComponent();
            SetItems();
            Print();
            this.Close();

        }


        private void SetItems()
        {
            var cid = Static_Variables.colourid;

            using (var context = new Production18())
            {
                var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                           where DELTIO_FINISH_SUPER1.COL_ID == cid
                           select DELTIO_FINISH_SUPER1;

                var fin1 = from Finish1 in context.Finish1
                           where Finish1.FINISHAP_ID.Equals(dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault())
                           select Finish1;

                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          where DELTIO_FINISH_SUPER.TOTAL_ID == dfs1.Select(c => c.TOTAL_ID).FirstOrDefault()
                          select DELTIO_FINISH_SUPER;

                var boms4 = from BOMS4 in context.BOMS4
                            where BOMS4.FINISHAP_ID == dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault()
                            select BOMS4;

                var boms3A = from boms3a in context.boms3a
                             where boms3a.FINISHAP_ID == dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault() && boms3a.NO == dfs1.Select(c => c.SIZE).FirstOrDefault()
                             select boms3a;

                printed = Convert.ToInt32(Static_Variables.sequence);

                unnum.Text = cid + "-" + printed;
                bk.Text = "*" + cid + "-" + printed + "*";

                time.Text = DateTime.Now.ToString();

                pnum.Text = Static_Variables.sequence.ToString();


                totalid.Text = dfs1.Select(c => c.TOTAL_ID).FirstOrDefault().ToString();
                colid.Text = dfs1.Select(c => c.COL_ID).FirstOrDefault().ToString();
                @ref.Text = dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();
                cname.Text = dfs1.Select(c => c.COLOR).FirstOrDefault().ToString();
                akp.Text = dfs1.Select(c => c.COL_ID).FirstOrDefault().ToString();
                totalid2.Text = dfs1.Select(c => c.TOTAL_ID).FirstOrDefault().ToString();
                colno.Text = dfs1.Select(c => c.COLORID).FirstOrDefault().ToString();
                size.Text = dfs1.Select(c => c.SIZE).FirstOrDefault().ToString();
                customer.Text = fin1.Select(c => c.COUNTRY).FirstOrDefault().ToString();
                sizeprod.Text = dfs1.Select(c => c.MACH_SIZE).FirstOrDefault();
                forma.Text = dfs1.Select(c => c.FORMES).FirstOrDefault();
                program.Text = dfs1.Select(c => c.PROGRAM).FirstOrDefault();
                oname.Text = dfs.Select(c => c.OrderNo).FirstOrDefault();
                ddate.Text = dfs.Select(c => c.DeliveryDate).FirstOrDefault();
                colcomb.Text = dfs1.Select(c => c.SuplierColor).FirstOrDefault() + " . " + dfs1.Select(c => c.SuplierColorDesc).FirstOrDefault();
                user.Text = "";
                date.Text = "";
                counter.Text = "";
                qty.Text = "";
                rider.Text = boms4.Select(c => c.LABEL).FirstOrDefault();
                box.Text = boms4.Select(c => c.BOX).FirstOrDefault();
                irontag.Text = boms4.Select(c => c.IRON).FirstOrDefault();
                sticker.Text = boms4.Select(c => c.STICKER).FirstOrDefault();
                riz.Text = boms4.Select(c => c.ΡΙΖΟΧΑΡΤΟ).FirstOrDefault();
                wrap.Text = boms4.Select(c => c.ΖΩΝΑΚΙ).FirstOrDefault();
                paper.Text = boms4.Select(c => c.PAPER).FirstOrDefault();
                protypo.Text = boms4.Select(c => c.FORM).FirstOrDefault();
                macnu.Text = "";
                macna.Text = "";
                comments.Text = fin1.Select(c => c.COMMENTS == null ? "" : c.COMMENTS).FirstOrDefault().ToString();
                mpat.Text = boms3A.Select(c => c.PAT).FirstOrDefault();
                mkal.Text = boms3A.Select(c => c.MKAL).FirstOrDefault();
                plat.Text = boms3A.Select(c => c.PKAL).FirstOrDefault();
                ylast.Text = boms3A.Select(c => c.YLAS).FirstOrDefault();
                plast.Text = boms3A.Select(c => c.PLAS).FirstOrDefault();
                spi.Text = fin1.Select(c => c.SPI == null ? "" : c.SPI).FirstOrDefault().ToString();
                commentsdfs.Text = dfs.Select(c => c.comments == null ? "" : c.comments).FirstOrDefault().ToString();
            }

        }

        private void Print()
        {
            var cid = Static_Variables.colourid;
            //List<UIElement> l = new List<UIElement>();
            using (var context = new Production18())
            {


                PrintDialog printDlg = new PrintDialog();
                Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                PrintTicket pt = printDlg.PrintTicket;
                pt.PageOrientation = PageOrientation.Portrait;

                var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                           where DELTIO_FINISH_SUPER1.COL_ID == Static_Variables.colourid
                           select DELTIO_FINISH_SUPER1;

             //   List<DELTIO_FINISH_SUPER1> dfs1l = dfs1.ToList();

                //try

                //var pq = LocalPrintServer.GetDefaultPrintQueue();

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

                    writer = PrintQueue.CreateXpsDocumentWriter(pq);
                    
                    


                    //printed++;

                   // dfs1l = dfs1l.Select(c => { c.GetType().GetProperties().Single(x => x.Name.Equals("sequence")).SetValue(c, printed.ToString(), null); return c; }).ToList();


                    pnum.Text = printed.ToString();
                    time.Text = DateTime.Now.ToString();
                    unnum.Text = cid + "-" + printed;
                    bk.Text = "*" + cid + "-" + printed + "*";


                    this.pan3.InvalidateVisual();

                    pan3.Measure(pageSize);
                    pan3.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                    FixedDocument fixedDoc = Static_Variables.fixedDoc;
                    Panel copy = XamlReader.Parse(XamlWriter.Save(pan3)) as Panel;
                    PageContent pageContent = new PageContent();
                    FixedPage page = new FixedPage();
                    page.Width = pageSize.Width;
                    page.Height = pageSize.Height;
                    ((IAddChild)pageContent).AddChild(page);
                    fixedDoc.Pages.Add(pageContent);
                    page.Children.Add(copy);

                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (Action)delegate ()
                    {

                        //writer.Write(pan3);

                    });

                }

              //  context.SubmitChanges();

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cid = colidsetter.Text;
            List<UIElement> l = new List<UIElement>();
            using (var context = new Production18())
            {


                PrintDialog printDlg = new PrintDialog();
                Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                PrintTicket pt = printDlg.PrintTicket;
                pt.PageOrientation = PageOrientation.Portrait;

                var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                           where DELTIO_FINISH_SUPER1.COL_ID == Convert.ToInt32(cid)
                           select DELTIO_FINISH_SUPER1;

                List<DELTIO_FINISH_SUPER1> dfs1l = dfs1.ToList();

                //try

                var pq = LocalPrintServer.GetDefaultPrintQueue();
                var writer = PrintQueue.CreateXpsDocumentWriter(pq);


                for (var i = 0; i < 2; i++)
                {

                    writer = PrintQueue.CreateXpsDocumentWriter(pq);


                    printed++;

                    dfs1l = dfs1l.Select(c => { c.GetType().GetProperties().Single(x => x.Name.Equals("sequence")).SetValue(c, printed.ToString(), null); return c; }).ToList();


                    pnum.Text = printed.ToString();
                    time.Text = DateTime.Now.ToString();
                    unnum.Text = cid + "-" + printed;
                    bk.Text = "*" + cid + "-" + printed + "*";

                    
                    this.pan3.InvalidateVisual();

                    //var bitmap = new RenderTargetBitmap(Convert.ToInt32(pan1.Width), Convert.ToInt32(pan1.Height), 96, 96, PixelFormats.Default);

                    pan3.Measure(pageSize);
                    pan3.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (Action)delegate ()
                    {

                        //pan3.Measure(pageSize);
                        //pan3.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                        //printDlg.PrintVisual(pan3, "print");


                        writer.Write(pan3);

                    });

                }


                //PrintDialog printDlg = new PrintDialog();
                //Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                //PrintTicket pt = printDlg.PrintTicket;
                //pt.PageOrientation = PageOrientation.Portrait;
                //printDlg.PrintDocument(new DocumentPaginatorImpl(l), "Print Job Description");

                context.SubmitChanges();

            }


        }

        private void setter_Click(object sender, RoutedEventArgs e)
        {
            var cid = colidsetter.Text;

            using (var context = new Production18())
            {
                var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                           where DELTIO_FINISH_SUPER1.COL_ID == Convert.ToInt32(cid)
                           select DELTIO_FINISH_SUPER1;

                var fin1 = from Finish1 in context.Finish1
                           where Finish1.FINISHAP_ID.Equals(dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault())
                           select Finish1;

                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          where DELTIO_FINISH_SUPER.TOTAL_ID == dfs1.Select(c => c.TOTAL_ID).FirstOrDefault()
                          select DELTIO_FINISH_SUPER;

                var boms4 = from BOMS4 in context.BOMS4
                            where BOMS4.FINISHAP_ID == dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault()
                            select BOMS4;

                var boms3A = from boms3a in context.boms3a
                             where boms3a.FINISHAP_ID == dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault() && boms3a.NO == dfs1.Select(c => c.SIZE).FirstOrDefault()
                             select boms3a;

                printed = Convert.ToInt32(dfs1.Select(c => c.sequence).FirstOrDefault());

                unnum.Text = cid + "-" + printed;
                bk.Text = "*" + cid + "-" + printed + "*";

                time.Text = DateTime.Now.ToString();

                pnum.Text = dfs1.Select(c => c.sequence).FirstOrDefault().ToString();


                totalid.Text = dfs1.Select(c => c.TOTAL_ID).FirstOrDefault().ToString();
                colid.Text = dfs1.Select(c => c.COL_ID).FirstOrDefault().ToString();
                @ref.Text = dfs1.Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();
                cname.Text = dfs1.Select(c => c.COLOR).FirstOrDefault().ToString();
                akp.Text = dfs1.Select(c => c.COL_ID).FirstOrDefault().ToString();
                totalid2.Text = dfs1.Select(c => c.TOTAL_ID).FirstOrDefault().ToString();
                colno.Text = dfs1.Select(c => c.COLORID).FirstOrDefault().ToString();
                size.Text = dfs1.Select(c => c.SIZE).FirstOrDefault().ToString();
                customer.Text = fin1.Select(c => c.COUNTRY).FirstOrDefault().ToString();
                sizeprod.Text = dfs1.Select(c => c.MACH_SIZE).FirstOrDefault();
                forma.Text = dfs1.Select(c => c.FORMES).FirstOrDefault();
                program.Text = dfs1.Select(c => c.PROGRAM).FirstOrDefault();
                oname.Text = dfs.Select(c => c.OrderNo).FirstOrDefault();
                ddate.Text = dfs.Select(c => c.DeliveryDate).FirstOrDefault();
                user.Text = "";
                date.Text = "";
                counter.Text = "";
                qty.Text = "";
                rider.Text = boms4.Select(c => c.LABEL).FirstOrDefault();
                box.Text = boms4.Select(c => c.BOX).FirstOrDefault();
                irontag.Text = boms4.Select(c => c.IRON).FirstOrDefault();
                sticker.Text = boms4.Select(c => c.STICKER).FirstOrDefault();
                riz.Text = boms4.Select(c => c.ΡΙΖΟΧΑΡΤΟ).FirstOrDefault();
                wrap.Text = boms4.Select(c => c.ΖΩΝΑΚΙ).FirstOrDefault();
                paper.Text = boms4.Select(c => c.PAPER).FirstOrDefault();
                protypo.Text = boms4.Select(c => c.FORM).FirstOrDefault();
                macnu.Text = "";
                macna.Text = "";
                comments.Text = fin1.Select(c => c.COMMENTS == null ? "" : c.COMMENTS).FirstOrDefault().ToString();
                mpat.Text = boms3A.Select(c => c.PAT).FirstOrDefault();
                mkal.Text = boms3A.Select(c => c.MKAL).FirstOrDefault();
                plat.Text = boms3A.Select(c => c.PKAL).FirstOrDefault();
                ylast.Text = boms3A.Select(c => c.YLAS).FirstOrDefault();
                plast.Text = boms3A.Select(c => c.PLAS).FirstOrDefault();
                spi.Text = fin1.Select(c => c.SPI).FirstOrDefault().ToString();
                commentsdfs.Text = dfs.Select(c => c.comments == null ? "" : c.comments).FirstOrDefault().ToString();
            }

        }
    }
}
