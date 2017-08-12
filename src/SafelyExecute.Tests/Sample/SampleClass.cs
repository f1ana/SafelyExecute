using System;
using System.Collections.Generic;

namespace SafelyExecute.Tests.Sample {
    public class SampleClass : SafelyExecuteBase {
        public SafelyExecuteResult ThisWorks() {
            return SafelyExecute(Delegate_ThisWorks, Delegate_ThisWorksFailed);
        }

        private void Delegate_ThisWorks() {
        }

        private void Delegate_ThisWorksFailed() {
        }

        public SafelyExecuteResult ThisDoesNotWork() {
            return SafelyExecute(Delegate_ThisDoesNotWork, Delegate_ThisDoesNotWorkFailed);
        }

        private void Delegate_ThisDoesNotWork() {
            throw new Exception();
        }

        private void Delegate_ThisDoesNotWorkFailed() {
        }

        public SafelyExecuteGenericResult<int> ThisGenericWorks() {
            return SafelyExecute(Delegate_ThisGenericWorks, Delegate_ThisGenericWorksFailed);
        }

        private int Delegate_ThisGenericWorks() {
            return 1;
        }
        private int Delegate_ThisGenericWorksFailed() {
            return -1;
        }

        public SafelyExecuteGenericResult<int> ThisGenericDefaultWorks()
        {
            return SafelyExecute(Delegate_ThisGenericDefaultWorks);
        }

        private int Delegate_ThisGenericDefaultWorks() {
            throw new Exception();
        }

        public SafelyExecuteGenericResult<List<int>> ThisGenericReferenceDefaultWorks() {
            return SafelyExecute(Delegate_ThisGenericReferenceDefaultWorks);
        }

        private List<int> Delegate_ThisGenericReferenceDefaultWorks() {
            throw new Exception();
        }


    }
}