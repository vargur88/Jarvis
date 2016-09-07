using System;
using System.Windows.Input;

namespace Helpers
{
	public class RelayCommand : ICommand
	{
		private Predicate<object> _canExecute;
		private Action<object> _execute;

		public RelayCommand(Action<object> execute)
			: this(null, execute)
		{}

		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
		{
			this._canExecute = canExecute;
			this._execute = execute;
		}

		private EventHandler _internalCanExecuteChanged;
		public event EventHandler CanExecuteChanged
		{
			add
			{
				_internalCanExecuteChanged += value;
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				_internalCanExecuteChanged -= value;
				CommandManager.RequerySuggested -= value;
			}
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke(parameter) ?? true;
		}

		public void Execute(object parameter)
		{
			_execute?.Invoke(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			var canExecuteChanged = _internalCanExecuteChanged;
			canExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
