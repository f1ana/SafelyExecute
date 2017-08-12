using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SafelyExecute {
    public abstract class SafelyExecuteBase {
        public virtual SafelyExecuteResult SafelyExecute(Action method, Action failMethod = null) {
            try {
                method();
                return new SafelyExecuteResult {Successful = true};
            }
            catch (Exception e) {
                failMethod?.Invoke();
                return new SafelyExecuteResult {Successful = false, raisedException = e};
            }
        }

        public virtual async Task<SafelyExecuteResult> SafelyExecuteAsync(Func<Task> method, Func<Task> failMethod = null) {
            try {
                await method();
                return new SafelyExecuteResult {Successful = true};
            }
            catch (Exception e) {
                if (failMethod != null) {
                    await failMethod.Invoke();
                }

                return new SafelyExecuteResult {Successful = false, raisedException = e};
            }
        }

        public virtual SafelyExecuteGenericResult<T> SafelyExecute<T>(Func<T> method, Func<T> failMethod = null) {
            try {
                var retVal = method();
                return new SafelyExecuteGenericResult<T> {Successful = true, Result = retVal};
            }
            catch (Exception e) {
                if (failMethod == null) {
                    var valueType = !typeof(T).GetTypeInfo().IsValueType;
                    T rv;
                    if (!valueType) {
                        rv = default(T);
                    }
                    else {
                        rv = (T) Activator.CreateInstance(typeof(T));
                    }
                    return new SafelyExecuteGenericResult<T> {Successful = false, raisedException = e, Result = rv};
                }

                var retVal = failMethod.Invoke();
                return new SafelyExecuteGenericResult<T> {Successful = false, raisedException = e, Result = retVal};
            }
        }

        public virtual async Task<SafelyExecuteGenericResult<T>> SafelyExecuteAsync<T>(Func<Task<T>> method,
            Func<Task<T>> failMethod = null) {
            try {
                var retVal = await method();
                return new SafelyExecuteGenericResult<T> {Successful = true, Result = retVal};
            }
            catch (Exception e) {
                if (failMethod == null) {
                    var valueType = !typeof(T).GetTypeInfo().IsValueType;
                    T rv;
                    if (!valueType) {
                        rv = default(T);
                    }
                    else {
                        rv = (T) Activator.CreateInstance(typeof(T));
                    }
                    return new SafelyExecuteGenericResult<T> {Successful = false, raisedException = e, Result = rv};
                }

                var retVal = await failMethod.Invoke();
                return new SafelyExecuteGenericResult<T> {Successful = false, raisedException = e, Result = retVal};
            }
        }
    }
}