using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2
{
    public abstract class Aircraft
    {
        public double Altitude { get; protected set; }

        // События для уведомлений о взлете и посадке
        public event Action<string>? OnTakeoff;
        public event Action<string>? OnLanding;

        public abstract bool Takeoff();
        public abstract void Land();

        // Вспомогательные методы для уведомлений
        protected void NotifyTakeoff(string message)
        {
            OnTakeoff?.Invoke(message);
        }

        protected void NotifyLanding(string message)
        {
            OnLanding?.Invoke(message);
        }
    }

}