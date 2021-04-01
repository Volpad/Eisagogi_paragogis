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
    /// Interaction logic for Kataxorisi_Paragogis.xaml
    /// </summary>
    public partial class Kataxorisi_Paragogis : Window
    {

        private static Production18 prod = new Production18();
        private static List<int> prodids;// = prod.DELTIO_FINISH_SUPER.OrderByDescending(f => f.TOTAL_ID).Select(c => c.TOTAL_ID).ToList();

        int availableboxes = 11;
        int counter = 0;

        public Kataxorisi_Paragogis(string refname)
        {
            InitializeComponent();

            Initialization(refname);

            //var prod = new Production18();

            prodids = prod.DELTIO_FINISH_SUPER.OrderByDescending(f => f.TOTAL_ID).Where(c => c.FINISHAP_ID == refname).Select(c => c.TOTAL_ID).ToList();

            Production_Insert(refname);

            prodnumber.Text = (counter+1).ToString() + " / " + prodids.Count.ToString();
        }

        private void Production_Insert(string refname)
        {
            using (var context = new Production18())
            {

                mref.Text = refname;



                //totalid.Text = context.DELTIO_FINISH_SUPER.OrderByDescending(f => f.TOTAL_ID).Where(c => c.FINISHAP_ID == refname).Select(d => d.TOTAL_ID).FirstOrDefault().ToString();
                totalid.Text = prodids[counter].ToString();
                morderno.Text = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.OrderNo).FirstOrDefault();
                mdeliverydate.Text = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.DeliveryDate).FirstOrDefault();
                mdeliverydate.Text = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.DeliveryDate).FirstOrDefault();
                mmonada.SelectedItem = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.orderUnit).FirstOrDefault();
                var varmachine = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID.ToString() == totalid.Text.TrimEnd()).Select(d => d.MHXANH.TrimEnd()).FirstOrDefault();
                mmachine.SelectedItem = context.GM.Where(c => c.FINISHAP_ID == refname && c.MHXANH == varmachine).Select(d => d.MHXANH).FirstOrDefault();
                mcomments.Text = context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.comments).FirstOrDefault();
                myarn.SelectedItem = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.YarnTitle).FirstOrDefault();
                mprog.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text).Select(d => d.PROGRAM).FirstOrDefault();


                int i = 1;
                int z = 1;

                var collist = context.Coloursview.ToList();

                foreach (var col in context.DELTIO_FINISH_SUPER1.GroupBy(g => new { g.COLORID, g.TOTAL_ID, g.QueueNo }).Where(d => d.Key.TOTAL_ID.ToString() == totalid.Text).OrderBy(c => c.Key.QueueNo))
                {
                    Border bord = (Border)FindName("bord" + i);
                    TextBlock rownum = (TextBlock)FindName("rownum" + i);
                    ComboBox colname = (ComboBox)FindName("colname" + i);
                    ComboBox colno = (ComboBox)FindName("colno" + i);
                    ComboBox prog = (ComboBox)FindName("prog" + i);
                    ComboBox yarn = (ComboBox)FindName("yarn" + i);
                    ComboBox special = (ComboBox)FindName("special" + i);
                    ComboBox suplcolno = (ComboBox)FindName("suplcolno" + i);
                    TextBox suplcolna = (TextBox)FindName("suplcolna" + i);
                    Button btn = (Button)FindName("btn" + i);


                    bord.Visibility = Visibility.Visible;
                    rownum.Visibility = Visibility.Visible;
                    colname.Visibility = Visibility.Visible;
                    colno.Visibility = Visibility.Visible;
                    prog.Visibility = Visibility.Visible;
                    yarn.Visibility = Visibility.Visible;
                    special.Visibility = Visibility.Visible;
                    suplcolna.Visibility = Visibility.Visible;
                    suplcolno.Visibility = Visibility.Visible;
                    btn.Visibility = Visibility.Visible;


                    z = 1;
                    foreach (var siz in context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == col.Key.TOTAL_ID && c.COLORID == col.Key.COLORID))
                    {
                        TextBox size = (TextBox)FindName("size" + i + z);
                        TextBox qty = (TextBox)FindName("qty" + i + z);
                        TextBox stock = (TextBox)FindName("stock" + i + z);
                        TextBox prod = (TextBox)FindName("prod" + i + z);


                        size.Visibility = Visibility.Visible;
                        qty.Visibility = Visibility.Visible;
                        stock.Visibility = Visibility.Visible;
                        prod.Visibility = Visibility.Visible;

                        size.Text = siz.SIZE;
                        qty.Text = siz.ΠΟΣΟΤΗΤΑ.ToString();
                        stock.Text = siz.InStock.ToString();
                        prod.Text = siz.Production.ToString();

                        z++;

                    }

                    //   colname.ItemsSource = collist.GroupBy(c => c.COLOR).OrderBy(f => f.Key).Select(d => d.Key);
                    var cna = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.COLOR).FirstOrDefault();
                    colname.SelectedItem = collist.Where(f => f.COLOR == cna).GroupBy(c => c.COLOR).Select(d => d.Key).FirstOrDefault(); //context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.COLOR).FirstOrDefault();
                                                                                                                                         //   colno.ItemsSource = collist.GroupBy(c => c.ColorId).OrderBy(f => f.Key).Select(d => d.Key);
                    var cno = col.Key.COLORID;
                    colno.SelectedItem = collist.Where(f => f.ColorId == cno).GroupBy(c => c.ColorId).Select(d => d.Key).FirstOrDefault();
                    //prog.ItemsSource = context.GMP.Where(c => c.FINISHAP_ID == refname).Select(d => d.PROGRAM);
                    //yarn.ItemsSource = context.BOMS_2.Where(c => c.FINISHAP_ID == refname && c.PART == "ΒΑΣΙΚΑ ΝΗΜΑΤΑ").Select(d => d.COLOR);


                    prog.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.PROGRAM).FirstOrDefault();
                    yarn.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.YarnTitle).FirstOrDefault();
                    special.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.SpecialInstruction).FirstOrDefault();
                    suplcolno.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.SuplierColor).FirstOrDefault();
                    suplcolna.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID.ToString() == totalid.Text && c.COLORID == col.Key.COLORID).Select(d => d.SuplierColorDesc).FirstOrDefault();

                    //colname.SelectionChanged += new SelectionChangedEventHandler(colorselection);
                    //colno.SelectionChanged += new SelectionChangedEventHandler(colorselection);

                    i++;

                }
            }
        }

        private void Initialization(string refname)
        {
            using (var context = new Production18())
            {



                var collist = context.Coloursview.ToList();


                mmachine.ItemsSource = context.GM.Where(c => c.FINISHAP_ID == refname).Select(d => d.MHXANH);
                myarn.ItemsSource = context.BOMS_2.Where(c => c.FINISHAP_ID == refname && c.PART == "ΒΑΣΙΚΑ ΝΗΜΑΤΑ").Select(d => d.COLOR);
                mprog.ItemsSource = context.GMP.Where(c => c.FINISHAP_ID == refname).Select(d => d.PROGRAM);
                mmonada.ItemsSource = context.DELTIO_FINISH_SUPER.GroupBy(c => c.orderUnit).Select(d => d.Key);
                morderno.ItemsSource = context.DELTIO_FINISH_SUPER.Where(f => f.FINISHAP_ID == refname).GroupBy(c => c.OrderNo).Select(d => d.Key);
                mcolno.ItemsSource = context.Coloursview.GroupBy(c => c.ColorId).Select(d => d.Key);
                mcolna.ItemsSource = context.Coloursview.GroupBy(c => c.COLOR).OrderBy(f => f.Key).Select(d => d.Key);

                for (int i = 1; i < availableboxes + 1; i++)
                {
                    Border bord = (Border)FindName("bord" + i);
                    TextBlock rownum = (TextBlock)FindName("rownum" + i);
                    ComboBox colname = (ComboBox)FindName("colname" + i);
                    ComboBox colno = (ComboBox)FindName("colno" + i);
                    ComboBox prog = (ComboBox)FindName("prog" + i);
                    ComboBox yarn = (ComboBox)FindName("yarn" + i);



                    for (int z = 1; z < 9; z++)
                    {
                        TextBox size = (TextBox)FindName("size" + i + z);
                        TextBox qty = (TextBox)FindName("qty" + i + z);
                        TextBox stock = (TextBox)FindName("stock" + i + z);
                        TextBox prod = (TextBox)FindName("prod" + i + z);

                        size.TextAlignment = TextAlignment.Center;
                        size.Visibility = Visibility.Collapsed;
                        qty.Visibility = Visibility.Collapsed;
                        stock.Visibility = Visibility.Collapsed;
                        prod.Visibility = Visibility.Collapsed;

                    }

                    ComboBox special = (ComboBox)FindName("special" + i);
                    ComboBox suplcolno = (ComboBox)FindName("suplcolno" + i);
                    TextBox suplcolna = (TextBox)FindName("suplcolna" + i);
                    Button btn = (Button)FindName("btn" + i);

                    bord.Visibility = Visibility.Collapsed;
                    rownum.Visibility = Visibility.Collapsed;
                    colname.Visibility = Visibility.Collapsed;
                    colno.Visibility = Visibility.Collapsed;
                    prog.Visibility = Visibility.Collapsed;
                    yarn.Visibility = Visibility.Collapsed;
                    special.Visibility = Visibility.Collapsed;
                    suplcolna.Visibility = Visibility.Collapsed;
                    suplcolno.Visibility = Visibility.Collapsed;
                    btn.Visibility = Visibility.Collapsed;

                    mcolno.Text = "";
                    mcolna.Text = "";
                    colname.ItemsSource = collist.GroupBy(c => c.COLOR).OrderBy(f => f.Key).Select(d => d.Key);
                    colno.ItemsSource = collist.GroupBy(c => c.ColorId).OrderBy(f => f.Key).Select(d => d.Key);
                    prog.ItemsSource = context.GMP.Where(c => c.FINISHAP_ID == refname).Select(d => d.PROGRAM);
                    yarn.ItemsSource = context.BOMS_2.Where(c => c.FINISHAP_ID == refname && c.PART == "ΒΑΣΙΚΑ ΝΗΜΑΤΑ").Select(d => d.COLOR);

                    colname.SelectionChanged += new SelectionChangedEventHandler(colorselection);
                    colno.SelectionChanged += new SelectionChangedEventHandler(colorselection);

                }
            }
        }

        private void colorselection(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            using (var context = new Production18())
            {
                if (box.Name.StartsWith("colname"))
                {
                    var i = box.Name.Substring(7, box.Name.Length - 7);
                    ComboBox box2 = (ComboBox)FindName("colno" + i);
#pragma warning disable CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
                    box2.Text = context.Coloursview.Where(c => c.COLOR == box.SelectedItem).Select(d => d.ColorId).FirstOrDefault();
#pragma warning restore CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
                }
                else if (box.Name.StartsWith("colno"))
                {
                    var i = box.Name.Substring(5, box.Name.Length - 5);
                    ComboBox box2 = (ComboBox)FindName("colname" + i);
#pragma warning disable CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
                    box2.Text = context.Coloursview.Where(c => c.ColorId == box.SelectedItem).Select(d => d.COLOR).FirstOrDefault();
#pragma warning restore CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void mcolno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new Production18())
            {
#pragma warning disable CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
                mcolna.Text = context.Coloursview.Where(c => c.ColorId == mcolno.SelectedItem).Select(f => f.COLOR).FirstOrDefault();
#pragma warning restore CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
            }
        }

        private void mcolna_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new Production18())
            {
#pragma warning disable CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
                mcolno.Text = context.Coloursview.Where(c => c.COLOR == mcolna.SelectedItem).Select(f => f.ColorId).FirstOrDefault();
#pragma warning restore CS0253 // Possible unintended reference comparison; to get a value comparison, cast the right hand side to type 'string'
            }
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (counter != 0)
            {
                counter--;
                prodnumber.Text = (counter + 1).ToString() + " / " + prodids.Count.ToString();
                Production_Insert(mref.Text);
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (counter < prodids.Count-1)
            {
                counter++;
                //  MessageBox.Show(prodids.Count.ToString());
                prodnumber.Text = (counter + 1).ToString() + " / " + prodids.Count.ToString();
                Production_Insert(mref.Text);
            }
        }
    }
}
