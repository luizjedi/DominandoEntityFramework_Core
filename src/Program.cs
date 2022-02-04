using diagnosticEFCore.Data;
using System;
using System.Diagnostics;
using System.Linq;

namespace diagnosticEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            DiagnosticListener.AllListeners.Subscribe(new MyInterceptorListener());

            using var db = new ApplicationContext();
            db.Database.EnsureCreated();

            _ = db.Departments.Where(p => p.Id > 0).ToArray();
        }
    }
}
