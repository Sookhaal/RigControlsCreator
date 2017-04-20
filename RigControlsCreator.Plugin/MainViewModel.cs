using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Autodesk.Maya.OpenMaya;
using Newtonsoft.Json;
using RigControlsCreator.Plugin.Commands;
using RigControlsCreator.Plugin.Models;
using RigControlsCreator.Plugin.Properties;
//using System.Xml.Serialization;

namespace RigControlsCreator.Plugin {
	public class MainViewModel : INotifyPropertyChanged {
		private string _text;
		private ICommand _placeOnSceneCommand;
		private ObservableCollection<Control> _controls;
		private Control _currentControl;
		private ICommand _saveChangesCommand;
		private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png" };
		//private readonly XmlSerializer _serializer;
		private readonly JsonSerializer _serializer;
		private readonly string _controlsPath = "\\Data\\_controls.json";
		private readonly string _rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private ICommand _addControlCommand;
		private ICommand _removeControlCommand;

		public string Text {
			get { return _text; }
			set {
				_text = value;
			}
		}

		public int SelectedIndex { get; set; }

		public ICommand PlaceOnSceneCommand {
			get {
				return _placeOnSceneCommand ?? (_placeOnSceneCommand = new RelayCommand(e => PlaceOnScene()));
			}
			set { _placeOnSceneCommand = value; }
		}

		public ICommand SaveChangesCommand {
			get { return _saveChangesCommand ?? (_saveChangesCommand = new RelayCommand(e => SaveChanges())); }
			set { _saveChangesCommand = value; }
		}

		public ICommand AddControlCommand {
			get { return _addControlCommand ?? (_addControlCommand = new RelayCommand(e => AddControl())); }
			set { _addControlCommand = value; }
		}

		public ICommand RemoveControlCommand {
			get { return _removeControlCommand ?? (_removeControlCommand = new RelayCommand(e => RemoveControl())); }
			set { _removeControlCommand = value; }
		}

		public ObservableCollection<Control> Controls {
			get { return _controls; }
			set {
				_controls = value;
				OnPropertyChanged(nameof(Controls));
			}
		}

		public Control CurrentControl {
			get { return _currentControl; }
			set {
				_currentControl = value;
				OnPropertyChanged(nameof(CurrentControl));

				if (_currentControl == null)
					return;

				var baseUri = new Uri(_rootPath + "\\" + CurrentControl.ImageRelativePath, UriKind.RelativeOrAbsolute);
				if (!File.Exists(baseUri.LocalPath))
					return;
				var img = new BitmapImage();
				img.BeginInit();
				img.UriSource = baseUri;
				img.EndInit();
				CurrentControl.Image = img;
			}
		}

		public MainViewModel() {
			var dataPath = _rootPath + _controlsPath;
			//_serializer = new XmlSerializer(typeof(List<Control>));
			_serializer = new JsonSerializer();

			if (!File.Exists(_rootPath + _controlsPath)) {
				_controls = new ObservableCollection<Control> { new Control() };
				return;
			}

			/*using (var reader = new StreamReader(_controlsPath.ToString())) {
				Controls = (List<Control>) _serializer.Deserialize(reader);
			}*/

			using (var reader = File.OpenText(_rootPath + _controlsPath)) {
				Controls = (ObservableCollection<Control>) _serializer.Deserialize(reader, typeof(ObservableCollection<Control>));
			}
		}

		public void PlaceOnScene() {
			MGlobal.executeCommand(CurrentControl.Script, true, true);
		}

		public void DropImage(DragEventArgs e) {
			var data = (string[]) e.Data.GetData(DataFormats.FileDrop, true);
			var filename = data?[0];
			if (!File.Exists(filename)) {
				return;
			}
			
			var fileInfo = new FileInfo(filename);

			if (ImageExtensions.Contains(fileInfo.Extension.ToLowerInvariant())) {
				var img = new BitmapImage();
				img.BeginInit();
				img.UriSource = new Uri(fileInfo.FullName, UriKind.RelativeOrAbsolute);
				img.EndInit();
				CurrentControl.Image = img;

				var baseUri = new Uri(_rootPath + "\\Data");

				var relativeUri = baseUri.MakeRelativeUri(CurrentControl.Image.UriSource);
				CurrentControl.ImageRelativePath = relativeUri.ToString();
			} else {
				MessageBox.Show("Not a valid image.");
			}
		}

		public void SaveChanges() {
			using (var writer = new StreamWriter(_rootPath + _controlsPath)) {
				var json = JsonConvert.SerializeObject(Controls, Formatting.Indented);
				//_serializer.Serialize(writer, Controls);
				writer.Write(json);
			}
		}

		public void AddControl() {
			Controls.Add(new Control());
			OnPropertyChanged(nameof(Controls));
			SelectedIndex = Controls.Count - 1;
			OnPropertyChanged(nameof(SelectedIndex));
		}

		private void RemoveControl() {
			Controls.Remove(CurrentControl);
			OnPropertyChanged(nameof(Controls));
			if (Controls.Count < 1)
				SelectedIndex = 0;
			else {
				SelectedIndex = Controls.Count - 1;
				OnPropertyChanged(nameof(SelectedIndex));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}