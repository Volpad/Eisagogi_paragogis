﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Database]
    public class ChangesToProduction : DataContext
    {
        //public ChangesToProduction() : base("Data Source=WALK-SERVER;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=true;") { }
        //public ChangesToProduction() : base("Data Source=SERVER-DC;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=true;") { }
        public ChangesToProduction() : base("Data Source=SERVER-DC;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=false; user=production; password=W4lkPr0duct!0n") { }

        //public Table<Production_Plan_Changes> Production_Plan_Changes;
        public Table<Machineqty> Machineqty;

    }
}
