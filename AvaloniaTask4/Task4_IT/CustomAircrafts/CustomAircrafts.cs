using System;
using Task2;

namespace CustomAircrafts
{
    public class MilitaryDrone : Aircraft
    {
        public double Altitude { get; private set; }
        public string CallSign { get; }
        public double MaxAltitude { get; }

        public MilitaryDrone(string callSign, double maxAltitude)
        {
            CallSign = callSign;
            MaxAltitude = maxAltitude;
        }

        public override bool Takeoff()
        {
            if (MaxAltitude <= 0) return false;
            
            Altitude = MaxAltitude * 0.7;
            NotifyTakeoff($"Военный дрон {CallSign} взлетел на {Altitude}м");
            return true;
        }

        public override void Land()
        {
            Altitude = 0;
            NotifyLanding($"Военный дрон {CallSign} приземлился");
        }
        
        public string SetAltitude(double newAltitude)
        {
            if (newAltitude <= MaxAltitude)
            {
                Altitude = newAltitude;
                return $"Высота изменена на {newAltitude}м";
            }
            return $"Ошибка: превышена максимальная высота {MaxAltitude}м";
        }
        
        public double GetAltitude() => Altitude;
    }

    public class Quadcopter : Aircraft
    {
        public enum FlightMode { Hover, Cruise, Sport }
        
        public double Altitude { get; private set; }
        public FlightMode Mode { get; private set; } = FlightMode.Hover;

        public override bool Takeoff()
        {
            Altitude = 100;
            NotifyTakeoff("Квадрокоптер взлетел");
            return true;
        }

        public override void Land()
        {
            Altitude = 0;
            NotifyLanding("Квадрокоптер приземлился");
        }
        
        public string SetFlightMode(FlightMode mode)
        {
            Mode = mode;
            return $"Режим изменен на: {mode}";
        }
        
        public double GetAltitude() => Altitude;
    }
    
    public class Drone : Aircraft
    {
        public double Altitude { get; private set; }
        public string Model { get; } = "Default Drone";

        public override bool Takeoff()
        {
            Altitude = 150;
            NotifyTakeoff($"Дрон {Model} взлетел на {Altitude}м");
            return true;
        }

        public override void Land()
        {
            Altitude = 0;
            NotifyLanding($"Дрон {Model} приземлился");
        }
        
        public string GetStatus() => $"Дрон {Model} на высоте {Altitude}м";
        
        public double GetAltitude() => Altitude;
    }
}