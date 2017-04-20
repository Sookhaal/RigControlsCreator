using System;
using System.Diagnostics;
using System.Windows.Input;

namespace RigControlsCreator.Plugin.Commands {
	public class RelayCommand : ICommand {
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;
		private object p;

		public RelayCommand(Action<object> execute) : this(execute, null) {

		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
			if (execute == null) {
				throw new ArgumentNullException("execute is null!");
			}

			_execute = execute;
			_canExecute = canExecute;
		}

		[DebuggerStepThrough]
		public bool CanExecute(object parameters) {
			return _canExecute?.Invoke(parameters) ?? true;
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameters) {
			_execute(parameters);
		}

	}
}