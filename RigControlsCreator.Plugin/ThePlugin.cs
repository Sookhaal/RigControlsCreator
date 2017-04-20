using Autodesk.Maya;
using Autodesk.Maya.OpenMaya;
using RigControlsCreator.Plugin;

[assembly: ExtensionPlugin(typeof(ThePlugin), "Any")]

namespace RigControlsCreator.Plugin {
	public class ThePlugin : IExtensionPlugin {
		public bool InitializePlugin() {
			MayaTheme.Initialize(null);
			return true;
		}

		public bool UninitializePlugin() {
			return true;
		}

		public string GetMayaDotNetSdkBuildVersion() {
			return "";
		}
	}
}