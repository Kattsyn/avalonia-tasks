using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2
{
    public class Helicopter : Aircraft
    {
        public override bool Takeoff()
        {
            Altitude = 500;
            NotifyTakeoff("Вертолет успешно взлетел!");
            return true;
        }

        public override void Land()
        {
            Altitude = 0;
            NotifyLanding("Вертолет приземлился.");
        }
    }
}