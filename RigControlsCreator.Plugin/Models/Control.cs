using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using RigControlsCreator.Plugin.Properties;

namespace RigControlsCreator.Plugin.Models {
	[JsonObject(MemberSerialization.OptIn)]
	public class Control : INotifyPropertyChanged {
		private string _imageRelativePath;
		private BitmapImage _image;
		private string _name;

		public Control(string name) {
			Name = name;
		}

		public Control() {
			Name = Guid.NewGuid().ToString().Replace("-", "");
		}

		[JsonProperty("Name")]
		public string Name {
			get { return _name; }
			set {
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		[JsonProperty("CurveScript")]
		public string Script { get; set; }

		[JsonProperty("ImagePath")]
		public string ImageRelativePath {
			get {
				return _imageRelativePath;
			}
			set
			{
				_imageRelativePath = value;
				OnPropertyChanged(nameof(ImageRelativePath));
			}
		}

		[JsonIgnore]
		public BitmapImage Image {
			get { return _image; }
			set {
				_image = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}