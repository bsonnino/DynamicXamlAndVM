using System.Windows;

namespace DynamicXamlAndVMRefactored
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var content = DynamicContentLoader.Load("Cust");
            if (DynamicContentLoader.Errors != null)
                MessageBox.Show(DynamicContentLoader.Errors, "Errors in compile");
            WndContent.Content = content;
        }
    }
}
