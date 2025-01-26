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
            string code = columns[0].Trim();
            string name = columns[1].Trim();

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
                string name = parts[0].Trim();
                bool supportsCFFT = parts[1].Trim() == "true";
                bool supportsDDJB = parts[2].Trim() == "true";
                bool supportsLWTT = parts[3].Trim() == "true";
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

//Basic Feature 1
// Load files and initialize dictionaries
Console.WriteLine("Loading Airlines. . .");
Dictionary<string, Airline> airlineDictionary = LoadAirlines("airlines.csv");
Console.WriteLine("{0} Airlines Loaded!", airlineDictionary.Count);

Console.WriteLine("Loading Boarding Gates. . .");
Dictionary<string, BoardingGate> boardingGateDictionary = LoadBoardingGates("boardinggates.csv");
Console.WriteLine("{0} Boarding Gates Loaded!", boardingGateDictionary.Count);
