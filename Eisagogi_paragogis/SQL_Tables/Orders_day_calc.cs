using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis.SQL_Tables
{
    [Table(Name = "Order_days_calculation")]
    public class Orders_day_calc
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public string Orders_ID { get; set; }
        [Column] public string Product_Name { get; set; }
        [Column] public string Orders_Number { get; set; }
        [Column] public Nullable<DateTime> Order_machine_start_date { get; set; }
        [Column] public Nullable<DateTime> Order_Del_date { get; set; }
        [Column] public Nullable<int> Order_QTY { get; set; }
        [Column] public Nullable<int> No_of_Machines { get; set; }
        [Column] public Nullable<double> Production_days { get; set; }
        [Column] public string Machine_Type { get; set; }
        [Column] public Nullable<DateTime> Date_asked { get; set; }


    }
}
