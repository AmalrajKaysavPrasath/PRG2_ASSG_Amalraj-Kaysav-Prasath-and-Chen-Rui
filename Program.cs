// See https://aka.ms/new-console-template for more information
using PRG_Assg2_Kaysav;
using System;
//LoadAirlines() 
static Dictionary<string, Airline> LoadAirlines(string filePath)
{
    var airlineDictionary = new Dictionary<string, Airline>();

    try
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))  // Skip header
        {
            var columns = line.Split(',');
            string name = columns[0].Trim();
            string code = columns[1].Trim();

            airlineDictionary[code] = new Airline { Code = code, Name = name };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading the CSV file: {ex.Message}");
    }

    return airlineDictionary;
}

//LoadBoardingGates()
static Dictionary<string, BoardingGate> LoadBoardingGates(string filePath)
{
    Dictionary<string, BoardingGate> gates = new Dictionary<string, BoardingGate>();

    try
    {
        var lines = File.ReadLines(filePath).Skip(1); // Skip header
        foreach (var line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 4)
            {
                // meenakshi this part wrong, look at csv file and see the order of special request codes
                string name = parts[0].Trim();
                bool supportsCFFT = parts[1].Trim().ToLower() == "true";
                bool supportsDDJB = parts[2].Trim().ToLower() == "true";
                bool supportsLWTT = parts[3].Trim().ToLower() == "true";
                gates[name] = new BoardingGate(name, supportsCFFT, supportsDDJB, supportsLWTT);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading boarding gates: " + ex.Message);
    }
    return gates;
}

//loadFlights 
static Dictionary<string, Flight> LoadFlights(string filename)
{
    Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

    using (StreamReader sr = new StreamReader(filename))
    {
        string? s = sr.ReadLine(); //heading
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(",");
            string flightNumber = parts[0].Trim();
            string origin = parts[1].Trim();
            string destination = parts[2].Trim();
            DateTime expectedTime = Convert.ToDateTime(parts[3].Trim());
            string request = parts[4].Trim();
            if (request == "")
            {
                Flight flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
            else if (request == "LWTT")
            {
                Flight flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
            else if (request == "DDJB")
            {
                Flight flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
            else if (request == "CFFT")
            {
                Flight flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
        }
    }
    return flights;
}
//Basic feature 4
static void ListBoardingGates(Dictionary<string, BoardingGate> gates)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-10} {"DDJB",-6} {"CFFT",-6} {"LWTT",-6} {"Assigned Flight",-15}");

    foreach (var gate in gates.Values)
    {
        string flight = "None";  // Default to "None" if no flight is assigned

        if (gate.Flight != null)
        {
            flight = gate.Flight.FlightNumber;
        }
        Console.WriteLine($"{gate.GateName,-10} {gate.SupportsDDJB,-6} {gate.SupportsCFFT,-6} {gate.SupportsLWTT,-6} {flight,-15}");
    }

    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}



//basic feature 7
static void DisplayAirlineFlights(Dictionary<string, Airline> airlineDictionary, Dictionary<string, Flight> flightDictionary)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-15} {"Airline Name",-30}");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-15} {airline.Name,-30}");
    }

    // Get user input for airline code
    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    // Check if the airline code exists in the dictionary
    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        // Display flights for the selected airline
        Console.WriteLine("\n=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-20} {"Origin",-20} {"Destination",-20} {"Expected Departure/Arrival Time",-30}");

        // Get the list of flights for the selected airline (convert AirlineCode to uppercase to avoid case mismatch)
        List<Flight> airlineFlights = new List<Flight>();
        foreach (KeyValuePair<string, Flight> flights in flightDictionary)
        {
            string Fobject = flights.Key;
            if (Fobject.StartsWith(airlineCode))
            {
                airlineFlights.Add(flights.Value);
            }
        }

        // Check if there are any flights for the selected airline
        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            // Display each flight's details
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine($"{flight.FlightNumber,-15} {selectedAirline.Name,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"),-30}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Airline Code.");
    }
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}


//basic feature 8 

