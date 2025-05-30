using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2
{
   public class Airplane : Aircraft
    {
        public double RunwayLength { get; private set; }

        public Airplane(double runwayLength)
        {
            RunwayLength = runwayLength;
        }

        public override bool Takeoff()
        {
            if (RunwayLength >= 500)
            {
                Altitude = 1000;
                NotifyTakeoff("Самолет успешно взлетел!");
                return true;
            }
            else
            {
                NotifyTakeoff("Не хватает длины взлетной полосы.");
                return false;
            }
        }

        public override void Land()
        {
            Altitude = 0;
            NotifyLanding("Самолет приземлился.");
        }
    }

}