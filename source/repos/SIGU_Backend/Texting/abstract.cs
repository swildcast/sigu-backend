using System;

namespace Texting
{
    // Simple disposable base class for tests — keep minimal to avoid referencing app projects here
    public abstract class TestBase : IDisposable
    {
        public virtual void Dispose()
        {
            // override in derived tests if needed
        }
    }
}
