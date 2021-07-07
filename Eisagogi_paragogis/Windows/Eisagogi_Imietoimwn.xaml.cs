using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Drawing;
using System.Diagnostics;

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for Eisagogi_Imietoimwn.xaml
    /// </summary>
    public partial class Eisagogi_Imietoimwn : Window
    {
        private int aa = 0;
        private int id = 0;
        private string accessno = "";
        private string path = @"S:\Production\PackingList";

        public Eisagogi_Imietoimwn()
        {
            InitializeComponent();
            ShipmentGridRefresh();

            


        }

        private void ShipmentGridRefresh()
        {
            using (var context = new Production18())
            {
                shipment_grid.ItemsSource = from c in context.shipment
                                            orderby c.shipmentNo descending
                                            select new
                                            {
                                                id = c.ID,
                                                AA = c.shipmentNo,
                                                Ημερομηνία = c.Date.Day + "/" + c.Date.Month + "/" + c.Date.Year,
                                                Τρόπος = c.truck,
                                                ΗμΑποστολής = c.ShipmentDate
                                            };
                ResetBoxes();
            }
        }

        private void ContentGridRefresh()
        {
            ram.Text = "";
            ar.Text = "";
            mesys.Text = "";
            xwrissys.Text = "";
            total.Text = "";

            using (var context = new Production18())
            {
                var shipmentno = context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.shipmentNo).FirstOrDefault();

                content_grid.ItemsSource = from c in context.Semi_finished_warehouse
                                           orderby c.Date descending
                                           where c.shipmentNo == shipmentno
                                           select new
                                           {
                                               id = c.ID,
                                               ShipmentNo = c.shipmentNo,
                                               Ημερομηνία = c.Date,
                                               AccessNo = c.AccessNo,
                                               Ραμ = c.Linking == "00" ? "Ναι" : "Οχι",
                                               Συσκ = c.Syskeuastika == "00" ? "Ναι" : "Οχι",
                                               Κωδικός = c.ItemCode,
                                               Χρώμα = c.ColorCode + "." + c.ColorGRdesc,
                                               Μέγεθος = c.SizeDesc,
                                               Ποσότητα = c.D_qty + " Δωδ και "+ c.P_qty + " κάλ"
                                             //  Δωδ = c.D_qty,
                                             //  Κάλ = c.P_qty
                                           };

                ram.Text = "Ραμμένα: " +  (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "00").Sum(x => x.D_qty) + context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "00").Sum(x => x.P_qty)/24).ToString() + " | " + (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "00").Sum(x => x.P_qty) - (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "00").Sum(x => x.P_qty)/24)*24).ToString();
                ar.Text = "Άραφτα: " +  (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "01").Sum(x => x.D_qty) + context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "01").Sum(x => x.P_qty)/24).ToString() + " | " + (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "01").Sum(x => x.P_qty) - (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Linking == "01").Sum(x => x.P_qty)/24)*24).ToString();
                mesys.Text = "Με συσκευαστικά: " +  (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "00").Sum(x => x.D_qty) + context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "00").Sum(x => x.P_qty)/24).ToString() + " | " + (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "00").Sum(x => x.P_qty) - (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "00").Sum(x => x.P_qty)/24)*24).ToString();
                xwrissys.Text = "Χωρίς συσκευαστικά: " +  (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "01").Sum(x => x.D_qty) + context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "01").Sum(x => x.P_qty)/24).ToString() + " | " + (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "01").Sum(x => x.P_qty) - (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa && c.Syskeuastika == "01").Sum(x => x.P_qty)/24)*24).ToString();
                total.Text = "Σύνολο: " + (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa).Sum(x => x.D_qty) + context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa).Sum(x => x.P_qty)/24) + " | " + (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa).Sum(x => x.P_qty) - (context.Semi_finished_warehouse.Where(c => c.shipmentNo == aa).Sum(x => x.P_qty)/24)*24);

                shipmentnobox.Text = aa.ToString();
                warehousebox.Text = context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.truck).FirstOrDefault();
                datecbox.Text = context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.Date).FirstOrDefault().ToShortDateString();
                if (context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.ShipmentDate).FirstOrDefault() == null)
                {
                    dateshippedbox.Text = "";
                }
                else
                {
                    dateshippedbox.Text = DateTime.Parse(context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.ShipmentDate).FirstOrDefault().ToString()).ToShortDateString();
                }
            }
        }

        private void shipment_grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                int index2 = data.IndexOf(",");
                data = data.Substring(index2 +1);
                int index = data.IndexOf("AA = ");
                index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 5, index2 - (index + 5));
                    int id = Convert.ToInt32(temp);
                    aa = id;
                    //  ContextMenu cm = this.FindResource("cmenu") as ContextMenu;
                    // cm.IsOpen = true;
                    ContentGridRefresh();
                }
            }
        }

        private void content_grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
                int index = data.IndexOf("id = ");
                int index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 5, index2 - (index + 5));
                    int iD = Convert.ToInt32(temp);
                    id = iD;


                    index = data.IndexOf("AccessNo = ");
                    accessno = data.Substring(index + 11);
                    index2 = accessno.IndexOf(",");
                    accessno = accessno.Substring(0,index2);

                      ContextMenu cm = this.FindResource("cmenu") as ContextMenu;
                     cm.IsOpen = true;
                   // ContentGridRefresh();
                }
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                var rowToBeDeleted = context.Semi_finished_warehouse.Where(c => c.ID == id).ToList();

                foreach (var rows in rowToBeDeleted)
                {
                    context.Semi_finished_warehouse.DeleteOnSubmit(rows);
                }
                context.SubmitChanges();
            }
            ContentGridRefresh();
        }

        private void button_eisag_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {

                int? max = context.shipment.Max(c => c.shipmentNo);

                if (combo_eisag.Text != "Επιλέξτε τρόπο αποστολής")
                {

                    shipment input = new shipment
                    {
                        Date = DateTime.Now,
                        shipmentNo = max + 1,
                        truck = combo_eisag.Text
                    };
                    context.shipment.InsertOnSubmit(input);
                    context.SubmitChanges();
                    ShipmentGridRefresh();
                }
                else
                {
                    MessageBox.Show("Επιλέξτε τρόπο αποστολής", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (shipmentnobox.Text != "")
            {
                excelExport();
            }
        }

        private void excelExport()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fi = new FileInfo(@"S:\Production\PackingList\Packing_list2.xlsx");


            var browse = new System.Windows.Forms.FolderBrowserDialog();
            browse.SelectedPath = path;
           // browse.SelectedPath = @"S:\Production\PackingList";
            // browse.ShowDialog();

            if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                fi = new FileInfo(browse.SelectedPath + @"\Packing_list2.xlsx");
                // do something with the folder path
                //MessageBox.Show(browse.SelectedPath);
                path = browse.SelectedPath;


                   // if (!File.Exists(@"S:\Production\PackingList\Packing_list.xlsx"))
                    if (!File.Exists(browse.SelectedPath + @"\Packing_list2.xlsx"))
                    {
                        createExcel(fi);
                        modifyExcel(fi);

                    }
                    else
                    {
                        modifyExcel(fi);
                    }

                
            }
        }

        private void modifyExcel(FileInfo fi)
        {

            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                using (var context = new Production18())
                {
                    excelPackage.Workbook.Properties.Modified = DateTime.Now;
                    excelPackage.Workbook.Properties.LastModifiedBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                    string sheetname = context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.ShipmentDate != null ? x.ShipmentDate : DateTime.Now.Date).FirstOrDefault().Value.ToString("dd-MM-yy") + " Αποστολή " + shipmentnobox.Text;

                    ExcelWorksheet anotherWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(c => c.Name == sheetname);
                    ExcelWorksheet inicialsheet = excelPackage.Workbook.Worksheets.FirstOrDefault(c => c.Name == "001");


                    // ExcelWorksheet namedWorksheet = excelPackage.Workbook.Worksheets[sheetname];

                    ExcelWorksheet worksheet;

                    if (anotherWorksheet == null)
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add(context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.ShipmentDate != null ? x.ShipmentDate : DateTime.Now.Date).FirstOrDefault().Value.ToString("dd-MM-yy") + " Αποστολή " + shipmentnobox.Text);
                        var result = MessageBox.Show("Το φύλλο δημιουργήθηκε" + Environment.NewLine + Environment.NewLine + "Θέλετε να ανοίξετε το φάκελο?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if(result == MessageBoxResult.Yes)
                        {
                            Process.Start(@path);
                        }
                        
                    }
                    else
                    {
                        excelPackage.Workbook.Worksheets.Add("002");
                        excelPackage.Workbook.Worksheets.Delete(sheetname);
                        worksheet = excelPackage.Workbook.Worksheets.Add(context.shipment.Where(c => c.shipmentNo == aa).Select(x => x.ShipmentDate).FirstOrDefault().Value.ToString("dd-MM-yy") + " Αποστολή " + shipmentnobox.Text);
                        excelPackage.Workbook.Worksheets.Delete("002");
                        var result = MessageBox.Show("Το φύλλο αντικαταστάθηκε" + Environment.NewLine + Environment.NewLine + "Θέλετε να ανοίξετε το φάκελο?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            Process.Start(@path);
                        }
                    }

                    if (inicialsheet != null)
                    {
                        excelPackage.Workbook.Worksheets.Delete("001");
                    }

                    worksheet.DefaultRowHeight = 16;
                    worksheet.Row(1).Height = 30;

                    worksheet.Column(1).Width = 7;
                    worksheet.Column(2).Width = 14;
                    worksheet.Column(3).Width = 10;
                    worksheet.Column(4).Width = 25;
                    worksheet.Column(5).Width = 10;
                    worksheet.Column(6).Width = 5;
                    worksheet.Column(7).Width = 5;
                    worksheet.Column(8).Width = 5;
                    worksheet.Column(9).Width = 5;
                    worksheet.Column(10).Width = 30;
                    worksheet.Column(11).Width = 15;
                    worksheet.Column(12).Width = 15;
                    worksheet.Column(13).Width = 12;

                    worksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;


                    //header
                    worksheet.Cells[2, 1].Value = "AA";
                    worksheet.Cells[2, 2].Value = "Item Code";
                    worksheet.Cells[2, 3].Value = "Color";
                    worksheet.Cells[2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 4].Value = "ColorName";
                    worksheet.Cells[2, 5].Value = "Size";
                    worksheet.Cells[2, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 6].Value = "Dozen";
                    worksheet.Cells[2, 7].Value = "Socks";
                    worksheet.Cells[2, 8].Value = "Link";
                    worksheet.Cells[2, 9].Value = "Pack";
                    worksheet.Cells[2, 10].Value = "Comments";
                    worksheet.Cells[2, 11].Value = "Customer";
                    worksheet.Cells[2, 12].Value = "Order No";
                    worksheet.Cells[2, 13].Value = "Card AA";

                    int counter = 1;
                    int row = 3;

                    foreach (var c in context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa))
                    {
                        worksheet.Cells[row, 1].Value = counter;
                        worksheet.Cells[row, 2].Value = c.ItemCode;
                        worksheet.Cells[row, 3].Value = c.ColorCode;
                        worksheet.Cells[row, 4].Value = c.ColorGRdesc;
                        worksheet.Cells[row, 5].Value = c.SizeDesc;
                        worksheet.Cells[row, 6].Value = c.D_qty;
                        worksheet.Cells[row, 7].Value = c.P_qty;
                        worksheet.Cells[row, 8].Value = c.Linking == "00" ? "" : "A";
                        worksheet.Cells[row, 9].Value = c.Syskeuastika == "00" ? "" : "Όχι";
                        worksheet.Cells[row, 10].Value = context.DELTIO_FINISH_SUPER1.Where(d => d.COL_ID == c.Col_ID).Select(f => f.SuplierColorDesc).FirstOrDefault();
                        worksheet.Cells[row, 11].Value = context.Finish1.Where(d => d.FINISHAP_ID == c.ItemCode).Select(f => f.COUNTRY).FirstOrDefault();
                        worksheet.Cells[row, 12].Value = context.DELTIO_FINISH_SUPER.Where(k => k.TOTAL_ID == context.DELTIO_FINISH_SUPER1.Where(d => d.COL_ID == c.Col_ID).Select(f => f.TOTAL_ID).FirstOrDefault()).Select(g => g.OrderNo).FirstOrDefault();
                        worksheet.Cells[row, 13].Value = c.AccessNo;

                        row++;
                        counter++;
                    }

                    ExcelRange range = worksheet.Cells[2, 1, row - 1, 13];
                    ExcelTable tab = worksheet.Tables.Add(range, "Packing_" + aa + DateTime.Now.ToString("dd_MM_yy"));
                    tab.TableStyle = TableStyles.Medium2;


                    worksheet.Cells[1, 1, 1, 2].Merge = true;
                    worksheet.Cells[1, 1].Value = "Packing List";
                    worksheet.Cells[1, 1].Style.Font.Size = 20;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;


                    worksheet.Cells[1, 3].Value = aa;
                    worksheet.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 3].Style.Font.Size = 20;
                    worksheet.Cells[1, 3].Style.Font.Bold = true;


                    var totald = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa).Sum(f => f.D_qty) + (context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa).Sum(f => f.P_qty) / 24);
                    var totals = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa).Sum(f => f.P_qty) - ((context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa).Sum(f => f.P_qty) / 24) * 24);
                    var totalp = (totald * 12) + (totals / 2);


                    worksheet.Cells[1, 4, 1, 10].Merge = true;
                    worksheet.Cells[1, 4].Value = "Ημερομηνία αποστολής: " + context.shipment.Where(d => d.shipmentNo == aa).Select(f => f.ShipmentDate != null ? f.ShipmentDate : DateTime.Now.Date).FirstOrDefault().Value.ToString("dd/MM/yy");
                    worksheet.Cells[1, 4].Style.Font.Size = 16;
                    worksheet.Cells[1, 4].Style.Font.Color.SetColor(System.Drawing.Color.Gray);



                    worksheet.Column(15).Width = 26;
                    worksheet.Cells[2, 15].Value = "Σύνολο Ραμμένων";
                    worksheet.Cells[3, 15].Value = "Σύνολο Άραφτων";
                    worksheet.Cells[5, 15].Value = "Σύνολο με συσκευαστικά";
                    worksheet.Cells[6, 15].Value = "Σύνολο χωρίς συσκευαστικά";
                    worksheet.Cells[8, 15].Value = "Γενικό Σύνολο";

                    worksheet.Cells[1, 16].Value = "Δωδ";
                    worksheet.Cells[1, 17].Value = "Καλ";


                    worksheet.Cells[2, 16].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "00").Sum(f => f.D_qty) + (context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "00").Sum(f => f.P_qty) / 24);
                    worksheet.Cells[2, 17].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "00").Sum(f => f.P_qty) - ((context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "00").Sum(f => f.P_qty) / 24) * 24);
                    worksheet.Cells[3, 16].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "01").Sum(f => f.D_qty) + (context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "01").Sum(f => f.P_qty) / 24);
                    worksheet.Cells[3, 17].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "01").Sum(f => f.P_qty) - ((context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Linking == "01").Sum(f => f.P_qty) / 24) * 24);
                    worksheet.Cells[5, 16].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "00").Sum(f => f.D_qty) + (context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "00").Sum(f => f.P_qty) / 24);
                    worksheet.Cells[5, 17].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "00").Sum(f => f.P_qty) - ((context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "00").Sum(f => f.P_qty) / 24) * 24);
                    worksheet.Cells[6, 16].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "01").Sum(f => f.D_qty) + (context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "01").Sum(f => f.P_qty) / 24);
                    worksheet.Cells[6, 17].Value = context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "01").Sum(f => f.P_qty) - ((context.Semi_finished_warehouse.Where(d => d.shipmentNo == aa && d.Syskeuastika == "01").Sum(f => f.P_qty) / 24) * 24);
                    worksheet.Cells[8, 16].Value = totald;
                    worksheet.Cells[8, 17].Value = totals;
                    worksheet.Cells[8, 18].Value = totalp;
                    worksheet.Cells[8, 19].Value = "(Ζεύγη)";

                    //Save your file
                    worksheet.View.TabSelected = true;
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.HorizontalCentered = true;
                    worksheet.PrinterSettings.PrintArea = worksheet.Cells["A:M"];
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;
                    worksheet.PrinterSettings.TopMargin = 1/2M;
                    worksheet.PrinterSettings.LeftMargin = 1/2M;
                    worksheet.PrinterSettings.RightMargin = 1/2M;
                    worksheet.PrinterSettings.BottomMargin = 1/2M;
                    excelPackage.Save();
                }
            }
            
        }

        private static void createExcel(FileInfo fi)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "Andreas";
                excelPackage.Workbook.Properties.Title = "";
                excelPackage.Workbook.Properties.Subject = "Packing list Φορτηγού";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("001");
                excelPackage.SaveAs(fi);
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

            using(var context = new Production18())
            {
                if (barcode.Text != "")
                {
                    if(context.eisagogiParagogis.Any(c => c.barcode == barcode.Text))
                    {
                        prodbox.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == (context.eisagogiParagogis.Where(f => f.barcode == barcode.Text).Select(g => g.Total_Id).FirstOrDefault())).Select(d => d.FINISHAP_ID).FirstOrDefault();
                        colorbox.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == (context.eisagogiParagogis.Where(f => f.barcode == barcode.Text).Select(g => g.Total_Id).FirstOrDefault())).Select(d => d.COLORID).FirstOrDefault() + "." 
                                        + context.DELTIO_FINISH_SUPER1.Where(c => c.TOTAL_ID == (context.eisagogiParagogis.Where(f => f.barcode == barcode.Text).Select(g => g.Total_Id).FirstOrDefault())).Select(d => d.COLOR).FirstOrDefault();
                        sizebox.Text = context.DELTIO_FINISH_SUPER1.Where(c => c.COL_ID == (context.eisagogiParagogis.Where(f => f.barcode == barcode.Text).Select(g => g.Col_Id).FirstOrDefault())).Select(d => d.SIZE).FirstOrDefault();
                        dozenbox.Text = context.eisagogiParagogis.Where(c => c.barcode == barcode.Text).Select(d => d.dozen).FirstOrDefault().ToString();
                        sockbox.Text = context.eisagogiParagogis.Where(c => c.barcode == barcode.Text).Select(d => d.socks).FirstOrDefault().ToString();

                        
                        if(context.Semi_finished_warehouse.Any(c => c.AccessNo == barcode.Text && c.shipmentNo.ToString() == shipmentnobox.Text))
                        {

                            linkedbox.Text = context.Semi_finished_warehouse.Where(c => c.AccessNo == barcode.Text && c.shipmentNo.ToString() == shipmentnobox.Text).Select(d => d.Linking).FirstOrDefault() == "00" ? "ΝΑΙ" : "ΟΧΙ";
                            syskbox.Text = context.Semi_finished_warehouse.Where(c => c.AccessNo == barcode.Text && c.shipmentNo.ToString() == shipmentnobox.Text).Select(d => d.Syskeuastika).FirstOrDefault() == "00" ? "ΝΑΙ" : "ΟΧΙ";
                        }
                        else
                        {
                            linkedbox.Text = "NAI";
                            syskbox.Text = "NAI";
                        }


                    }
                    else
                    {
                        MessageBox.Show("Το δελτίο δεν είναι καταχωρημένο απο τη παραγωγή", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        ResetBoxes();
                    }

                }

            }

        }

        private void ResetBoxes()
        {
            barcode.Text = "";
            linkedbox.Text = "";
            syskbox.Text = "";
            prodbox.Text = "";
            colorbox.Text = "";
            sizebox.Text = "";
            dozenbox.Text = "";
            sockbox.Text = "";
        }

        private void delete_Click_1(object sender, RoutedEventArgs e)
        {
            ResetBoxes();
        }

        private void syskbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(syskbox.Text == "" && barcode.Text != "")
            {
                syskbox.Text = "NAI";
            }
            else
            {
                if (barcode.Text != "")
                {
                    if (syskbox.Text == "01" || syskbox.Text == "ΟΧΙ" || syskbox.Text == "ΟΧΙ")
                    {
                        syskbox.Text = "ΟΧΙ";
                    }
                    else
                    {
                        syskbox.Text = "ΝΑΙ";
                    }
                }
            }
        }

        private void linkedbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (linkedbox.Text == "" && barcode.Text != "")
            {
                linkedbox.Text = "NAI";
            }
            else
            {
                if (barcode.Text != "")
                {
                    if (linkedbox.Text == "01" || linkedbox.Text == "ΟΧΙ" || linkedbox.Text == "ΟΧΙ")
                    {
                        linkedbox.Text = "ΟΧΙ";
                    }
                    else
                    {
                        linkedbox.Text = "ΝΑΙ";
                    }
                }
            }
        }

        private void linkedbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (linkedbox.Text.Contains("01"))
            {
                linkedbox.Text = "ΟΧΙ";
            }
            if (linkedbox.Text == "00")
            {
                linkedbox.Text = "ΝΑΙ";
            }


        }

        private void syskbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (syskbox.Text.Contains("01"))
            {
                syskbox.Text = "ΟΧΙ";
            }
            if (syskbox.Text == "00")
            {
                syskbox.Text = "ΝΑΙ";
            }
        }

        private void linkedbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (barcode.Text == "")
            {
                barcode.Focus();
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (shipmentnobox.Text == "")
            {
                MessageBox.Show("Δεν έχετε επιλέξει αποστολή", "", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetBoxes();
                barcode.Focus();
            }
            else
            {
                using (var context = new Production18())
                {
                    if (context.eisagogiParagogis.Any(c => c.barcode == barcode.Text))
                    {
                        if (context.Semi_finished_warehouse.Where(c => c.shipmentNo.ToString() == shipmentnobox.Text).Any(d => d.AccessNo == barcode.Text))
                        {
                            foreach (var c in context.Semi_finished_warehouse.Where(d => d.AccessNo == barcode.Text && d.shipmentNo.ToString() == shipmentnobox.Text))
                            {
                                c.Linking = linkedbox.Text == "OXI" || linkedbox.Text == "ΟΧΙ" ? "01" : "00";
                                c.Syskeuastika = syskbox.Text == "OXI" || syskbox.Text == "ΟΧΙ" ? "01" : "00";
                            }
                        }
                        else
                        {


                            Semi_finished_warehouse input = new Semi_finished_warehouse
                            {
                                shipmentNo = Convert.ToInt32(shipmentnobox.Text),
                                Date = DateTime.Now,
                                AccessNo = barcode.Text,
                                ItemCode = prodbox.Text,
                                ColorCode = context.DELTIO_FINISH_SUPER1.Where(f => f.COL_ID == context.eisagogiParagogis.Where(c => c.barcode == barcode.Text).Select(d => d.Col_Id).FirstOrDefault()).Select(g => g.COLORID).FirstOrDefault(),
                                ColorGRdesc = context.DELTIO_FINISH_SUPER1.Where(f => f.COL_ID == context.eisagogiParagogis.Where(c => c.barcode == barcode.Text).Select(d => d.Col_Id).FirstOrDefault()).Select(g => g.COLOR).FirstOrDefault(),
                                SizeDesc = sizebox.Text, //context.DELTIO_FINISH_SUPER1.Where(f => f.COL_ID == context.eisagogiParagogis.Where(c => c.barcode == barcode.Text).Select(d => d.Col_Id).FirstOrDefault()).Select(g => g.SIZE).FirstOrDefault(),
                                D_qty = Convert.ToInt32(dozenbox.Text),
                                P_qty = Convert.ToInt32(sockbox.Text),
                                Sequence_No = 0,
                                Col_ID = context.eisagogiParagogis.Where(c => c.barcode == barcode.Text).Select(d => d.Col_Id).FirstOrDefault(),

                                Linking = linkedbox.Text == "OXI" || linkedbox.Text == "ΟΧΙ" ? "01" : "00",
                                Syskeuastika = syskbox.Text == "OXI" || syskbox.Text == "ΟΧΙ" ? "01" : "00"

                            };
                            context.Semi_finished_warehouse.InsertOnSubmit(input);
                        }
                        context.SubmitChanges();
                        ContentGridRefresh();
                        ResetBoxes();
                        barcode.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Το δελτίο δεν είναι περασμένο απο τη παραγωγή", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        ResetBoxes();
                        barcode.Focus();
                    }
                }
            }
        }

        private void shipment_grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
                int index = data.IndexOf("id = ");
                int index2 = data.IndexOf(",");
                if (index > 0)
                {
                    var temp = data.Substring(index + 5, index2 - (index + 5));
                    int iD = Convert.ToInt32(temp);
                    id = iD;

                    ContextMenu cm = this.FindResource("cmenu2") as ContextMenu;
                    cm.IsOpen = true;
                }
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Production18())
            {
                
               // var f = context.shipment.Where(c => c.ID == id).Select(d => d.ShipmentDate).FirstOrDefault().Equals(null);
                if (context.shipment.Where(c => c.ID == id).Select(d => d.ShipmentDate).FirstOrDefault().Equals(null))
                {
                    foreach (var c in context.shipment.Where(d => d.ID == id))
                    {
                        c.ShipmentDate = DateTime.Now;
                    }
                    context.SubmitChanges();
                }
            }
            ShipmentGridRefresh();
        }      
    }
}
