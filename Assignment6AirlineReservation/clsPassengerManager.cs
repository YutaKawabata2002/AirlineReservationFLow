using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Manager class for handling passenger-related operations
    /// </summary>
    internal class clsPassengerManager
    {
        /// <summary>
        /// Retrieves all passengers for a specific flight
        /// </summary>
        /// <param name="sFlightID">The ID of the flight</param>
        /// <returns>List of Passenger objects</returns>
        public static List<clsPassenger> GetPassengers(string sFlightID)
        {
            try
            {
                List<clsPassenger> passengers = new List<clsPassenger>();

                // SQL Query to fetch passengers based on the selected FlightID
                string sSQL = $"SELECT PASSENGER.Passenger_ID, First_Name, Last_Name, Seat_Number " +
                              $"FROM FLIGHT_PASSENGER_LINK " +
                              $"INNER JOIN PASSENGER ON FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID " +
                              $"WHERE FLIGHT_PASSENGER_LINK.FLIGHT_ID = {sFlightID}";

                // Create a connection to the database
                clsDataAccess db = new clsDataAccess();

                // Execute the SQL query
                int iRetVal = 0;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRetVal);

                // Populate the list of passengers from the DataSet
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    passengers.Add(new clsPassenger()
                    {
                        PassengerID = dr["Passenger_ID"].ToString(),
                        FirstName = dr["First_Name"].ToString(),
                        LastName = dr["Last_Name"].ToString(),
                        SeatNumber = dr["Seat_Number"].ToString()
                    });
                }

                return passengers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Adds a new passenger and assigns a seat
        /// </summary>
        /// <param name="newPassenger">The passenger object to add</param>
        /// <param name="seatNumber">The seat number to assign</param>
        public static void AddPassenger(string firstName, string lastName, string flightID, string seatNumber)
        {
            try
            {
                // SQL to insert a new passenger
                string insertPassengerSQL = $"INSERT INTO PASSENGER (First_Name, Last_Name) " +
                                            $"VALUES ('{firstName}', '{lastName}');";

                clsDataAccess db = new clsDataAccess();
                db.ExecuteNonQuery(insertPassengerSQL);

                // Get the ID of the newly inserted passenger
                string getPassengerIDSQL = "SELECT MAX(PASSENGER_ID) AS NewPassengerID FROM PASSENGER;";
                int iRetVal = 0; // Declare iRetVal here
                DataSet ds = db.ExecuteSQLStatement(getPassengerIDSQL, ref iRetVal);

                string newPassengerID = ds.Tables[0].Rows[0]["NewPassengerID"].ToString();

                // Link the passenger to a flight and assign a seat
                string linkPassengerSQL = $"INSERT INTO FLIGHT_PASSENGER_LINK (FLIGHT_ID, PASSENGER_ID, SEAT_NUMBER) " +
                                          $"VALUES ('{flightID}', '{newPassengerID}', '{seatNumber}');";

                db.ExecuteNonQuery(linkPassengerSQL);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Updates a passenger's seat number
        /// </summary>
        /// <param name="passengerID">The passenger ID</param>
        /// <param name="newSeatNumber">The new seat number</param>
        public static void UpdatePassengerSeat(string passengerID, string newSeatNumber)
        {
            try
            {
                // If SEAT_NUMBER is numeric, don't use quotes around it
                string updateSeatSQL = $"UPDATE FLIGHT_PASSENGER_LINK " +
                                       $"SET SEAT_NUMBER = {newSeatNumber} " + // No quotes for numeric
                                       $"WHERE PASSENGER_ID = {passengerID};"; // No quotes for numeric IDs

                clsDataAccess db = new clsDataAccess();
                db.ExecuteNonQuery(updateSeatSQL);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating seat: {ex.Message}");
            }
        }


        /// <summary>
        /// Deletes a passenger from the system
        /// </summary>
        /// <param name="passengerID">The passenger ID to delete</param>
        public static void DeletePassenger(string passengerID)
        {
            try
            {
                // Create an instance of clsDataAccess to execute SQL statements
                clsDataAccess db = new clsDataAccess();

                // Delete the passenger's link to the flight
                string deleteLinkSQL = $"DELETE FROM FLIGHT_PASSENGER_LINK WHERE PASSENGER_ID = {passengerID};";
                int rowsAffectedLink = db.ExecuteNonQuery(deleteLinkSQL);

                if (rowsAffectedLink == 0)
                {
                    throw new Exception($"No records found in FLIGHT_PASSENGER_LINK for Passenger_ID = {passengerID}.");
                }

                // Delete the passenger from the PASSENGER table
                string deletePassengerSQL = $"DELETE FROM PASSENGER WHERE PASSENGER_ID = {passengerID};";
                int rowsAffectedPassenger = db.ExecuteNonQuery(deletePassengerSQL);

                if (rowsAffectedPassenger == 0)
                {
                    throw new Exception($"No records found in PASSENGER table for Passenger_ID = {passengerID}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting passenger: {ex.Message}");
            }
        }

    }


}
