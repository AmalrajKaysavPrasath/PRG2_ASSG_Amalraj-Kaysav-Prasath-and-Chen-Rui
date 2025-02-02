// See https://aka.ms/new-console-template for more information
using PRG_Assignment_2_Chen_Rui;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

Console.WriteLine("Hello, World!");

Dictionary<string, Flight> FlightDict = new Dictionary<string, Flight>();
Dictionary<string, Airline> AirlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> BoardingGateDict = new Dictionary<string, BoardingGate>();

string[] csvLines = File.ReadAllLines("flights.csv");

string[] heading = csvLines[0].Split(',');
void InitialiseFlightLoading(Dictionary<string, Flight> FlightDict)
{
    for (int i = 1; i < csvLines.Length; i++)
    {
        string[] data = csvLines[i].Split(',');
        Flight flight = new Flight(data[0], data[1], data[2], Convert.ToDateTime(data[3]), "On Time", data[4]);
        FlightDict.Add(data[0], flight);
    }
}

InitialiseFlightLoading(FlightDict);

string[] csvLines2 = File.ReadAllLines("airlines.csv");

string[] heading2 = csvLines2[0].Split(',');
void InitialiseAirlineLoading(Dictionary<string, Airline> AirlineDict)
{
    for (int i = 1; i < csvLines2.Length; i++)
    {
        string[] data = csvLines2[i].Split(',');
        Airline airline = new Airline(data[0], data[1], new Dictionary<string, Flight>());
        AirlineDict.Add(data[0], airline);
    }
}

InitialiseAirlineLoading(AirlineDict);

string[] csvLines3 = File.ReadAllLines("boardinggates.csv");

string[] heading3 = csvLines3[0].Split(',');
void InitialiseBoardingGateLoading(Dictionary<string, BoardingGate> BoardingGateDict)
{
    for (int i = 1; i < csvLines3.Length; i++)
    {
        string[] data = csvLines3[i].Split(',');
        BoardingGate boardinggate = new BoardingGate(data[0], Convert.ToBoolean(data[1]), Convert.ToBoolean(data[2]), Convert.ToBoolean(data[3]), null);
        BoardingGateDict.Add(data[0], boardinggate);
    }
}

InitialiseBoardingGateLoading(BoardingGateDict);

void DisplayFlights(Dictionary<string, Flight> FlightDict)
{
    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
    "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

    foreach (KeyValuePair<string, Flight> entry in FlightDict)
    {
        Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
         entry.Value.FlightNumber, "Unassigned", entry.Value.Origin, entry.Value.Destination, entry.Value.ExpectedTime);
    }
}

DisplayFlights(FlightDict);

void AssignBoardingGate()
{
    while (true)
    {
        Console.Write("What is the Flight Number you would like to select: ");
        string flightselect = Console.ReadLine();
        foreach (KeyValuePair<string, Flight> entry in FlightDict)
        {
            if (entry.Value.FlightNumber == flightselect)
            {
                Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
                entry.Value.FlightNumber, "Unassigned", entry.Value.Origin, entry.Value.Destination, entry.Value.ExpectedTime);
            }
        }

        Console.Write("What is the Boarding Gate you would like to select: ");
        string boardinggateselect = Console.ReadLine();
        foreach (KeyValuePair<string, BoardingGate> entry in BoardingGateDict)
        {
            if (entry.Value.GateName == boardinggateselect)
            {
                if (entry.Value.Flight == null)
                {
                    Console.WriteLine("The Boarding Gate is Avaliable.");

                    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}",
                    "Gate Name", "Suppoprt CFFT?", "Support DJJB?", "Support LWTT?");

                    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}",
                    entry.Value.GateName, entry.Value.SupportsCFFT, entry.Value.SupportsDJJB, entry.Value.SupportsLWTT);

                    Console.Write("Would you like to change the status of the Flight? (Y/N) ");

                    string answer = Console.ReadLine();

                    if (answer == "Y")
                    {
                        Console.Write("What would you like to change the status to? ");
                        string new_status = Console.ReadLine();
                        foreach (KeyValuePair<string, Flight> entry2 in FlightDict)
                        {
                            if (entry2.Value.FlightNumber == flightselect)
                            {
                                entry2.Value.Status = new_status;
                            }
                        }
                    }

                    foreach (KeyValuePair<string, Flight> entry2 in FlightDict)
                    {
                        if (entry2.Value.FlightNumber == flightselect)
                        {
                            entry.Value.Flight = entry2.Value;
                        }
                    }

                    Console.WriteLine("Assigned Successfully.");
                }
                else
                {
                    Console.WriteLine("The Boarding Gate is already assigned");
                    break;
                }
            }
        }
        break;
    }
}

AssignBoardingGate();
void CreateFlight()
{
    while (true)
    {
        Console.Write("What is the Flight Number of the Flight you would like to add? ");
        string newflightnum = Console.ReadLine();

        Console.Write("What is the Origin of the Flight you would like to add? ");
        string newflightorigin = Console.ReadLine();

        Console.Write("What is the Destination of the Flight you would like to add? ");
        string newflightdestination = Console.ReadLine();

        Console.Write("What is the Time of Arrival/Departure of the Flight you would like to add? ");
        string newflighttime = Console.ReadLine();

        Console.Write("Is there a special request to the Flight you would like to add? ");
        string newflightreq = Console.ReadLine();

        Flight flight = new Flight(newflightnum, newflightorigin, newflightdestination, Convert.ToDateTime(newflighttime), "On Time", newflightreq);

        FlightDict.Add(newflightnum, flight);

        Console.WriteLine("Flight has been added");

        Console.Write("Would you like to continue adding another flight? (Y/N) ");
        if (Console.ReadLine() == "N")
        {
            break;
        }
    }
}

var sortedByKey = FlightDict.OrderBy(KeyValuePair => KeyValuePair.Key).ToList();
foreach (var pair in sortedByKey)
{
    Console.WriteLine($"{pair.Key}: {pair.Value}");
}