using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PRG_Assignment_2_Chen_Rui
{
    public class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        public string SpecialRequestCode { get; set; }
        public string BoardingGate { get; set; }

        public Flight(string fn, string ori, string dest, DateTime et, string status = "On Time")
        {
            FlightNumber = fn;
            Origin = ori;
            Destination = dest;
            ExpectedTime = et;
            Status = status;
        }

        public virtual double CalculateFees()
        {
            return 100.0;
        }

        public override string ToString()
        {
            return $"Flight: {FlightNumber}\tOrigin: {Origin}\tDestination: {Destination}\tExpectedTime: {ExpectedTime}\tStatus: {Status}";
        }
    }

    class DDJBFlight : Flight
    {
        public DDJBFlight(string fn, string ori, string dest, DateTime et)
            : base(fn, ori, dest, et)
        {
        }

        public override double CalculateFees()
        {
            double fee = 0;
            if (Origin == "Singapore (SIN)")
                fee += 800;
            if (Destination == "Singapore (SIN)")
                fee += 500;
            fee += 300;
            fee += 300;
            return fee;
        }
    }

    class LWTTFlight : Flight
    {
        public LWTTFlight(string fn, string ori, string dest, DateTime et)
            : base(fn, ori, dest, et)
        {
        }

        public override double CalculateFees()
        {
            double fee = 0;
            if (Origin == "Singapore (SIN)")
                fee += 800;
            if (Destination == "Singapore (SIN)")
                fee += 500;
            fee += 300;
            fee += 500;
            return fee;
        }
    }

    class NormalFlight : Flight
    {
        public NormalFlight(string fn, string ori, string dest, DateTime et)
            : base(fn, ori, dest, et)
        {
        }

        public override double CalculateFees()
        {
            double fee = 0;
            if (Origin == "Singapore (SIN)")
                fee += 800;
            if (Destination == "Singapore (SIN)")
                fee += 500;
            fee += 300;
            return fee;
        }
    }

    class CFFTFlight : Flight
    {
        public CFFTFlight(string fn, string ori, string dest, DateTime et)
            : base(fn, ori, dest, et)
        {
        }

        public override double CalculateFees()
        {
            double fee = 0;
            if (Origin == "Singapore (SIN)")
                fee += 800;
            if (Destination == "Singapore (SIN)")
                fee += 500;
            fee += 300;
            fee += 150;
            return fee;
        }
    }
}
