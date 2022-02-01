using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore.DicasETruques.Domain
{
    [Keyless]
    public class UserFunction
    {
        public Guid UserId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
