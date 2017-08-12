using System;

namespace SafelyExecute.Tests.Sample {
    public class SafelyExecuteSubClass : SafelyExecuteBase {
        public override SafelyExecuteGenericResult<T> SafelyExecute<T>(Func<T> method, Func<T> failMethod = null) {
            var result = base.SafelyExecute(method, failMethod);

            if (!result.Successful) {
                throw new PlatformNotSupportedException();
            }

            return result;
        }
    }
}