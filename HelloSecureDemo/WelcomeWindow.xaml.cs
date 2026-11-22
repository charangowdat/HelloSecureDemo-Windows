using System.Windows;
namespace HelloSecureDemo;
public partial class WelcomeWindow : Window
{
    public WelcomeWindow() => InitializeComponent();
    private void Exit_Click(object sender, RoutedEventArgs e) => Close();
}
