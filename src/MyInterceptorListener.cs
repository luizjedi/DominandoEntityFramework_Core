using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace diagnosticEFCore
{
    public class MyInterceptorListener : IObserver<DiagnosticListener>
    {
        private readonly MyInterceptor _interceptor = new MyInterceptor();

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(_interceptor);
            }
        }
    }
}