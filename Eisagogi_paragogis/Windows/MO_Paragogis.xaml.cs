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
    /// Interaction logic for MO_Paragogis.xaml
    /// </summary>
    public partial class MO_Paragogis : Window
    {
        public MO_Paragogis()
        {
            InitializeComponent();

         //   int days = BusinessDaysUntil(first.DisplayDate, last.DisplayDate);

        }



        public static int BusinessDaysUntil(DateTime firstDay, DateTime lastDay)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            return businessDays;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int days = BusinessDaysUntil(first.SelectedDate.Value, last.SelectedDate.Value);
           // MessageBox.Show(days.ToString());

            using(var context = new Production18())
            {
                var  text = context.eisagogiParagogis.Where(i => i.date < last.SelectedDate.Value.AddDays(1) && i.date > first.SelectedDate.Value.AddDays(0)).Sum(c => ((c.dozen * 24 + c.socks)) / days) / 24;
                bd.Text = days.ToString();
                mo.Text = "MO: " + Decimal.Round(Convert.ToDecimal(text), 1).ToString();

                

            }

        }
    }
}