static void ModifyFlightDetails(Dictionary<string, Airline> airlineDictionary, Dictionary<string, Flight> flightDictionary)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-15} {"Airline Name",-30}");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-15} {airline.Name,-30}");
    }

    // Get user input for airline code
    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
        Console.WriteLine("Flight Number  Airline Name  Origin          Destination      Expected ");

        // Get the list of flights for the selected airline (convert AirlineCode to uppercase to avoid case mismatch)
        List<Flight> airlineFlights = new List<Flight>();
        foreach (KeyValuePair<string, Flight> flights in flightDictionary)
        {
            string Fobject = flights.Key;
            if (Fobject.StartsWith(airlineCode))
            {
                airlineFlights.Add(flights.Value);
            }
        }

        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine(flight.FlightNumber + "  " + selectedAirline.Name + "  " + flight.Origin + "  " + flight.Destination + "  " + flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"));
            }

            Console.WriteLine("\nChoose an existing Flight to modify or delete:");
            string flightNumber = Console.ReadLine().ToUpper();

            var selectedFlight = airlineFlights.FirstOrDefault(f => f.FlightNumber == flightNumber);

            if (selectedFlight != null)
            {
                Console.WriteLine("\n1. Modify Flight");
                Console.WriteLine("2. Delete Flight");
                Console.Write("Choose an option: ");
                string modifyOption = Console.ReadLine();

                if (modifyOption == "1")
                {
                    Console.WriteLine("\n1. Modify Basic Information");
                    Console.WriteLine("2. Modify Status");
                    Console.WriteLine("3. Modify Special Request Code");
                    Console.WriteLine("4. Modify Boarding Gate");
                    Console.Write("Choose an option: ");
                    string modificationOption = Console.ReadLine();

                    switch (modificationOption)
                    {
                        case "1":
                            Console.Write("\nEnter new Origin: ");
                            selectedFlight.Origin = Console.ReadLine();
                            Console.Write("Enter new Destination: ");
                            selectedFlight.Destination = Console.ReadLine();
                            Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy hh:mm tt): ");
                            selectedFlight.ExpectedTime = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy hh:mm tt", null);
                            break;
                        case "2":
                            Console.Write("\nEnter new Status: ");
                            selectedFlight.Status = Console.ReadLine();
                            break;
                        case "3":
                            Console.Write("\nEnter new Special Request Code: ");
                            selectedFlight.SpecialRequestCode = Console.ReadLine();
                            break;
                        case "4":
                            Console.Write("\nEnter new Boarding Gate: ");
                            string gateName = Console.ReadLine();

                            // Create a new BoardingGate object
                            selectedFlight.BoardingGate = new BoardingGate
                            {
                                GateName = gateName,
                                SupportsCFFT = true,
                                SupportsDDJB = true,
                                SupportsLWTT = true
                            };

                            break;
                        default:
                            Console.WriteLine("Invalid option. Returning to menu...");
                            break;
                    }

                    // Display updated flight details
                    Console.WriteLine("\nFlight updated!");
                    Console.WriteLine("=============================================");
                    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
                    Console.WriteLine($"Origin: {selectedFlight.Origin}");
                    Console.WriteLine($"Destination: {selectedFlight.Destination}");
                    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
                    Console.WriteLine($"Status: {selectedFlight.Status}");
                    Console.WriteLine($"Special Request Code: {selectedFlight.SpecialRequestCode ?? "None"}");
                    Console.WriteLine($"Boarding Gate: {selectedFlight.BoardingGate?.GateName ?? "Unassigned"}");
                }
                else if (modifyOption == "2")
                {
                    Console.WriteLine("\nAre you sure you want to delete this flight? [Y/N]");
                    string confirmation = Console.ReadLine().ToUpper();

                    if (confirmation == "Y")
                    {
                        // Remove the flight from the airline's flight dictionary
                        selectedAirline.RemoveFlight(selectedFlight);
                        Console.WriteLine("Flight has been deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Flight deletion canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Returning to menu...");
                }
            }
            else
            {
                Console.WriteLine("Flight not found.");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Airline Code.");
    }

    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}



