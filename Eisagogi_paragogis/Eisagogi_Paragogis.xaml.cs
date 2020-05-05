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
    /// Interaction logic for Eisagogi_Paragogis.xaml
    /// </summary>
    public partial class Eisagogi_Paragogis : Window
    {

        private static Production18 production18 = new Production18();
        private static ChangesToProduction changesToProduction = new ChangesToProduction();

        private static IEnumerable<Machineqty> MachineQty = from Machineqty in production18.Machineqty
                                                            orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                                            where Machineqty.MachineNo != null && Machineqty.queueNo != "deleted"
                                                            select Machineqty;

        private static IEnumerable<Machineqty> Machinenamecreate = from Machineqty in changesToProduction.Machineqty
                                                                   orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                                                   select Machineqty;

        List<Machineqty> machinenamecreate = MachineQty.ToList();
        List<Machineqty> machineqty = MachineQty.ToList();
        Canvas canvas;
        int txtfontsize = 9;

        public Eisagogi_Paragogis()
        {
            InitializeComponent();

            int rowcounter = 0;
            int machinecounter = 0;
            string machine;
            int margincalc = 0;
            var Machinename = new TextBox();

            while (machinecounter < 55)
            {
                TextBox[] Ref = new TextBox[100];
                var txt4 = new TextBox();

                switch (machinecounter)
                {
                    case 0:
                        machine = "55.M4";
                        canvas = M55;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 1:
                        machine = "56.M6";
                        canvas = M56;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 2:
                        machine = "49.M6";
                        canvas = M49;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 3:
                        machine = "10";
                        canvas = M10;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 4:
                        machine = "09";
                        canvas = M09;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 5:
                        machine = "08";
                        canvas = M08;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 6:
                        machine = "07";
                        canvas = M07;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 7:
                        machine = "06";
                        canvas = M06;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 8:
                        machine = "05";
                        canvas = M05;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 9:
                        machine = "04";
                        canvas = M04;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 10:
                        machine = "03";
                        canvas = M03;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 11:
                        machine = "02";
                        canvas = M02;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 12:
                        machine = "01";
                        canvas = M01;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 13:
                        machine = "29";
                        canvas = M29;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 14:
                        machine = "30";
                        canvas = M30;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 15:
                        machine = "47";
                        canvas = M47;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 16:
                        machine = "48";
                        canvas = M48;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 17:
                        machine = "71";
                        canvas = M71;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 18:
                        machine = "12";
                        canvas = M12;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 19:
                        machine = "13";
                        canvas = M13;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 20:
                        machine = "14";
                        canvas = M14;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 21:
                        machine = "15";
                        canvas = M15;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 22:
                        machine = "16";
                        canvas = M16;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 23:
                        machine = "17";
                        canvas = M17;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 24:
                        machine = "18";
                        canvas = M18;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 25:
                        machine = "11";
                        canvas = M11;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 26:
                        machine = "19";
                        canvas = M19;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 27:
                        machine = "26";
                        canvas = M26;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 28:
                        machine = "52";
                        canvas = M52;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 29:
                        machine = "51";
                        canvas = M51;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 30:
                        machine = "57";
                        canvas = M57;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 31:
                        machine = "67";
                        canvas = M67;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 32:
                        machine = "66";
                        canvas = M66;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 33:
                        machine = "65";
                        canvas = M65;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 34:
                        machine = "64";
                        canvas = M64;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 35:
                        machine = "46";
                        canvas = M46;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 36:
                        machine = "32";
                        canvas = M32;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 37:
                        machine = "59";
                        canvas = M59;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 38:
                        machine = "22";
                        canvas = M22;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 39:
                        machine = "35";
                        canvas = M35;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 40:
                        machine = "28";
                        canvas = M28;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 41:
                        machine = "42";
                        canvas = M42;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 42:
                        machine = "53";
                        canvas = M53;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 43:
                        machine = "54";
                        canvas = M54;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 44:
                        machine = "61";
                        canvas = M61;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 45:
                        machine = "62";
                        canvas = M62;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 46:
                        machine = "68";
                        canvas = M68;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 47:
                        machine = "69";
                        canvas = M69;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 48:
                        machine = "70";
                        canvas = M70;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 49:
                        machine = "33";
                        canvas = M33;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 50:
                        machine = "34";
                        canvas = M34;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 51:
                        machine = "20";
                        canvas = M20;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 52:
                        machine = "21";
                        canvas = M21;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 53:
                        machine = "27";
                        canvas = M27;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;
                    case 54:
                        machine = "25";
                        canvas = M25;
                        margincalc = Create_Boxes(margincalc, machinecounter, Ref, txt4, machine);
                        break;

                    default:
                        throw new Exception("Invalid Column Counter");
                }
                canvas.UpdateLayout();

                foreach (Machineqty machinenamecreate in Machinenamecreate.Where(n => n.MachineNo.StartsWith(machine, StringComparison.OrdinalIgnoreCase)))
                {
                    if (rowcounter == 0)
                    {
                        Machinename = create_Machinename_boxes(rowcounter, machinecounter, machinenamecreate);
                        CreateGrid(rowcounter, machinecounter, Machinename.Text);
                    }
                    rowcounter++;

                }
                rowcounter = 0;

                foreach (Machineqty machineqty in machineqty.Where(n => n.MachineNo.StartsWith(machine, StringComparison.OrdinalIgnoreCase)))
                {

                    BrushConverter bc = new BrushConverter();
                    canvas.Background = (Brush)bc.ConvertFrom("#f2f2f2");

                    //string finishapid = "";
                    //int? rest = 0;

                    //Find_finishapid_rest(machineqty, ref finishapid, ref rest);


                    //TotalId = create_TotalId_boxes(rowcounter, machinecounter, machineqty);
                    //Productname = create_Productname_boxes(rowcounter, machinecounter, TotalId, machineqty);
                    //Rest = create_Rest_boxes(rowcounter, machinecounter, rest);
                    //idbox = create_idboxes(rowcounter, machinecounter, machineqty);
                    //ChbBox = create_Chboxes(rowcounter, machinecounter, machineqty);
                    //delbutton = create_delete_buttons(rowcounter, machinecounter);
                    //morph_boxes(rowcounter, TotalId, Productname, Rest, delbutton, machineqty, bc);

                    //Productname.MouseRightButtonUp += new MouseButtonEventHandler(Pname_rightClick);

                    rowcounter++;
                }
                machinecounter++;
                rowcounter = 0;
            }
        }



        private TextBox create_Machinename_boxes(int rowcounter, int machinecounter, Machineqty machineqty)
        {
            TextBox Machinename = (TextBox)FindName("Title" + "M" + machinecounter + "R" + rowcounter);
            Machinename.Text = machineqty.MachineNo.ToUpper();// + "  " + machinename(machineqty.MachineNo);
            Machinename.Background = machinecolour(machineqty.MachineNo, Machinename);
            return Machinename;
        }

        private object machinename(string machineNo)
        {
            machineNo = machineNo.TrimEnd();
            machineNo = machineNo.Substring(machineNo.Length - 2).ToUpper();
            switch (machineNo)
            {
                case "M4":
                    return "Matec 240";
                case "M6":
                    return "Matec 168";
                case "L2":
                    return "Lonati 220";
                case "L4":
                    return "Lonati 168";
                case "L3":
                    return "Lonati 220K";
                case "S2":
                    return "SanGiacomo 168";
                case "M5":
                    return "Matec 200";
                case "L1":
                    return "Lonati 240";
                case "S1":
                    return "SanGiacomo 216";
                case "K3":
                    return "Komet 176 scp";
                case "L9":
                    return "Lonati 132";
                case "L8":
                    return "Lonati 132";
                case "B1":
                    return "Busi 240";
                case "B2":
                    return "Busi 220";
                case "K6":
                    return "Komet 76 BR";
                case "B4":
                    return "Busi 168";
                case "L6":
                    return "Lonati 160";
                case "K5":
                    return "Komet 124 HCS";
                case "K4":
                    return "Komet 136 HCS";
                default:
                    return "Wrong input";
            }

        }
        private Brush machinecolour(string machineNo, TextBox mname)
        {
            BrushConverter bc = new BrushConverter();
            machineNo = machineNo.TrimEnd();
            machineNo = machineNo.Substring(machineNo.Length - 2).ToUpper();
            switch (machineNo)
            {
                case "M4":
                    mname.Foreground = new SolidColorBrush(Colors.Black);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#b7d8a1");
                case "M6":
                    mname.Foreground = new SolidColorBrush(Colors.Black);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#85bd5f");
                case "L2":
                    mname.Foreground = new SolidColorBrush(Colors.DarkRed);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#51b3d2");
                case "L4":
                    mname.Foreground = new SolidColorBrush(Colors.DarkRed);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#0498c1");
                case "L3":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#027291");
                case "S2":
                    mname.Foreground = new SolidColorBrush(Colors.Black);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#a6a6a6");
                case "M5":
                    mname.Foreground = new SolidColorBrush(Colors.DarkRed);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#76b54c");
                case "L1":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#03729e");
                case "S1":
                    mname.Foreground = new SolidColorBrush(Colors.Black);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#a6a6a6");
                case "K3":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#84aa91");
                case "L9":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#7cc0da");
                case "B1":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#4b889a");
                case "B2":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#4eaec7");
                case "K6":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#84aa91");
                case "B4":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#735183");
                case "L6":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#366988");
                case "K5":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#84aa91");
                case "K4":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#84aa91");
                case "L8":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#67b5cc");
                case "K1":
                    mname.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#84aa91");
                default:
                    mname.Foreground = new SolidColorBrush(Colors.Black);
                    return (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFF");
            }
        }

        private int Create_Boxes(int margincalc, int machinecounter, TextBox[] Ref, TextBox txt4, string machine)
        {
           // var add = new Button();

            MachineName(machinecounter, Ref, txt4);
            



            //Addbutton(machinecounter, add, machine);


            //for (int i = 0; i < 6; i++)
            //{
            //    var txt2 = new TextBox();
            //    var txt = new TextBox();
            //    var txt3 = new TextBox();
            //    var id = new TextBox();
            //    var Chb = new CheckBox();
            //    var del = new Button();

            //    margincalc = 24 + 17 * i;

            //    TotalIdbox(margincalc, machinecounter, i, txt2);
            //    ProductNamebox(margincalc, machinecounter, i, txt);
            //    Restbox(margincalc, machinecounter, i, txt2, txt, txt3);
            //    Checkbox_Idbox(machinecounter, i, id, Chb);
            //    Delbutton(margincalc, machinecounter, i, txt2, txt, txt3, del);

            //    pan2.Name = "S" + canvas.Name;

            //    pan2.RegisterName(del.Name, del);
            //    pan2.RegisterName(Chb.Name, Chb);
            //    pan2.RegisterName(id.Name, id);
            //    pan2.RegisterName(txt3.Name, txt3);
            //    pan2.RegisterName(txt.Name, txt);
            //    pan2.RegisterName(txt2.Name, txt2);
            //    canvas.Children.Add(txt2);
            //    canvas.Children.Add(txt);
            //    canvas.Children.Add(txt3);
            //    canvas.Children.Add(id);
            //    canvas.Children.Add(Chb);
            //    canvas.Children.Add(del);
            //}


            //pan2.RegisterName(add.Name, add);
            //canvas.Children.Add(add);

            pan2.RegisterName(txt4.Name, txt4);
            canvas.Children.Add(txt4);


            return margincalc;
        }

        private void MachineName(int machinecounter, TextBox[] Ref, TextBox txt4)
        {
            txt4.Name = "Title" + "M" + machinecounter + "R0";
            txt4.FontSize = txtfontsize + 6;
            txt4.FontWeight = FontWeights.Bold;
            txt4.FontFamily = new FontFamily("Calibri");
            txt4.Margin = new Thickness(0, 2, 0, 0);
            txt4.Width = 132;
            txt4.Height = 20;
            txt4.TextAlignment = TextAlignment.Center;
            txt4.IsReadOnly = true;
        }

        private void CreateGrid(int rowcounter, int machinecounter, string text)
        {
           // var test = "D" + text.Substring(0, 2);
            DataGrid grid = (DataGrid)FindName("D" + text.Substring(0, 2));

            using (var context = new Production18())
            {
                //var mqty = from Machineqty in context.Machineqty
                //           orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                //           where Machineqty.MachineNo.StartsWith(text.Substring(0, 2)) && Machineqty.queueNo != "deleted" && Machineqty.Status == false
                //           select Machineqty;

                var findname = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                               select DELTIO_FINISH_SUPER;

                var griddata = from Machineqty in context.Machineqty
                               join name in findname
                               on Machineqty.AccessNo equals name.TOTAL_ID into g
                               orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                               where Machineqty.MachineNo.StartsWith(text.Substring(0, 2)) && Machineqty.queueNo != "deleted" && Machineqty.Status == false
                               select new
                               {
                                   Name = g.Select(c => c.FINISHAP_ID).FirstOrDefault(),
                                   TotalId = Machineqty.AccessNo.ToString(),
                                   Rest = Machineqty.Rest.ToString()
                               };


                grid.ItemsSource = griddata;
                grid.HeadersVisibility = DataGridHeadersVisibility.None;
                grid.IsReadOnly = true;
                //grid.FontSize = 10;
                grid.FontFamily = new FontFamily("Arial");
                grid.Foreground = Brushes.DarkSlateGray;
                grid.GridLinesVisibility = DataGridGridLinesVisibility.None;
                grid.BorderThickness = new Thickness(0);
                grid.Width = 131;

                if (griddata.Count() == 0)
                {
                    grid.Visibility = Visibility.Collapsed;
                }


                    grid.Columns[0].Width = 68;
                    grid.Columns[1].Width = 37;
                    grid.Columns[2].Width = 25;


            }

            //throw new NotImplementedException();
        }


        private void Grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;

            try
            {
                int index = grid.SelectedValue.ToString().IndexOf("TotalId = ") + 10;
                var totalid = grid.SelectedValue.ToString().Substring(index, 5);
                var machinename = grid.Name.Substring(1);
                Static_Variables.kattotalid = Convert.ToInt32(totalid);
                Static_Variables.katmachineno = machinename;

            }
            catch
            {

            }
            Window kattsouv = new KataxwrisiTsouvaliou();
            kattsouv.Show();

        }

    }
}
