using SalesReport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Common;
using System.Net;
using System.Net.Http;
 
namespace SalesReport.Controllers
{
    public class HomeController : Controller
    {
        ReportModel m = new ReportModel();
        public ActionResult Index(ReportModel m)
        {
            
            string connectionString = "Data Source=codingtemplesql.database.windows.net;Initial Catalog=AdventureWorks;Integrated Security=False;User ID=SalesUser;Password=CodingTempleStudent!;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            List<string> states = new List<string>();
            List<SalesRow> DollarSales = new List<SalesRow>();
            List<SalesQuantity> QuantitySales = new List<Models.SalesQuantity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {   
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "sp_GetStates";
                command.CommandType = CommandType.StoredProcedure;


                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(reader.GetString(reader.GetOrdinal("StateProvince")));
                    }
                }

                    SqlCommand DollarCommand = connection.CreateCommand();
                    DollarCommand.CommandText = "sp_GetTopSalesByDollars";
                    DollarCommand.CommandType = CommandType.StoredProcedure;
                if (m.SelectedStateProvince == null)
                {
                    m.SelectedStateProvince = "This part took me a while";
                    DollarCommand.Parameters.AddWithValue("@StateProvince", m.SelectedStateProvince);
                }
                else
                {
                    DollarCommand.Parameters.AddWithValue("@StateProvince", m.SelectedStateProvince);

                }


                using (SqlDataReader reader = DollarCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesRow r = new SalesRow();
                            r.Product = reader.GetString(reader.GetOrdinal("Name"));
                            r.Amount = reader.GetDecimal(reader.GetOrdinal("TotalDollars"));
                            DollarSales.Add(r);
                        }
                    }
                /////
                SqlCommand QuantityCommand = connection.CreateCommand();
                QuantityCommand.CommandText = "sp_GetTopSalesByQuantity";
                QuantityCommand.CommandType = CommandType.StoredProcedure;
                if (m.SelectedStateProvince == null)
                {
                    m.SelectedStateProvince = "This part took me a while";
                    QuantityCommand.Parameters.AddWithValue("@StateProvince", m.SelectedStateProvince);
                }
                else
                {
                    QuantityCommand.Parameters.AddWithValue("@StateProvince", m.SelectedStateProvince);

                }


                using (SqlDataReader reader = QuantityCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesQuantity k = new SalesQuantity();
                        k.Product = reader.GetString(reader.GetOrdinal("Name"));
                        k.Quantity = reader.GetInt32(reader.GetOrdinal("QuantitySold"));
                        QuantitySales.Add(k);
                    }
                }

                connection.Close();
                    m.States = states.ToArray();
                    m.TotalSales = DollarSales.ToArray();
                    m.TotalSalesQuantity = QuantitySales.ToArray();
                }
              
            return View(m);
        }       
    }
}