using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Stratysis.Domain.Core
{
    public class Slice
    {
        public DateTime DateTime { get; set; }
        
        public IDictionary<string, Bar> Bars { get; set; } = new Dictionary<string, Bar>();
    }
}
