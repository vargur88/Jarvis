using System;
using System.Windows.Input;

namespace Helpers
{
	public class RelayCommandBase<T> : ICommand
	{
		protected readonly Action<T> _execute = null;
		protected readonly Predicate<T> _canExecute = null;

		public RelayCommandBase(Action<T> execute)
			: this(null, execute)
		{}

		public RelayCommandBase(Predicate<T> canExecute, Action<T> execute)
		{
			_execute = execute ?? throw new ArgumentNullException("execute");
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke((T)parameter) ?? true;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecute != null)
					CommandManager.RequerySuggested += value;
			}
			remove
			{
				if (_canExecute != null)
					CommandManager.RequerySuggested -= value;
			}
		}

		public void Execute(object parameter)
		{
			_execute((T)parameter);
		}
	}

	public class RelayCommand : RelayCommandBase<object>
	{
		public RelayCommand(Action<object> execute)
			: this(null, execute)
		{}

		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
			: base(canExecute, execute)
		{}
	}
}
