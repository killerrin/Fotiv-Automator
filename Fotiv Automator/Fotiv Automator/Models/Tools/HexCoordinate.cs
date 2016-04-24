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

        public bool IsCoordinate(int x, int y) { return X == x && Y == y; }
        public bool HasNegative() { return X < 0 || Y < 0; }

        #region Direction
        public HexCoordinate North() => this + new HexCoordinate(0, -1);
        public HexCoordinate South() => this + new HexCoordinate(0, 1);

        public HexCoordinate West() => this + new HexCoordinate(-1, 0);
        public HexCoordinate East() => this + new HexCoordinate(1, 0);

        public HexCoordinate NorthWest() => this + new HexCoordinate(-1, -1);
        public HexCoordinate NorthEast() => this + new HexCoordinate(1, -1);

        public HexCoordinate SouthWest() => this + new HexCoordinate(-1, 1);
        public HexCoordinate SouthEast() => this + new HexCoordinate(1, 1);
        #endregion

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
        #endregion

        #region Operator Overloads
        public static bool operator ==(HexCoordinate one, HexCoordinate two)
        {
            return one.Equals(two);
        }
        public static bool operator !=(HexCoordinate one, HexCoordinate two)
        {
            return one.Equals(two);
        }

        public static HexCoordinate operator +(HexCoordinate one, HexCoordinate two)
        {
            return new HexCoordinate(one.X + two.X, one.Y + two.Y);
        }
        public static HexCoordinate operator -(HexCoordinate one, HexCoordinate two)
        {
            return new HexCoordinate(one.X - two.X, one.Y - two.Y);
        }
        #endregion

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
