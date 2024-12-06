namespace OlympicsMauiApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

		DB.InitDB();
	}
}
