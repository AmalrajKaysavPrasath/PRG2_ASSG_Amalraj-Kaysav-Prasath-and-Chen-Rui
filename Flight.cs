using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PRG_Assignment_2_Chen_Rui
{
    // 航班类
    class Flight : IComparable<Flight>
    {
        // 属性
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        public string SpecialReq { get; set; }

        // 构造函数
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, string specialReq)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
            SpecialReq = specialReq;
        }

        public int CompareTo(Flight other)
        {
            if (other == null) return 1;

            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public virtual double CalculateFees()
        {
            double fees = 300;

            if (Destination == "Singapore")
            {
                fees += 500;
            }

            if (Origin == "Singapore")
            {
                fees += 800;
            }

            return fees;
        }

        public virtual string ToString()
        {
            return "Flight Number:" + FlightNumber +
                " Origin:" + Origin +
                " Destination:" + Destination +
                " Expected Time:" + ExpectedTime +
                " Status:" + Status +
                " Special Request:" + SpecialReq;
        }
    }

    // 普通航班类
    class NORMFlight : Flight
    {
        // 构造函数
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, string specialReq) : base(flightNumber, origin, destination, expectedTime, status, specialReq)
        {

        }

        public override double CalculateFees()
        {
            double fees = 300;

            if (Destination == "Singapore")
            {
                fees += 500;
            }

            if (Origin == "Singapore")
            {
                fees += 800;
            }

            return fees;
        }

        public override string ToString()
        {
            return "Flight Number:" + FlightNumber +
                " Origin:" + Origin +
                " Destination:" + Destination +
                " Expected Time:" + ExpectedTime +
                " Status:" + Status;
        }
    }

    // 延迟航班类
    class LWTTFlight : Flight
    {
        // 属性
        public double RequestFee { get; set; }

        // 构造函数
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, string specialReq, double requestFee) : base(flightNumber, origin, destination, expectedTime, status, specialReq)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            double fees = 800;

            if (Destination == "Singapore")
            {
                fees += 500;
            }

            if (Origin == "Singapore")
            {
                fees += 800;
            }

            return fees;
        }

        public override string ToString()
        {
            return "Flight Number:" + FlightNumber +
                " Origin:" + Origin +
                " Destination:" + Destination +
                " Expected Time:" + ExpectedTime +
                " Status:" + Status +
                " Request Fee:" + RequestFee;
        }
    }

    // 求机桥航班类
    class DDJBFlight : Flight
    {
        // 属性
        public double RequestFee { get; set; }

        // 构造函数
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, string specialReq, double requestFee) : base(flightNumber, origin, destination, expectedTime, status, specialReq)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            double fees = 600;

            if (Destination == "Singapore")
            {
                fees += 500;
            }

            if (Origin == "Singapore")
            {
                fees += 800;
            }

            return fees;
        }

        public override string ToString()
        {
            return "Flight Number:" + FlightNumber +
                " Origin:" + Origin +
                " Destination:" + Destination +
                " Expected Time:" + ExpectedTime +
                " Status:" + Status +
                " Request Fee:" + RequestFee;
        }
    }

    // 中转航班类
    class CFFTFlight : Flight
    {
        // 属性
        public double RequestFee { get; set; }

        // 构造函数
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, string specialReq, double requestFee) : base(flightNumber, origin, destination, expectedTime, status, specialReq)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            double fees = 450;

            if (Destination == "Singapore")
            {
                fees += 500;
            }

            if (Origin == "Singapore")
            {
                fees += 800;
            }

            return fees;
        }

        public override string ToString()
        {
            return "Flight Number:" + FlightNumber +
                " Origin:" + Origin +
                " Destination:" + Destination +
                " Expected Time:" + ExpectedTime +
                " Status:" + Status +
                " Request Fee:" + RequestFee;
        }
    }

    // 航空公司类
    class Airline
    {
        // 属性
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        // 构造函数
        public Airline(string name, string code, Dictionary<string, Flight> flights)
        {
            Name = name;
            Code = code;
            Flights = flights;
        }

        public bool AddFlight(Flight FlightToAdd)
        {
            Console.Write("What is the Flight Number of the Flight you want to add? ");
            string FlightNum = Console.ReadLine();
            Flights.Add(FlightNum, FlightToAdd);
            return true;
        }

        public double CalculateFees()
        {
            double fees = 0;

            foreach (KeyValuePair<string, Flight> entry in Flights)
            {
                fees += Convert.ToInt32(entry.Value.CalculateFees);
            }

            int numflights = Flights.Count;

            if (numflights > 5)
            {
                fees = fees * 0.97;
            }

            int triple_discount = numflights / 3;

            fees -= triple_discount * 350;

            foreach (KeyValuePair<string, Flight> entry in Flights)
            {
                if (entry.Value.ExpectedTime.Hour < 11 || entry.Value.ExpectedTime.Hour > 20)
                {
                    fees -= 110;
                }

                if (entry.Value.Origin == "Dubai" || entry.Value.Origin == "Bangkok" || entry.Value.Origin == "Tokyo")
                {
                    fees -= 25;
                }

                if (entry.Value.SpecialReq == "None")
                {
                    fees -= 50;
                }
            }

            return fees;
        }

        public bool RemoveFlight(Flight FlightToRemove)
        {
            Flights.Remove(FlightToRemove.FlightNumber);
            return true;
        }

        public virtual string ToString()
        {
            return "Name:" + Name +
                " Code:" + Code +
                " Flights:" + Flights;
        }
    }

    // 登机口类
    class BoardingGate
    {
        // 属性
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDJJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        // 构造函数
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDJJB, bool supportsLWTT, Flight flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDJJB = supportsDJJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }

        public double CalculateFees()
        {
            double fees = 0;

            return fees;
        }

        public virtual string ToString()
        {
            return "Gate Name:" + GateName +
                " Support CFFT:" + SupportsCFFT +
                " Support DJJB:" + SupportsDJJB +
                " Support LWTT:" + SupportsLWTT +
                " Flight:" + Flight;
        }
    }

    // 航站楼类
    class Terminal
    {
        // 属性
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }

        // 构造函数
        public Terminal(string terminalName, Dictionary<string, Airline> airlines, Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, double> gateFees)
        {
            TerminalName = terminalName;
            Airlines = airlines;
            Flights = flights;
            BoardingGates = boardingGates;
            GateFees = gateFees;
        }

        public bool AddAirline(Airline AirlineToAdd)
        {
            Console.Write("What is the Airline code of the Airline you want to add? ");
            string AirlineCode = Console.ReadLine();
            Airlines.Add(AirlineCode, AirlineToAdd);
            return true;
        }

        public bool AddBoardingGate(BoardingGate BoardingGateToAdd)
        {
            Console.Write("What is the Boarding Gate Name of the Boarding Gate you want to add? ");
            string BoardingGateName = Console.ReadLine();
            BoardingGates.Add(BoardingGateName, BoardingGateToAdd);
            return true;
        }

        public Airline GetAirlineFromFlight(Flight FlightToFind)
        {


            return null;
        }

        public bool PrintAirlineFees()
        {
            foreach (KeyValuePair<string, Airline> entry in Airlines)
            {
                double fees = Convert.ToDouble(entry.Value.CalculateFees);
                Console.WriteLine(entry.Value.Code + " " + fees);
            }

            return true;
        }
        public virtual string ToString()
        {
            return "Terminal Name:" + TerminalName +
                " Airlines:" + Airlines +
                " Flights:" + Flights +
                " Boarding Gates:" + BoardingGates +
                " Gate Fees:" + GateFees;
        }
    }

}
