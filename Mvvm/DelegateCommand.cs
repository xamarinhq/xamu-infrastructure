//
// DelegateCommand.cs
//
// Author:
//       Mark Smith <mark.smith@xamarin.com>
//
// Copyright (c) 2016 Xamarin, Microsoft.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Implementation of ICommand using delegates.
    /// This is preferred over Command in Forms so it can be mocked/replaced
    /// in the ViewModel and have your VM not take a dependency on Forms.
    /// </summary>
	public sealed class DelegateCommand : IDelegateCommand
	{
		readonly Action<object> command;
		readonly Func<object, bool> canExecute;
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Creates a new DelegateCommand.
        /// </summary>
        /// <param name="command">Delegate to call for command</param>
		public DelegateCommand (Action<object> command) : this (command, null)
		{
		}

        /// <summary>
        /// Creates a new DelegateCommand.
        /// </summary>
        /// <param name="command">Delegate to call for command</param>
		public DelegateCommand (Action command) : this (command, null)
		{
		}

        /// <summary>
        /// Creates a new DelegateCommand.
        /// </summary>
        /// <param name="command">Delegate to call for command</param>
        /// <param name="test">Delegate to call for CanExecute</param>
		public DelegateCommand (Action command, Func<bool> test)
		{
			if (command == null)
				throw new ArgumentNullException ("command", "Command cannot be null.");
			this.command = delegate {
				command ();
			};
			if (test != null) {
				this.canExecute = delegate {
					return test ();
				};
			}
		}

        /// <summary>
        /// Creates a new DelegateCommand.
        /// </summary>
        /// <param name="command">Delegate to call for command</param>
        /// <param name="test">Delegate to call for CanExecute</param>
		public DelegateCommand (Action<object> command, Func<object, bool> test)
		{
			if (command == null)
				throw new ArgumentNullException ("command", "Command cannot be null.");

			this.command = command;
			this.canExecute = test;
		}

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
		public void RaiseCanExecuteChanged ()
		{
			this.CanExecuteChanged?.Invoke (this, EventArgs.Empty);
		}

        /// <summary>
        /// Checks to see if the command is valid.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
		public bool CanExecute (object parameter)
		{
			return (this.canExecute == null) || this.canExecute (parameter);
		}

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
		public void Execute (object parameter)
		{
			this.command (parameter);
		}
	}

    /// <summary>
    /// Generic form of the DelegateCommand with a parameter.
    /// </summary>
	public sealed class DelegateCommand<T> : IDelegateCommand
	{
		readonly Action<T> command;
		readonly Func<T, bool> canExecute;
		public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Creates a new Delegate command
        /// </summary>
        /// <param name="command">Delegate to invoke</param>
		public DelegateCommand (Action<T> command) : this (command, null)
		{
		}

        /// <summary>
        /// Creates a new Delegate command
        /// </summary>
        /// <param name="command">Delegate to invoke</param>
        /// <param name="test">Delegate for CanExecute</param>
		public DelegateCommand (Action<T> command, Func<T, bool> test)
		{
			if (command == null)
				throw new ArgumentNullException ("command", "Command cannot be null.");

			this.command = command;
			this.canExecute = test;
		}

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
		public void RaiseCanExecuteChanged ()
		{
			this.CanExecuteChanged?.Invoke (this, EventArgs.Empty);
		}

        /// <summary>
        /// Returns whether the command is valid.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
		public bool CanExecute (object parameter)
		{
			return (this.canExecute == null) || this.canExecute ((T)parameter);
		}

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
		public void Execute (object parameter)
		{
			this.command ((T)parameter);
		}
	}
}

