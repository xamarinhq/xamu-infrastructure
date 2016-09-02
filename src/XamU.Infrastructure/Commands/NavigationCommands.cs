//
// NavigationCommands.cs
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

namespace XamarinUniversity.Commands
{
    /// <summary>
    /// Commands which perform common navigation 
    /// </summary>
    public static class NavigationCommands
    {
        /// <summary>
        /// Field to hold back nav command
        /// </summary>
        static NavigateBackCommand navBackCommand;

        /// <summary>
        /// A command which performs a NavigationService.GoBack
        /// </summary>
        public static NavigateBackCommand GoBack => (navBackCommand != null)
                    ? navBackCommand
                    : (navBackCommand = new NavigateBackCommand ());

        /// <summary>
        /// Field to hold fwd nav command
        /// </summary>
        static NavigateToCommand navToCommand;

        /// <summary>
        /// A command which performs a NavigationService.Navigate
        /// </summary>
        public static NavigateToCommand NavigateTo => (navToCommand != null)
                    ? navToCommand
                    : (navToCommand = new NavigateToCommand ());
    }
}

