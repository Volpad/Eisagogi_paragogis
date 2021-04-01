using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Eisagogi_paragogis
{
    [Table(Name = "Colour_Specs")]
    public class Colour_Specs
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ID { get; set; }
        [Column] public string reference { get; set; }
        [Column] public string machine { get; set; }
        [Column] public string program { get; set; }
        [Column] public string colour { get; set; }
        [Column] public string v0 { get; set; }
        [Column] public string v0c { get; set; }
        [Column] public string v0y { get; set; }
        [Column] public Nullable<int> v0q { get; set; }
        [Column] public string v1 { get; set; }
        [Column] public string v1c { get; set; }
        [Column] public string v1y { get; set; }
        [Column] public Nullable<int> v1q { get; set; }
        [Column] public string v2 { get; set; }
        [Column] public string v2c { get; set; }
        [Column] public string v2y { get; set; }
        [Column] public Nullable<int> v2q { get; set; }
        [Column(Name = "v2.1")] public string v2_1 { get; set; }
        [Column(Name = "v2.1c")] public string v2_1c { get; set; }
        [Column(Name = "v2.1y")] public string v2_1y { get; set; }
        [Column(Name = "v2.1q")] public Nullable<int> v2_1q { get; set; }
        [Column(Name = "v2.2")] public string v2_2 { get; set; }
        [Column(Name = "v2.2c")] public string v2_2c { get; set; }
        [Column(Name = "v2.2y")] public string v2_2y { get; set; }
        [Column(Name = "v2.2q")] public Nullable<int> v2_2q { get; set; }
        [Column] public string v3 { get; set; }
        [Column] public string v3c { get; set; }
        [Column] public string v3y { get; set; }
        [Column] public Nullable<int> v3q { get; set; }
        [Column(Name = "v3.1")] public string v3_1 { get; set; }
        [Column(Name = "v3.1c")] public string v3_1c { get; set; }
        [Column(Name = "v3.1y")] public string v3_1y { get; set; }
        [Column(Name = "v3.1q")] public Nullable<int> v3_1q { get; set; }
        [Column(Name = "v3.2")] public string v3_2 { get; set; }
        [Column(Name = "v3.2c")] public string v3_2c { get; set; }
        [Column(Name = "v3.2y")] public string v3_2y { get; set; }
        [Column(Name = "v3.2q")] public Nullable<int> v3_2q { get; set; }
        [Column] public string v4 { get; set; }
        [Column] public string v4c { get; set; }
        [Column] public string v4y { get; set; }
        [Column] public Nullable<int> v4q { get; set; }
        [Column] public string v5 { get; set; }
        [Column] public string v5c { get; set; }
        [Column] public string v5y { get; set; }
        [Column] public Nullable<int> v5q { get; set; }
        [Column] public string v6 { get; set; }
        [Column] public string v6c { get; set; }
        [Column] public string v6y { get; set; }
        [Column] public Nullable<int> v6q { get; set; }
        [Column] public string v7 { get; set; }
        [Column] public string v7c { get; set; }
        [Column] public string v7y { get; set; }
        [Column] public Nullable<int> v7q { get; set; }
        [Column] public string v8 { get; set; }
        [Column] public string v8c { get; set; }
        [Column] public string v8y { get; set; }
        [Column] public Nullable<int> v8q { get; set; }
        [Column(Name = "2f2.2")] public string f22_2 { get; set; }
        [Column(Name = "2f2.2c")] public string f22_2c { get; set; }
        [Column(Name = "2f2.2y")] public string f22_2y { get; set; }
        [Column(Name = "2f2.2q")] public Nullable<int> f22_2q { get; set; }
        [Column(Name = "2f2.4")] public string f22_4 { get; set; }
        [Column(Name = "2f2.4c")] public string f22_4c { get; set; }
        [Column(Name = "2f2.4y")] public string f22_4y { get; set; }
        [Column(Name = "2f2.4q")] public Nullable<int> f22_4q { get; set; }
        [Column] public string l1 { get; set; }
        [Column] public string l1c { get; set; }
        [Column] public string l1y { get; set; }
        [Column] public Nullable<int> l1q { get; set; }
        [Column] public string l2 { get; set; }
        [Column] public string l2c { get; set; }
        [Column] public string l2y { get; set; }
        [Column] public Nullable<int> l2q { get; set; }
        [Column(Name = "p1.1c")] public string p1_1c { get; set; }
        [Column(Name = "p1.1y")] public string p1_1y { get; set; }
        [Column(Name = "p1.1q")] public Nullable<int> p1_1q { get; set; }
        [Column(Name = "p1.2c")] public string p1_2c { get; set; }
        [Column(Name = "p1.2y")] public string p1_2y { get; set; }
        [Column(Name = "p1.2q")] public Nullable<int> p1_2q { get; set; }
        [Column(Name = "p1.3c")] public string p1_3c { get; set; }
        [Column(Name = "p1.3y")] public string p1_3y { get; set; }
        [Column(Name = "p1.3q")] public Nullable<int> p1_3q { get; set; }
        [Column(Name = "p2.1c")] public string p2_1c { get; set; }
        [Column(Name = "p2.1y")] public string p2_1y { get; set; }
        [Column(Name = "p2.1q")] public Nullable<int> p2_1q { get; set; }
        [Column(Name = "p2.2c")] public string p2_2c { get; set; }
        [Column(Name = "p2.2y")] public string p2_2y { get; set; }
        [Column(Name = "p2.2q")] public Nullable<int> p2_2q { get; set; }
        [Column(Name = "p2.3c")] public string p2_3c { get; set; }
        [Column(Name = "p2.3y")] public string p2_3y { get; set; }
        [Column(Name = "p2.3q")] public Nullable<int> p2_3q { get; set; }
        [Column(Name = "p3.1c")] public string p3_1c { get; set; }
        [Column(Name = "p3.1y")] public string p3_1y { get; set; }
        [Column(Name = "p3.1q")] public Nullable<int> p3_1q { get; set; }
        [Column(Name = "p3.2c")] public string p3_2c { get; set; }
        [Column(Name = "p3.2y")] public string p3_2y { get; set; }
        [Column(Name = "p3.2q")] public Nullable<int> p3_2q { get; set; }
        [Column(Name = "p3.3c")] public string p3_3c { get; set; }
        [Column(Name = "p3.3y")] public string p3_3y { get; set; }
        [Column(Name = "p3.3q")] public Nullable<int> p3_3q { get; set; }
        [Column(Name = "p4.1c")] public string p4_1c { get; set; }
        [Column(Name = "p4.1y")] public string p4_1y { get; set; }
        [Column(Name = "p4.1q")] public Nullable<int> p4_1q { get; set; }
        [Column(Name = "p4.2c")] public string p4_2c { get; set; }
        [Column(Name = "p4.2y")] public string p4_2y { get; set; }
        [Column(Name = "p4.2q")] public Nullable<int> p4_2q { get; set; }
        [Column(Name = "p4.3c")] public string p4_3c { get; set; }
        [Column(Name = "p4.3y")] public string p4_3y { get; set; }
        [Column(Name = "p4.3q")] public Nullable<int> p4_3q { get; set; }
        [Column(Name = "p5.1c")] public string p5_1c { get; set; }
        [Column(Name = "p5.1y")] public string p5_1y { get; set; }
        [Column(Name = "p5.1q")] public Nullable<int> p5_1q { get; set; }
        [Column(Name = "p5.2c")] public string p5_2c { get; set; }
        [Column(Name = "p5.2y")] public string p5_2y { get; set; }
        [Column(Name = "p5.2q")] public Nullable<int> p5_2q { get; set; }
        [Column(Name = "p5.3c")] public string p5_3c { get; set; }
        [Column(Name = "p5.3y")] public string p5_3y { get; set; }
        [Column(Name = "p5.3q")] public Nullable<int> p5_3q { get; set; }


    }
}
