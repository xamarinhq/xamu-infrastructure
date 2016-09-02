//
// NavigateToCommand.cs
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
using System.Windows.Input;
using XamarinUniversity.Interfaces;
using XamarinUniversity.Services;

namespace XamarinUniversity.Commands
{
    /// <summary>
    /// This class implements an ICommand which will use the registered INavigationService
    /// to perform a NavigateAsync to a specific page
    /// </summary>
    public class NavigateToCommand : ICommand
    {
        /// <summary>
        /// Protected ctor - only allow library to create command
        /// unless you derive from it. Should alway use NavigationCommands.
        /// </summary>
        protected internal NavigateToCommand ()
        {
        }

        /// <summary>
        /// Event raised when the state of the NavigateBackCommand has changed.
        /// </summary>
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        /// <summary>
        /// This is called to determine whether the command can be executed.
        /// </summary>
        /// <returns>True if the command is valid</returns>
        /// <param name="parameter">PageKey to navigate to</param>
        public bool CanExecute (object parameter)
        {
            // Must have a page key.
            return parameter != null;
        }

        /// <summary>
        /// This is called to execute the command.
        /// </summary>
        /// <param name="parameter">Page Key to navigate to</param>
        public async void Execute (object parameter)
        {
            if (parameter != null)
            {
                var ns = XamUInfrastructure.ServiceLocator.Get<INavigationService> ();
                if (ns != null) {
                    await ns.NavigateAsync (parameter);
                }
            }
        }
    }
}

