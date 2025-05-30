using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Windows.Input;
using ReactiveUI;

namespace Task2
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _status = "Ожидание действий...";
        public string Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        public Airplane Airplane { get; }
        public Helicopter Helicopter { get; }

        public ICommand TakeoffAirplaneCommand { get; }
        public ICommand LandAirplaneCommand { get; }
        public ICommand TakeoffHelicopterCommand { get; }
        public ICommand LandHelicopterCommand { get; }

        public MainWindowViewModel()
        {
            Airplane = new Airplane(600);
            Helicopter = new Helicopter();

            // Подписка на события взлета и посадки
            Airplane.OnTakeoff += message => UpdateStatus(message);
            Airplane.OnLanding += message => UpdateStatus(message);
            Helicopter.OnTakeoff += message => UpdateStatus(message);
            Helicopter.OnLanding += message => UpdateStatus(message);

            // Команды
            TakeoffAirplaneCommand = new RelayCommand(TakeoffAirplane);
            LandAirplaneCommand = new RelayCommand(LandAirplane);
            TakeoffHelicopterCommand = new RelayCommand(TakeoffHelicopter);
            LandHelicopterCommand = new RelayCommand(LandHelicopter);
        }

        private void UpdateStatus(string message)
        {
            // Выводим в консоль для отладки 
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Устанавливаем кодировку UTF-8 для консоли
            Console.WriteLine($"Status обновлен на: {message}");

            // Обновляем UI только на UI потоке
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Status = message;
                System.Diagnostics.Debug.WriteLine(Status); // Для проверки в Output
            });
        }

        private void TakeoffAirplane()
        {
            Console.WriteLine("Команда взлета самолета выполнена.");
            Airplane.Takeoff();
        }

        private void LandAirplane()
        {
            Console.WriteLine("Команда посадки самолета выполнена.");
            Airplane.Land();
        }

        private void TakeoffHelicopter()
        {
            Console.WriteLine("Команда взлета вертолета выполнена.");
            Helicopter.Takeoff();
        }

        private void LandHelicopter()
        {
            Console.WriteLine("Команда посадки вертолета выполнена.");
            Helicopter.Land();
        }
    }

    // Простая реализация ICommand
    
}
