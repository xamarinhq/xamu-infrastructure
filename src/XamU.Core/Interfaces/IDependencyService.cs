//
// IDependencyService.cs
//
// Author:
//       Mark Smith <smmark@microsoft.com>
//
// Copyright (c) 2016-2018 Xamarin, Microsoft.
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

namespace XamarinUniversity.Infrastructure
{
	/// <summary>
	/// Type of dependency creation to perform
	/// </summary>
	public enum DependencyScope
	{
		/// <summary>
		/// Use a single instance (cached off)
		/// </summary>
		Global,
        /// <summary>
		/// Create a new instance of the dependency
		/// </summary>
		NewInstance
	}

	/// <summary>
	/// Interface to wrap a ServiceLocator
	/// </summary>
	public interface IDependencyService
	{
		/// <summary>
		/// Register a specific type as an abstraction
		/// </summary>
		/// <typeparam name="T">The class to register</typeparam>
		void Register<T> () where T : class, new();

		/// <summary>
		/// Register a specific abstraction associated to a type.
		/// </summary>
		/// <typeparam name="T">The abstraction</typeparam>
		/// <typeparam name="TImpl">The implementation</typeparam>
		void Register<T, TImpl> () where T : class where TImpl : class, T, new();

		/// <summary>
		/// Register a specific instance of an abstraction.
		/// </summary>
		/// <typeparam name="T">Abstraction type</typeparam>
		/// <param name="impl">Instance to use</param>
		void Register<T>(T impl) where T : class;

		/// <summary>
		/// Retrieve a specific implementation from the locator.
        /// Defaults to DependencyScope.Global
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		T Get<T>() where T : class;

		/// <summary>
		/// Retrieve a specific implementation from the locator.
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="scope">Global or new instance</param>
        /// <remarks>
        /// Note: the scope parameter is only used when the object was registered as a type and not an instance.
        /// - In the case of an instance registration, that same instance is always returned.
        /// - In the case of no registration found, a newly created object is always returned.
        /// </remarks>
		T Get<T>(DependencyScope scope) where T : class;
	}
}