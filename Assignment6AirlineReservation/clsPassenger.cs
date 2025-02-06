using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Class to represent a passenger in the airline reservation system
    /// </summary>
    internal class clsPassenger
    {
        /// <summary>
        /// The unique identifier for the passenger
        /// </summary>
        public string PassengerID { get; set; }

        /// <summary>
        /// The passenger's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The passenger's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The seat number assigned to the passenger
        /// </summary>
        public string SeatNumber { get; set; }

        /// <summary>
        /// Returns a string representation of the passenger
        /// </summary>
        /// <returns>String containing passenger's full name</returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}