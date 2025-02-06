using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Manager class for handling flight-related operations
    /// </summary>
    internal class clsFlightManager
    {
        /// <summary>
        /// Retrieves all flights from the database
        /// </summary>
        /// <returns>List of Flight objects</returns>
        /// <exception cref="Exception">Thrown when database operations fail</exception>
        public static List<clsFlight> GetFlights()
        {
            try
            {
                List<clsFlight> flights = new List<clsFlight>();

                //Get the SQL string
                string sSQL = clsSQL.GetFlights();

                //Create a connection to the database
                clsDataAccess db = new clsDataAccess();

                //Get the data from the database
                int iRetVal = 0;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRetVal);

                //Loop through the data and create flight objects
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    flights.Add(new clsFlight()
                    {
                        sFlightID = dr["Flight_ID"].ToString(),
                        FlightNumber = dr["Flight_Number"].ToString(),
                        AircraftType = dr["Aircraft_Type"].ToString()
                    });
                }

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}