using System.Windows;
using RigControlsCreator.Plugin;

namespace RigControlCreator.Windows {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void AppStartup(object sender, StartupEventArgs e) {
			MainWindow = new MainView();
			MainWindow.Show();
		}
	}
}
