using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCMovie.Services
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
