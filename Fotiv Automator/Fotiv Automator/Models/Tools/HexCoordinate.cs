using Fotiv_Automator.Infrastructure.Extensions;
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

        #region Ring Collection
        public List<HexCoordinate> GetRingAroundHex(int ringNumber)
        {
            var ring = new List<HexCoordinate>();
            HexCoordinate tempCoordinate = this;

            // We start off by going north to the correct ring, and then adding it to our list
            for (int i = 0; i < ringNumber; i++)
            {
                tempCoordinate = tempCoordinate.North();
            }
            ring.Add(tempCoordinate);

            // After that, we proceed to go clockwise around the ring until we come back to the start
            for (int i = 0; i < ringNumber; i++)
            {
                tempCoordinate = tempCoordinate.SouthEast();

                // If the ring is an odd number, you need to re-align the coordinates back to where whey should be
                if (IntExtensions.IsOdd(i)) tempCoordinate = tempCoordinate.North();

                ring.Add(tempCoordinate);
            }

            // The rightmost segment is easy because we can go straight down the required number of times
            for (int i = 0; i < ringNumber; i++)
            {
                tempCoordinate = tempCoordinate.South();
                ring.Add(tempCoordinate);
            }

            // We utilize Current Ring - 1 because we only want this to run on rings that are greater than 1
            for (int i = 0; i < ringNumber - 1; i++)
            {
                if (ringNumber.IsEven())
                {
                    if (i.IsEven())
                        tempCoordinate = tempCoordinate.SouthWest();
                    else
                        tempCoordinate = tempCoordinate.West();
                }
                else
                {
                    if (i.IsEven())
                        tempCoordinate = tempCoordinate.West();
                    else
                        tempCoordinate = tempCoordinate.SouthWest();
                }

                ring.Add(tempCoordinate);
            }

            // Coming into this statement, we are now at the bottom 3 coordinates.
            // Since our grid is laid out vertically, we can assume that these three hexes will be directly west of each other
            // So we only have to go west twice to make our way to the next segment 
            for (int i = 0; i < 2; i++)
            {
                tempCoordinate = tempCoordinate.West();
                ring.Add(tempCoordinate);
            }

            // We utilize Current Ring - 1 because we only want this to run on rings that are greater than 1
            for (int i = 0; i < ringNumber - 1; i++)
            {
                if (i.IsEven())
                    tempCoordinate = tempCoordinate.NorthWest();
                else
                    tempCoordinate = tempCoordinate.West();

                ring.Add(tempCoordinate);
            }

            // The left most segment is easy because we can just go straight up
            for (int i = 0; i < ringNumber; i++)
            {
                tempCoordinate = tempCoordinate.North();
                ring.Add(tempCoordinate);
            }

            // We utilize Current Ring - 1 because we only want this to run on rings that are greater than 1
            for (int i = 0; i < ringNumber - 1; i++)
            {
                if (ringNumber.IsEven())
                {
                    if (i.IsEven())
                        tempCoordinate = tempCoordinate.East();
                    else
                        tempCoordinate = tempCoordinate.NorthEast();
                }
                else
                {
                    if (i.IsEven())
                        tempCoordinate = tempCoordinate.NorthEast();
                    else
                        tempCoordinate = tempCoordinate.East();
                }

                ring.Add(tempCoordinate);
            }

            return ring;
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
            return !one.Equals(two);
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
