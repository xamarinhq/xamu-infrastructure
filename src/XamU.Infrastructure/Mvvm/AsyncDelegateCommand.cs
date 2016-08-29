//
// AsyncDelegateCommand.cs
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
using System.Threading.Tasks;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// A base ICommand implementation that supports async/await.
    /// </summary>
	public class AsyncDelegateCommand : IDelegateCommand
	{
		protected readonly Predicate<object> canExecute;
		protected Func<object, Task> asyncExecute;
		public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Creates a new async delegate command.
        /// </summary>
        /// <param name="execute">Method to call when command is executed.</param>
		public AsyncDelegateCommand(Func<Task> execute)
			: this(_ => execute(), null)
		{
		}

        /// <summary>
        /// Creates a new async delegate command.
        /// </summary>
        /// <param name="execute">Method to call when command is executed.</param>
		public AsyncDelegateCommand(Func<object, Task> execute)
			: this(execute, null)
		{
		}

        /// <summary>
        /// Creates a new async delegate command.
        /// </summary>
        /// <param name="execute">Method to call when command is executed.</param>
        /// <param name="canExecute">Method to call to determine whether command is valid.</param>
        public AsyncDelegateCommand(Func<Task> execute, Func<bool> canExecute)
			: this(_ => execute(), _ => canExecute())
		{
		}

        /// <summary>
        /// Creates a new async delegate command.
        /// </summary>
        /// <param name="asyncExecute">Method to call when command is executed.</param>
        /// <param name="canExecute">Method to call to determine whether command is valid.</param>
		public AsyncDelegateCommand(Func<object, Task> asyncExecute,
			Predicate<object> canExecute)
		{
			this.asyncExecute = asyncExecute;
			this.canExecute = canExecute;
		}

        /// <summary>
        /// Raise the CanExecuteChanged handler.
        /// </summary>
		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

        /// <summary>
        /// Returns whether the command is possible right now.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
		public bool CanExecute(object parameter)
		{
			return canExecute == null || canExecute (parameter);
		}

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
		public async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

        /// <summary>
        /// Executes the command and returns an awaitable task.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="parameter">Parameter.</param>
		public async Task ExecuteAsync(object parameter)
		{
			await asyncExecute(parameter);
		}
	}

    /// <summary>
    /// A generic ICommand implementation that supports async/await.
    /// </summary>
    public class AsyncDelegateCommand<T> : IAsyncDelegateCommand<T>
	{
		protected readonly Predicate<T> canExecute;
		protected Func<T, Task> asyncExecute;
		public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Creates a new async delegate command.
        /// </summary>
        /// <param name="execute">Method to call when command is executed.</param>
        public AsyncDelegateCommand(Func<T, Task> execute)
			: this(execute, null)
		{
		}

        /// <summary>
        /// Creates a new async delegate command.
        /// </summary>
        /// <param name="asyncExecute">Method to call when command is executed.</param>
        /// <param name="canExecute">Method to determine whether command is valid.</param>
        public AsyncDelegateCommand(Func<T, Task> asyncExecute,
			Predicate<T> canExecute)
		{
			this.asyncExecute = asyncExecute;
			this.canExecute = canExecute;
		}

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

        /// <summary>
        /// Returns whether the command is valid at this moment.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
		public bool CanExecute(object parameter)
		{
			return (canExecute == null) || canExecute((T)parameter);
		}

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
		public async void Execute(object parameter)
		{
			await ExecuteAsync((T)parameter);
		}

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
		public async Task ExecuteAsync(T parameter)
		{
			await asyncExecute(parameter);
		}
	}
}
