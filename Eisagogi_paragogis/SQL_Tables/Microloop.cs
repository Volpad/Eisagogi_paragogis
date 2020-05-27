using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eisagogi_paragogis
{
    [Database]
    class Microloop : DataContext
    {
        public Microloop() : base("Data Source=WALK-SERVER;Initial Catalog=MICROLOOP;Persist Security Info=True;Integrated Security=true;") { }

#pragma warning disable CS0649 // Field 'Microloop.walk_prd_FullItemLinesList' is never assigned to, and will always have its default value null
        public Table<walk_prd_FullItemLinesList> walk_prd_FullItemLinesList;
#pragma warning restore CS0649 // Field 'Microloop.walk_prd_FullItemLinesList' is never assigned to, and will always have its default value null

    }
}
