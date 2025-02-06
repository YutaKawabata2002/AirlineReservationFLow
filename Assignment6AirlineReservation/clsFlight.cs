using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Class to represent a flight in the airline reservation system
    /// </summary>
    internal class clsFlight
    {
        /// <summary>
        /// The unique identifier for the flight
        /// </summary>
        public string sFlightID { get; set; }

        /// <summary>
        /// The flight number
        /// </summary>
        public string FlightNumber { get; set; }

        /// <summary>
        /// The type of aircraft used for this flight
        /// </summary>
        public string AircraftType { get; set; }

        /// <summary>
        /// Returns a string representation of the flight
        /// </summary>
        /// <returns>String containing flight number and aircraft type</returns>
        public override string ToString()
        {
            return $"{FlightNumber} - {AircraftType}";
        }
    }
}
