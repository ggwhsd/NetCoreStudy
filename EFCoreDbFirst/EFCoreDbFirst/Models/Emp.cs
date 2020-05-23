using System;
using System.Collections.Generic;

namespace EFCoreDbFirst.Models
{
    public partial class Emp
    {
        public int Empno { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
