using System;

namespace SafelyExecute {
    public class SafelyExecuteGenericResult<T> {
        public Exception raisedException;
        public T Result;
        public bool Successful;
    }
}