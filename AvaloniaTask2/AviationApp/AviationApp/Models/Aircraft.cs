using System;

namespace AviationApp.Models
{
    public abstract class Aircraft
    {
        public event EventHandler<string>? StatusChanged;
    
        public double Height { get; protected set; }
        public bool IsFlying => Height > 0;
        public string AircraftType => this.GetType().Name;

        protected void OnStatusChanged(string message)
        {
            StatusChanged?.Invoke(this, message);
        }

        public abstract bool TakeOff();
        public abstract bool Land();
    }
    

    
}