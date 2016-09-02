//
// DependencyServiceWrapper.cs
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

using XamarinUniversity.Interfaces;
using Xamarin.Forms;

namespace XamarinUniversity.Services
{
    /// <summary>
    /// Wrapper around static Xamarin.Forms DependencyService to allow it to
    /// be turned into a mockable interface for unit testing.
    /// </summary>
    public class DependencyServiceWrapper : IDependencyService
    {
        /// <summary>
        /// Retrieve a dependency based on the abstraction <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Get<T>() where T : class
        {
            return DependencyService.Get<T>();
        }

        /// <summary>
        /// Register a specific type as an abstraction
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        public void Register<T> () where T : class, new()
        {
            DependencyService.Register<T> ();
        }

        /// <summary>
        /// Register a type along with an abstraction type.
        /// </summary>
        /// <typeparam name="T">Abstraction type</typeparam>
        /// <typeparam name="TImpl">Type to create</typeparam>
        public void Register<T, TImpl> () 
            where T : class
            where TImpl : class, T, new()
        {
            DependencyService.Register<T, TImpl> ();
        }
    }
}