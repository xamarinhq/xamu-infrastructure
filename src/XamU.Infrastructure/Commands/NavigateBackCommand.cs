//
// NavigateBackCommand.cs
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
    /// This command uses the registered INavigationService to perform
    /// a backwards navigation.
    /// </summary>
    public class NavigateBackCommand : ICommand
    {
        bool monitorNavigationStack;

        /// <summary>
        /// Protected ctor - only allow library to create command
        /// unless you derive from it. Should alway use NavigationCommands.
        /// </summary>
        protected internal NavigateBackCommand ()
        {
        }

        /// <summary>
        /// True/False to monitor the enavigation and raise our CanExecuteChanged
        /// in response.
        /// </summary>
        public bool MonitorNavigationStack
        {
            get
            {
                return monitorNavigationStack;
            }

            set
            {
                monitorNavigationStack = value;

                var ns = XamUInfrastructure.ServiceLocator.Get<INavigationService> ();
                if (ns != null) {
                    // Always unsubscribe to ensure we are never > 1.
                    ns.Navigated -= OnUpdateCanExecuteChanged;
                    ns.NavigatedBack -= OnUpdateCanExecuteChanged;

                    if (monitorNavigationStack) {
                        ns.Navigated += OnUpdateCanExecuteChanged;
                        ns.NavigatedBack += OnUpdateCanExecuteChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Event raised when the state of the NavigateBackCommand has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// This is called when the navigation stack has changed.
        /// It refreshes the state of the command.
        /// </summary>
        /// <param name="sender">this</param>
        /// <param name="e">Empty EventArgs</param>
        void OnUpdateCanExecuteChanged (object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke (this, EventArgs.Empty);
        }

        /// <summary>
        /// This is called to determine whether the command can be executed.
        /// We use the current navigation stack state.
        /// </summary>
        /// <returns>True if the command is valid</returns>
        /// <param name="parameter">Parameter.</param>
        public bool CanExecute (object parameter)
        {
            var ns = XamUInfrastructure.ServiceLocator.Get<INavigationService> ();
            return ns != null && ns.CanGoBack;
        }

        /// <summary>
        /// This is called to execute the command.
        /// </summary>
        /// <param name="parameter">Not used</param>
        public async void Execute (object parameter)
        {
            var ns = XamUInfrastructure.ServiceLocator.Get<INavigationService> ();
            if (ns != null) {
                await ns.GoBackAsync ();
            }
        }
    }
}

