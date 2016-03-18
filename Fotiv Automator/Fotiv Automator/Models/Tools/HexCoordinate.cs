using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Tools
{
    public struct HexCoordinate
    {
        public int X;
        public int Y;

        public HexCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }


        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is HexCoordinate)
                return Equals((HexCoordinate)obj);
            return false;
        }
        public bool Equals(HexCoordinate other)
        {
            return X == other.X &&
                   Y == other.Y;
        }
    }
}
