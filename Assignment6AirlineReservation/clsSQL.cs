using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Class containing SQL queries for the airline reservation system
    /// </summary>
    internal class clsSQL
    {
        /// <summary>
        /// Retrieves the SQL query for getting all flights from the database
        /// </summary>
        /// <returns>SQL query string that selects Flight_ID, Flight_Number, and Aircraft_Type from the FLIGHT table</returns>
        /// <exception cref="Exception">Thrown when there is an error constructing the SQL query</exception>
        public static string GetFlights()
        {
            try
            {
                string sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the SQL query for getting passengers for a specific flight
        /// </summary>
        /// <param name="sFlightID">The ID of the flight to get passengers for</param>
        /// <returns>SQL query string that selects passenger information for the specified flight</returns>
        /// <exception cref="Exception">Thrown when there is an error constructing the SQL query</exception>
        public static string GetPassengers(string sFlightID)
        {
            try
            {
                //Get the passengers for Flight ID 1
                string sSQL = "SELECT PASSENGER.Passenger_ID, First_Name, Last_Name, Seat_Number " +
                              "FROM FLIGHT_PASSENGER_LINK, FLIGHT, PASSENGER " +
                          "WHERE FLIGHT.FLIGHT_ID = FLIGHT_PASSENGER_LINK.FLIGHT_ID AND " +
                          "FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID AND " +
                          "FLIGHT.FLIGHT_ID = 1";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}