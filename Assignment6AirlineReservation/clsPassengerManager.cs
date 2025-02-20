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
                // Execute the SQL query from clsSQL
                clsDataAccess db = new clsDataAccess();
                int iRetVal = 0;
                DataSet ds = db.ExecuteSQLStatement(clsSQL.GetPassengers(sFlightID), ref iRetVal);

                // Convert DataSet to List of clsPassenger
                List<clsPassenger> passengers = new List<clsPassenger>();
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
                throw new Exception($"{nameof(clsPassengerManager)}.{nameof(GetPassengers)} -> {ex.Message}");
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
                clsDataAccess db = new clsDataAccess();

                // Insert new passenger
                db.ExecuteNonQuery(clsSQL.InsertPassenger(firstName, lastName));

                // Get the ID of the newly inserted passenger
                int iRetVal = 0;
                DataSet ds = db.ExecuteSQLStatement(clsSQL.GetLatestPassengerID(), ref iRetVal);
                string newPassengerID = ds.Tables[0].Rows[0]["NewPassengerID"].ToString();

                // Link passenger to flight and assign a seat
                db.ExecuteNonQuery(clsSQL.LinkPassengerToFlight(flightID, newPassengerID, seatNumber));
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsPassengerManager)}.{nameof(AddPassenger)} -> {ex.Message}");
            }
        }
        public static void UpdatePassengerSeat(string passengerID, string newSeatNumber)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();
                db.ExecuteNonQuery(clsSQL.UpdatePassengerSeat(passengerID, newSeatNumber));
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsPassengerManager)}.{nameof(UpdatePassengerSeat)} -> {ex.Message}");
            }
        }

        public static void DeletePassenger(string passengerID)
        {
            try
            {
                clsDataAccess db = new clsDataAccess();

                int rowsAffectedLink = db.ExecuteNonQuery(clsSQL.DeletePassengerLink(passengerID));
                if (rowsAffectedLink == 0)
                {
                    throw new Exception($"No records found in FLIGHT_PASSENGER_LINK for Passenger_ID = {passengerID}.");
                }

                int rowsAffectedPassenger = db.ExecuteNonQuery(clsSQL.DeletePassenger(passengerID));
                if (rowsAffectedPassenger == 0)
                {
                    throw new Exception($"No records found in PASSENGER table for Passenger_ID = {passengerID}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(clsPassengerManager)}.{nameof(DeletePassenger)} -> {ex.Message}");
            }
        }
    }
}
