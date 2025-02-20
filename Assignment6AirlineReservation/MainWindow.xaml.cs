using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        clsDataAccess clsData;
        wndAddPassenger wndAddPass;

        bool bAddPassengerMode;
        bool bChnageSeatMode;

        /// <summary>
        /// Constructor initializes the UI components and loads flight data.
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                cbChooseFlight.ItemsSource = clsFlightManager.GetFlights(); //get Flights information using the function in clsFlightManager class

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles selection of a flight from the dropdown.
        /// Updates passenger information and seat map visibility.
        /// </summary>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get the selected flight
                clsFlight selectedFlight = (clsFlight)cbChooseFlight.SelectedItem;

                if (selectedFlight == null)
                    return;

                cbChoosePassenger.IsEnabled = true;
                gPassengerCommands.IsEnabled = true;

                // Toggle visibility of canvases based on flight type
                if (selectedFlight.sFlightID == "1")
                {
                    CanvasA380.Visibility = Visibility.Hidden;
                    Canvas767.Visibility = Visibility.Visible;
                }
                else
                {
                    Canvas767.Visibility = Visibility.Hidden;
                    CanvasA380.Visibility = Visibility.Visible;
                }

                // Get passengers for the selected flight and update the passengers combo box
                cbChoosePassenger.ItemsSource = clsPassengerManager.GetPassengers(selectedFlight.sFlightID);
                cbChoosePassenger.IsEnabled = true;

                // Call FillPassengers to update seat colors
                FillPassengers(selectedFlight);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles passenger selection and highlights the selected passenger's seat.
        /// Updates seat colors and seat number display.
        /// </summary>
        private void cbChoosePassenger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get the selected passenger
                clsPassenger selectedPassenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                if (selectedPassenger == null)
                {
                    // Clear the seat number display if no passenger is selected
                    lblPassengersSeatNumber.Content = string.Empty;
                    return;
                }

                // Determine the canvas for the selected flight
                clsFlight selectedFlight = (clsFlight)cbChooseFlight.SelectedItem;
                Canvas seatCanvas = selectedFlight.sFlightID == "1" ? cA380_Seats : c767_Seats;

                // Reset all seats to blue (available)
                foreach (Label seat in seatCanvas.Children.OfType<Label>())
                {
                    seat.Background = Brushes.Blue;
                }

                // Fill passengers' seats as red (taken)
                List<clsPassenger> passengers = clsPassengerManager.GetPassengers(selectedFlight.sFlightID);
                foreach (var passenger in passengers)
                {
                    foreach (Label seat in seatCanvas.Children.OfType<Label>())
                    {
                        if (seat.Content != null && seat.Content.ToString() == passenger.SeatNumber)
                        {
                            seat.Background = Brushes.Red;
                            break;
                        }
                    }
                }

                // Highlight the selected passenger's seat if assigned
                if (!string.IsNullOrEmpty(selectedPassenger.SeatNumber))
                {
                    foreach (Label seat in seatCanvas.Children.OfType<Label>())
                    {
                        if (seat.Content != null && seat.Content.ToString() == selectedPassenger.SeatNumber)
                        {
                            seat.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00FD00"));
                            break;
                        }
                    }

                    // Display the assigned seat number
                    lblPassengersSeatNumber.Content = selectedPassenger.SeatNumber;
                }
                else
                {
                    // Clear the seat number if none is assigned
                    lblPassengersSeatNumber.Content = string.Empty;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Handles errors by displaying a message box and logging the error to a file.
        /// </summary>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }

        //FillPassengers
        //Reset all seats in the selected flights to blue
        //loop through each passenger in the list
        //then loop through each seat in the selected flight Like "c767_Seats.Children"
        //then compare the passengers seat to the label's content and if then match then change the background to red because the seat is taken
        //

        /// <summary>
        /// Updates the seat map with passenger assignments for the selected flight.
        /// Marks all occupied seats as red.
        /// </summary>
        private void FillPassengers(clsFlight selectedFlight)
        {
            try
            {
                // Toggle visibility of canvases based on the selected flight
                if (selectedFlight.sFlightID == "1")
                {
                    Canvas767.Visibility = Visibility.Collapsed;
                    CanvasA380.Visibility = Visibility.Visible;

                }
                else
                {
                    CanvasA380.Visibility = Visibility.Collapsed;
                    Canvas767.Visibility = Visibility.Visible;
                }

                // Determine the canvas for the selected flight
                Canvas seatCanvas = selectedFlight.sFlightID == "1" ? cA380_Seats : c767_Seats;

                // Ensure the selected canvas is rendered
                seatCanvas.UpdateLayout();

                // Reset all seats in the selected flight to blue
                foreach (Label seat in seatCanvas.Children.OfType<Label>())
                {
                    seat.Background = Brushes.Blue; // Reset seat to available (blue)
                }

                // Get the list of passengers for the selected flight
                List<clsPassenger> passengers = clsPassengerManager.GetPassengers(selectedFlight.sFlightID);

                // Loop through each passenger
                foreach (var passenger in passengers)
                {
                    // Loop through each seat in the selected flight
                    foreach (Label seat in seatCanvas.Children.OfType<Label>())
                    {
                        // Compare the passenger's seat number to the label's content
                        if (seat.Content != null && seat.Content.ToString() == passenger.SeatNumber)
                        {
                            // Change the background to red because the seat is taken
                            seat.Background = Brushes.Red;
                            break; // Exit the loop once the match is found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Opens the Add Passenger window and prompts the user to assign a seat.
        /// </summary>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open the Add Passenger window
                wndAddPass = new wndAddPassenger();
                bool? result = wndAddPass.ShowDialog(); // Show the dialog and get the result

                // Check if the user clicked "Save" in the Add Passenger window
                if (result == true) // This means Save was clicked
                {
                    // Retrieve passenger details from the Add Passenger window
                    string firstName = wndAddPass.FirstName;
                    string lastName = wndAddPass.LastName;

                    // Check if a flight is selected
                    clsFlight selectedFlight = (clsFlight)cbChooseFlight.SelectedItem;
                    if (selectedFlight == null)
                    {
                        MessageBox.Show("Please select a flight before adding a passenger.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Store the details temporarily
                    wndAddPass.Tag = new clsPassenger
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        SeatNumber = null // Seat will be assigned later
                    };

                    // Set Add Passenger mode to allow seat selection
                    bAddPassengerMode = true;

                    // Disable other controls to enforce seat selection
                    cbChooseFlight.IsEnabled = false;
                    cbChoosePassenger.IsEnabled = false;
                    gPassengerCommands.IsEnabled = false;

                    // Inform the user to select a seat
                    MessageBox.Show("Please select a seat for the new passenger.");
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Enables Change Seat mode, allowing the user to select a new seat for the passenger.
        /// </summary>
        private void cmdChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a passenger is selected
                if (cbChoosePassenger.SelectedItem == null)
                {
                    MessageBox.Show("Please select a passenger to change their seat.");
                    return;
                }

                // Lock down the window to force seat selection
                bChnageSeatMode = true;
                cbChooseFlight.IsEnabled = false;
                cbChoosePassenger.IsEnabled = false;
                gPassengerCommands.IsEnabled = false;

                // Inform the user to select a new seat
                MessageBox.Show("Please select a new seat for the passenger.");
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles seat selection from the seat map during Add Passenger or Change Seat modes.
        /// </summary>
        private void Seat_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Get the clicked seat
                Label clickedSeat = (Label)sender;
                string seatNumber = clickedSeat.Content.ToString(); // Extract seat number

                // Add Passenger Mode
                if (bAddPassengerMode)
                {
                    if (clickedSeat.Background == Brushes.Blue) // Check if the seat is available
                    {
                        // Retrieve passenger details from the Tag property
                        if (wndAddPass.Tag is clsPassenger newPassenger)
                        {
                            string flightID = ((clsFlight)cbChooseFlight.SelectedItem).sFlightID;

                            // Insert the new passenger into the database and assign the seat
                            clsPassengerManager.AddPassenger(newPassenger.FirstName, newPassenger.LastName, flightID, seatNumber);

                            // Mark the seat as green to indicate the selected passenger
                            clickedSeat.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00FD00"));

                            // Refresh the passenger list to include the new passenger
                            List<clsPassenger> passengers = clsPassengerManager.GetPassengers(flightID);
                            cbChoosePassenger.ItemsSource = passengers;

                            // Select the newly added passenger
                            clsPassenger addedPassenger = passengers.LastOrDefault();
                            if (addedPassenger != null)
                            {
                                cbChoosePassenger.SelectedItem = addedPassenger; // Highlight in dropdown
                                lblPassengersSeatNumber.Content = addedPassenger.SeatNumber; // Display seat number
                            }

                            MessageBox.Show("Passenger added successfully!");

                            // Reset Add Passenger mode and re-enable controls
                            bAddPassengerMode = false;
                            cbChooseFlight.IsEnabled = true;
                            cbChoosePassenger.IsEnabled = true;
                            gPassengerCommands.IsEnabled = true;

                            // Refresh the seat map
                            FillPassengers((clsFlight)cbChooseFlight.SelectedItem);
                        }
                        else
                        {
                            MessageBox.Show("Error: Passenger details are missing. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("This seat is already taken. Please select an available seat.");
                    }
                }

                // Change Seat Mode
                else if (bChnageSeatMode)
                {
                    if (clickedSeat.Background == Brushes.Blue) // Check if the seat is available
                    {
                        // Get the selected passenger
                        clsPassenger selectedPassenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                        if (selectedPassenger == null)
                        {
                            MessageBox.Show("No passenger is selected. Please select a passenger first.");
                            return;
                        }

                        // Save the old seat number
                        string oldSeatNumber = selectedPassenger.SeatNumber;

                        // Update the passenger's seat in the database
                        clsPassengerManager.UpdatePassengerSeat(selectedPassenger.PassengerID, seatNumber);

                        // Update the passenger object in memory
                        selectedPassenger.SeatNumber = seatNumber;

                        // Clear the old seat in the UI
                        Canvas seatCanvas = ((clsFlight)cbChooseFlight.SelectedItem).sFlightID == "1" ? c767_Seats : cA380_Seats;
                        foreach (Label seat in seatCanvas.Children.OfType<Label>())
                        {
                            if (seat.Content != null && seat.Content.ToString() == oldSeatNumber)
                            {
                                seat.Background = Brushes.Blue; // Mark the old seat as available
                                break;
                            }
                        }

                        // Mark the new seat as occupied
                        clickedSeat.Background = Brushes.Red;

                        // Refresh the seat number display
                        lblPassengersSeatNumber.Content = seatNumber;

                        MessageBox.Show("Seat updated successfully!");

                        // Reset Change Seat mode and re-enable controls
                        bChnageSeatMode = false;
                        cbChooseFlight.IsEnabled = true;
                        cbChoosePassenger.IsEnabled = true;
                        gPassengerCommands.IsEnabled = true;

                        // Refresh the passenger list and seats
                        FillPassengers((clsFlight)cbChooseFlight.SelectedItem);
                    }
                    else
                    {
                        MessageBox.Show("This seat is already taken. Please select an available seat.");
                    }
                }
                // Regular Seat Selection
                else
                {
                    if (clickedSeat.Background == Brushes.Red) // Check if the seat is occupied
                    {
                        // Find the passenger associated with the clicked seat
                        foreach (clsPassenger passenger in cbChoosePassenger.Items)
                        {
                            if (passenger.SeatNumber == seatNumber)
                            {
                                cbChoosePassenger.SelectedItem = passenger; // Select the passenger in the dropdown
                                lblPassengersSeatNumber.Content = seatNumber; // Display their seat number
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This seat is currently empty.");
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the selected passenger and updates the UI.
        /// </summary>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ensure a passenger is selected
                if (cbChoosePassenger.SelectedItem == null)
                {
                    MessageBox.Show("Please select a passenger to delete.", "No Passenger Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get the selected passenger
                clsPassenger selectedPassenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                // Confirm deletion
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete {selectedPassenger.FirstName} {selectedPassenger.LastName}?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Delete the passenger using the clsPassengerManager class
                    clsPassengerManager.DeletePassenger(selectedPassenger.PassengerID);

                    // Refresh the passenger combo box
                    clsFlight selectedFlight = (clsFlight)cbChooseFlight.SelectedItem;
                    if (selectedFlight != null)
                    {
                        cbChoosePassenger.ItemsSource = clsPassengerManager.GetPassengers(selectedFlight.sFlightID);
                    }

                    // Refresh seat display
                    FillPassengers(selectedFlight);

                    // Clear the seat number display
                    lblPassengersSeatNumber.Content = string.Empty;

                    MessageBox.Show("Passenger deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

    }
}
