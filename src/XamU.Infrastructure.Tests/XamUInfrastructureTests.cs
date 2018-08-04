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
            XamUInfrastructure.Reset();
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
            Assert.AreSame(ds1, ds2);
        }

        [TestMethod]
        public void RegisterNavigationAddsBothInterfaces()
        {
            var mds = new MockDependencService();
            var sl = XamUInfrastructure.Init(mds, RegisterBehavior.Navigation);

            Assert.IsTrue(mds.HasType(typeof(INavigationPageService)));
            Assert.IsTrue(mds.HasType(typeof(INavigationService)));
            Assert.IsFalse(mds.HasType(typeof(IMessageVisualizerService)));
            Assert.IsTrue(mds.HasType(typeof(IDependencyService)));
        }

        [TestMethod]
        public void RegisterVisualizaerAddsInterface()
        {
            var mds = new MockDependencService();
            var sl = XamUInfrastructure.Init(mds, RegisterBehavior.MessageVisualizer);

            Assert.IsFalse(mds.HasType(typeof(INavigationPageService)));
            Assert.IsFalse(mds.HasType(typeof(INavigationService)));
            Assert.IsTrue(mds.HasType(typeof(IMessageVisualizerService)));
            Assert.IsTrue(mds.HasType(typeof(IDependencyService)));
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
            readonly List<Type> registeredTypes = new List<Type>();

            public bool HasType(Type type)
            {
                return registeredTypes.Contains(type);
            }

            public void Register<T>() where T : class, new()
            {
                registeredTypes.Add(typeof(T));
            }

            public void Register<T, TImpl>() where T : class where TImpl : class, T, new()
            {
                registeredTypes.Add(typeof(T));
            }

            public void Register<T>(T impl) where T : class
            {
                registeredTypes.Add(typeof(T));
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
