using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AviationApp.Models;

namespace AviationApp.ViewModels;
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Aircraft? _selectedAircraft;
        private string _statusLog = string.Empty;

        public ObservableCollection<Aircraft> Aircrafts { get; } = new();
        
        public Aircraft? SelectedAircraft
        {
            get => _selectedAircraft;
            set => SetField(ref _selectedAircraft, value);
        }

        public string StatusLog
        {
            get => _statusLog;
            set => SetField(ref _statusLog, value);
        }

        // Команды
        public ICommand AddAirplaneCommand { get; }
        public ICommand AddHelicopterCommand { get; }
        public ICommand TakeOffCommand { get; }
        public ICommand LandCommand { get; }

        public MainWindowViewModel()
        {
            AddAirplaneCommand = new RelayCommand(AddAirplane);
            AddHelicopterCommand = new RelayCommand(AddHelicopter);
            TakeOffCommand = new RelayCommand(TakeOff);
            LandCommand = new RelayCommand(Land);
        }

        private void AddAirplane()
        {
            var airplane = new Airplane(600);
            airplane.StatusChanged += (_, message) => 
                StatusLog += $"{DateTime.Now:T} {message}\n";
            Aircrafts.Add(airplane);
        }

        private void AddHelicopter()
        {
            var helicopter = new Helicopter();
            helicopter.StatusChanged += (_, message) => 
                StatusLog += $"{DateTime.Now:T} {message}\n";
            Aircrafts.Add(helicopter);
        }

        private void TakeOff()
        {
            if (SelectedAircraft?.TakeOff() == true)
            {
                StatusLog += $"{DateTime.Now:T} Взлет успешен!\n";
            }
        }

        private void Land()
        {
            SelectedAircraft?.Land();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
