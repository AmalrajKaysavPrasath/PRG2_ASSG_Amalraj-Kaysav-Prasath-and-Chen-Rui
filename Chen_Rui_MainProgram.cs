using PRG_Assignment_2_Chen_Rui;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

Console.WriteLine("Program Start!");

Dictionary<string, Flight> FlightDict = new Dictionary<string, Flight>();
Dictionary<string, Airline> AirlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> BoardingGateDict = new Dictionary<string, BoardingGate>();

string[] flightcsvLines = File.ReadAllLines("flights.csv");

string[] flightheading = flightcsvLines[0].Split(',');
void InitialiseFlightLoading(Dictionary<string, Flight> FlightDict)
{
    for (int i = 1; i < flightcsvLines.Length; i++)
    {
        string[] data = flightcsvLines[i].Split(',');
        Flight flight = new Flight(data[0], data[1], data[2], Convert.ToDateTime(data[3]), "On Time", data[4]);
        FlightDict.Add(data[0], flight);
    }
}

InitialiseFlightLoading(FlightDict);

string[] airlinecsvLines = File.ReadAllLines("airlines.csv");

string[] airlineheading = airlinecsvLines[0].Split(',');
void InitialiseAirlineLoading(Dictionary<string, Airline> AirlineDict)
{
    for (int i = 1; i < airlinecsvLines.Length; i++)
    {
        string[] data = airlinecsvLines[i].Split(',');
        Airline airline = new Airline(data[0], data[1], new Dictionary<string, Flight>());
        AirlineDict.Add(data[0], airline);
    }
}

InitialiseAirlineLoading(AirlineDict);

string[] gatecsvLines = File.ReadAllLines("boardinggates.csv");

string[] gateheading = gatecsvLines[0].Split(',');
void InitialiseBoardingGateLoading(Dictionary<string, BoardingGate> BoardingGateDict)
{
    for (int i = 1; i < gatecsvLines.Length; i++)
    {
        string[] data = gatecsvLines[i].Split(',');
        BoardingGate boardinggate = new BoardingGate(data[0], Convert.ToBoolean(data[1]), Convert.ToBoolean(data[2]), Convert.ToBoolean(data[3]), null);
        BoardingGateDict.Add(data[0], boardinggate);
    }
}

InitialiseBoardingGateLoading(BoardingGateDict);

void DisplayFlights(Dictionary<string, Flight> FlightDict) //List All Flights With Their Basic Information
{
    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
    "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

    foreach (KeyValuePair<string, Flight> entry in FlightDict)
    {
        Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
         entry.Value.FlightNumber, "Unassigned", entry.Value.Origin, entry.Value.Destination, entry.Value.ExpectedTime);
    }
}

void DisplayBoardingGates(Dictionary<string, BoardingGate> BoardingGateDict) //List All Boarding Gates
{
    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
    "Gate Name", "Support CFFT?", "Support DJJB?", "Support LWTT?", "Assigned Flight");

    foreach (KeyValuePair<string, BoardingGate> entry in BoardingGateDict)
    {
        string supportCFFT = "No";
        string supportDJJB = "No";
        string supportLWTT = "No";
        string assignedFlight = "";

        if (entry.Value.SupportsCFFT == true)
        {
            supportCFFT = "Yes";
        }

        if (entry.Value.SupportsDJJB == true)
        {
            supportDJJB = "Yes";
        }

        if (entry.Value.SupportsLWTT == true)
        {
            supportLWTT = "Yes";
        }

        if (entry.Value.Flight == null)
        {
            assignedFlight = "Unassigned";
        }

        else
        {
            assignedFlight = entry.Value.Flight.FlightNumber;
        }

        Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-30}",
         entry.Value.GateName, supportCFFT, supportDJJB, supportLWTT, assignedFlight);
    }
}

void AssignBoardingGate() //Assign A Boarding Gate To A Flight
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
            }
        }
    }
}

void CreateFlight() //Create A New Flight
{
    string continueflightcreation = "Y";
    while (continueflightcreation == "Y")
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
        if (Console.ReadLine() == "Y")
        {
            continueflightcreation = "Y";
        }
        else
        {
            continueflightcreation = "N";
        }
    }
}


void ArrangeFlight()
{
    var sortedByKey = FlightDict.OrderBy(KeyValuePair => KeyValuePair.Value.ExpectedTime).ToList();
    foreach (var pair in sortedByKey)
    {
        Console.WriteLine("{0,-15} {1,-15} {2,-20} {3,-20} {4,-10} {5,-30} ",
                "Flight Number", "Origin", "Destination", "Expected Time", "Status", "Special Request Code");

        Console.WriteLine("{0,-15} {1,-15} {2,-20} {3,-20} {4,-10} {5,-30} ",
        pair.Key, pair.Value.Origin, pair.Value.Destination, pair.Value.ExpectedTime, pair.Value.Status, pair.Value.SpecialReq);
    }
}

string contdisplay = "Y";

void DisplayMenu()
{
    Console.WriteLine("-------------------MENU-------------------");
    Console.WriteLine("1) List All Flights");
    Console.WriteLine("2) List All Boarding Gates");
    Console.WriteLine("3) Assign Boarding Gate To Flight");
    Console.WriteLine("4) Create A New Flight");
    Console.WriteLine("5) Display Airline Flight Details");
    Console.WriteLine("6) Modify Flight Details");
    Console.WriteLine("7) Arrange Flights In Order");
    Console.WriteLine("8) Assign All Flights To Gate");
    Console.WriteLine("9) Display Airline Total Fee");
    Console.WriteLine("0) Exit Program");
    Console.WriteLine("-------------------MENU-------------------");
    Console.Write("Enter Choice: ");
    string choice = Console.ReadLine();

    if (choice == "1")
    {
        DisplayFlights(FlightDict);
    }
    else if (choice == "2")
    {
        DisplayBoardingGates(BoardingGateDict);
    }
    else if (choice == "3")
    {
        AssignBoardingGate();
    }
    else if (choice == "4")
    {
         CreateFlight();
    }
    else if (choice == "5")
    {

    }
    else if (choice == "6")
    {

    }
    else if (choice == "7")
    {
        ArrangeFlight();
    }
    else if (choice == "8")
    {

    }
    else if (choice == "9")
    {

    }
    else if (choice == "0")
    {
        contdisplay = "N";
    }
    else
    {
        Console.WriteLine("Please Choose A Avaliable Action");
    }

}

while (contdisplay == "Y")
{
    DisplayMenu();
}
