using System;
using System.Collections.Generic;
using System.Text;
using Fotiv_Automator.Models.Tools;
using Fotiv_Automator.Models.StarMapGenerator.Models;

namespace Fotiv_Automator.Models.StarMapGenerator
{
    public class StarSectorGenerator
    {
        public HexCoordinate WidthHeight { get { return new HexCoordinate(Width, Height); } }
        public int Width = 1;
        public int Height = 1;

        public Dice Die = new Dice();

        public StarSectorGenerator(HexCoordinate widthHeight) : this(widthHeight.X, widthHeight.Y) { }
        public StarSectorGenerator(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public StarSector Generate()
        {
            StarSector sector = new StarSector();
            Die = new Dice();

            for (int x = 0; x < Width; x++)
            {
                var row = new List<StarSystem>(Height);
                for (int y = 0; y < Height; y++)
                {
                    var starSystem = StarSystem.Generate(Die);
                    starSystem.Coordinate = new HexCoordinate(x, y);
                    row.Add(starSystem);
                }
                sector.Sector.Add(row);
            }
            return sector;
        }
    }
}
