using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
    /// Interaction logic for forma_ektyposis.xaml
    /// </summary>
    public partial class forma_ektyposis : Window
    {
        //original diastaseis Height="204.661" Width="460.594"


        int totalId = 0;
        public forma_ektyposis()
        {
            InitializeComponent();
            totalid.PreviewTextInput += new TextCompositionEventHandler(NumberValidationTextBox);
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                printers.Items.Add(printer);
            }
            PrinterSettings settings = new PrinterSettings();
            printers.Text = settings.PrinterName;
            Static_Variables.printer = settings.PrinterName;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void paketo_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          select DELTIO_FINISH_SUPER;
                totalId = Convert.ToInt32(totalid.Text);
                if (dfs.Any(c => c.TOTAL_ID == totalId))
                {
                    Static_Variables.fixedDoc = new FixedDocument();
                    Static_Variables.totalid = totalId;
                    Window deltiom = new DeltioMixanis();
                  
                  //  PreviewD.Document = Static_Variables.fixedDoc;

                    

                    var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                               where DELTIO_FINISH_SUPER1.TOTAL_ID == Static_Variables.totalid
                               orderby DELTIO_FINISH_SUPER1.QueueNo, DELTIO_FINISH_SUPER1.SIZESORTING
                               select DELTIO_FINISH_SUPER1;

                    foreach (var c in dfs1)
                    {
                        Static_Variables.colourid = c.COL_ID;
                        int? prodcounter = c.Production;
                        int sequence = Convert.ToInt32(c.sequence);
                        int counter = 0;
                        
                        while (prodcounter > 0 && counter < 5)
                        {
                            Static_Variables.sequence = sequence + 1;
                            Window kp = new KartaProiontos();
                            prodcounter = prodcounter - 500;
                            sequence++;
                            counter++;
                        }

                        c.sequence = sequence.ToString();

                    }
                    context.SubmitChanges();
                    PreviewD.Document = Static_Variables.fixedDoc;
                    PreviewD.Visibility = Visibility.Visible;
                    this.Width = 840;
                    this.Height = 750;
                    DocumentViewer.FitToMaxPagesAcrossCommand.Execute("1", PreviewD);
                }
                else
                {
                    MessageBox.Show("Λάθος αριθμός παραγωγής");
                }
            }

        }

        private void deltmix_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          select DELTIO_FINISH_SUPER;
                totalId = Convert.ToInt32(totalid.Text);
                if (dfs.Any(c => c.TOTAL_ID == totalId))
                {
                    Static_Variables.fixedDoc = new FixedDocument();
                    Static_Variables.totalid = totalId;
                    Window deltiom = new DeltioMixanis();
                    PreviewD.Visibility = Visibility.Visible;
                    this.Width = 840;
                    this.Height = 750;
                    DocumentViewer.FitToMaxPagesAcrossCommand.Execute("1", PreviewD);
                    PreviewD.Document = Static_Variables.fixedDoc;
                }
                else
                {
                    MessageBox.Show("Λάθος αριθμός παραγωγής");
                }
            }
        }

        private void printers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Static_Variables.printer = (sender as ComboBox).SelectedItem as string;
        }

        private void totalid_GotFocus(object sender, RoutedEventArgs e)
        {
            totalid.Text = "";
        }

        private void karteles_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                          select DELTIO_FINISH_SUPER;
                totalId = Convert.ToInt32(totalid.Text);
                if (dfs.Any(c => c.TOTAL_ID == totalId))
                {
                    Static_Variables.fixedDoc = new FixedDocument();
                    Static_Variables.totalid = totalId;
                    //Window deltiom = new DeltioMixanis();
                    var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                               where DELTIO_FINISH_SUPER1.TOTAL_ID == Static_Variables.totalid
                               orderby DELTIO_FINISH_SUPER1.QueueNo, DELTIO_FINISH_SUPER1.SIZESORTING
                               select DELTIO_FINISH_SUPER1;

                    foreach (var c in dfs1)
                    {
                        Static_Variables.colourid = c.COL_ID;
                        int? prodcounter = c.Production;
                        int sequence = Convert.ToInt32(c.sequence);
                        int counter = 0;
                        
                        while (prodcounter > 0 && counter < 5)
                        {
                            Static_Variables.sequence = sequence + 1;
                            Window kp = new KartaProiontos();
                            prodcounter = prodcounter - 500;
                            sequence++;
                            counter++;
                        }

                        c.sequence = sequence.ToString();

                    }
                    context.SubmitChanges();
                    PreviewD.Document = Static_Variables.fixedDoc;
                    PreviewD.Visibility = Visibility.Visible;
                    this.Width = 840;
                    this.Height = 750;
                    DocumentViewer.FitToMaxPagesAcrossCommand.Execute("1", PreviewD);
                }
                else
                {
                    MessageBox.Show("Λάθος αριθμός παραγωγής");
                }
            }
        }
    }
}
