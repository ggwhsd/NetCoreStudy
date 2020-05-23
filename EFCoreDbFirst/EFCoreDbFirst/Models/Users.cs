using System;
using System.Collections.Generic;

namespace EFCoreDbFirst.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public DateTime? LastConnectTime { get; set; }
    }
}
