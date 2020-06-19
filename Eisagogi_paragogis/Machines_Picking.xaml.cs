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
    /// Interaction logic for Machines_Picking.xaml
    /// </summary>
    public partial class Machines_Picking : Window
    {

        string finishid = Static_Variables.finishid;

        public Machines_Picking()
        {
            InitializeComponent();

            initialize_boxes();

        }

        private void initialize_boxes()
        {
            using (var context = new Production18())
            {
                var i = 1;
                var test = context.EXOPLISMOS1.OrderBy(f => f.MHXANH).Where(g => g.Available == true).GroupBy(d => d.MHXANH);
                foreach (var c in context.EXOPLISMOS1.OrderBy(f => f.MHXANH).Where(g => g.Available == true).GroupBy(d => d.MHXANH))
                {
                    CheckBox cb = (CheckBox)FindName("cb" + i);
                    TextBox tb = (TextBox)FindName("tb" + i);

                    cb.Visibility = Visibility.Visible;
                    tb.Visibility = Visibility.Visible;

                    if (context.GM.Where(d => d.FINISHAP_ID == finishid).Any(f => f.MHXANH == c.Key))
                    {
                        cb.IsChecked = true;
                    }
                    else
                    {
                        cb.IsChecked = false;
                    }
                    tb.Text = c.Key;
                    i++;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            using (var context = new Production18())
            {
                while (i < context.EXOPLISMOS1.OrderBy(f => f.MHXANH).Where(g => g.Available == true).GroupBy(d => d.MHXANH).Count())
                {
                    CheckBox cb = (CheckBox)FindName("cb" + i);
                    TextBox tb = (TextBox)FindName("tb" + i);

                    if (cb.IsChecked == true)
                    {
                        if(!context.GM.Where(c => c.FINISHAP_ID == finishid).Any(d => d.MHXANH == tb.Text))
                        {
                            GM input = new GM
                            {
                                FINISHAP_ID = finishid,
                                MHXANH = tb.Text
                            };
                            context.GM.InsertOnSubmit(input);

                            Product_spec_changes inputp = new Product_spec_changes
                            {
                                user_modified = Environment.UserName,
                                date_modified = DateTime.Now,
                                specification_changed = "Machine",
                                old_value = "Machine Added",
                                new_value = tb.Text,
                                ProductName = finishid
                            };
                            context.Product_Spec_Changes.InsertOnSubmit(inputp);
                            Static_Variables.ProductMachinesChanged = true;
                        }
                    }
                    else
                    {
                        if (context.GM.Where(c => c.FINISHAP_ID == finishid).Any(d => d.MHXANH == tb.Text))
                        {
                            var items = context.GM.Where(c => c.FINISHAP_ID == finishid && c.MHXANH == tb.Text).ToList();

                            foreach (var item in items)
                            {
                                context.GM.DeleteOnSubmit(item);

                                Product_spec_changes inputp = new Product_spec_changes
                                {
                                    user_modified = Environment.UserName,
                                    date_modified = DateTime.Now,
                                    specification_changed = "Machine",
                                    old_value = "Machine Removed",
                                    new_value = tb.Text,
                                    ProductName = finishid
                                };
                                context.Product_Spec_Changes.InsertOnSubmit(inputp);
                                Static_Variables.ProductMachinesChanged = true;

                            }
                        }
                    }

                    i++;
                }
                context.SubmitChanges();
            }
            this.Close();
        }
    }
}
