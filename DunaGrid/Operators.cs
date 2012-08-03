using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid
{
    [Flags]
    public enum Operators
    {
        equal = 0x1,
        not_equal = 0x2,
        greater_than = 0x4,
        lower_than = 0x8,
        like = 0x16,
        regexp = 0x32
    }
}
