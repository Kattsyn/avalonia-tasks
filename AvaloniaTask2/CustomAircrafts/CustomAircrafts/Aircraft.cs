namespace CustomAircrafts;

public abstract class Aircraft
{
    public double Altitude { get; protected set; }
    
    public event Action<string>? OnTakeoff;
    public event Action<string>? OnLanding;

    public abstract bool Takeoff();
    public abstract void Land();
    
    protected void NotifyTakeoff(string message)
    {
        OnTakeoff?.Invoke(message);
    }

    protected void NotifyLanding(string message)
    {
        OnLanding?.Invoke(message);
    }
}