//Advanced Feature (1)
static void ProcessUnassignedFlights(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates)
{
    Console.WriteLine("==================================================");
    Console.WriteLine("Processing Unassigned Flights");
    Console.WriteLine("==================================================");
    Queue<Flight> unassignedFlights = new Queue<Flight>(flights.Values.Where(f => f.BoardingGate == null));
    int unassignedFlightsCount = unassignedFlights.Count;
    int unassignedGatesCount = boardingGates.Values.Count(g => g.Flight == null);
    Console.WriteLine($"Total unassigned flights: {unassignedFlightsCount}");
    Console.WriteLine($"Total unassigned boarding gates: {unassignedGatesCount}");
    Console.WriteLine("--------------------------------------------------");

    int processedCount = 0;
    int assignedCount = 0;

    Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-25} {4,-15} {5,-10}",
        "Flight", "Origin", "Destination", "Time", "Special Request", "Gate");

    while (unassignedFlights.Count > 0)
    {
        Flight flight = unassignedFlights.Dequeue();
        BoardingGate matchingGate = FindMatchingGate(flight, boardingGates);

        if (matchingGate != null)
        {
            matchingGate.Flight = flight;
            flight.BoardingGate = matchingGate;

            assignedCount++;
        }

        processedCount++;

        Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-25:dd/MM/yyyy HH:mm} {4,-15} {5,-10}",
         flight.FlightNumber,
         flight.Origin,
         flight.Destination,
         flight.ExpectedTime,
         flight.SpecialRequestCode ?? "N/A",
         flight.BoardingGate?.GateName ?? "Not Assigned");  // Checking if BoardingGate is null

    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"Total flights processed: {processedCount}");
    Console.WriteLine($"Total gates assigned: {assignedCount}");
    double automaticAssignmentPercentage = (double)assignedCount / flights.Count * 100;
    Console.WriteLine($"Percentage of flights automatically assigned: {automaticAssignmentPercentage:F2}%");
}

static BoardingGate FindMatchingGate(Flight flight, Dictionary<string, BoardingGate> boardingGates)
{
    if (!string.IsNullOrEmpty(flight.SpecialRequestCode))
    {
        return boardingGates.Values.FirstOrDefault(g => g.Flight == null &&
            ((flight.SpecialRequestCode == "CFFT" && g.SupportsCFFT) ||
             (flight.SpecialRequestCode == "DDJB" && g.SupportsDDJB) ||
             (flight.SpecialRequestCode == "LWTT" && g.SupportsLWTT)));
    }
    else
    {
        return boardingGates.Values.FirstOrDefault(g => g.Flight == null && !g.SupportsCFFT && !g.SupportsDDJB && !g.SupportsLWTT);
    }
}



//PROGRAM CODE

//Basic Feature 1 
// Load files and initialize dictionaries
Console.WriteLine("Loading Airlines. . .");
Dictionary<string, Airline> airlineDictionary = LoadAirlines("airlines.csv");
Console.WriteLine("{0} Airlines Loaded!", airlineDictionary.Count);

Dictionary<string, Flight> flightDictionary = LoadFlights("flights.csv");
Console.WriteLine("{0} Flights Loaded!", flightDictionary.Count);

Console.WriteLine("Loading Boarding Gates. . .");
Dictionary<string, BoardingGate> boardingGateDictionary = LoadBoardingGates("boardinggates.csv");
Console.WriteLine("{0} Boarding Gates Loaded!", boardingGateDictionary.Count);


// Define and initialize the flights and boardingGates dictionaries
Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();



// Menu Loop
bool flag = true;
do
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Display Unassigned flights");
    Console.WriteLine("0. Exit");
    Console.Write("\nPlease select your option: ");

    string option = Console.ReadLine();

    while (option != "1" && option != "2" && option != "3" && option != "4" && option != "5" && option != "6" && option != "7" && option != "0")
    {
        Console.WriteLine("\nInput invalid. Try Again.");
        Console.Write("Please select your option: ");
        option = Console.ReadLine();
    }
static void ListAllFlights()
    {

    }
static void AssignBoardingGate()
    {

    }
static void CreateFlight()
    {

    }
static void DisplayFlightSchedule()
    {

    }

    if (option == "1")
        ListAllFlights();
    else if (option == "2")
        ListBoardingGates(boardingGateDictionary);
    else if (option == "3")
        AssignBoardingGate();
    else if (option == "4")
        CreateFlight();
    else if (option == "5")
        DisplayAirlineFlights(airlineDictionary, flightDictionary);
    else if (option == "6")
        ModifyFlightDetails(airlineDictionary, flightDictionary);
    else if (option == "7")
        DisplayFlightSchedule();
    else if (option == "8")
        ProcessUnassignedFlights(flights, boardingGates);
    else
    {
        Console.WriteLine("Goodbye!");
        flag = false;
    }

} while (flag == true);
