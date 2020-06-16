using System;
using System.Collections.Generic;
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

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for Product_Specification.xaml
    /// </summary>
    public partial class Product_Specification : Window
    {
        String finishId = "redmwc35";
        bool tochange = false;

        public Product_Specification()
        {
            finishId = Static_Variables.finishid;
            InitializeComponent();
            Initial_Values();


        }

        private void Initial_Values()
        {
            using (var context = new Production18())
            {
                prodname.IsReadOnly = true;
                description.IsReadOnly = true;
                customer.IsReadOnly = true;
                composition.IsReadOnly = true;
                plexi.IsReadOnly = true;
                myti.IsReadOnly = true;
                eidod.IsReadOnly = true;
                etiketa.IsReadOnly = true;
                syskeuasia.IsReadOnly = true;
                irontag.IsReadOnly = true;
                sticker.IsReadOnly = true;
                rizoxarto.IsReadOnly = true;
                zonaki.IsReadOnly = true;
                paper.IsReadOnly = true;
                protypo.IsReadOnly = true;
                dateCreated.IsReadOnly = true;
                userCreated.IsReadOnly = true;
                dateModified.IsReadOnly = true;
                userModified.IsReadOnly = true;
                comments.IsReadOnly = true;
                machines.IsReadOnly = true;
                programs.IsReadOnly = true;
                yarns.IsReadOnly = true;
                sizes.IsReadOnly = true;

                prodname.Text = finishId.ToUpper();
                description.Text = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.DESCRIPTION).FirstOrDefault().TrimEnd() : "";
                description.Height = double.NaN;
                customer.Text = context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.COUNTRY).FirstOrDefault().TrimEnd();
                composition.Text = context.Finish1.Where(c => c.FINISHAP_ID.Equals(finishId)).Select(d => d.COMP.Equals(null) ? "" : d.COMP).FirstOrDefault().TrimEnd();
                plexi.Text = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.PLEJH).FirstOrDefault().TrimEnd() : "";
                myti.Text = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.ENOSI).FirstOrDefault().TrimEnd() : "";
                eidod.Text = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.SPI.Equals(null) ? "" : d.SPI).FirstOrDefault().TrimEnd() : "";
                dateCreated.Text = context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.DATE1).FirstOrDefault().ToString("dd/MM/yy");
                userCreated.Text = context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.EGKRI).FirstOrDefault().TrimEnd();
                comments.Text = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.COMMENTS.Equals(null) ? "" : d.COMMENTS).FirstOrDefault().TrimEnd() : "";

                etiketa.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.LABEL).FirstOrDefault().TrimEnd();
                syskeuasia.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.BOX).FirstOrDefault().TrimEnd();
                irontag.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.IRON).FirstOrDefault().TrimEnd();
                sticker.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.STICKER).FirstOrDefault().TrimEnd();
                rizoxarto.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.ΡΙΖΟΧΑΡΤΟ).FirstOrDefault().TrimEnd();
                zonaki.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.ΖΩΝΑΚΙ).FirstOrDefault().TrimEnd();
                paper.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.PAPER).FirstOrDefault().TrimEnd();
                protypo.Text = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.FORM).FirstOrDefault().TrimEnd();

                dateModified.Text = "";
                userModified.Text = "";

                machines.ItemsSource = context.GM.Where(c => c.FINISHAP_ID == finishId).Select(d => new { mach = d.MHXANH.TrimEnd() });

                programs.ItemsSource = context.GMP.Where(c => c.FINISHAP_ID == finishId).Select(d => new { prog = d.PROGRAM.TrimEnd() });

                machines.HeadersVisibility = DataGridHeadersVisibility.None;
                programs.HeadersVisibility = DataGridHeadersVisibility.None;

                yarns.ItemsSource = context.BOMS_2.Where(c => c.FINISHAP_ID == finishId).Select(d => new { qty = d.ΠΟΣΟΤΗΤΑ, d.PART, Νήμα = d.COLOR });

                sizes.ItemsSource = context.boms3a.Where(c => c.FINISHAP_ID == finishId).OrderBy(f => f.SIZESORTING).Select(d => new { d.NO, d.MACH_SIZE, d.FORMES, d.PAT, d.MKAL, d.PKAL, d.YLAS, d.PLAS, d.WEIG });
                image.Source = null;

                foreach (var IMAGE in context.IMAGES.Where(c => c.GUIDE == finishId))
                {
                    if (IMAGE.PICTURE != null)
                    {
                        BitmapImage bmp = new BitmapImage();
                        MemoryStream strmImg = new MemoryStream(IMAGE.PICTURE);
                        bmp.BeginInit();
                        bmp.StreamSource = strmImg;
                        bmp.EndInit();

                        if (bmp == null)
                        {
                            image.Source = null;
                        }
                        else
                        {
                            image.Source = bmp;
                        }
                    }
                    else
                    {
                        image.Source = null;
                    }
                }

            }
        }

        private void productref_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (var context = new Production18())
                {
                    if (context.Finish1.Any(c => c.FINISHAP_ID == productref.Text))
                    {
                        finishId = productref.Text;
                        Initial_Values();

                    }
                    else
                    {
                        MessageBox.Show("No product available");
                        productref.Text = "";
                    }
                }


            }
        }

        private void copy_ref_Click(object sender, RoutedEventArgs e)
        {
            string temp = "";
            Ask_Product dataBox = new Ask_Product("Enter Product Ref", "");
            if (dataBox.ShowDialog() == true)
            {
                temp = dataBox.Answer;


                using (var context = new Production18())
                {
                    if (context.Finish1.Any(c => c.FINISHAP_ID == temp))
                    {
                        MessageBox.Show("Ο κωδικός υπάρχει");
                    }
                    else
                    {
                        Finish1 input = new Finish1
                        {
                            FINISHAP_ID = temp.ToUpper(),
                            DESCRIPTION = description.Text,
                            COUNTRY = customer.Text,
                            DATE1 = DateTime.Now,
                            COMMENTS = comments.Text,
                            EGKRI = Environment.UserName,
                            PLEJH = plexi.Text,
                            ENOSI = myti.Text,
                            cor = false,
                            SPI = eidod.Text,
                            COMP = composition.Text,
                            Inactive = false,
                        };
                        context.Finish1.InsertOnSubmit(input);

                        BOMS4 input2 = new BOMS4
                        {
                            FINISHAP_ID = temp.ToUpper(),
                            LABEL = etiketa.Text,
                            BOX = syskeuasia.Text,
                            IRON = irontag.Text,
                            STICKER = sticker.Text,
                            ΡΙΖΟΧΑΡΤΟ = rizoxarto.Text,
                            ΖΩΝΑΚΙ = zonaki.Text,
                            PAPER = paper.Text,
                            FORM = protypo.Text
                        };
                        context.BOMS4.InsertOnSubmit(input2);

                        IMAGES image = new IMAGES
                        {
                            GUIDE = temp.ToUpper(),
                        };
                        context.IMAGES.InsertOnSubmit(image);


                        var gm = from c in context.GM.Where(c => c.FINISHAP_ID == finishId).ToArray()
                                 select new GM
                                 {
                                     FINISHAP_ID = temp.ToUpper(),
                                     MHXANH = c.MHXANH
                                 };
                        context.GM.InsertAllOnSubmit(gm);

                        var gmp = from c in context.GMP.Where(c => c.FINISHAP_ID == finishId).ToArray()
                                  select new GMP
                                  {
                                      FINISHAP_ID = temp.ToUpper(),
                                      PROGRAM = c.PROGRAM
                                  };
                        context.GMP.InsertAllOnSubmit(gmp);

                        var boms2 = from c in context.BOMS_2.Where(c => c.FINISHAP_ID == finishId).ToArray()
                                    select new BOMS_2
                                    {
                                        FINISHAP_ID = temp.ToUpper(),
                                        COLOR = c.COLOR,
                                        PART = c.PART,
                                        ΠΟΣΟΤΗΤΑ = c.ΠΟΣΟΤΗΤΑ
                                    };
                        context.BOMS_2.InsertAllOnSubmit(boms2);

                        var boms3A = from c in context.boms3a.Where(c => c.FINISHAP_ID == finishId).ToArray()
                                     select new boms3a
                                     {
                                         FINISHAP_ID = temp.ToUpper(),
                                         NO = c.NO,
                                         MACH_SIZE = c.MACH_SIZE,
                                         PAT = c.PAT,
                                         MKAL = c.MKAL,
                                         PKAL = c.PKAL,
                                         YLAS = c.YLAS,
                                         PLAS = c.PLAS,
                                         WEIG = c.WEIG,
                                         FORMES = c.FORMES,
                                         SIZESORTING = c.SIZESORTING,
                                         SizeHeadings = c.SizeHeadings,
                                         ProductionTime = c.ProductionTime,
                                     };
                        context.boms3a.InsertAllOnSubmit(boms3A);


                        

                        context.SubmitChanges();

                    }
                }
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {

            //  var test = comments.Foreground.ToString();

            //if(comments.Foreground.ToString() == "#FFFF0000")


            if (tochange)
            {
                tochange = false;
                Change.Content = "Change Values";
                customer.Visibility = Visibility.Visible;
                myti.Visibility = Visibility.Visible;
                mytic.Visibility = Visibility.Collapsed;
                customerc.Visibility = Visibility.Collapsed;
                protypo.Visibility = Visibility.Visible;
                protypoc.Visibility = Visibility.Collapsed;

                description.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                composition.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                plexi.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                eidod.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                etiketa.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                syskeuasia.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                irontag.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                sticker.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                rizoxarto.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                zonaki.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                paper.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);
                comments.TextChanged -= new TextChangedEventHandler(Textbox_TextChanged);

                protypoc.SelectionChanged -= new SelectionChangedEventHandler(Combo_SelectionChanged);
                mytic.SelectionChanged -= new SelectionChangedEventHandler(Combo_SelectionChanged);
                customerc.SelectionChanged -= new SelectionChangedEventHandler(Combo_SelectionChanged);

                string desc = "";
                string comp = "";

                using (var context = new Production18())
                {
                    desc = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.DESCRIPTION).FirstOrDefault().TrimEnd() : "";
                    comp = context.Finish1.Where(c => c.FINISHAP_ID.Equals(finishId)).Select(d => d.COMP.Equals(null) ? "" : d.COMP).FirstOrDefault().TrimEnd();
                }



                using (var context = new Production18())
                {
                    if (description.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Description",
                            old_value = desc,
                            new_value = description.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.DESCRIPTION = description.Text;
                        }
                        description.Foreground = new SolidColorBrush(Colors.Black);
                    }

                    if (composition.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Composition",
                            old_value = comp,
                            new_value = composition.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.COMP = composition.Text;
                        }
                        composition.Foreground = new SolidColorBrush(Colors.Black);
                    }


                    context.SubmitChanges();
                }


            }
            else
            {
                tochange = true;
                Change.Content = "SAVE";

                description.IsReadOnly = false;
                composition.IsReadOnly = false;
                plexi.IsReadOnly = false;
                eidod.IsReadOnly = false;
                etiketa.IsReadOnly = false;
                syskeuasia.IsReadOnly = false;
                irontag.IsReadOnly = false;
                sticker.IsReadOnly = false;
                rizoxarto.IsReadOnly = false;
                zonaki.IsReadOnly = false;
                paper.IsReadOnly = false;
                comments.IsReadOnly = false;

                description.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                composition.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                plexi.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                eidod.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                etiketa.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                syskeuasia.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                irontag.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                sticker.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                rizoxarto.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                zonaki.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                paper.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);
                comments.TextChanged += new TextChangedEventHandler(Textbox_TextChanged);

                customer.Visibility = Visibility.Collapsed;
                myti.Visibility = Visibility.Collapsed;
                mytic.Visibility = Visibility.Visible;
                customerc.Visibility = Visibility.Visible;
                protypo.Visibility = Visibility.Collapsed;
                protypoc.Visibility = Visibility.Visible;



                mytic.IsEditable = true;
                customerc.IsEditable = true;
                protypoc.IsEditable = true;
                mytic.SelectedItem = myti.Text;
                customerc.SelectedItem = customer.Text;
                mytic.SelectedItem = myti.Text;



                using (var context = new Production18())
                {
                    mytic.ItemsSource = context.Finish1.GroupBy(c => c.ENOSI).OrderBy(f => f.Key).Select(d => d.Key);
                    customerc.ItemsSource = context.Finish1.GroupBy(c => c.COUNTRY).OrderBy(f => f.Key).Select(d => d.Key);
                    protypoc.ItemsSource = context.BOMS4.GroupBy(c => c.FORM).OrderBy(f => f.Key).Select(d => d.Key);
                }

                protypoc.SelectionChanged += new SelectionChangedEventHandler(Combo_SelectionChanged);
                mytic.SelectionChanged += new SelectionChangedEventHandler(Combo_SelectionChanged);
                customerc.SelectionChanged += new SelectionChangedEventHandler(Combo_SelectionChanged);

            }
            
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            TextBox tb = sender as TextBox;
            tb.Foreground = (Brush)bc.ConvertFrom("#FFFF0000");
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ComboBox tb = sender as ComboBox;
            tb.Foreground = (Brush)bc.ConvertFrom("#FFFF0000");
        }
    }
}
