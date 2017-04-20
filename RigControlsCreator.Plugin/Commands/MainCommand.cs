using System;
using System.Windows;
using Autodesk.Maya.OpenMaya;
using RigControlsCreator.Plugin.Commands;

[assembly: MPxCommandClass(typeof(MainCommand), "RigControlsCreator")]

namespace RigControlsCreator.Plugin.Commands {
	public class MainCommand : MPxCommand, IMPxCommand {
		private MainView _mainWindow;

		public override void doIt(MArgList args) {
			AppDomain.CurrentDomain.UnhandledException += Handler;

			_mainWindow = new MainView();
			_mainWindow.Show();
		}

		private void Handler(object sender, UnhandledExceptionEventArgs args) {
			var e = (Exception) args.ExceptionObject;
			//MessageBox.Show(e.Message + "/n" + e.StackTrace + "/n" + e.Data + "/n" + e, e.StackTrace);
			MessageBox.Show(e.ToString(), e.Source);
		}
	}
}