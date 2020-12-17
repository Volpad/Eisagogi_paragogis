using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Eisagogi_paragogis
{
    [Database]
    public class Production18 : DataContext
    {
        //public Production18() : base("Data Source=Server-DC;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=true;") { }

        //public Production18() : base("Data Source=WALK-SERVER;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=true;") { }
        // public Production18() : base("Data Source=walksocks.ath.cx:8033;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=true;") { }

        public Production18() : base("Data Source=Server-DC;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=false; user=production; password=W4lkPr0duct!0n") { }



        public Table<eisagogiParagogis> eisagogiParagogis;
        public Table<BOMS_2> BOMS_2;
        public Table<BOMS3> BOMS3;
        public Table<boms3a> boms3a;
        public Table<BOMS4> BOMS4;
        public Table<DELTIO_FINISH_SUPER> DELTIO_FINISH_SUPER;
        public Table<DELTIO_FINISH_SUPER1> DELTIO_FINISH_SUPER1;
        public Table<EXOPLISMOS> EXOPLISMOS;
        public Table<EXOPLISMOS1> EXOPLISMOS1;
        public Table<Finish1> Finish1;
        public Table<FinMachine> FinMachine;
        public Table<GM> GM;
        public Table<GMP> GMP;
        public Table<IMAGES> IMAGES;
        public Table<Machineqty> Machineqty;
        public Table<OrderRegister> OrderRegister;
        public Table<Orders_Fin> Orders_Fin;
        public Table<ProductionQTY> ProductionQTY;
        public Table<RAW_MATE> RAW_MATE;
        public Table<Semi_finished_warehouse> Semi_finished_warehouse;
        public Table<shipment> shipment;
        public Table<SIZESORTING> SIZESORTING;
        public Table<sizeStandard> sizeStandard;
        public Table<special_Processing_Instruction> special_Processing_Instruction;
        public Table<Staff> Staff;
        public Table<Unit_> Unit;
        public Table<DailyProduction> DailyProduction;
        public Table<Production_Plan_Changes> Production_Plan_Changes;
        public Table<ToBePlanned> ToBePlanned;
        public Table<ItemBalances> ItemBalances;
        public Table<walk_prd_FullItemLinesList> walk_prd_FullItemLinesList;
        public Table<Boarding> Boading;
        public Table<MachinePosition> MachinePosition;
        public Table<Product_spec_changes> Product_Spec_Changes;
        public Table<ColoursView> Coloursview;

    }
}
