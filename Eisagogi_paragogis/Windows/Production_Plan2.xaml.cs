using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Data.Entity;
using System.Printing;

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for Production_Plan2.xaml
    /// </summary>
    /// 


    public partial class Production_Plan2 : Window
    {

        private static Production18 production18 = new Production18();
#pragma warning disable CS0414 // The field 'Production_Plan2.toggleflashingmachines' is assigned but its value is never used
        bool toggleflashingmachines = true;
#pragma warning restore CS0414 // The field 'Production_Plan2.toggleflashingmachines' is assigned but its value is never used
#pragma warning disable CS0169 // The field 'Production_Plan2.loopingTimer' is never used
        private DispatcherTimer loopingTimer;
#pragma warning restore CS0169 // The field 'Production_Plan2.loopingTimer' is never used
#pragma warning disable CS0414 // The field 'Production_Plan2.loop' is assigned but its value is never used
        bool loop = true;
#pragma warning restore CS0414 // The field 'Production_Plan2.loop' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'Production_Plan2.MachineRedLimit' is assigned but its value is never used
        int MachineRedLimit = 21;
#pragma warning restore CS0414 // The field 'Production_Plan2.MachineRedLimit' is assigned but its value is never used
        StackPanel stack = new StackPanel();


        public Production_Plan2()
        {
            InitializeComponent();
        }

        void Production_Plan_Closing(object sender, CancelEventArgs e)
        {


        }

        private void WindowKeyDown(object sender, KeyEventArgs e)

        {

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                //saveprocedure();
            }

            if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)

            {
                Button del = new Button();
                TextBox Finid = new TextBox();
                TextBox Totalidbox = new TextBox();
                TextBox Rest = new TextBox();
                TextBox Mac = new TextBox();
                TextBox id = new TextBox();
                CheckBox Chb = new CheckBox();
#pragma warning disable CS0219 // The variable 'Totalidint' is assigned but its value is never used
                int? Totalidint = 0;
#pragma warning restore CS0219 // The variable 'Totalidint' is assigned but its value is never used
#pragma warning disable CS0168 // The variable 'control' is declared but never used
                string control;
#pragma warning restore CS0168 // The variable 'control' is declared but never used
#pragma warning disable CS0168 // The variable 'changedtype' is declared but never used
                string changedtype;
#pragma warning restore CS0168 // The variable 'changedtype' is declared but never used

            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {

            var grid = sender as DataGrid;

            var test = grid.SelectedValue.ToString();

            int index = test.LastIndexOf("Cid =");
            if (index > 0)
                test = test.Substring(index, index);

            int? colid = Convert.ToInt32(grid.SelectedValue.ToString().Substring(8, 6));


            //var qeisagogi1 = from c in Eisagogioaragogis
            //                 join ct in Semifinished
            //                 on c.barcode equals ct.AccessNo into g
            //                 from ct in g.DefaultIfEmpty()
            //                 join ctc in Shipment
            //                 on ct == null ? null : ct.shipmentNo equals ctc.shipmentNo into f
            //                 from ctc in f.DefaultIfEmpty()
            //                 where c.Total_Id == Static_Variables.prodviewtotalid && c.Col_Id == colid
            //                 select new
            //                 {
            //                     c.AutoNo,
            //                     c.date,
            //                     c.barcode,
            //                     c.Machine,
            //                     c.dozen,
            //                     c.socks,
            //                     c.user,
            //                     shipmentno = ct == null ? null : ct.shipmentNo,
            //                     shipmentdate = ctc == null ? null : ctc.ShipmentDate
            //                 };

            //grid.ItemsSource = qeisagogi1;

        }

        private void Flashing_Machines(object sender, RoutedEventArgs e)
        {
            //StackPanel stack = new StackPanel();
            BrushConverter bc = new BrushConverter();
            stack = (StackPanel)FindName("SM55");
            //if (toggleflashingmachines)
            //{
            //    loopingTimer.Start();
            //    toggleflashingmachines = false;
            //}
            //else
            //{
            //    toggleflashingmachines = true;
            //    loopingTimer.Stop();
            //    foreach (StackPanel stack in listofpanels)
            //    {
            //        stack.Background = (Brush)bc.ConvertFrom("#e0e0e0");
            //    }
            //    listofpanels.Clear();

            //}

        }

        private void loopColours(object sender, EventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            stack = (StackPanel)FindName("");
#pragma warning disable CS0168 // The variable 'machine' is declared but never used
            string machine;
#pragma warning restore CS0168 // The variable 'machine' is declared but never used
#pragma warning disable CS0219 // The variable 'machinecounter' is assigned but its value is never used
            int machinecounter = 0;
#pragma warning restore CS0219 // The variable 'machinecounter' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'suminmachine' is assigned but its value is never used
            int? suminmachine = 0;
#pragma warning restore CS0219 // The variable 'suminmachine' is assigned but its value is never used

            //using (var context = new Production18())
            //{


            //    while (machinecounter < 55)
            //    {
            //        switch (machinecounter)
            //        {
            //            case 0:
            //                machine = "55.M4";
            //                stack = SM55;
            //                break;
            //            case 1:
            //                machine = "56.M6";
            //                stack = SM56;
            //                break;
            //            case 2:
            //                machine = "49.M6";
            //                stack = SM49;
            //                break;
            //            case 3:
            //                machine = "10";
            //                stack = SM10;
            //                break;
            //            case 4:
            //                machine = "09";
            //                stack = SM09;
            //                break;
            //            case 5:
            //                machine = "08";
            //                stack = SM08;
            //                break;
            //            case 6:
            //                machine = "07";
            //                stack = SM07;
            //                break;
            //            case 7:
            //                machine = "06";
            //                stack = SM06;
            //                break;
            //            case 8:
            //                machine = "05";
            //                stack = SM05;
            //                break;
            //            case 9:
            //                machine = "04";
            //                stack = SM04;
            //                break;
            //            case 10:
            //                machine = "03";
            //                stack = SM03;
            //                break;
            //            case 11:
            //                machine = "02";
            //                stack = SM02;
            //                break;
            //            case 12:
            //                machine = "01";
            //                stack = SM01;
            //                break;
            //            case 13:
            //                machine = "29";
            //                stack = SM29;
            //                break;
            //            case 14:
            //                machine = "30";
            //                stack = SM30;
            //                break;
            //            case 15:
            //                machine = "47";
            //                stack = SM47;
            //                break;
            //            case 16:
            //                machine = "48";
            //                stack = SM48;
            //                break;
            //            case 17:
            //                machine = "71";
            //                stack = SM71;
            //                break;
            //            case 18:
            //                machine = "12";
            //                stack = SM12;
            //                break;
            //            case 19:
            //                machine = "13";
            //                stack = SM13;
            //                break;
            //            case 20:
            //                machine = "14";
            //                stack = SM14;
            //                break;
            //            case 21:
            //                machine = "15";
            //                stack = SM15;
            //                break;
            //            case 22:
            //                machine = "16";
            //                stack = SM16;
            //                break;
            //            case 23:
            //                machine = "17";
            //                stack = SM17;
            //                break;
            //            case 24:
            //                machine = "18";
            //                stack = SM18;
            //                break;
            //            case 25:
            //                machine = "11";
            //                stack = SM11;
            //                break;
            //            case 26:
            //                machine = "19";
            //                stack = SM19;
            //                break;
            //            case 27:
            //                machine = "26";
            //                stack = SM26;
            //                break;
            //            case 28:
            //                machine = "52";
            //                stack = SM52;
            //                break;
            //            case 29:
            //                machine = "51";
            //                stack = SM51;
            //                break;
            //            case 30:
            //                machine = "57";
            //                stack = SM57;
            //                break;
            //            case 31:
            //                machine = "67";
            //                stack = SM67;
            //                break;
            //            case 32:
            //                machine = "66";
            //                stack = SM66;
            //                break;
            //            case 33:
            //                machine = "65";
            //                stack = SM65;
            //                break;
            //            case 34:
            //                machine = "64";
            //                stack = SM64;
            //                break;
            //            case 35:
            //                machine = "46";
            //                stack = SM46;
            //                break;
            //            case 36:
            //                machine = "32";
            //                stack = SM32;
            //                break;
            //            case 37:
            //                machine = "59";
            //                stack = SM59;
            //                break;
            //            case 38:
            //                machine = "22";
            //                stack = SM22;
            //                break;
            //            case 39:
            //                machine = "35";
            //                stack = SM35;
            //                break;
            //            case 40:
            //                machine = "28";
            //                stack = SM28;
            //                break;
            //            case 41:
            //                machine = "42";
            //                stack = SM42;
            //                break;
            //            case 42:
            //                machine = "53";
            //                stack = SM53;
            //                break;
            //            case 43:
            //                machine = "54";
            //                stack = SM54;
            //                break;
            //            case 44:
            //                machine = "61";
            //                stack = SM61;
            //                break;
            //            case 45:
            //                machine = "62";
            //                stack = SM62;
            //                break;
            //            case 46:
            //                machine = "68";
            //                stack = SM68;
            //                break;
            //            case 47:
            //                machine = "69";
            //                stack = SM69;
            //                break;
            //            case 48:
            //                machine = "70";
            //                stack = SM70;
            //                break;
            //            case 49:
            //                machine = "33";
            //                stack = SM33;
            //                break;
            //            case 50:
            //                machine = "35";
            //                stack = SM35;
            //                break;
            //            case 51:
            //                machine = "20";
            //                stack = SM20;
            //                break;
            //            case 52:
            //                machine = "21";
            //                stack = SM21;
            //                break;
            //            case 53:
            //                machine = "27";
            //                stack = SM27;
            //                break;
            //            case 54:
            //                machine = "25";
            //                stack = SM25;
            //                break;

            //            default:
            //                throw new Exception("Invalid Column Counter");
            //        }

            //        suminmachine = machineqty.Where(n => n.MachineNo.StartsWith(machine, StringComparison.OrdinalIgnoreCase) && n.queueNo != "deleted").Sum(x => x.Rest);

            //        if (suminmachine < MachineRedLimit)
            //        {
            //            listofpanels.Add(stack);
            //        }
            //        machinecounter++;
            //    }

            //    if (loop)
            //    {
            //        foreach (StackPanel stack in listofpanels)
            //        {
            //            stack.Background = new SolidColorBrush(Colors.Red);
            //        }
            //        loop = false;
            //    }
            //    else
            //    {
            //        foreach (StackPanel stack in listofpanels)
            //        {
            //            stack.Background = (Brush)bc.ConvertFrom("#e0e0e0");
            //        }
            //        loop = true;
            //    }
            //}

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //int? totalmachines = 0;
            //int? prevrestmac = 0;
            //int? restonmac = 0;

            //machineqty = MachineQty.ToList();
            //DateTime lasttime = dailyProduction.OrderByDescending(x => x.id).Select(i => i.time).FirstOrDefault();

            //foreach (Machineqty machineqty in MachineQty.Where(i => i.queueNo != "deleted"))
            //{

            //    int? d = Eisagogioaragogis.Where(i => i.Machine == machineqty.MachineNo.Trim() && i.Total_Id == machineqty.AccessNo && i.date > lasttime).Sum(c => c.dozen * 24);
            //    int? s = Eisagogioaragogis.Where(i => i.Machine == machineqty.MachineNo.Trim() && i.Total_Id == machineqty.AccessNo && i.date > lasttime).Sum(c => c.socks);

            //    if ((d + s) != 0)
            //    {
            //        machineqty.DailyQty = (d + s) / 24;
            //    }
            //}
            //prevrestmac = int.Parse(dailiproduction.OrderByDescending(x => x.id).Select(i => i.restonmac).First().ToString());

            //totalmachines = Eisagogioaragogis.Where(i => i.date > lasttime).Sum(c => c.dozen * 24) + Eisagogioaragogis.Where(i => i.date > lasttime).Sum(c => c.socks);
            //totalmachines = totalmachines / 24;



            //var machineqtyfilter1 = from MachineQty in production18.Machineqty
            //                        where MachineQty.queueNo != "deleted"
            //                        group MachineQty by MachineQty.AccessNo into g
            //                        select new { AccessNo = g.Key, Rest = g.Select(x => x.Rest).FirstOrDefault() };

            //restonmac = machineqtyfilter1.Sum(i => i.Rest);




            //if (totalmachines == 0)
            //{
            //    MessageBox.Show("Δεν έγινε κάποια αλλαγή. \n\nΗ τελευταία ανανέωση έγινε στις " + dailiproduction.OrderByDescending(x => x.id).Select(i => i.time).First() + " \nκαι ήταν " + dailiproduction.OrderByDescending(x => x.id).Select(i => i.dozens).First() + " δωδεκάδες");
            //}
            //else
            //{
            //    MessageBox.Show("Μαζεύτηκαν " + (totalmachines) + " δωδεκάδες απο τις " + dailiproduction.OrderByDescending(x => x.id).Select(i => i.time).First() + "\n\nΤο υπόλοιπο συνολο σε όλες τις μηχανές είναι: " + restonmac + " δωδεκάδες");


            //    DailyProduction refresh = new DailyProduction
            //    {
            //        time = DateTime.Now,
            //        username = userName,
            //        dozens = totalmachines,
            //        restonmac = restonmac,
            //        prevrestmac = prevrestmac
            //    };
            //    production18.DailyProduction.InsertOnSubmit(refresh);

            //    production18.SubmitChanges();

            //    machineqty = MachineQty.ToList();
            //    dailiproduction = dailyProduction.ToList();

            //}
        }
    }


}
