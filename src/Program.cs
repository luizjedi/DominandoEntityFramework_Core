using EFCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                var sql = db.Departments.Where(p => p.Id > 0).ToQueryString();
            
                Console.WriteLine(sql);
            }
        }
    }
}
