using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms;
using XamarinUniversity.Services;

namespace XamU.Infrastructure.Tests
{
    [TestClass]
    public class DependencyServiceWrapperTests
    {
        public interface IService1 { }

        public class ServiceClass1 : IService1
        {
        }

        public class ServiceClass2
        {
            public IService1 Service1 { get; }

            public ServiceClass2(IService1 service1)
            {
                Service1 = service1;
            }
        }

        [TestInitialize]
        public void Setup()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            DependencyService.Register<ServiceClass1>();
        }

        [TestMethod]
        public void CheckGetMethod()
        {
            DependencyServiceWrapper wrapper = new DependencyServiceWrapper();

            var instance = wrapper.Get<IService1>();
            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(ServiceClass1));
        }

        [TestMethod]
        public void CheckGetDefaultMethod()
        {
            DependencyServiceWrapper wrapper = new DependencyServiceWrapper();

            var instance1 = wrapper.Get<IService1>();
            Assert.IsNotNull(instance1);

            var instance2 = wrapper.Get<IService1>();
            Assert.IsNotNull(instance2);

            Assert.AreEqual(instance1, instance2);
        }

        [TestMethod]
        public void CheckGetInstanceMethod()
        {
            DependencyServiceWrapper wrapper = new DependencyServiceWrapper();

            var instance1 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.NewInstance);
            Assert.IsNotNull(instance1);

            var instance2 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.NewInstance);
            Assert.IsNotNull(instance2);

            Assert.AreNotEqual(instance1, instance2);
        }


        [TestMethod]
        public void CheckGetGlobalMethod()
        {
            DependencyServiceWrapper wrapper = new DependencyServiceWrapper();

            var instance1 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.Global);
            Assert.IsNotNull(instance1);

            var instance2 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.Global);
            Assert.IsNotNull(instance2);

            Assert.AreEqual(instance1, instance2);
        }

        [TestMethod]
        public void CreateObjectWithDefaultParameters()
        {
            DependencyServiceWrapper wrapper = new DependencyServiceWrapper();

            var instance1 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.Global);
            Assert.IsNotNull(instance1);

            var instance2 = wrapper.Get<ServiceClass2>();
            Assert.IsNotNull(instance2);
            Assert.AreEqual(instance1, instance2.Service1);
        }

        [TestMethod]
        public void CreateObjectWithLocalParameters()
        {
            DependencyServiceWrapper wrapper = new DependencyServiceWrapper();

            var instance1 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.NewInstance);
            Assert.IsNotNull(instance1);

            var instance2 = wrapper.Get<IService1>(XamarinUniversity.Infrastructure.DependencyScope.Global);
            Assert.IsNotNull(instance2);

            var instance3 = wrapper.Get<ServiceClass2>(XamarinUniversity.Infrastructure.DependencyScope.NewInstance);
            Assert.IsNotNull(instance3);
            Assert.AreNotEqual(instance1, instance3.Service1);
            Assert.AreNotEqual(instance2, instance3.Service1);
        }

    }
}
