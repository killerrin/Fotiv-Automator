using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Tools
{
    public struct HexCoordinate : IEquatable<HexCoordinate>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public HexCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsCoordinate(int x, int y)
        {
            return X == x && Y == y;
        }

        #region Distance
        public int Distance(HexCoordinate other)
        {
            int xSteps = Math.Abs(X - other.X);
            int ySteps = Math.Abs(Y - other.Y);

            return Math.Max(xSteps, ySteps) + Math.Abs(xSteps - ySteps);
        }

        public bool WithinDistance(HexCoordinate hex, int maxDistance)
        {
            return Distance(hex) < maxDistance;
        }
        #endregion

        #region Equals
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

        public static bool operator ==(HexCoordinate one, HexCoordinate two)
        {
            return one.Equals(two);
        }
        public static bool operator !=(HexCoordinate one, HexCoordinate two)
        {
            return one.Equals(two);
        }
        #endregion

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
