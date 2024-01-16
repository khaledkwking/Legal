namespace Infrastructure
{
    using System;
    using System.Diagnostics;

    public abstract class DispoInfrastructurebleResource : IDisposable
    {
        ~DispoInfrastructurebleResource()
        {
            Dispose(false);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}