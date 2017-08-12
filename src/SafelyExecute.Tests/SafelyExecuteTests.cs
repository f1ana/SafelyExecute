using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafelyExecute.Tests.Sample;

namespace SafelyExecute.Tests {
    [TestClass]
    public class SafelyExecuteTests {
        [TestMethod]
        public void TestActionPassed() {
            var c = new SampleClass();
            var result = c.ThisWorks();
            Assert.IsTrue(result.Successful);
            Assert.IsNull(result.raisedException);
        }

        [TestMethod]
        public void TestActionFailed() {
            var c = new SampleClass();
            var result = c.ThisDoesNotWork();
            Assert.IsFalse(result.Successful);
            Assert.IsNotNull(result.raisedException);
            Assert.IsInstanceOfType(result.raisedException, typeof(Exception));
        }

        [TestMethod]
        public void TestFunctionPassed() {
            var c = new SampleClass();
            var result = c.ThisGenericWorks();
            Assert.IsTrue(result.Successful);
            Assert.IsNull(result.raisedException);
            Assert.AreEqual(result.Result, 1);
        }

        [TestMethod]
        public void TestFunctionPrimitivePassed() {
            var c = new SampleClass();
            var result = c.ThisGenericDefaultWorks();
            Assert.IsFalse(result.Successful);
            Assert.IsNotNull(result.raisedException);
            Assert.IsInstanceOfType(result.raisedException, typeof(Exception));
            Assert.AreEqual(result.Result, 0);
        }

        [TestMethod]
        public void TestFunctionReferencePassed() {
            var c = new SampleClass();
            var result = c.ThisGenericReferenceDefaultWorks();
            Assert.IsFalse(result.Successful);
            Assert.IsNotNull(result.raisedException);
            Assert.IsInstanceOfType(result.raisedException, typeof(Exception));
            Assert.IsInstanceOfType(result.Result, typeof(List<int>));
        }

        [TestMethod]
        public async Task TestAsyncFunctionPassed() {
            var x = new Func<Task>(async () => { await Task.Delay(1000); });
            var c = new SampleClass();
            var result = await c.SafelyExecuteAsync(x);
            Assert.IsTrue(result.Successful);
            Assert.IsNull(result.raisedException);
        }

        [TestMethod]
        public async Task TestAsyncTypedFunctionPassed() {
            var x = new Func<Task<int>>(async () => {
                await Task.Delay(1000);
                return 1;
            });
            var c = new SampleClass();
            var result = await c.SafelyExecuteAsync(x);
            Assert.IsTrue(result.Successful);
            Assert.IsNull(result.raisedException);
            Assert.AreEqual(result.Result, 1);
        }

        [TestMethod]
        public async Task TestAsyncTypedFunctionExceptionPassed() {
            var x = new Func<Task<int>>(async () => {
                await Task.Delay(1000);
                throw new Exception("uh oh");
            });
            var y = new Func<Task<int>>(async () => {
                await Task.Delay(1);
                return 1;
            });

            var c = new SampleClass();
            var result = await c.SafelyExecuteAsync(x, y);
            Assert.IsFalse(result.Successful);
            Assert.IsNotNull(result.raisedException);
            Assert.IsInstanceOfType(result.raisedException, typeof(Exception));
            Assert.AreEqual(result.Result, 1);
        }

        [TestMethod]
        public void TestOverrideBehavior() {
            Assert.ThrowsException<PlatformNotSupportedException>(() => {
                Func<int> test = () => {
                    throw new Exception();
                };

                var c = new SafelyExecuteSubClass();
                c.SafelyExecute(test);
            });
        }
    }
}