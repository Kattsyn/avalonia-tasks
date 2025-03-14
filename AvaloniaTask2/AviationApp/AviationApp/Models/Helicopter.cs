namespace AviationApp.Models;
public class Helicopter : Aircraft
{
    public override bool TakeOff()
    {
        if (IsFlying)
        {
            OnStatusChanged("Вертолет уже в воздухе!");
            return false;
        }
        
        Height = 500;
        OnStatusChanged("Вертолет взлетел");
        return true;
    }

    public override bool Land()
    {
        if (!IsFlying)
        {
            OnStatusChanged("Вертолет уже на земле!");
            return false;
        }
        
        Height = 0;
        OnStatusChanged("Вертолет приземлился");
        return true;
    }
}