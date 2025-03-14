namespace AviationApp.Models;

public class Airplane : Aircraft
{
    public double RunwayLength { get; }

    public Airplane(double runwayLength)
    {
        RunwayLength = runwayLength;
    }

    public override bool TakeOff()
    {
        if (IsFlying)
        {
            OnStatusChanged("Самолет уже в воздухе!");
            return false;
        }
        
        if (RunwayLength >= 500)
        {
            Height = 10000;
            OnStatusChanged($"Самолет взлетел с полосы {RunwayLength} м");
            return true;
        }
        
        OnStatusChanged("Слишком короткая взлетная полоса!");
        return false;
    }

    public override bool Land()
    {
        if (!IsFlying)
        {
            OnStatusChanged("Самолет уже на земле!");
            return false;
        }
        
        Height = 0;
        OnStatusChanged("Самолет приземлился");
        return true;
    }
}