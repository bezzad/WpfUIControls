using System.Windows;

namespace TestUIElements
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.MessageBoxShow("Test", "test title", Buttons.OK, Icons.Warning, AnimateStyle.SlideDown);
        }
    }
}
