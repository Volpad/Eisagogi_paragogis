using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml;
using System.Xml.Linq;

namespace Eisagogi_paragogis
{
    /// <summary>
    /// Interaction logic for Product_Specification.xaml
    /// </summary>
    public partial class Product_Specification : Window
    {
        String finishId = "";
        bool tochange = false;

        public Product_Specification()
        {
            finishId = Static_Variables.finishid;
            InitializeComponent();
            Initial_Values();

            if (Static_Variables.print_form)
            {
                print_();
            }

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

                dateModified.Text = context.Product_Spec_Changes.Any(f => f.ProductName == finishId) ? context.Product_Spec_Changes.Where(c => c.ProductName == finishId).OrderByDescending(d => d.date_modified).Select(f => f.date_modified.Value).FirstOrDefault().ToString("dd/MM/yy") : "";
                userModified.Text = context.Product_Spec_Changes.Any(f => f.ProductName == finishId) ? context.Product_Spec_Changes.Where(c => c.ProductName == finishId).OrderByDescending(d => d.date_modified).Select(f => f.user_modified).FirstOrDefault() : ""; ;

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
                string plex = "";
                string eido = "";
                string etik = "";
                string sysk = "";
                string iron = "";
                string stick = "";
                string rizox = "";
                string zon = "";
                string pap = "";
                string comm = "";

                using (var context = new Production18())
                {
                    desc = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.DESCRIPTION).FirstOrDefault().TrimEnd() : "";
                    comp = context.Finish1.Where(c => c.FINISHAP_ID.Equals(finishId)).Select(d => d.COMP.Equals(null) ? "" : d.COMP).FirstOrDefault().TrimEnd();
                    plex = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.PLEJH).FirstOrDefault().TrimEnd() : "";
                    eido = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.SPI.Equals(null) ? "" : d.SPI).FirstOrDefault().TrimEnd() : "";
                    etik = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.LABEL).FirstOrDefault().TrimEnd();
                    sysk = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.BOX).FirstOrDefault().TrimEnd();
                    iron = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.IRON).FirstOrDefault().TrimEnd();
                    stick = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.STICKER).FirstOrDefault().TrimEnd();
                    rizox = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.ΡΙΖΟΧΑΡΤΟ).FirstOrDefault().TrimEnd();
                    zon = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.ΖΩΝΑΚΙ).FirstOrDefault().TrimEnd();
                    pap = context.BOMS4.Where(c => c.FINISHAP_ID == finishId).Select(d => d.PAPER).FirstOrDefault().TrimEnd();
                    comm = context.Finish1.Any(c => c.FINISHAP_ID == finishId) ? context.Finish1.Where(c => c.FINISHAP_ID == finishId).Select(d => d.COMMENTS.Equals(null) ? "" : d.COMMENTS).FirstOrDefault().TrimEnd() : "";
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
                    if (plexi.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Plexi",
                            old_value = plex,
                            new_value = plexi.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.PLEJH = plexi.Text;
                        }
                        plexi.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (eidod.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Eidiki Odigia",
                            old_value = eido,
                            new_value = eidod.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.SPI = eidod.Text;
                        }
                        eidod.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (etiketa.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Etiketa",
                            old_value = etik,
                            new_value = etiketa.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.LABEL = etiketa.Text;
                        }
                        etiketa.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (syskeuasia.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Tropos Syskeuasias (box)",
                            old_value = sysk,
                            new_value = syskeuasia.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.BOX = syskeuasia.Text;
                        }
                        syskeuasia.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (irontag.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Irontag",
                            old_value = iron,
                            new_value = irontag.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.IRON = irontag.Text;
                        }
                        irontag.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (sticker.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Sticker",
                            old_value = stick,
                            new_value = sticker.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.STICKER = sticker.Text;
                        }
                        sticker.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (rizoxarto.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Rizoxarto",
                            old_value = rizox,
                            new_value = rizoxarto.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.ΡΙΖΟΧΑΡΤΟ = rizoxarto.Text;
                        }
                        rizoxarto.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (zonaki.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Zonaki",
                            old_value = zon,
                            new_value = zonaki.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.ΖΩΝΑΚΙ = zonaki.Text;
                        }
                        zonaki.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (paper.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Paper",
                            old_value = pap,
                            new_value = paper.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.PAPER = paper.Text;
                        }
                        paper.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (comments.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Comments",
                            old_value = comm,
                            new_value = comments.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.COMMENTS = comments.Text;
                        }
                        comments.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (protypoc.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Protypo Sideromatos",
                            old_value = protypo.Text,
                            new_value = protypoc.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.BOMS4.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.FORM = protypoc.Text;
                        }
                        protypoc.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (mytic.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Rafi Mytis",
                            old_value = myti.Text,
                            new_value = mytic.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.ENOSI= mytic.Text;
                        }
                        mytic.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    if (customerc.Foreground.ToString() == "#FFFF0000")
                    {
                        Product_spec_changes input = new Product_spec_changes
                        {
                            user_modified = Environment.UserName,
                            date_modified = DateTime.Now,
                            specification_changed = "Rafi Mytis",
                            old_value = customer.Text,
                            new_value = customerc.Text,
                            ProductName = finishId
                        };
                        context.Product_Spec_Changes.InsertOnSubmit(input);
                        foreach (var c in context.Finish1.Where(d => d.FINISHAP_ID == finishId))
                        {
                            c.COUNTRY = customerc.Text;
                        }
                        customerc.Foreground = new SolidColorBrush(Colors.Black);
                    }


                    context.SubmitChanges();
                }

                Initial_Values();

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

        private void MachinesPicking_Click(object sender, RoutedEventArgs e)
        {
            Static_Variables.finishid = finishId;
            Window mp = new Machines_Picking();

            //await Task.Run(() => mp.Show());

            mp.ShowDialog();

            if (Static_Variables.ProductMachinesChanged)
            {
                using (var context = new Production18())
                {
                    machines.ItemsSource = context.GM.Where(c => c.FINISHAP_ID == finishId).Select(d => new { mach = d.MHXANH.TrimEnd() });
                }
                Static_Variables.ProductMachinesChanged = false;
            }
        }

        internal void print_Click(object sender, RoutedEventArgs e)
        {
            print_();

        }

        private void print_()
        {
            if (Static_Variables.print_form)
            {
                PrintDialog printDlg = new PrintDialog();
                Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);

                specifications.Measure(pageSize);
                specifications.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                PrintDialog dl = new PrintDialog();


                if (printDlg.ShowDialog() == true)
                {

                    System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);


                    double scale = Math.Min(specifications.ActualWidth / capabilities.PageImageableArea.ExtentWidth, specifications.ActualHeight / capabilities.PageImageableArea.ExtentHeight);
                    specifications.LayoutTransform = new ScaleTransform(scale, scale);
                    Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                    specifications.Measure(sz);
                    specifications.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));
                    specifications.UpdateLayout();

                    printDlg.PrintVisual(specifications, "First Fit to Page WPF Print");
                }
                Static_Variables.print_form = false;

            }
            else
            {
                Static_Variables.finishid = finishId;
                Static_Variables.print_form = true;
                Window wd = new Product_Specification();

            }
        }
    }
}
