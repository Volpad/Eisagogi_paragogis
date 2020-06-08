﻿using System;
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
using System.Printing;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Runtime.Caching;
using Eisagogi_paragogis.Domain;
using System.Threading;
using System.IO;

namespace Eisagogi_paragogis
{

    public partial class Production_Plan : Window
    {
        private static Production18 production18 = new Production18();
       
        
        private static ChangesToProduction changesToProduction = new ChangesToProduction();

        private static IEnumerable<Machineqty> MachineQty = from Machineqty in production18.Machineqty
                                                            orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                                            where Machineqty.MachineNo != null && Machineqty.queueNo != "deleted"
                                                            select Machineqty;

        private static IEnumerable<DELTIO_FINISH_SUPER1> Deltio_Finish_Super1 = from DELTIO_FINISH_SUPER1 in production18.DELTIO_FINISH_SUPER1
                                                                                orderby DELTIO_FINISH_SUPER1.TOTAL_ID descending
                                                                                select DELTIO_FINISH_SUPER1;

        private static IEnumerable<DELTIO_FINISH_SUPER> Deltio_Finish_Super = from DELTIO_FINISH_SUPER in production18.DELTIO_FINISH_SUPER
                                                                              orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                                                                              select DELTIO_FINISH_SUPER;

        private static IEnumerable<ProductionQTY> productionQTY = from ProductionQTY in production18.ProductionQTY
                                                                  orderby ProductionQTY.TOTAL_ID descending
                                                                  select ProductionQTY;

        private static IEnumerable<eisagogiParagogis> Eisagogioaragogis = from eisagogiParagogis in production18.eisagogiParagogis
                                                                          orderby eisagogiParagogis.Total_Id descending
                                                                          where eisagogiParagogis.date > DateTime.Now.AddMonths(-10) && eisagogiParagogis.barcode != null
                                                                          select eisagogiParagogis;

        private static IEnumerable<DailyProduction> dailyProduction = from DailyProduction in production18.DailyProduction
                                                                      where DailyProduction.username == System.Security.Principal.WindowsIdentity.GetCurrent().Name
                                                                      orderby DailyProduction.id descending
                                                                          select DailyProduction;

        private static IEnumerable<Production_Plan_Changes> production_Plan_Changes = from Production_Plan_Changes in production18.Production_Plan_Changes
                                                                                      where Production_Plan_Changes.date >  DateTime.Now.AddDays(-1)
                                                                                      select Production_Plan_Changes;

        private static IEnumerable<Machineqty> Machinenamecreate = from Machineqty in changesToProduction.Machineqty
                                                                   orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                                                   select Machineqty;
        
        List<Production_Plan_Changes> production_plan_changes = production_Plan_Changes.ToList();
        List<Machineqty> machineqty = MachineQty.ToList();
        List<DELTIO_FINISH_SUPER1> deltio_finish_super1 = Deltio_Finish_Super1.ToList();
        List<DELTIO_FINISH_SUPER> deltio_finish_super = Deltio_Finish_Super.ToList();
        List<eisagogiParagogis> eisagogiparagogis = Eisagogioaragogis.ToList();
        List<Machineqty> machineQty = MachineQty.ToList();
        List<DailyProduction> dailiproduction = dailyProduction.ToList();
        List<Machineqty> machinenamecreate = MachineQty.ToList();

        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        private bool saved = true;
        int txtfontsize = 9;
        Canvas canvas;
        int tempfromTotalIdbox;
        int maxid = 0;
        bool toggleflashingmachines = true;
        private DispatcherTimer loopingTimer;
        bool loop = true;
        int MachineRedLimit = 21;
        StackPanel stack = new StackPanel();
        List<StackPanel> listofpanels = new List<StackPanel>();
        List<TextBox> listofprevioustextboxes = new List<TextBox>();
        int changecounter = 0;
        string ProductForBalance;
        private static string connectionString = "server=SERVER-DC;database=PRODUCTION18;Trusted_Connection=Yes;Integrated Security=false; user=production; password=W4lkPr0duct!0n";
        static System.Random rnd = new System.Random();
        static int random = rnd.Next(1, 100000);
        SqlDependencyEx Check_machineQty = new SqlDependencyEx(connectionString, "Production18", "Machineqty", identity: random);
        //double DaysWorkingPermonth = 21.32; //Υπολογισμός 4 βδομάδες μαζί με Σάββατα
        double DaysWorkingPermonth = 20; //Υπολογισμός 4 βδομάδες ΧΩΡΙΣ Σάββατα

       // static ThreadStart ts = new ThreadStart(Restart_Check_machineQty);
       // Thread backgroundThread = new Thread(ts);


        public Production_Plan()
        {
            InitializeComponent();

            loopingTimer = new DispatcherTimer();
            loopingTimer.Tick += new EventHandler(loopColours);
            loopingTimer.Interval = new TimeSpan(0, 0, 1);
            int rowcounter = 0;
            int margincalc = 0;
            string machine;
            int machinecounter = 0;
            var Machinename = new TextBox();
            var TotalId = new TextBox();
            var Productname = new TextBox();
            var Rest = new TextBox();
            var idbox = new TextBox();
            CheckBox ChbBox = new CheckBox();
            var delbutton = new Button();
            var addbutton = new Button();
            
            //  var MACHINEQTY = MachineQty.ToList();

           // TextBlock textMO = (TextBlock)FindName("totalMO");
           
            //using ( var context = new Production18())
            //{
            //    text = eisagogiparagogis.Where(i => i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / DaysWorkingPermonth) / 24;

            //}
            var  text = eisagogiparagogis.Where(i => i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / DaysWorkingPermonth) / 24;
            //textMO.Text = "MO: " + Decimal.Round(Convert.ToDecimal(text), 1).ToString(); 
            Button updateMo = (Button)FindName("Update");
            updateMo.Content = "MO: " + Decimal.Round(Convert.ToDecimal(text), 1).ToString(); 

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
                    }
                    rowcounter++;

                }
                rowcounter = 0;

