using AvaloniaTask1.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AvaloniaTask1.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    // Свойства для ввода
    private string _num1Numerator = "2";
    public string Num1Numerator
    {
        get => _num1Numerator;
        set { _num1Numerator = value; OnPropertyChanged(); }
    }

    private string _num1Denominator = "3";
    public string Num1Denominator
    {
        get => _num1Denominator;
        set { _num1Denominator = value; OnPropertyChanged(); }
    }

    private string _num2Numerator = "2";
    public string Num2Numerator
    {
        get => _num2Numerator;
        set { _num2Numerator = value; OnPropertyChanged(); }
    }

    private string _num2Denominator = "7";
    public string Num2Denominator
    {
        get => _num2Denominator;
        set { _num2Denominator = value; OnPropertyChanged(); }
    }

    // Результат
    private string _result = "Результат появится здесь";
    public string Result
    {
        get => _result;
        set { _result = value; OnPropertyChanged(); }
    }

    // Команды
    public ICommand AddCommand { get; }
    public ICommand SubtractCommand { get; }
    public ICommand MultiplyCommand { get; }
    public ICommand DivideCommand { get; }

    public MainWindowViewModel()
    {
        // Инициализация команд с привязкой к UI-потоку

        AddCommand = new RelayCommand(Add);
        SubtractCommand = new RelayCommand(Subtract);
        MultiplyCommand = new RelayCommand(Multiply);
        DivideCommand = new RelayCommand(Divide);
    }

    private RationalNumber ParseNumber(string numeratorStr, string denominatorStr)
    {
        if (!int.TryParse(numeratorStr, out int numerator))
            throw new ArgumentException("Некорректный числитель");
        
        if (!int.TryParse(denominatorStr, out int denominator) || denominator == 0)
            throw new ArgumentException("Некорректный знаменатель");
        
        return new RationalNumber(numerator, denominator);
    }

    private void Add()
    {
        try
        {
            var num1 = ParseNumber(Num1Numerator, Num1Denominator);
            var num2 = ParseNumber(Num2Numerator, Num2Denominator);
            Result = $"{num1} + {num2} = {num1 + num2}";
        }
        catch (Exception ex)
        {
            Result = $"Ошибка: {ex.Message}";
        }
    }

    // Аналогично для Subtract, Multiply, Divide
    private void Subtract()
    {
        try
        {
            var num1 = ParseNumber(Num1Numerator, Num1Denominator);
            var num2 = ParseNumber(Num2Numerator, Num2Denominator);
            Result = $"{num1} - {num2} = {num1 - num2}";
        }
        catch (Exception ex)
        {
            Result = $"Ошибка: {ex.Message}";
        }
    }

    private void Multiply()
    {
        try
        {
            var num1 = ParseNumber(Num1Numerator, Num1Denominator);
            var num2 = ParseNumber(Num2Numerator, Num2Denominator);
            Result = $"{num1} * {num2} = {num1 * num2}";
        }
        catch (Exception ex)
        {
            Result = $"Ошибка: {ex.Message}";
        }
    }

    private void Divide()
    {
        try
        {
            var num1 = ParseNumber(Num1Numerator, Num1Denominator);
            var num2 = ParseNumber(Num2Numerator, Num2Denominator);
            Result = $"{num1} / {num2} = {num1 / num2}";
        }
        catch (DivideByZeroException)
        {
            Result = "Ошибка: деление на ноль!";
        }
        catch (Exception ex)
        {
            Result = $"Ошибка: {ex.Message}";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged;
    
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}