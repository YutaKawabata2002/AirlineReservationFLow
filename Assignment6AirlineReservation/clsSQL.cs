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
                return $"SELECT PASSENGER.Passenger_ID, First_Name, Last_Name, Seat_Number " +
                       $"FROM FLIGHT_PASSENGER_LINK " +
                       $"INNER JOIN PASSENGER ON FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID " +
                       $"WHERE FLIGHT_PASSENGER_LINK.FLIGHT_ID = {sFlightID}";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(GetPassengers)} -> {ex.Message}");
            }
        }

        /// <summary>
        /// SQL query for inserting a new passenger
        /// </summary>
        public static string InsertPassenger(string firstName, string lastName)
        {
            try
            {
                return $"INSERT INTO PASSENGER (First_Name, Last_Name) VALUES ('{firstName}', '{lastName}');";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(InsertPassenger)} -> {ex.Message}");
            }
        }

        /// <summary>
        /// SQL query for retrieving the latest passenger ID
        /// </summary>
        public static string GetLatestPassengerID()
        {
            try
            {
                return "SELECT MAX(PASSENGER_ID) AS NewPassengerID FROM PASSENGER;";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(GetLatestPassengerID)} -> {ex.Message}");
            }
        }

        /// <summary>
        /// SQL query for linking a passenger to a flight with a seat assignment
        /// </summary>
        public static string LinkPassengerToFlight(string flightID, string passengerID, string seatNumber)
        {
            try
            {
                return $"INSERT INTO FLIGHT_PASSENGER_LINK (FLIGHT_ID, PASSENGER_ID, SEAT_NUMBER) " +
                       $"VALUES ('{flightID}', '{passengerID}', '{seatNumber}');";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(LinkPassengerToFlight)} -> {ex.Message}");
            }
        }

        /// <summary>
        /// SQL query for updating a passenger's seat
        /// </summary>
        public static string UpdatePassengerSeat(string passengerID, string newSeatNumber)
        {
            try
            {
                return $"UPDATE FLIGHT_PASSENGER_LINK " +
                       $"SET SEAT_NUMBER = {newSeatNumber} " +
                       $"WHERE PASSENGER_ID = {passengerID};";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(UpdatePassengerSeat)} -> {ex.Message}");
            }
        }

        /// <summary>
        /// SQL query for deleting a passenger link from a flight
        /// </summary>
        public static string DeletePassengerLink(string passengerID)
        {
            try
            {
                return $"DELETE FROM FLIGHT_PASSENGER_LINK WHERE PASSENGER_ID = {passengerID};";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(DeletePassengerLink)} -> {ex.Message}");
            }
        }

        /// <summary>
        /// SQL query for deleting a passenger from the system
        /// </summary>
        public static string DeletePassenger(string passengerID)
        {
            try
            {
                return $"DELETE FROM PASSENGER WHERE PASSENGER_ID = {passengerID};";
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsSQL)}.{nameof(DeletePassenger)} -> {ex.Message}");
            }
        }
    }
}