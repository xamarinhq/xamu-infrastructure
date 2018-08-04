using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamarinUniversity.Infrastructure;
using XamarinUniversity.Services;

namespace XamU.Infrastructure.Tests
{
    [TestClass]
    public class XamUInfrastructureTests
    {
        [TestInitialize]
        public void Reset()
        {
            // Reset private field.
            XamUInfrastructure.serviceLocator = null;
        }

        [TestMethod]
        public void CheckThatDefaultDependencyServiceSetsServiceLocator()
        {
            var ds = XamUInfrastructure.Init();
            Assert.IsNotNull(XamUInfrastructure.ServiceLocator);
            Assert.AreSame(ds, XamUInfrastructure.ServiceLocator);
        }

        [TestMethod]
        public void CheckThatSuppliedDependencyServiceSetsServiceLocator()
        {
            var ds1 = new MockDependencService();
            var ds2 = XamUInfrastructure.Init(ds1);
            Assert.IsNotNull(XamUInfrastructure.ServiceLocator);
            Assert.AreSame(ds1, XamUInfrastructure.ServiceLocator);
        }

        [TestMethod]
        public void CheckThatUsingServiceLocatorBeforeInitThrowsException()
        {
            var sl = XamUInfrastructure.ServiceLocator;
            Assert.IsNotNull(sl);
            Assert.IsNotNull(XamUInfrastructure.ServiceLocator);
            Assert.ThrowsException<InvalidOperationException>(() => XamUInfrastructure.Init(new MockDependencService()));
        }

        [TestMethod]
        public void AllowMultipleInitCalls()
        {
            var ds = XamUInfrastructure.Init();
            var ds2 = XamUInfrastructure.Init();
            Assert.AreSame(ds,ds2);
        }

        class MockDependencService : IDependencyService
        {
            public void Register<T>() where T : class, new()
            {
            }

            public void Register<T, TImpl>() where T : class where TImpl : class, T, new()
            {
            }

            public void Register<T>(T impl) where T : class
            {
            }

            public T Get<T>() where T : class
            {
                throw new NotImplementedException();
            }

            public T Get<T>(DependencyScope scope) where T : class
            {
                throw new NotImplementedException();
            }
        }
    }
}