                foreach (Machineqty machineqty in machineqty.Where(n => n.MachineNo.StartsWith(machine, StringComparison.OrdinalIgnoreCase)))
                {

                    BrushConverter bc = new BrushConverter();
                    canvas.Background = (Brush)bc.ConvertFrom("#f2f2f2");

                    string finishapid = "";
                    int? rest = 0;

                    Find_finishapid_rest(machineqty, ref finishapid, ref rest);


                    TotalId = create_TotalId_boxes(rowcounter, machinecounter, machineqty);
                    Productname = create_Productname_boxes(rowcounter, machinecounter, TotalId, machineqty);
                    Rest = create_Rest_boxes(rowcounter, machinecounter, rest);
                    idbox = create_idboxes(rowcounter, machinecounter, machineqty);
                    ChbBox = create_Chboxes(rowcounter, machinecounter, machineqty);
                    delbutton = create_delete_buttons(rowcounter, machinecounter);
                    morph_boxes(rowcounter, TotalId, Productname, Rest, delbutton, machineqty, bc);

                    Productname.MouseRightButtonUp += new MouseButtonEventHandler(Pname_rightClick);

                    rowcounter++;
                }
                machinecounter++;
                rowcounter = 0;
            }
            Check_machineQty.TableChanged += (o, e) => toReload(e.Data.Value);
            Check_machineQty.Start();

         //   backgroundThread.Start();

        }

        //private static void Restart_Check_machineQty()
        //{
        //    if (!Check_machineQty.Active)
        //    {
        //        string filePath = @"S:\Production\RestartedLog.txt";
        //        using (StreamWriter writer = new StreamWriter(filePath, true))
        //        {
        //            writer.WriteLine("-----------------------------------------------------------------------------");
        //            writer.WriteLine("Date : " + DateTime.Now.ToString());
        //            writer.WriteLine("User: " + Environment.UserName);
        //            writer.WriteLine();
        //        }
        //        MessageBox.Show("needs to start");
        //        Check_machineQty.Start();
        //    }
        //    Thread.Sleep(5000);
        //    Restart_Check_machineQty();
        //}

        private void toReload(string value)
        {
            string macno;
            string macpos;
            using (var context = new Production18())
            {
                macno = value.Substring(0, 5);
                macpos = context.MachinePosition.Where(c => c.MachineNo.StartsWith(macno)).Select(f => f.MachinePos).FirstOrDefault();

            }

            this.Dispatcher.Invoke(() =>
            {
                reloadMachine(macno, macpos);

            });

        }


        private void Pname_rightClick(object sender, MouseButtonEventArgs e)
        {
            TextBox t = sender as TextBox;

            ProductForBalance = t.Text;
            ContextMenu cm = this.FindResource("cmenu") as ContextMenu;
            cm.IsOpen = true;
            //throw new NotImplementedException();
        }

        private TextBox create_Machinename_boxes(int rowcounter, int machinecounter, Machineqty machineqty)
        {
            TextBox Machinename = (TextBox)FindName("Title" + "M" + machinecounter + "R" + rowcounter);
            Machinename.Text = machineqty.MachineNo.ToUpper() + "  " + machinename(machineqty.MachineNo);
            Machinename.Background = machinecolour(machineqty.MachineNo, Machinename);


            return Machinename;
        }

        private static void morph_boxes(int rowcounter, TextBox TotalId, TextBox Productname, TextBox Rest, Button delbutton, Machineqty machineqty, BrushConverter bc)
        {
            if (rowcounter % 2 == 0) { }
            else
            {
                Productname.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#f2f2f2");
                TotalId.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#f2f2f2");
                Rest.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#f2f2f2");
            }
            if (machineqty.Status == true)
            {
                Productname.Foreground = new SolidColorBrush(Colors.Red);
                TotalId.Foreground = new SolidColorBrush(Colors.Red);
                Rest.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (TotalId.Text == "")
            {
                Productname.Visibility = Visibility.Collapsed;
                TotalId.Visibility = Visibility.Collapsed;
                Rest.Visibility = Visibility.Collapsed;
                delbutton.Visibility = Visibility.Collapsed;
            }
        }

        private Button create_delete_buttons(int rowcounter, int machinecounter)
        {
            Button delbutton = (Button)FindName("del" + "M" + machinecounter + "R" + rowcounter);
            delbutton.Visibility = Visibility.Visible;
            return delbutton;
        }

        private CheckBox create_Chboxes(int rowcounter, int machinecounter, Machineqty machineqty)
        {
            CheckBox ChbBox = (CheckBox)FindName("chb" + "M" + machinecounter + "R" + rowcounter);
            ChbBox.IsChecked = machineqty.Status;
            return ChbBox;
        }

        private TextBox create_idboxes(int rowcounter, int machinecounter, Machineqty machineqty)
        {
            TextBox idbox = (TextBox)FindName("id" + "M" + machinecounter + "R" + rowcounter);
            idbox.Text = machineqty.ID.ToString();
            return idbox;
            
        }

        private TextBox create_Rest_boxes(int rowcounter, int machinecounter, int? rest)
        {
            TextBox Rest = (TextBox)FindName("Rest" + "M" + machinecounter + "R" + rowcounter);
            Rest.Text = rest.ToString();
            if (rest < 0)
            {
                Rest.Foreground = new SolidColorBrush(Colors.Red);
            }
            Rest.Visibility = Visibility.Visible;
            return Rest;
        }

        private TextBox create_Productname_boxes(int rowcounter, int machinecounter, TextBox TotalId, Machineqty machineqty)
        {
            TextBox Productname = (TextBox)FindName("Name" + "M" + machinecounter + "R" + rowcounter);
            if (!String.IsNullOrEmpty(machineqty.AccessNo.ToString()))
            {
                Productname.Text = deltio_finish_super.Where(n => n.TOTAL_ID.Equals(Convert.ToInt32(TotalId.Text))).Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();
            }
            else
            {
                Productname.Text = "";
            }
            Productname.Visibility = Visibility.Visible;
            return Productname;
        }

        private TextBox create_TotalId_boxes(int rowcounter, int machinecounter, Machineqty machineqty)
        {
            TextBox TotalId = (TextBox)FindName("TotalId" + "M" + machinecounter + "R" + rowcounter);
            if (!String.IsNullOrEmpty(machineqty.AccessNo.ToString()))
                TotalId.Text = machineqty.AccessNo.ToString();
            TotalId.Visibility = Visibility.Visible;

      //      using (var context = new Production18())
         //   {
              //  var order = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.OrderNo).FirstOrDefault() : null;
               // var deliveryDate = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.DeliveryDate).FirstOrDefault() : null;
               // var Comments = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.comments).FirstOrDefault() : null;

                var order = deltio_finish_super.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? deltio_finish_super.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.OrderNo).FirstOrDefault() : null;
                var deliveryDate = deltio_finish_super.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? deltio_finish_super.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.DeliveryDate).FirstOrDefault() : null;
                var Comments = deltio_finish_super.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? deltio_finish_super.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.comments).FirstOrDefault() : null;

                if (order != null && deliveryDate != null && Comments != null)
                {
                    TotalId.ToolTip = "Order: " + order + " \n " + "Delivery Date: " + deliveryDate + " \n " + "Comments: " + Comments;
                }
                else if (order == null && deliveryDate != null && Comments != null)
                {
                    TotalId.ToolTip = "Delivery Date: " + deliveryDate + " \n " + "Comments: " + Comments;
                }
                else if (order != null && deliveryDate == null && Comments != null)
                {
                    TotalId.ToolTip = "Order: " + order + " \n " + "Comments: " + Comments;
                }
                else if (order != null && deliveryDate != null && Comments == null)
                {
                    TotalId.ToolTip = "Order: " + order + " \n " + "Delivery Date: " + deliveryDate;
                }
                else if (order == null && deliveryDate == null && Comments != null)
                {
                    TotalId.ToolTip = "Comments: " + Comments;
                }
                else if (order == null && deliveryDate != null && Comments == null)
                {
                    TotalId.ToolTip = "Delivery Date: " + deliveryDate;
                }
                else if (order != null && deliveryDate == null && Comments == null)
                {
                    TotalId.ToolTip = "Order: " + order;
                }
           // }

            return TotalId;
        }

        private void Find_finishapid_rest(Machineqty machineqty, ref string finishapid, ref int? rest)
        {

            int? sumoftsouvalia = 0;

            int? Orderqty = deltio_finish_super1.Where(i => i.TOTAL_ID == machineqty.AccessNo).Sum(x => x.Production);

           // foreach (DELTIO_FINISH_SUPER deltio_finish_super in deltio_finish_super.Where(n => n.TOTAL_ID.Equals(machineqty.AccessNo)))
           //// foreach (var deltio_finish_super in context.DELTIO_FINISH_SUPER.Where(n => n.TOTAL_ID.Equals(machineqty.AccessNo)))
           // {
           //     finishapid = deltio_finish_super.FINISHAP_ID;
           // }

            //foreach (DELTIO_FINISH_SUPER1 deltio_finish_super1 in deltio_finish_super1.Where(n => n.TOTAL_ID.Equals(machineqty.AccessNo)))
            foreach (var deltio_finish_super1 in deltio_finish_super1.Where(n => n.TOTAL_ID.Equals(machineqty.AccessNo)))
            {
                //  var dos = context.eisagogiParagogis.Any(i => i.Col_Id == deltio_finish_super1.COL_ID) ? context.eisagogiParagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.dozen * 24) : 0 ;
                //var soc = context.eisagogiParagogis.Any(i => i.Col_Id == deltio_finish_super1.COL_ID) ? context.eisagogiParagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.socks) : 0;
                sumoftsouvalia = sumoftsouvalia + eisagogiparagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.dozen * 24) + eisagogiparagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.socks);
            }

            rest = (Orderqty - sumoftsouvalia) / 24;
        }

        private int Create_Boxes(int margincalc, int machinecounter, TextBox[] Ref, TextBox txt4, string machine)
        {
            var add = new Button();

            MachineName(machinecounter, Ref, txt4);



            Addbutton(machinecounter, add, machine);


            for (int i = 0; i < 6; i++)
            {
                var txt2 = new TextBox();
                var txt = new TextBox();
                var txt3 = new TextBox();
                var id = new TextBox();
                var Chb = new CheckBox();
                var del = new Button();

                margincalc = 24 + 17 * i;

                TotalIdbox(margincalc, machinecounter, i, txt2);
                ProductNamebox(margincalc, machinecounter, i, txt);
                Restbox(margincalc, machinecounter, i, txt2, txt, txt3);
                Checkbox_Idbox(machinecounter, i, id, Chb);
                Delbutton(margincalc, machinecounter, i, txt2, txt, txt3, del);

                pan2.Name = "S" + canvas.Name;

                pan2.RegisterName(del.Name, del);
                pan2.RegisterName(Chb.Name, Chb);
                pan2.RegisterName(id.Name, id);
                pan2.RegisterName(txt3.Name, txt3);
                pan2.RegisterName(txt.Name, txt);
                pan2.RegisterName(txt2.Name, txt2);
                canvas.Children.Add(txt2);
                canvas.Children.Add(txt);
                canvas.Children.Add(txt3);
                canvas.Children.Add(id);
                canvas.Children.Add(Chb);
                canvas.Children.Add(del);
            }


            pan2.RegisterName(add.Name, add);
            canvas.Children.Add(add);

            pan2.RegisterName(txt4.Name, txt4);
            canvas.Children.Add(txt4);
            return margincalc;
        }

        private void Addbutton(int machinecounter, Button add, string machinename)
        {
            TextBlock addTextblock = new TextBlock();
            TextBox textfinder = new TextBox();
            add.Name = "AddM" + machinecounter;
            add.Width = 132;
            add.Height = 15;
            add.Margin = new Thickness(0, 112, 0, 0);
            add.Click += new RoutedEventHandler(add_click);
            add.Content = addTextblock;
            //addTextblock.Text = "null";
            addTextblock.Margin = new Thickness(90, -4, 0, 0);
            double? text;

                var Machinenamecreate = from Machineqty in changesToProduction.Machineqty
                                        orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                        select Machineqty;
                var Eisagogioaragogis = from eisagogiParagogis in production18.eisagogiParagogis
                                        orderby eisagogiParagogis.Total_Id descending
                                        where eisagogiParagogis.date > DateTime.Now.AddMonths(-10) && eisagogiParagogis.barcode != null
                                        select eisagogiParagogis;


                string machinename2 = Machinenamecreate.Where(i => i.MachineNo.StartsWith(machinename)).Select(k => k.MachineNo).FirstOrDefault().ToString();

                if (Eisagogioaragogis.Where(i => i.Machine == machinename2.TrimEnd()).Equals(null))
                {
                    text = 0;

                }
                else
                {
                text = Eisagogioaragogis.Where(i => i.Machine == machinename2.TrimEnd().ToUpper() && i.date > DateTime.Now.Date.AddDays(-28)).Sum(c => ((c.dozen * 24 + c.socks)) / DaysWorkingPermonth) / 24;
                }

                decimal test2 = Convert.ToDecimal(text);

                if (test2 == 0)
                {
                    addTextblock.Text = null;
                }
                else
                {
                    addTextblock.Text = Decimal.Round(test2, 1).ToString();
                }
            
        }

        private void Delbutton(int margincalc, int machinecounter, int i, TextBox txt2, TextBox txt, TextBox txt3, Button del)
        {
            del.Name = "del" + "M" + machinecounter + "R" + i;
            del.Margin = new Thickness(txt.Width + txt2.Width + txt3.Width, margincalc, 0, 0);
            del.Width = 20;
            del.Height = 15;
            del.Click += new RoutedEventHandler(del_click);
            del.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(del_doubleclick);
            del.Visibility = Visibility.Collapsed;
            del.MouseEnter += new MouseEventHandler(GetMouseEnterEvent);
            del.MouseLeave += new MouseEventHandler(GetMouseEventLeave);
            del.ClearValue(Button.BackgroundProperty);
        }

        private static void Checkbox_Idbox(int machinecounter, int i, TextBox id, CheckBox Chb)
        {
            id.Name = "id" + "M" + machinecounter + "R" + i;
            id.Visibility = Visibility.Collapsed;
            Chb.Name = "chb" + "M" + machinecounter + "R" + i;
            Chb.Visibility = Visibility.Collapsed;
        }

        private void Restbox(int margincalc, int machinecounter, int i, TextBox txt2, TextBox txt, TextBox txt3)
        {
            txt3.Name = "Rest" + "M" + machinecounter + "R" + i;
            txt3.FontSize = txtfontsize ;
            txt3.FontFamily = new FontFamily("Calibri");
            txt3.Margin = new Thickness(txt.Width + txt2.Width, margincalc, 0, 0);
            txt3.Width = 25;
            txt3.Height = 15;
            txt3.TextAlignment = TextAlignment.Center;
            txt3.IsReadOnly = true;
            txt3.Visibility = Visibility.Collapsed;
        }

        private void ProductNamebox(int margincalc, int machinecounter, int i, TextBox txt)
        {
            txt.Name = "Name" + "M" + machinecounter + "R" + i;
            txt.FontSize = txtfontsize;
            txt.FontFamily = new FontFamily("Calibri");
            txt.Margin = new Thickness(0, margincalc, 0, 0);
            txt.Width = 55;
            txt.Height = 15;
            txt.TextAlignment = TextAlignment.Left;
            txt.IsReadOnly = true;
            txt.Visibility = Visibility.Collapsed;

        }

        private void TotalIdbox(int margincalc, int machinecounter, int i, TextBox txt2)
        {
            txt2.Name = "TotalId" + "M" + machinecounter + "R" + i;
            txt2.FontFamily = new FontFamily("Calibri");
            txt2.FontSize = txtfontsize;
            txt2.Margin = new Thickness(55, margincalc, 0, 0);
            txt2.Width = 30;
            txt2.Height = 15;
            txt2.TextAlignment = TextAlignment.Center;
            txt2.IsReadOnly = false;
            txt2.Visibility = Visibility.Collapsed;
            txt2.GotFocus += new RoutedEventHandler(TotalId_Got_Focus);
            txt2.LostFocus += new RoutedEventHandler(TotalId_Lost_Focus);
            txt2.PreviewTextInput += new TextCompositionEventHandler(NumberValidationTextBox);
            txt2.KeyUp += new KeyEventHandler(TotalId_Key_Pressed);
            txt2.MouseDoubleClick += new MouseButtonEventHandler(TotalId_Double_Clicked);
            //txt2.ToolTip = Mouse.GetPosition(txt2).X + " " + Mouse.GetPosition(txt2).Y + " \n " + txt2.TabIndex;

            txt2.TabIndex = i;
            txt2.MaxLength = 5;
        }


        private void GetMouseEventLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            Button s = sender as Button;

            using (var context = new Production18())
            {
                var mqty = from Machineqty in context.Machineqty
                           orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                           where Machineqty.MachineNo != null  && Machineqty.queueNo != "deleted"
                           select Machineqty;


                    string tbname = s.Name.Substring(3, s.Name.Length - 3);
                    TextBox accessnobox = (TextBox)FindName("TotalId" + tbname);

                try
                {


                    int accessno = Convert.ToInt32(accessnobox.Text);


                    foreach (var d in mqty.Where(f => f.AccessNo == accessno))
                    {
                        string name = d.MachineNo;
                        name = name.Substring(0, 2);
                        StackPanel stack = (StackPanel)FindName("SM" + name);
                        stack.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#e0e0e0");
                    }
                }
                catch
                {
                    var ppc = from Production_Plan_Changes in context.Production_Plan_Changes
                              orderby Production_Plan_Changes.id descending
                              where Production_Plan_Changes.user == userName
                              select Production_Plan_Changes;

                    int accessno = Convert.ToInt32(ppc.Select(c => c.Value_set).FirstOrDefault());
                    string dname = ppc.Select(c => c.Machine).FirstOrDefault().Substring(0,2);
                    StackPanel stack1 = (StackPanel)FindName("SM" + dname);
                    stack1.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#e0e0e0");

                    foreach (var d in mqty.Where(f => f.AccessNo == accessno))
                    {
                        string name = d.MachineNo;
                        name = name.Substring(0, 2);
                        StackPanel stack = (StackPanel)FindName("SM" + name);
                        stack.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#e0e0e0");
                    }
                }
                
            }

        }

        private void GetMouseEnterEvent(object sender, MouseEventArgs e)
        {
            Button s = sender as Button;

            using (var context = new Production18())
            {
                var mqty = from Machineqty in context.Machineqty
                           orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                           where Machineqty.MachineNo != null && Machineqty.queueNo != "deleted"
                           select Machineqty;


                    string tbname = s.Name.Substring(3, s.Name.Length - 3);
                    TextBox accessnobox = (TextBox)FindName("TotalId"+tbname);


                    int accessno = Convert.ToInt32(accessnobox.Text);


                    foreach (var d in mqty.Where(f => f.AccessNo == accessno))
                    {
                        string name = d.MachineNo;
                        name = name.Substring(0, 2);
                        StackPanel stack = (StackPanel)FindName("SM" + name);
                        stack.Background = new SolidColorBrush(Colors.LightGreen);
                    }

                
            }

                //throw new NotImplementedException();
        }

        private void TotalId_Double_Clicked(object sender, MouseButtonEventArgs e)
        {
            TextBox totalid = sender as TextBox;
            Static_Variables.prodviewtotalid = Convert.ToInt32(totalid.Text);

            productionView prodview = new productionView();
            
            prodview.Show();
        }

        private void TotalId_Key_Pressed(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {

                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                keyboardFocus.MoveFocus(tRequest);
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                TextBox TotalId = sender as TextBox;
                TotalId.Text = tempfromTotalIdbox.ToString();
            }

        }

        private void TotalId_Got_Focus(object sender, RoutedEventArgs e)
        {
            TextBox TotalId = sender as TextBox;
            tempfromTotalIdbox = Convert.ToInt32(TotalId.Text);

            listofprevioustextboxes.Add(TotalId);

            // throw new NotImplementedException();
        }
        //fixed
        private void TotalId_Lost_Focus(object sender, RoutedEventArgs e)
        {
            Button del = new Button();
            TextBox Finid = new TextBox();
            TextBox Totalid = sender as TextBox;
            TextBox Rest = new TextBox();
            TextBox Mac = new TextBox();
            TextBox id = new TextBox();
            CheckBox Chb = new CheckBox();
            //string QueueNo;

            Find_Textboxes_From_TotalId(Totalid, out Finid,out del, out Rest, out Chb, out Mac,out id);


            IEnumerable<Machineqty> machineqtyfilter = from Machineqty in production18.Machineqty
                                                       where Machineqty.ID == Convert.ToInt32(id.Text)
                                                       select Machineqty;

            if (Convert.ToInt32(Totalid.Text) == tempfromTotalIdbox)
            {

            }
            else
            {
                if (Totalid.Text.Length < 5)
                {
                    Totalid.Text = tempfromTotalIdbox.ToString();
                    SystemSounds.Beep.Play();
                }
                else
                {

                    Production_Plan_Changes input = new Production_Plan_Changes
                    {
                        Value_set = Convert.ToInt32(Totalid.Text),
                        Machine = Mac.Text.Substring(0, 5),
                        user = userName,
                        date = DateTime.Now,
                        control = Totalid.Name,
                        Change_Type = "alter"

                    };
                    production18.Production_Plan_Changes.InsertOnSubmit(input);
                    changecounter++;

                    alter_boxes( Finid, Totalid, Rest, Mac, id, machineqtyfilter);

                    production18.SubmitChanges();
                    production_plan_changes = production_Plan_Changes.ToList();
                    machineqty = MachineQty.ToList();

                }
            }
        }

        private void alter_boxes(TextBox Finid, TextBox Totalid, TextBox Rest, TextBox Mac, TextBox id, IEnumerable<Machineqty> machineqtyfilter)
        {
            string QueueNo = machineqtyfilter.Where(n => n.ID.Equals(Convert.ToInt32(id.Text))).Select(c => c.queueNo).FirstOrDefault().ToString();

            machineqty = machineqtyfilter.Select(c =>
            { c.GetType().GetProperties().Single(x => x.Name.Equals("queueNo")).SetValue(c, "deleted", null); return c; }).ToList();
            //saved = false;
            //Image image = (Image)FindName("savedi");

            int? sumoftsouvalia = 0;
            int? Orderqtyoftotalid = deltio_finish_super1.Where(i => i.TOTAL_ID == Convert.ToInt32(Totalid.Text)).Sum(x => x.Production);
            foreach (DELTIO_FINISH_SUPER1 deltio_finish_super1 in deltio_finish_super1.Where(n => n.TOTAL_ID.Equals(Convert.ToInt32(Totalid.Text))))
            {
                sumoftsouvalia = sumoftsouvalia + eisagogiparagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.dozen * 24) + eisagogiparagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.socks);
            }

            int? restoftotalid = (Orderqtyoftotalid - sumoftsouvalia) / 24;


            Machineqty ord = new Machineqty
            {
                AccessNo = Convert.ToInt32(Totalid.Text),
                MachineNo = Mac.Text.Substring(0, 5),
                Rest = restoftotalid,
                Orderqty = Orderqtyoftotalid,
                Productionqty = sumoftsouvalia,
                queueNo = QueueNo
            };
            production18.Machineqty.InsertOnSubmit(ord);
            //saved = false;

            //image.Visibility = Visibility.Collapsed;

            Finid.Text = deltio_finish_super.Where(n => n.TOTAL_ID.Equals(Convert.ToInt32(Totalid.Text))).Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();

            int? Orderqty = deltio_finish_super1.Where(i => i.TOTAL_ID == Convert.ToInt32(Totalid.Text)).Sum(x => x.Production);
            foreach (DELTIO_FINISH_SUPER1 deltio_finish_super1 in deltio_finish_super1.Where(n => n.TOTAL_ID.Equals(Convert.ToInt32(Totalid.Text))))
            {
                sumoftsouvalia = sumoftsouvalia + eisagogiparagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.dozen * 24) + eisagogiparagogis.Where(i => i.Col_Id == deltio_finish_super1.COL_ID).Sum(x => x.socks);
            }

            int? rest = (Orderqty - sumoftsouvalia) / 24;

                Rest.Text = rest.ToString();

            return;
        }

        private void MachineName(int machinecounter, TextBox[] Ref, TextBox txt4)
        {
            txt4.Name = "Title" + "M" + machinecounter + "R0";
            txt4.FontSize = txtfontsize + 1;
            txt4.FontWeight = FontWeights.Bold;
            txt4.FontFamily = new FontFamily("Calibri");
            txt4.Margin = new Thickness(0, 2, 0, 0);
            txt4.Width = 132;
            txt4.Height = 20;
            txt4.TextAlignment = TextAlignment.Center;
            txt4.IsReadOnly = true;


        }

        //fixed
        private void add_click(object sender, RoutedEventArgs e)
        {
            Button add = sender as Button;
            string name = add.Name;
            string mac = "Title" + name.Remove(0, 3) + "R0";
            int temp = 0;
            TextBox Mac = (TextBox)FindName(mac);
            int checkVisible = 0;
            TextBox Name = new TextBox();
            TextBox TotalId = new TextBox();
            TextBox Rest = new TextBox();
            Button del = new Button();
            TextBox id = new TextBox();
            CheckBox chb = new CheckBox();
            
            int counter = 0;
            using (var context = new Production18())
            {
                counter = machineqty.Count(n => n.MachineNo.StartsWith(Mac.Text.Substring(0, 5), StringComparison.OrdinalIgnoreCase));
            }

            if (counter > 4)
            {
                MessageBox.Show("MachineName is Full");
            }
            else
            {

                DataBox dataBox = new DataBox("Enter Access Number", "");
                if (dataBox.ShowDialog() == true)
                {
                    temp = Convert.ToInt32(dataBox.Answer);
                }

                if (!dataBox.Answer.Equals(null))
                {



                    try
                    {
                        using (var context = new Production18())
                        {
                            var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                                      orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                                      select DELTIO_FINISH_SUPER;

                            var mqty = from Machineqty in context.Machineqty
                                       orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                       where Machineqty.MachineNo != null 
                                       select Machineqty;

                            var check = dfs.Where(n => n.TOTAL_ID.Equals(temp)).Select(c => c.FINISHAP_ID).FirstOrDefault();

                            if (check.Equals(null))
                                throw new Exception("wrong production code");

                            if (maxid == 0)
                            {
                                maxid = mqty.Max(x => x.ID);
                            }

                            maxid++;
                        }


                        while (checkVisible < 6)
                        {
                            switch (checkVisible)
                            {
                                case 0:
                                    Name = (TextBox)FindName("Name" + name.Remove(0, 3) + "R0");
                                    TotalId = (TextBox)FindName("TotalId" + name.Remove(0, 3) + "R0");
                                    Rest = (TextBox)FindName("Rest" + name.Remove(0, 3) + "R0");
                                    del = (Button)FindName("del" + name.Remove(0, 3) + "R0");
                                    chb = (CheckBox)FindName("chb" + name.Remove(0, 3) + "R0");
                                    id = (TextBox)FindName("id" + name.Remove(0, 3) + "R0");
                                    break;
                                case 1:
                                    Name = (TextBox)FindName("Name" + name.Remove(0, 3) + "R1");
                                    TotalId = (TextBox)FindName("TotalId" + name.Remove(0, 3) + "R1");
                                    Rest = (TextBox)FindName("Rest" + name.Remove(0, 3) + "R1");
                                    del = (Button)FindName("del" + name.Remove(0, 3) + "R1");
                                    chb = (CheckBox)FindName("chb" + name.Remove(0, 3) + "R1");
                                    id = (TextBox)FindName("id" + name.Remove(0, 3) + "R1");
                                    break;
                                case 2:
                                    Name = (TextBox)FindName("Name" + name.Remove(0, 3) + "R2");
                                    TotalId = (TextBox)FindName("TotalId" + name.Remove(0, 3) + "R2");
                                    Rest = (TextBox)FindName("Rest" + name.Remove(0, 3) + "R2");
                                    del = (Button)FindName("del" + name.Remove(0, 3) + "R2");
                                    chb = (CheckBox)FindName("chb" + name.Remove(0, 3) + "R2");
                                    id = (TextBox)FindName("id" + name.Remove(0, 3) + "R2");
                                    break;
                                case 3:
                                    Name = (TextBox)FindName("Name" + name.Remove(0, 3) + "R3");
                                    TotalId = (TextBox)FindName("TotalId" + name.Remove(0, 3) + "R3");
                                    Rest = (TextBox)FindName("Rest" + name.Remove(0, 3) + "R3");
                                    del = (Button)FindName("del" + name.Remove(0, 3) + "R3");
                                    chb = (CheckBox)FindName("chb" + name.Remove(0, 3) + "R3");
                                    id = (TextBox)FindName("id" + name.Remove(0, 3) + "R3");
                                    break;
                                case 4:
                                    Name = (TextBox)FindName("Name" + name.Remove(0, 3) + "R4");
                                    TotalId = (TextBox)FindName("TotalId" + name.Remove(0, 3) + "R4");
                                    Rest = (TextBox)FindName("Rest" + name.Remove(0, 3) + "R4");
                                    del = (Button)FindName("del" + name.Remove(0, 3) + "R4");
                                    chb = (CheckBox)FindName("chb" + name.Remove(0, 3) + "R4");
                                    id = (TextBox)FindName("id" + name.Remove(0, 3) + "R4");
                                    break;
                                case 5:
                                    Name = (TextBox)FindName("Name" + name.Remove(0, 3) + "R5");
                                    TotalId = (TextBox)FindName("TotalId" + name.Remove(0, 3) + "R5");
                                    Rest = (TextBox)FindName("Rest" + name.Remove(0, 3) + "R5");
                                    del = (Button)FindName("del" + name.Remove(0, 3) + "R5");
                                    chb = (CheckBox)FindName("chb" + name.Remove(0, 3) + "R5");
                                    id = (TextBox)FindName("id" + name.Remove(0, 3) + "R5");
                                    break;

                                default:
                                    throw new Exception("Invalid Column Counter");
                            }
                            if (Name.Visibility == Visibility.Collapsed)
                            {
                                checkVisible = 6;
                            }
                            else
                            {
                                checkVisible++;
                            }
                        }


                        Name.Visibility = Visibility.Visible;
                        TotalId.Visibility = Visibility.Visible;
                        Rest.Visibility = Visibility.Visible;
                        del.Visibility = Visibility.Visible;

                        using (var context = new Production18())
                        {

                            Production_Plan_Changes input = new Production_Plan_Changes
                            {
                                Value_set = temp,
                                Machine = Mac.Text.Substring(0, 5),
                                user = userName,
                                date = DateTime.Now,
                                control = TotalId.Name,
                                Change_Type = "add"
                            };
                            context.Production_Plan_Changes.InsertOnSubmit(input);

                            var machineqtyfilter1 = from Machineqty in context.Machineqty
                                                    orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                                                    where Machineqty.MachineNo == Mac.Text.Substring(0, 5) && Machineqty.AccessNo > 0 && Machineqty.queueNo != "deleted"
                                                    select Machineqty;

                            foreach (var filter in machineqtyfilter1)
                            {
                                filter.queueNo = (Convert.ToInt32(filter.queueNo) + 1).ToString();

                            }

                            var dfs = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                                      orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                                      select DELTIO_FINISH_SUPER;

                            var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
                                       orderby DELTIO_FINISH_SUPER1.TOTAL_ID descending
                                       select DELTIO_FINISH_SUPER1;

                            Name.Text = dfs.Where(n => n.TOTAL_ID.Equals(temp)).Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();
                            TotalId.Text = temp.ToString();
                            id.Text = maxid.ToString();

                            int? sumoftsouvalia = 0;
                            int? Orderqty = dfs1.Where(i => i.TOTAL_ID == temp).Sum(x => x.Production);

                            foreach (var DFS1 in dfs1.Where(n => n.TOTAL_ID.Equals(temp)))
                            {
                                var dos = context.eisagogiParagogis.Any(i => i.Col_Id == DFS1.COL_ID) ? context.eisagogiParagogis.Where(i => i.Col_Id == DFS1.COL_ID).Sum(x => x.dozen * 24) : 0;
                                var soc = context.eisagogiParagogis.Any(i => i.Col_Id == DFS1.COL_ID) ? context.eisagogiParagogis.Where(i => i.Col_Id == DFS1.COL_ID).Sum(x => x.socks) : 0;
                                sumoftsouvalia = sumoftsouvalia + dos + soc;
                            }

                            int? rest = (Orderqty - sumoftsouvalia) / 24;

                            Rest.Text = rest.ToString();

                            Machineqty newProduction = new Machineqty
                            {
                                AccessNo = temp,
                                MachineNo = Mac.Text.Substring(0, 5),
                                queueNo = "0",
                                Orderqty = (Orderqty / 24),
                                Rest = rest,
                                Productionqty = (sumoftsouvalia / 24),
                                DailyQty = 0,
                                Status = false
                            };

                            context.Machineqty.InsertOnSubmit(newProduction);
                            context.SubmitChanges();

                        }

                    }
                    catch
                    {
                        if (dataBox.Answer != "")
                        {
                            MessageBox.Show("Ο αριθμός παραγωγής " + dataBox.Answer + " δεν υπάρχει");
                        }
                    }

                }
            }
        }
        //fixed
        private void del_doubleclick(object sender, MouseButtonEventArgs e)
        {

            
            Button del = sender as Button;

            TextBox Finid, Totalid, Rest, Mac, id;
            CheckBox Chb;
            Find_Textboxes(del, out Finid, out Totalid, out Rest, out Chb, out Mac, out id);

            int totalid = Convert.ToInt32(Totalid.Text);


            Finid.Visibility = Visibility.Collapsed;
            Totalid.Visibility = Visibility.Collapsed;
            Rest.Visibility = Visibility.Collapsed;
            del.Visibility = Visibility.Collapsed;
            Finid.Foreground = new SolidColorBrush(Colors.Black);
            Totalid.Foreground = new SolidColorBrush(Colors.Black);
            Rest.Foreground = new SolidColorBrush(Colors.Black);
            Totalid.Text = "";
            Finid.Text = "";
            Rest.Text = "";
            Chb.IsChecked = false;

            using (var context = new Production18())
            {
                //context.Machineqty.Load();
                //context.Production_Plan_Changes.Load();


                var production_plan_Changes = from Production_Plan_Changes in context.Production_Plan_Changes
                                              where Production_Plan_Changes.date > DateTime.Now.AddDays(-1)
                                              select Production_Plan_Changes;

                //var mAchineqty = from Machineqty in context.Machineqty
                //                 orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                //                 where Machineqty.MachineNo != null && Machineqty.queueNo != "deleted"
                //                 select Machineqty;

                Production_Plan_Changes input1 = new Production_Plan_Changes
                {
                    Value_set = totalid,
                    Machine = Mac.Text.Substring(0, 5),
                    user = userName,
                    date = DateTime.Now,
                    control = Totalid.Name,
                    Change_Type = "delete"

                };
                context.Production_Plan_Changes.InsertOnSubmit(input1);

                var machineqtyfilter1 = from Machineqty in context.Machineqty
                                       where Machineqty.ID == Convert.ToInt32(id.Text)
                                       select Machineqty;

                machineqty = machineqtyfilter1.AsEnumerable().Select(c =>
                { c.GetType().GetProperties().Single(x => x.Name.Equals("queueNo")).SetValue(c, "deleted", null); return c; }).ToList();



                ChangeSet changeSet = context.GetChangeSet();
                Production_Plan_Changes r = (Production_Plan_Changes)changeSet.Inserts.Last();

                

                //MessageBox.Show(r.ToString());
                context.SubmitChanges();

                production_plan_changes = production_Plan_Changes.ToList();
                machineqty = MachineQty.ToList();

                e.Handled = true;
                reloadMachine(Mac.Text, Mac.Name);
            }



        }

        private void reloadMachine(string text, string mname)
        {
            using (var context = new Production18())
            {

                var test = text.Substring(0, 5);
                var mqty = from Machineqty in context.Machineqty
                           orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                           where Machineqty.MachineNo == text.Substring(0,5) && Machineqty.queueNo != "deleted" 
                           select Machineqty;

                var findname = from DELTIO_FINISH_SUPER in context.DELTIO_FINISH_SUPER
                               select DELTIO_FINISH_SUPER;

                try
                {
                    TextBox Mac = (TextBox)FindName(mname);

                }
                catch (Exception e)
                {
                    MessageBox.Show("exception " + e.Message);
                }

               // TextBox Mac = (TextBox)FindName(mname);
                TextBox Name = new TextBox();
                TextBox TotalId = new TextBox();
                TextBox Rest = new TextBox();
                Button del = new Button();
                TextBox id = new TextBox();
                CheckBox chb = new CheckBox();
                int count = 0;

                mname = mname.Remove(0, 5);
                int index = mname.IndexOf("R");
                mname = mname.Substring(0, index);

                foreach (var mqty2 in mqty)
                {

                    
                    Name = (TextBox)FindName("Name" + mname + "R" + count);
                    TotalId = (TextBox)FindName("TotalId" + mname + "R" + count);
                    Rest = (TextBox)FindName("Rest" + mname + "R" + count);
                    del = (Button)FindName("del" + mname + "R" + count);
                    chb = (CheckBox)FindName("chb" + mname + "R" + count);
                    id = (TextBox)FindName("id" + mname + "R" + count);

                    Name.Text = findname.Where(i => i.TOTAL_ID == mqty2.AccessNo).Select(c => c.FINISHAP_ID).FirstOrDefault().ToString();
                    TotalId.Text = mqty2.AccessNo.ToString();

                    var order = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ?  context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.OrderNo).FirstOrDefault() : null;
                    var deliveryDate = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.DeliveryDate).FirstOrDefault() : null;
                    var Comments = context.DELTIO_FINISH_SUPER.Any(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)) ? context.DELTIO_FINISH_SUPER.Where(c => c.TOTAL_ID == Convert.ToInt32(TotalId.Text)).Select(f => f.comments).FirstOrDefault() : null;

                    if (order != null && deliveryDate != null && Comments != null)
                    {
                        TotalId.ToolTip = "Order: " + order + " \n " + "Delivery Date: " + deliveryDate + " \n " + "Comments: " + Comments; 
                    }
                    else if(order == null && deliveryDate != null && Comments != null)
                    {
                        TotalId.ToolTip = "Delivery Date: " + deliveryDate + " \n " + "Comments: " + Comments;
                    }
                    else if(order != null && deliveryDate == null && Comments != null)
                    {
                        TotalId.ToolTip = "Order: " + order + " \n "  + "Comments: " + Comments;
                    }
                    else if(order != null && deliveryDate != null && Comments == null)
                    {
                        TotalId.ToolTip = "Order: " + order + " \n " + "Delivery Date: " + deliveryDate;
                    }
                    else if(order == null && deliveryDate == null && Comments != null)
                    {
                        TotalId.ToolTip =  "Comments: " + Comments;
                    }
                    else if (order == null && deliveryDate != null && Comments == null)
                    {
                        TotalId.ToolTip = "Delivery Date: " + deliveryDate;
                    }
                    else if (order != null && deliveryDate == null && Comments == null)
                    {
                        TotalId.ToolTip = "Order: " + order;
                    }



                    //TotalId.ToolTip = (TotalId.Text != null || (order.Equals("") && deliveryDate.Equals("") && Comments.Equals(""))) ? 
                    //    (order != null ? order  : null) + (deliveryDate != null ? deliveryDate :null) + (Comments != null ? Comments : null): null;

                    


                    int? sumoftsouvalia = 0;
                    int? rest = 0;
                    int? Orderqty = context.DELTIO_FINISH_SUPER1.Where(i => i.TOTAL_ID == mqty2.AccessNo).Sum(x => x.Production);

                    foreach (var g in context.DELTIO_FINISH_SUPER1.Where(n => n.TOTAL_ID.Equals(mqty2.AccessNo)))
                    {
                        var temp = context.eisagogiParagogis.Any(i => i.Col_Id == g.COL_ID) ? context.eisagogiParagogis.Where(i => i.Col_Id == g.COL_ID).Sum(x => x.dozen * 24) : 0;
                        var temp2 = context.eisagogiParagogis.Any(i => i.Col_Id == g.COL_ID) ? context.eisagogiParagogis.Where(i => i.Col_Id == g.COL_ID).Sum(x => x.socks) : 0;
                        sumoftsouvalia = sumoftsouvalia + temp + temp2;
                    }

                    rest = (Orderqty - sumoftsouvalia) / 24;


                    Rest.Text = rest.ToString();
                    chb.IsChecked = mqty2.Status;
                    id.Text = mqty2.ID.ToString();

                    if (mqty2.Status)
                    {
                        Name.Foreground = new SolidColorBrush(Colors.Red);
                        TotalId.Foreground = new SolidColorBrush(Colors.Red);
                        Rest.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Name.Foreground = new SolidColorBrush(Colors.Black);
                        TotalId.Foreground = new SolidColorBrush(Colors.Black);
                        Rest.Foreground = new SolidColorBrush(Colors.Black);
                    }

                    Name.Visibility = Visibility.Visible;
                    TotalId.Visibility = Visibility.Visible;
                    Rest.Visibility = Visibility.Visible;
                    del.Visibility = Visibility.Visible;
                    
                    count++;
                }
                while (count < 6)
                {
                    Name = (TextBox)FindName("Name" + mname + "R" + count);
                    TotalId = (TextBox)FindName("TotalId" + mname + "R" + count);
                    Rest = (TextBox)FindName("Rest" + mname + "R" + count);
                    del = (Button)FindName("del" + mname + "R" + count);
                    chb = (CheckBox)FindName("chb" + mname + "R" + count);
                    id = (TextBox)FindName("id" + mname + "R" + count);

                    Name.Visibility = Visibility.Collapsed;
                    TotalId.Visibility = Visibility.Collapsed;
                    Rest.Visibility = Visibility.Collapsed;
                    del.Visibility = Visibility.Collapsed;
                    Name.Text = "";
                    Rest.Text = "";
                    TotalId.Text = "";
                    id.Text = "";
                    chb.IsChecked = false;
                    count++;
                }

            }



        }

        //fixed
        private void del_click(object sender, RoutedEventArgs e)
        {

            Button del = sender as Button;
            TextBox Finid, Totalid, Rest, Mac, id;
            CheckBox Chb;
            Find_Textboxes(del, out Finid, out Totalid, out Rest, out Chb, out Mac, out id);

            if (Chb.IsChecked == true)
            {
                Chb.IsChecked = false;
                Finid.Foreground = new SolidColorBrush(Colors.Black);
                Totalid.Foreground = new SolidColorBrush(Colors.Black);
                Rest.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Chb.IsChecked = true;
                Finid.Foreground = new SolidColorBrush(Colors.Red);
                Totalid.Foreground = new SolidColorBrush(Colors.Red);
                Rest.Foreground = new SolidColorBrush(Colors.Red);
            }

            using (var context = new Production18())
            {



                IEnumerable<Machineqty> machineqtyfilter = from Machineqty in context.Machineqty
                                                           where Machineqty.ID == Convert.ToInt32(id.Text)
                                                           select Machineqty;

                machineqty = machineqtyfilter.Select(c =>
                { c.GetType().GetProperties().Single(x => x.Name.Equals("Status")).SetValue(c, Chb.IsChecked, null); return c; })
                        .ToList();

                context.SubmitChanges();
            }
            //saved = false;
            //Image image = (Image)FindName("savedi");
            //image.Visibility = Visibility.Collapsed;
        }

        private void Find_Textboxes(Button del, out TextBox Finid, out TextBox Totalid, out TextBox Rest, out CheckBox Chb, out TextBox Mac, out TextBox id)
        {
            string name = del.Name;
            string finid = "Name" + name.Remove(0, 3);
            string totalid = "TotalId" + name.Remove(0, 3);
            string rest = "Rest" + name.Remove(0, 3);
            string chb = "chb" + name.Remove(0, 3);
            string mac = "Title" + name.Remove(0, 3);
            string ID = "id" + name.Remove(0, 3);
            int index = mac.IndexOf("R");
            if (index > 0)
            {
                mac = mac.Substring(0, index + 1) + "0";
            }



            Finid = (TextBox)pan2.FindName(finid);
            Totalid = (TextBox)FindName(totalid);
            Rest = (TextBox)FindName(rest);
            Chb = (CheckBox)FindName(chb);
            Mac = (TextBox)FindName(mac);
            id = (TextBox)FindName(ID);
        }

        private void Find_Textboxes_From_TotalId(TextBox Totalid, out TextBox Finid, out Button del, out TextBox Rest, out CheckBox Chb, out TextBox Mac, out TextBox id)
        {
            string name = Totalid.Name;
            string finid = "Name" + name.Remove(0, 7);
            string totalid = "TotalId" + name.Remove(0, 7);
            string rest = "Rest" + name.Remove(0, 7);
            string chb = "chb" + name.Remove(0, 7);
            string mac = "Title" + name.Remove(0, 7);
            string ID = "id" + name.Remove(0, 7);
            string Del = "del" + name.Remove(0, 7);
            int index = mac.IndexOf("R");
            if (index > 0)
            {
                mac = mac.Substring(0, index + 1) + "0";
            }



            Finid = (TextBox)pan2.FindName(finid);
            del = (Button)FindName(Del);
            Rest = (TextBox)FindName(rest);
            Chb = (CheckBox)FindName(chb);
            Mac = (TextBox)FindName(mac);
            id = (TextBox)FindName(ID);
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

        private Brush machinecolour(string machineNo,TextBox mname)
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

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            saveprocedure();
        }

        private void saveprocedure()
        {
            if (saved)
            {

            }
            else
            {
                Image image = (Image)FindName("savedi");
                image.Visibility = Visibility.Visible;
               // production18.SubmitChanges();
                saved = true;
            }
        }

        private void Production_Plan_Closing(object sender, CancelEventArgs e)
        {
            //backgroundThread.Abort();
            Check_machineQty.Stop();
            //Check_eisagogiParagogis.Stop();
            // notificationTest.Termination();
            // If not saved, notify user and ask for a response
            if (!this.saved)
            {
                string msg = "Close without saving?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "Data App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    // If user doesn't want to close, cancel closure
                    e.Cancel = true;
                }
            }
            
        }

        private void Production_Plan_Closed(object sender, EventArgs e)
        {

            foreach (Window win in Application.Current.Windows)
            {
                if (win.Tag != null)
                {
                    if (win.Tag.ToString() == "productionView" || win.Tag.ToString() == "mo" || win.Tag.ToString() == "teleiwmena_tsouvalia" || win.Tag.ToString() == "kataxwrisi_deltiwn_paragogis" || win.Tag.ToString() == "eisagogi_imietoimwn" || win.Tag.ToString() == "tsouvali" || win.Tag.ToString() == "eisagogiapoparagogi" || win.Tag.ToString() == "RemainingProductions" || win.Tag.ToString() == "balance" || win.Tag.ToString() == "deltiomixanis" || win.Tag.ToString() == "ektypwsi")
                    {
                        win.Close();
                    }
                }
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Flashing_Machines(object sender, RoutedEventArgs e)
        {
            //StackPanel stack = new StackPanel();
            BrushConverter bc = new BrushConverter();
            stack = (StackPanel)FindName("SM55");
            if (toggleflashingmachines)
            {
                loopingTimer.Start();
                toggleflashingmachines = false;
            }
            else
            {
                toggleflashingmachines = true;
                loopingTimer.Stop();
                foreach (StackPanel stack in listofpanels)
                {
                    stack.Background = (Brush)bc.ConvertFrom("#e0e0e0");
                }
                listofpanels.Clear();

            }

        }

        private void loopColours(object sender, EventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            stack = (StackPanel)FindName("");
            string machine;
            int machinecounter = 0;
            int? suminmachine = 0;

            using (var context = new Production18())
            {
                var mqty = from Machineqty in production18.Machineqty
                           orderby Machineqty.queueNo descending, Machineqty.AccessNo descending, Machineqty.ID
                           where Machineqty.MachineNo != null && Machineqty.queueNo != "deleted" && Machineqty.Status == false
                           select Machineqty;

                while (machinecounter < 55)
                {
                    switch (machinecounter)
                    {
                        case 0:
                            machine = "55.M4";
                            stack = SM55;
                            break;
                        case 1:
                            machine = "56.M6";
                            stack = SM56;
                            break;
                        case 2:
                            machine = "49.M6";
                            stack = SM49;
                            break;
                        case 3:
                            machine = "10";
                            stack = SM10;
                            break;
                        case 4:
                            machine = "09";
                            stack = SM09;
                            break;
                        case 5:
                            machine = "08";
                            stack = SM08;
                            break;
                        case 6:
                            machine = "07";
                            stack = SM07;
                            break;
                        case 7:
                            machine = "06";
                            stack = SM06;
                            break;
                        case 8:
                            machine = "05";
                            stack = SM05;
                            break;
                        case 9:
                            machine = "04";
                            stack = SM04;
                            break;
                        case 10:
                            machine = "03";
                            stack = SM03;
                            break;
                        case 11:
                            machine = "02";
                            stack = SM02;
                            break;
                        case 12:
                            machine = "01";
                            stack = SM01;
                            break;
                        case 13:
                            machine = "29";
                            stack = SM29;
                            break;
                        case 14:
                            machine = "30";
                            stack = SM30;
                            break;
                        case 15:
                            machine = "47";
                            stack = SM47;
                            break;
                        case 16:
                            machine = "48";
                            stack = SM48;
                            break;
                        case 17:
                            machine = "71";
                            stack = SM71;
                            break;
                        case 18:
                            machine = "12";
                            stack = SM12;
                            break;
                        case 19:
                            machine = "13";
                            stack = SM13;
                            break;
                        case 20:
                            machine = "14";
                            stack = SM14;
                            break;
                        case 21:
                            machine = "15";
                            stack = SM15;
                            break;
                        case 22:
                            machine = "16";
                            stack = SM16;
                            break;
                        case 23:
                            machine = "17";
                            stack = SM17;
                            break;
                        case 24:
                            machine = "18";
                            stack = SM18;
                            break;
                        case 25:
                            machine = "11";
                            stack = SM11;
                            break;
                        case 26:
                            machine = "19";
                            stack = SM19;
                            break;
                        case 27:
                            machine = "26";
                            stack = SM26;
                            break;
                        case 28:
                            machine = "52";
                            stack = SM52;
                            break;
                        case 29:
                            machine = "51";
                            stack = SM51;
                            break;
                        case 30:
                            machine = "57";
                            stack = SM57;
                            break;
                        case 31:
                            machine = "67";
                            stack = SM67;
                            break;
                        case 32:
                            machine = "66";
                            stack = SM66;
                            break;
                        case 33:
                            machine = "65";
                            stack = SM65;
                            break;
                        case 34:
                            machine = "64";
                            stack = SM64;
                            break;
                        case 35:
                            machine = "46";
                            stack = SM46;
                            break;
                        case 36:
                            machine = "32";
                            stack = SM32;
                            break;
                        case 37:
                            machine = "59";
                            stack = SM59;
                            break;
                        case 38:
                            machine = "22";
                            stack = SM22;
                            break;
                        case 39:
                            machine = "35";
                            stack = SM35;
                            break;
                        case 40:
                            machine = "28";
                            stack = SM28;
                            break;
                        case 41:
                            machine = "42";
                            stack = SM42;
                            break;
                        case 42:
                            machine = "53";
                            stack = SM53;
                            break;
                        case 43:
                            machine = "54";
                            stack = SM54;
                            break;
                        case 44:
                            machine = "61";
                            stack = SM61;
                            break;
                        case 45:
                            machine = "62";
                            stack = SM62;
                            break;
                        case 46:
                            machine = "68";
                            stack = SM68;
                            break;
                        case 47:
                            machine = "69";
                            stack = SM69;
                            break;
                        case 48:
                            machine = "70";
                            stack = SM70;
                            break;
                        case 49:
                            machine = "33";
                            stack = SM33;
                            break;
                        case 50:
                            machine = "35";
                            stack = SM35;
                            break;
                        case 51:
                            machine = "20";
                            stack = SM20;
                            break;
                        case 52:
                            machine = "21";
                            stack = SM21;
                            break;
                        case 53:
                            machine = "27";
                            stack = SM27;
                            break;
                        case 54:
                            machine = "25";
                            stack = SM25;
                            break;

                        default:
                            throw new Exception("Invalid Column Counter");
                    }

                    
                    
                    suminmachine = mqty.Where(n => n.MachineNo.ToUpper().StartsWith(machine.ToUpper()) && n.queueNo != "deleted").Sum(x => x.Rest);

                    if (suminmachine < MachineRedLimit)
                    {
                        listofpanels.Add(stack);
                    }
                    machinecounter++;
                }

                if (loop)
                {
                    foreach (StackPanel stack in listofpanels)
                    {
                        stack.Background = new SolidColorBrush(Colors.Red);
                    }
                    loop = false;
                }
                else
                {
                    foreach (StackPanel stack in listofpanels)
                    {
                        stack.Background = (Brush)bc.ConvertFrom("#e0e0e0");
                    }
                    loop = true;
                }
            }

        }

        //fixed
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int? totalmachines = 0;
            int? prevrestmac = 0;
            int? restonmac = 0;

            using (var context = new Production18())
            {

                machineqty = MachineQty.ToList();
                DateTime lasttime = context.DailyProduction.OrderByDescending(x => x.id).Select(i => i.time).FirstOrDefault();

                foreach (var x in context.Machineqty.Where(i => i.queueNo != "deleted"))
                {

                    int? d = context.eisagogiParagogis.Any(i => i.Machine == x.MachineNo.Trim() && i.Total_Id == x.AccessNo && i.date > lasttime) ? context.eisagogiParagogis.Where(i => i.Machine == x.MachineNo.Trim() && i.Total_Id == x.AccessNo && i.date > lasttime).Sum(c => c.dozen * 24) : 0;
                    int? s = context.eisagogiParagogis.Any(i => i.Machine == x.MachineNo.Trim() && i.Total_Id == x.AccessNo && i.date > lasttime) ? context.eisagogiParagogis.Where(i => i.Machine == x.MachineNo.Trim() && i.Total_Id == x.AccessNo && i.date > lasttime).Sum(c => c.socks) : 0;

                    if ((d + s) != 0)
                    {
                        x.DailyQty = (d + s) / 24;
                    }
                }
                prevrestmac = int.Parse(context.DailyProduction.OrderByDescending(x => x.id).Select(i => i.restonmac).First().ToString());

                var dos = context.eisagogiParagogis.Any(i => i.date > lasttime) ? context.eisagogiParagogis.Where(i => i.date > lasttime).Sum(c => c.dozen * 24) : 0;
                var soc = context.eisagogiParagogis.Any(i => i.date > lasttime) ? context.eisagogiParagogis.Where(i => i.date > lasttime).Sum(c => c.socks) : 0;

                totalmachines = dos + soc;
                totalmachines = totalmachines / 24;



                var machineqtyfilter1 = from MachineQty in context.Machineqty
                                        where MachineQty.queueNo != "deleted"
                                        group MachineQty by MachineQty.AccessNo into g
                                        select new { AccessNo = g.Key, Rest = g.Select(x => x.Rest).FirstOrDefault() };

                restonmac = machineqtyfilter1.Sum(i => i.Rest);


            }

            if (totalmachines == 0)
            {
                MessageBox.Show("Δεν έγινε κάποια αλλαγή. \n\nΗ τελευταία ανανέωση έγινε στις " + dailiproduction.OrderByDescending(x => x.id).Select(i => i.time).First() + " \nκαι ήταν " + dailiproduction.OrderByDescending(x => x.id).Select(i => i.dozens).First() + " δωδεκάδες");
            }
            else
            {
                MessageBox.Show("Μαζεύτηκαν " + (totalmachines) + " δωδεκάδες απο τις " + dailiproduction.OrderByDescending(x => x.id).Select(i => i.time).First() + "\n\nΤο υπόλοιπο συνολο σε όλες τις μηχανές είναι: " + restonmac + " δωδεκάδες");
                
               
                using (var context = new Production18())
                {
                    DailyProduction refresh = new DailyProduction
                    {
                        time = DateTime.Now,
                        username = userName,
                        dozens = totalmachines,
                        restonmac = restonmac,
                        prevrestmac = prevrestmac
                    };

                    context.DailyProduction.InsertOnSubmit(refresh);
                    context.SubmitChanges();

                }

                //DailyProduction refresh = new DailyProduction
                //{
                //    time = DateTime.Now,
                //    username = userName,
                //    dozens = totalmachines,
                //    restonmac = restonmac,
                //    prevrestmac = prevrestmac
                //};
                //production18.DailyProduction.InsertOnSubmit(refresh);

              //  production18.SubmitChanges();

                //machineqty = MachineQty.ToList();
                //dailiproduction = dailyProduction.ToList();
                    
                //saved = false;
                //Image image = (Image)FindName("savedi");
                //image.Visibility = Visibility.Collapsed;

                //update_boxes();
            }
            
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)

        {

            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TextBox t = (TextBox)FindName("search");
                t.Focus();
            }


            //if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            //{
            //    //saveprocedure();
            //}

            //                if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            //                {
            //                Button del = new Button();
            //                TextBox Finid = new TextBox();
            //                TextBox Totalidbox = new TextBox();
            //                TextBox Rest = new TextBox();
            //                TextBox Mac = new TextBox();
            //                TextBox id = new TextBox();
            //                CheckBox Chb = new CheckBox();
            //#pragma warning disable CS0219 // The variable 'Totalidint' is assigned but its value is never used
            //                int? Totalidint = 0;
            //#pragma warning restore CS0219 // The variable 'Totalidint' is assigned but its value is never used
            //#pragma warning disable CS0168 // The variable 'control' is declared but never used
            //                string control;
            //#pragma warning restore CS0168 // The variable 'control' is declared but never used
            //#pragma warning disable CS0168 // The variable 'changedtype' is declared but never used
            //                string changedtype;
            //#pragma warning restore CS0168 // The variable 'changedtype' is declared but never used
            //                if (changecounter == 0)
            //                {
            //                    SystemSounds.Beep.Play();
            //                    //MessageBox.Show("Ήθελες και Ctrl+z ε??? /nΔεν το έχω ακόμα έτοιμο...");
            //                }
            //                else
            //                {
            //                    MessageBox.Show("Δεν δουλεύει σωστά ακόμα.");
            //                    TextBox lastbox = listofprevioustextboxes[listofprevioustextboxes.Count - changecounter];
            //                    lastbox.Undo();
            //                    listofprevioustextboxes.RemoveAt(listofprevioustextboxes.Count - 1);
            //                    changecounter--;
            //                }
            //                //MessageBox.Show("CTRL + C Pressed!");
            //                }

        }

        private void update_boxes()
        {
            MessageBox.Show("Boxes not updated yet");
            //throw new NotImplementedException();
            PrintDialog printDlg = new PrintDialog();
            //PageMediaSize pageSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
            pan2.Measure(pageSize);
            pan2.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));


            PrintTicket pt = printDlg.PrintTicket;
            pt.PageOrientation = PageOrientation.Landscape;



            //pt.PageMediaSize = pageSize;
            //printDlg.ShowDialog();



            //Double xScale = (printableWidth - 1122.24 * 2) / printableWidth;
            //Double yScale = (printableHeight - 793.92 * 2) / printableHeight;

            //pan2.LayoutTransform = new MatrixTransform(xScale, 0, 0, yScale, 1122.24, 793.92);



            //printDlg.PrintVisual(pan2, "Window Printing");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button s = sender as Button;
            if (s.Name.Equals("walkorders"))
            {
                Static_Variables.isWalk = true;
                Static_Variables.isothersopen = true;
            }
            else
            {
                Static_Variables.isWalk = false;
                Static_Variables.isothersopen = true;
            }

            var f = false;

            foreach (Window win in Application.Current.Windows)
            {
                if (win.Tag != null)
                {
                    if (win.Tag.ToString() == "RemainingProductions")
                    {
                        win.Focus();
                        
                        f = true;
                    }
                }

            }

            NewWindowHandler(this, e);
        }

        private void NewWindowHandler(object sender, RoutedEventArgs e)
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private void ThreadStartingPoint()
        {
            Remaining_Productions remaining_Productions = new Remaining_Productions();
            remaining_Productions.Show();
            System.Windows.Threading.Dispatcher.Run();
        }


        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox search = sender as TextBox;

            foreach (TextBox tb in FindVisualChildren<TextBox>(this).Where(c => c.Visibility == Visibility.Visible && c.Name != search.Name && (c.Name.StartsWith("Name") || c.Name.StartsWith("Tot"))))
            {
                if (tb.Text.ToUpper().Contains(search.Text.ToUpper()) && search.Text != "")
                {
                    tb.Background = new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    BrushConverter bc = new BrushConverter();
                    var name = tb.Name.ToString();
                    int index = name.LastIndexOf("R");
                    var test = name.Substring(index + 1, name.Length - index - 1);
                    var test2 = Convert.ToInt32(test) % 2;

                    if (test2 != 0)
                    {
                        tb.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#f2f2f2");
                    }
                    else
                    {
                        tb.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFF");
                    }
                }
            }
            
            
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

                {

                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)

                    {

                        yield return (T)child;

                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void search_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox s = sender as TextBox;

            if (s.Text == "")
            {
                s.Text = "Search";
            }

            foreach (TextBox tb in FindVisualChildren<TextBox>(this).Where(c => c.Visibility == Visibility.Visible && c.Name != search.Name && (c.Name.StartsWith("Name") || c.Name.StartsWith("Tot"))))
            {

                BrushConverter bc = new BrushConverter();
                var name = tb.Name.ToString();
                int index = name.LastIndexOf("R");
                var test = name.Substring(index + 1, name.Length - index - 1);
                var test2 = Convert.ToInt32(test) % 2;

                if (test2 != 0)
                {
                    tb.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#f2f2f2");
                }
                else
                {
                    tb.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFF");
                }

            }
        }

        private void search_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        
        private void search_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (s.Text == "Search")
            {

                s.Text = "";
            }
            else
            {
                s.SelectAll();
            }
        }

        private void search_GotMouseCapture(object sender, MouseEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (s.Text == "Search")
            {

                s.Text = "";
            }
            else
            {
                s.SelectAll();
            }
        }

        private void CheckBalance_Click(object sender, RoutedEventArgs e)
        {
            Balance balance = new Balance();
            balance.Show();



        }

        private void prod_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ResetFocusArea.Focus();

                using (var context = new Production18())
                {
                    var dfs = from DELTIO_FINISH_SUPER in production18.DELTIO_FINISH_SUPER
                              orderby DELTIO_FINISH_SUPER.TOTAL_ID descending
                              select DELTIO_FINISH_SUPER;

                    var p = dfs.Where(c => c.FINISHAP_ID.Equals(prod.Text)).Select(d => d.TOTAL_ID).FirstOrDefault();

                    if (p != 0)
                    {
                        Static_Variables.prodviewtotalid = p;

                        productionView prodview = new productionView();

                        prodview.Show();

                    }
                    else
                    {


                        if (prod.Text.Equals("Product") || prod.Text.Equals(""))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Wrong Product Ref");
                        }
                    }

                }
            }
            
            if (e.Key == Key.Escape)
            {
                prod.Text = "Product";
                ResetFocusArea.Focus();
            }

        }

        private void prod_GotMouseCapture(object sender, MouseEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (s.Text == "Product")
            {

                s.Text = "";
            }
            else
            {
                if (s.SelectionLength > 0)
                {
                    //s.Select(0, 0);
                    s.SelectionLength = 0;
                }
                else
                {
                    s.SelectAll();
                }
            }

        }

        private void prod_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
                        TextBox s = sender as TextBox;
            if (s.Text == "Product")
            {

                s.Text = "";
            }
            else
            {
                if (s.SelectionLength > 0)
                {
                    //s.Select(0, 0);
                    s.SelectionLength = 0;
                }
                else
                {
                    s.SelectAll();
                }
            }

        }

        private void prod_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox s = sender as TextBox;

            if (s.Text == "")
            {
                s.Text = "Product";
            }
            else
            {

            }

        }

        private void karta_Click(object sender, RoutedEventArgs e)
        {
            KartaProiontos karta = new KartaProiontos();
            karta.Show();
        }

        private void Balance_Click(object sender, RoutedEventArgs e)
        {
            Static_Variables.finishid = ProductForBalance;

            Window balance = new Balance();
            balance.Show();

        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            Static_Variables.finishid = "w100";

            Window testgrid = new testgrid();
            testgrid.Show();

        }

        private void eisagogi_Click(object sender, RoutedEventArgs e)
        {
            Window eisagogi = new Eisagogi_Paragogis();
            eisagogi.Show();

        }

        private void DeltioM_Click(object sender, RoutedEventArgs e)
        {
            var f = false;

            foreach (Window win in Application.Current.Windows)
            {
                if (win.Tag != null)
                {
                    if (win.Tag.ToString() == "ektypwsi")
                    {
                        win.Focus();
                        f = true;
                    }
                }

            }
            if (!f)
            {
                Window print = new forma_ektyposis();
                print.Show();
            }


            //Static_Variables.totalid = 15783;
            //Window deltiom = new DeltioMixanis();
            ////  deltiom.Show();

            //using (var context = new Production18())
            //{
            //    var dfs1 = from DELTIO_FINISH_SUPER1 in context.DELTIO_FINISH_SUPER1
            //               where DELTIO_FINISH_SUPER1.TOTAL_ID == Static_Variables.totalid
            //               orderby DELTIO_FINISH_SUPER1.QueueNo, DELTIO_FINISH_SUPER1.SIZESORTING
            //               select DELTIO_FINISH_SUPER1;

            //    foreach (var c in dfs1)
            //    {
            //        Static_Variables.colourid = c.COL_ID;
            //        int? prodcounter = c.Production;
            //        int sequence = Convert.ToInt32(c.sequence);

            //        while (prodcounter > 0)
            //        {
            //            Static_Variables.sequence = sequence + 1;
            //            Window kp = new KartaProiontos();
            //            prodcounter = prodcounter - 500;
            //            sequence++;
            //        }

            //        c.sequence = sequence.ToString();

            //    }
            //    context.SubmitChanges();
            //}
        }

        private void printers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
            Static_Variables.printer = (sender as ComboBox).SelectedItem as string;
        }

        private void eisagogi2_Click(object sender, RoutedEventArgs e)
        {
            Window eisagogi2 = new Eisagogi_apo_paragogi();
            eisagogi2.Show();
        }

        private void Hmietoima_Click(object sender, RoutedEventArgs e)
        {
            Window Imietoima = new Eisagogi_Imietoimwn();
            Imietoima.Show();

        }

        private void deltiaMixanis_Click(object sender, RoutedEventArgs e)
        {
            Window deltia = new Kataxwrisi_Deltiwn_Paragogis();
            deltia.Show();

        }

        private void teleiwmena_tsouvalia_Click(object sender, RoutedEventArgs e)
        {
            Window tel_tsouv = new Teleiwmena_tsouvalia();
            tel_tsouv.Show();
        }

        private void MO_calc_Click(object sender, RoutedEventArgs e)
        {
            Window mo = new MO_Paragogis();
            mo.Show();
        }
    }
}




// Adding new object to db

//Machineqty ord = new Machineqty
//{
//    AccessNo = temp,
//    MachineNo = Mac.Text.Substring(0, 5),
//    queueNo = "5"
// };
//production18.Machineqty.InsertOnSubmit(ord);       
//saved = false;