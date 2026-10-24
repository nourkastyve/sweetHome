namespace SweetHome;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage()) { BarBackgroundColor = Color.FromArgb("#8D4B55"), BarTextColor = Colors.White };
    }
}
