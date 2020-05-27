using Eisagogi_paragogis.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eisagogi_paragogis
{
    public class NotificationTest
    {


       // private static string connectionString = "Data Source=WALK-SERVER;Initial Catalog=PRODUCTION18;Persist Security Info=True;Integrated Security=true;";
        //private static string connectionString = "server=WALK-SERVER;database=PRODUCTION18;Persist Security Info=True;Integrated Security=true;";
        private static string connectionString = "server=SERVER-DC;database=PRODUCTION18;Trusted_Connection=Yes;";
        private SqlConnection connection = new SqlConnection(connectionString);

        SqlDependencyEx listener = new SqlDependencyEx(connectionString, "Production18", "Machineqty",identity: 1);

        void SomeOtherMethod()
        {
            listener.TableChanged += (o, e) => MessageBox.Show("Έγινε αλλαγή"); //Console.WriteLine("Your table was changed!");
            listener.Start();

        }

        public void Initialization()
        {
            // Create a dependency connection.
           // CanRequestNotifications();
            connection.Open();
            bool started = SqlDependency.Start(connectionString, @"eisagogiMachinesChangeQueue");
            Console.WriteLine("Started.. " + started);
            //SomeMethod();
            SomeOtherMethod();
            // connection.Close();
            // SqlDependency.Stop(connectionString, @"eisagogiMachinesChangeQueue");

        }




        void SomeMethod()
        {

            // Assume connection is an open SqlConnection.


            // Create a new SqlCommand object.
            using (SqlCommand command = new SqlCommand("SELECT AutoNo, date FROM Production18.dbo.eisagogiParagogis", connection))
            {
               // CanRequestNotifications();
                // Create a dependency and associate it with the SqlCommand.
                //SqlDependency dependency = new SqlDependency(command);
                SqlDependency dependency = new SqlDependency(command, "Service=eisagogiMachinesChangeService;", Int32.MaxValue);

                // Maintain the reference in a class member.

                // Subscribe to the SqlDependency event.
                dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);
               
                
                // Execute the command.
                //command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   
                    // Process the DataReader.
                }
            }
        }

        // Handler method
        void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;

            dependency.OnChange -= OnDependencyChange;

            if (e.Type == SqlNotificationType.Change)
            {
                MessageBox.Show("έγινε αλλαγή " + e.Info.ToString());
            }
            // SqlDependency.Stop(connectionString, @"eisagogiMachinesChangeQueue");
            // Handle the event (for example, invalidate this cache entry).
            SomeMethod();
        }

        public void Termination()
        {
            // Release the dependency.
            SqlDependency.Stop(connectionString, @"eisagogiMachinesChangeQueue");
            connection.Close();
            listener.Stop();
        }



        private bool CanRequestNotifications()
        {
            SqlClientPermission permission =
                new SqlClientPermission(
                PermissionState.Unrestricted);
            try
            {
                permission.Demand();
                // MessageBox.Show("granded");
                return true;
            }
            catch (System.Exception)
            {
                MessageBox.Show("denied");

                return false;
            }
        }


    }
}
