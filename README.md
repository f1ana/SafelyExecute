# SafelyExecute
A simple, extensible approach to reduce try..catch boilerplate. Targets .NET 4.5, 4.6, and .NET Standard 1.3.

**What does this do?**

This library is intended to reduce the amount of time and effort which a developer needs to write code that does not create unhandled or unwanted exceptions.

**Why would I use this?**

During the development process, a developer might not spend enough time hardening the application, focusing more on functionality first.  Unhandled exceptions may be difficult to troubleshoot, and a developer might not be aware of which APIs actually generate exceptions without studying the source or CLR.  SafelyExecute is effectively an extensible wrapper for try..catch, so that exception handling logic and logging can be completely centralized.  If an exception is thrown, you have the option of returning a default return value.  If not, the library will attempt to return the default value of T, or create an instance.

**Examples**

This package contains extension methods that will work on any object, a static class, and also an abstract class which services could then inherit from.  The trick is that you are passing a method, optionally async, as the first argument.  A second method argument serves as a fallback.

*Standard generic usage*

	   public SafelyExecuteGenericResult<int> ThisGenericWorks() {
           return SafelyExecute(Private_ThisGenericWorks, Private_ThisGenericWorksFailed);
       }

       private int Private_ThisGenericWorks() {
           return 1;
       }
       private int Private_ThisGenericWorksFailed() {
           return -1;
       }

*Async usage*

       // Inline async functions
       var x = new Func<Task<int>>(async () => {
           await Task.Delay(1000);
		   return 1;
       });
       var y = new Func<Task<int>>(async () => {
           await Task.Delay(1);
           return -1;
       });
       // Async using extension method!
       SafelyExecuteGenericResult<int> r = await this.SafelyExecuteAsync(x, y);

*Overriding methods to add centralized logging*

    public class SafelyExecuteSubClass : SafelyExecuteBase {
        public override SafelyExecuteGenericResult<T> SafelyExecute<T>(Func<T> method, Func<T> failMethod = null) {
            var result = base.SafelyExecute(method, failMethod);

            if (!result.Successful) {
                LogError("An error has occurred.", result.raisedException);
            }

            return result;
        }
    }

Although you can create functions or methods within another function using Func or Action, I have noticed that this can cause a decrease in performance, as they are re-created during every call to the parent function or method.  Generally, I create a work method and a "fail" method for every time I call SafelyExecute.  It seems like extra work at first, but I feel like this generally makes me a more effective developer.

**Contributions**

I welcome any and all suggestions or improvements to the codebase.  Thanks for dropping by and hope you find a good use for this library!
