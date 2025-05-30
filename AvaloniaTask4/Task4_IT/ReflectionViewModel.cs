using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace Task2
{
    public class ReflectionViewModel : ViewModelBase
    {
        private string _dllPath = string.Empty;

        public string DllPath
        {
            get => _dllPath;
            set => this.RaiseAndSetIfChanged(ref _dllPath, value);
        }

        public ObservableCollection<TypeDisplay> FoundTypes { get; } = new();
        private TypeDisplay? _selectedType;

        public TypeDisplay? SelectedType
        {
            get => _selectedType;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedType, value);
                LoadMethods();
            }
        }

        public ObservableCollection<MethodDisplay> Methods { get; } = new();
        private MethodDisplay? _selectedMethod;

        public MethodDisplay? SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedMethod, value);
                LoadParameters();
            }
        }

        public ObservableCollection<ParameterViewModel> Parameters { get; } = new();
        private string _result = string.Empty;

        public string Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }

        public ICommand LoadAssemblyCommand { get; }
        public ICommand ExecuteCommand { get; }
        public ICommand ResetInstanceCommand { get; }
        public ICommand BrowseCommand { get; }

        public ObservableCollection<ParameterViewModel> ConstructorParameters { get; } = new();

        public ReflectionViewModel()
        {
            LoadAssemblyCommand = new RelayCommand(LoadAssembly);
            ExecuteCommand = new RelayCommand(Execute);
            BrowseCommand = new RelayCommand(BrowseForDll);
            ResetInstanceCommand = new RelayCommand(ResetInstance);
        }

        private void ResetInstance()
        {
            if (SelectedType != null && _instances.ContainsKey(SelectedType.Type))
            {
                _instances.Remove(SelectedType.Type);
                Result = "Экземпляр сброшен. Следующий вызов создаст новый объект.";
            }
            else
            {
                Result = "Нет активного экземпляра для сброса";
            }
        }
        
        private void LoadAssembly()
        {
            _instances.Clear();
            FoundTypes.Clear();
            Methods.Clear();
            Parameters.Clear();
            Result = string.Empty;
            if (string.IsNullOrWhiteSpace(DllPath)) return;
            try
            {
                var asm = Assembly.LoadFrom(DllPath);
                var baseType = typeof(Aircraft); // Можно заменить на интерфейс
                var types = asm.GetTypes().Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var t in types) FoundTypes.Add(new TypeDisplay(t));
            }
            catch (Exception ex)
            {
                Result = $"Ошибка загрузки: {ex.Message}";
            }
        }

        private async void BrowseForDll()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filters.Add(new FileDialogFilter { Name = "DLL Files", Extensions = { "dll" } });
                dialog.AllowMultiple = false;

                var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
                    ?.MainWindow;
                var files = await dialog.ShowAsync(mainWindow);

                if (files != null && files.Length > 0)
                {
                    DllPath = files[0];
                }
            }
            catch (Exception ex)
            {
                Result = $"Ошибка выбора файла: {ex.Message}";
            }
        }

        private void LoadMethods()
        {
            if (SelectedType != null && _instances.ContainsKey(SelectedType.Type))
            {
                _instances.Remove(SelectedType.Type);
            }

            Methods.Clear();
            Parameters.Clear();
            ConstructorParameters.Clear(); // Очищаем параметры конструктора

            if (SelectedType == null) return;

            // Загружаем конструкторы
            var constructors = SelectedType.Type.GetConstructors();
            if (constructors.Length > 0)
            {
                foreach (var param in constructors[0].GetParameters())
                {
                    ConstructorParameters.Add(new ParameterViewModel(param));
                }
            }

            // Загружаем методы
            foreach (var m in SelectedType.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                           BindingFlags.DeclaredOnly))
                Methods.Add(new MethodDisplay(m));
        }

        private void LoadParameters()
        {
            Parameters.Clear();
            if (SelectedMethod == null) return;
            foreach (var p in SelectedMethod.Method.GetParameters())
                Parameters.Add(new ParameterViewModel(p));
        }

        private readonly Dictionary<Type, object> _instances = new();

        private void Execute()
        {
            if (SelectedType == null || SelectedMethod == null) return;

            try
            {
                object instance;

                // Проверяем, есть ли уже экземпляр этого типа
                if (!_instances.TryGetValue(SelectedType.Type, out instance))
                {
                    // Создаем новый экземпляр, если его еще нет
                    if (ConstructorParameters.Count > 0)
                    {
                        var ctorParams = ConstructorParameters.Select(p => p.GetValue()).ToArray();
                        var constructor = SelectedType.Type.GetConstructors().First();
                        instance = constructor.Invoke(ctorParams);
                    }
                    else
                    {
                        instance = Activator.CreateInstance(SelectedType.Type);
                    }

                    // Сохраняем экземпляр для последующих вызовов
                    _instances[SelectedType.Type] = instance;
                }

                var paramValues = Parameters.Select(p => p.GetValue()).ToArray();
                var result = SelectedMethod.Method.Invoke(instance, paramValues);

                // Для методов, возвращающих void, выводим специальное сообщение
                Result = SelectedMethod.Method.ReturnType == typeof(void)
                    ? "Метод выполнен успешно"
                    : result?.ToString() ?? "null";
            }
            catch (Exception ex)
            {
                Result = $"Ошибка выполнения: {ex.InnerException?.Message ?? ex.Message}";
            }
        }

        public class ParameterViewModel : ViewModelBase
        {
            public ParameterInfo Info { get; }
            private string _input = string.Empty;

            public string Input
            {
                get => _input;
                set => this.RaiseAndSetIfChanged(ref _input, value);
            }

            public string Name => Info.Name;
            public string TypeName => Info.ParameterType.Name;

            public ParameterViewModel(ParameterInfo info)
            {
                Info = info;
            }

            public object? GetValue()
            {
                try
                {
                    if (Info.ParameterType == typeof(string)) return Input;
                    if (Info.ParameterType == typeof(int)) return int.TryParse(Input, out int i) ? i : 0;
                    if (Info.ParameterType == typeof(double)) return double.TryParse(Input, out double d) ? d : 0.0;
                    if (Info.ParameterType == typeof(bool)) return bool.TryParse(Input, out bool b) && b;

                    // Для enum
                    if (Info.ParameterType.IsEnum)
                        return Enum.TryParse(Info.ParameterType, Input, out object? enumVal) ? enumVal : null;

                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public class TypeDisplay
        {
            public string Display { get; }
            public Type Type { get; }

            public TypeDisplay(Type type)
            {
                Type = type;
                Display = type.FullName ?? type.Name;
            }
        }

        public class MethodDisplay
        {
            public string Display { get; }
            public MethodInfo Method { get; }

            public MethodDisplay(MethodInfo method)
            {
                Method = method;
                Display = method.Name;
            }
        }
    }
}