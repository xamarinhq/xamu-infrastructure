using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamarinUniversity.Infrastructure;

namespace XamU.Infrastructure.Tests
{
    /// <summary>
    ///This is a test class for ObservableDictionaryTest and is intended
    ///to contain all ObservableDictionaryTest Unit Tests.  It only tests
    /// the new features -- not the entire Dictionary semantics.
    ///</summary>
    [TestClass]
    public class ObservableDictionaryTests
    {
        private readonly string[] changedProperties = {"Count", "Keys", "Values"};

        [TestMethod]
        public void AddStringTest()
        {
            var target = new ObservableDictionary<string,int>();

            bool hitChange = false;
            const string key = "Hello";
            const int value = 10;

            target.PropertyChanged += (s, e) => { Assert.IsTrue(e.PropertyName == $"Item[{key}]" || changedProperties.Contains(e.PropertyName)); };

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
                Assert.AreSame(target, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                var item = (KeyValuePair<string, int>)e.NewItems[0];
                Assert.AreEqual(key, item.Key);
                Assert.AreEqual(value, item.Value);
            };

            target[key] = value;
            Assert.IsTrue(hitChange);
        }

        [TestMethod]
        public void AddTest()
        {
            var target = new ObservableDictionary<int, string>();

            bool hitChange = false;
            const int key = 10;
            const string value = "Hello";

            target.PropertyChanged += (s, e) => { Assert.IsTrue(e.PropertyName == $"Item[{key}]" || changedProperties.Contains(e.PropertyName)); };

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
                Assert.AreSame(target, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                var item = (KeyValuePair<int, string>)e.NewItems[0];
                Assert.AreEqual(key, item.Key);
                Assert.AreEqual(value, item.Value);
            };

            target[key] = value;
            Assert.IsTrue(hitChange);
        }

        [TestMethod()]
        public void ReplaceTest()
        {
            var target = new ObservableDictionary<int, string>();

            bool hitChange = false;
            const int key = 10;
            const string value = "Hello";
            const string value2 = "World";

            target[key] = value;

            target.PropertyChanged += (s, e) => { Assert.IsTrue(e.PropertyName == $"Item[{key}]" || changedProperties.Contains(e.PropertyName)); };

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
                Assert.AreSame(target, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Replace, e.Action);
                var oldItem = (KeyValuePair<int, string>)e.OldItems[0];
                Assert.AreEqual(key, oldItem.Key);
                Assert.AreEqual(value, oldItem.Value);
                var newItem = (KeyValuePair<int, string>)e.NewItems[0];
                Assert.AreEqual(key, newItem.Key);
                Assert.AreEqual(value2, newItem.Value);
            };

            target[key] = value2;
            Assert.IsTrue(hitChange);
        }

        [TestMethod()]
        public void ClearTest()
        {
            var target = new ObservableDictionary<int, string>();

            bool hitChange = false;
            const int key = 10;
            const string value = "Hello";

            target[key] = value;

            target.PropertyChanged += (s, e) => { Assert.IsTrue(changedProperties.Contains(e.PropertyName)); };

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
                Assert.AreSame(target, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
            };

            target.Clear();

            Assert.IsTrue(hitChange);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var target = new ObservableDictionary<int, string>();

            bool hitChange = false;
            const int key = 10;
            const string value = "Hello";

            target[key] = value;

            target.PropertyChanged += (s, e) => { Assert.IsTrue(e.PropertyName == $"Item[{key}]" || changedProperties.Contains(e.PropertyName)); };

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
                Assert.AreSame(target, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                var oldItem = (KeyValuePair<int, string>)e.OldItems[0];
                Assert.AreEqual(key, oldItem.Key);
                Assert.AreEqual(value, oldItem.Value);
            };

            target.Remove(key);

            Assert.IsTrue(hitChange);
        }

        [TestMethod]
        public void AlternateDictionary()
        {
            var target = new ObservableDictionary<int, string>(new ConcurrentDictionary<int, string>());
            bool hitChange = false;
            const int key = 10;
            const string value = "Hello";

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
                Assert.AreSame(target, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                var item = (KeyValuePair<int, string>)e.NewItems[0];
                Assert.AreEqual(key, item.Key);
                Assert.AreEqual(value, item.Value);
            };

            target[key] = value;
            Assert.IsTrue(hitChange);
        }

        [TestMethod]
        public void MassUpdateTest()
        {
            var target = new ObservableDictionary<int, string>();

            bool hitChange = false;

            target.CollectionChanged += (s, e) =>
            {
                hitChange = true;
            };

            using (target.BeginMassUpdate())
            {
                target[1] = "Hello";
                target[2] = "World";
                target[3] = "Testing..";

                Assert.IsFalse(hitChange);
            }

            Assert.IsTrue(hitChange);
        }
    }
}
