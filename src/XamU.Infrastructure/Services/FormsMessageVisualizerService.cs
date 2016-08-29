//
// FormsMessageVisualizerService.cs
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

using System.Threading.Tasks;
using XamarinUniversity.Interfaces;
using Xamarin.Forms;

namespace XamarinUniversity.Services
{
    /// <summary>
    /// Wrapper around Page.DisplayAlert to turn it into a Message Visualizer 
    /// service for Xamarin.Forms which can be used from a ViewModel and mocked
    /// for Unit Testing.
    /// </summary>
    public class FormsMessageVisualizerService : IMessageVisualizerService
    {
        /// <summary>
        /// Show a message using the Forms DisplayAlert method.
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="ok">Ok.</param>
        /// <param name="cancel">Cancel.</param>
        public async Task<bool> ShowMessage(
            string title, string message, string ok, string cancel=null)
        {
            if (cancel == null) {
                await Application.Current.MainPage.DisplayAlert(title, message, ok);
                return true;
            }

            return await Application.Current.MainPage.DisplayAlert(
                title, message, ok, cancel);
        }
    }
}

