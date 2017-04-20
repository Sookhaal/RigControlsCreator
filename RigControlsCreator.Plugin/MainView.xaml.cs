using System.Windows;

namespace RigControlsCreator.Plugin {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainView : Window {
		MainViewModel _vm;
		public MainView() {
			InitializeComponent();
			DataContext = new MainViewModel();
			_vm = (MainViewModel) DataContext;
		}

		private void Image_OnDrop(object sender, DragEventArgs e) {
			_vm.DropImage(e);
		}
	}
}
