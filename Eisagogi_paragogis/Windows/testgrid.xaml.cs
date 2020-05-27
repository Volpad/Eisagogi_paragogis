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
    /// Interaction logic for testgrid.xaml
    /// </summary>
    /// 

    public partial class testgrid : Window
    {

        private static Microloop microloop= new Microloop();

       
        private static IEnumerable<walk_prd_FullItemLinesList> test = from walk_prd_FullItemLinesList in microloop.walk_prd_FullItemLinesList

                                                                      select walk_prd_FullItemLinesList;
        public testgrid()
        {
            InitializeComponent();

          

            using (var context = new Production18())
            {

                var test = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
                           where walk_prd_FullItemLinesList.ItemCode.Equals("W100") && walk_prd_FullItemLinesList.ESDCreated > DateTime.Parse("21/11/2018") && walk_prd_FullItemLinesList.ESDCreated < DateTime.Parse("22/11/2019") && walk_prd_FullItemLinesList.CompWRHCode == "1"// && walk_prd_FullItemLinesList.EntryTypeDescription.Contains("Stock - Sales (VALUE/QTY)") && !walk_prd_FullItemLinesList.EntryTypeDescription.Contains("return")
                           select walk_prd_FullItemLinesList;

                var test2 = from ItemBalances in context.ItemBalances
                            join wpfll in test
                            on new { ItemBalances.ColorCode, ItemBalances.SizeCode} equals new { wpfll.ColorCode, wpfll.SizeCode } into g
                            orderby ItemBalances.ColorCode
                            where ItemBalances.ItemCode == "w100"
                            select new
                            {
                                ItemBalances.ItemCode,
                                Χρώμα = ItemBalances.ColorCode + "." + ItemBalances.ColorGRdesc,
                                ItemBalances.SizeDesc,
                                Αποθήκη = ItemBalances.Warehouse,
                                Παραγγελίες = ItemBalances.OrderQty,
                                Πωλήσεις = g.Sum(d => (Nullable<int>)d.Quantity),
                            };


                //var test3 = from walk_prd_FullItemLinesList in context.walk_prd_FullItemLinesList
                //           where walk_prd_FullItemLinesList.ItemCode.Contains("W100") && walk_prd_FullItemLinesList.ColorCode == "02"  && walk_prd_FullItemLinesList.ESDCreated > DateTime.Parse("21/11/2019") && walk_prd_FullItemLinesList.ESDCreated < DateTime.Parse("22/11/2019") && walk_prd_FullItemLinesList.CompWRHCode == "1" && walk_prd_FullItemLinesList.EntryTypeDescription.Contains("Stock - Sales (VALUE/QTY)") && !walk_prd_FullItemLinesList.EntryTypeDescription.Contains("return")
                //           select walk_prd_FullItemLinesList;

                grid.IsReadOnly = true;
                grid2.IsReadOnly = true;
                grid.ItemsSource = test2;//.Where(c => c.EntryTypeDescription.Contains("sales") && !c.EntryTypeDescription.Contains("return"));
               // grid2.ItemsSource = test3;
                

            }
        }
    }
}
