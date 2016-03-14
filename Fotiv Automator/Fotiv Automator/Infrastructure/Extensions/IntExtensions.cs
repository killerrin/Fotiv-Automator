using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Infrastructure.Extensions
{
    public static class IntExtensions
    {
        public static bool SameID(this int id1, int id2)
        {
            return id1 == id2;
        }

        public static bool IsBetween(this int num, int low, int high)
        {
            return num >= low && num <= high;
        }
    }
}
