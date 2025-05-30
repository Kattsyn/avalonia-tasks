using Avalonia.Controls;

namespace Task2
{
    public partial class ReflectionView : UserControl
    {
        public ReflectionView()
        {
            InitializeComponent();
            DataContext = new ReflectionViewModel();
        }
    }
}
