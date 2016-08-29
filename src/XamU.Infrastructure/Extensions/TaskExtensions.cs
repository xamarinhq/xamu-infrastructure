//
// TaskExtensions.cs
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Extensions for the System.Threading.Tasks.Task type.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// This method can be used to ignore the result of a Task without
        /// losing the ability to throw the exception if the task fails.
        /// </summary>
        /// <example>
        /// <code>
        ///     Task.Run(() => ...).IgnoreResult();
        /// </code>
        /// </example>
        /// <param name="task">Task to ignore</param>
        /// <param name="faultHandler">Optional handler for the exception; if null then method throws on UI thread.</param>
        /// <param name="member">Caller name</param>
        /// <param name="lineNumber">Line number.</param>
        public static void IgnoreResult(this Task task, Action<Exception> faultHandler = null, [CallerMemberName] string member = "", [CallerLineNumber] int lineNumber = 0)
        {
            task.ContinueWith(tr =>
            {
                Debug.WriteLine ("Encountered {0} at {1}, line #{2}",
                            task.Exception.GetType (), member, lineNumber);
                Debug.WriteLine(task.Exception.Flatten());

                if (faultHandler != null)
                {
                    faultHandler.Invoke (task.Exception);
                }
                else
                {
                    Debug.WriteLine ("WARNING: exception {0} was ignored!", task.Exception.GetType ());
                }

            }, CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}

