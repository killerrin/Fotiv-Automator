using System;
using System.Collections.Generic;
using System.Text;
using Fotiv_Automator.Models.Tools;
using Fotiv_Automator.Models.StarMapGenerator.Models;
using System.Diagnostics;
using Fotiv_Automator.Models.StarMapGenerator.Models.Enums;
using Fotiv_Automator.Infrastructure.Extensions;

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

        public static StarSector Load(List<string> input)
        {
            StarSector sector = new StarSector();

            foreach (var line in input)
            {
                Debug.WriteLine(line);
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith("Hex")) // StarSystem
                {
                    int hexPosition = line.IndexOf(':');
                    int resourcePosition = line.IndexOf("T");

                    string hex = line.Substring(hexPosition + 1, resourcePosition - hexPosition - 1);
                    var splitHex = hex.Split(',');
                    StarSystem currentStarSystem = new StarSystem();
                    currentStarSystem.Coordinate = new HexCoordinate(int.Parse(splitHex[0]), int.Parse(splitHex[1]));

                    if (currentStarSystem.Coordinate.Y == 0)
                        sector.Sector.Add(new List<StarSystem>());
                    sector.Sector[sector.Sector.Count - 1].Add(currentStarSystem);
                }
                else if (line.Contains("Radiation")) // Star
                {
                    string[] split = line.Split('\t');

                    Star star = new Star();
                    star.Classification = (StarClassification)Enum.Parse(typeof(StarClassification), split[0].Replace(" ", ""));
                    star.Age = (StarAge)Enum.Parse(typeof(StarAge), split[1].Replace(" ", ""));
                    star.Radiation = (RadiationLevel)Enum.Parse(typeof(RadiationLevel), split[2].Replace(" ", ""));

                    var systemColumn = sector.Sector[sector.Sector.Count - 1];
                    systemColumn[systemColumn.Count - 1].Stars.Add(star);
                }
                else if (char.IsDigit(line[0])) // Body
                {
                    string[] split = line.Split('\t');

                    CelestialBody body = new CelestialBody();
                    body.ResourceValue = StringExtensions.GetLeadingNumber(line);
                    body.CelestialType = (CelestialBodyType)Enum.Parse(typeof(CelestialBodyType), split[1].Replace(" ", ""));
                    body.TerraformingTier = (TerraformationTier)Enum.Parse(typeof(TerraformationTier), split[2].Replace(" ", ""));
                    body.StageOfLife = (LifeStage)Enum.Parse(typeof(LifeStage), split[3].Replace(" ", ""));

                    var systemColumn = sector.Sector[sector.Sector.Count - 1];
                    var system = systemColumn[systemColumn.Count - 1];
                    var star = system.Stars[system.Stars.Count - 1];
                    star.CelestialBodies.Add(body);
                }
                else if (char.IsWhiteSpace(line[0])) // Starts with a Tab
                {
                    
                    if (char.IsDigit(line[1])) // Orbiting Body
                    {
                        string[] split = line.Split('\t');

                        CelestialSatellite satellite = new CelestialSatellite();
                        satellite.ResourceValue = StringExtensions.GetLeadingNumber(line);
                        satellite.CelestialType = (CelestialSatelliteType)Enum.Parse(typeof(CelestialSatelliteType), split[2].Replace(" ", ""));
                        satellite.TerraformingTier = (TerraformationTier)Enum.Parse(typeof(TerraformationTier), split[3].Replace(" ", ""));
                        satellite.StageOfLife = (LifeStage)Enum.Parse(typeof(LifeStage), split[4].Replace(" ", ""));

                        var systemColumn = sector.Sector[sector.Sector.Count - 1];
                        var system = systemColumn[systemColumn.Count - 1];
                        var star = system.Stars[system.Stars.Count - 1];
                        var body = star.CelestialBodies[star.CelestialBodies.Count - 1];
                        body.OrbitingSatellites.Add(satellite);
                    }
                    else if (line.Substring(1).StartsWith("TL")) // Sentient on Body
                    {
                        string[] split = line.Split('\t');

                        SentientSpecies species = new SentientSpecies();
                        species.TechLevel = (CivilizationTechLevel)Enum.Parse(typeof(CivilizationTechLevel), split[1].Substring(4).Replace(" ", ""));

                        // Civilization Traits
                        string[] civTraits = split[2].Split(' ');
                        for (int i = 0; i < civTraits.Length; i++)
                        {
                            string trait = civTraits[i];
                            if (civTraits[i] == "Peace")
                            {
                                trait += civTraits[i + 1];
                                i++;
                            }

                            if (!string.IsNullOrWhiteSpace(trait))
                                species.Traits.Add((CivilizationTraits)Enum.Parse(typeof(CivilizationTraits), trait));
                        }

                        // Species Classification
                        string[] speciesClassification = split[3].Split(' ');
                        for (int i = 0; i < speciesClassification.Length; i++)
                        {
                            string classification = speciesClassification[i];
                            if (speciesClassification[i] == "Space")
                            {
                                classification += speciesClassification[i + 1];
                                i++;
                            }

                            if (!string.IsNullOrWhiteSpace(classification))
                                species.Classifications.Add((AnimalClassification)Enum.Parse(typeof(AnimalClassification), classification));
                        }

                        var systemColumn = sector.Sector[sector.Sector.Count - 1];
                        var system = systemColumn[systemColumn.Count - 1];
                        var star = system.Stars[system.Stars.Count - 1];
                        var body = star.CelestialBodies[star.CelestialBodies.Count - 1];
                        body.Sentients.Add(species);
                    }
                    else if (char.IsWhiteSpace(line.Substring(1)[0])) // Sentient on Satellite
                    {
                        string[] split = line.Split('\t');

                        SentientSpecies species = new SentientSpecies();
                        species.TechLevel = (CivilizationTechLevel)Enum.Parse(typeof(CivilizationTechLevel), split[2].Substring(4).Replace(" ", ""));

                        // Civilization Traits
                        string[] civTraits = split[3].Split(' ');
                        for (int i = 0; i < civTraits.Length; i++)
                        {
                            string trait = civTraits[i];
                            if (civTraits[i] == "Peace")
                            {
                                trait += civTraits[i + 1];
                                i++;
                            }

                            if (!string.IsNullOrWhiteSpace(trait))
                                species.Traits.Add((CivilizationTraits)Enum.Parse(typeof(CivilizationTraits), trait));
                        }

                        // Species Classification
                        string[] speciesClassification = split[4].Split(' ');
                        for (int i = 0; i < speciesClassification.Length; i++)
                        {
                            string classification = speciesClassification[i];
                            if (speciesClassification[i] == "Space")
                            {
                                classification += speciesClassification[i + 1];
                                i++;
                            }

                            if (!string.IsNullOrWhiteSpace(classification))
                                species.Classifications.Add((AnimalClassification)Enum.Parse(typeof(AnimalClassification), classification));
                        }

                        var systemColumn = sector.Sector[sector.Sector.Count - 1];
                        var system = systemColumn[systemColumn.Count - 1];
                        var star = system.Stars[system.Stars.Count - 1];
                        var body = star.CelestialBodies[star.CelestialBodies.Count - 1];

                        if (body.OrbitingSatellites.Count > 0)
                        {
                            var satellite = body.OrbitingSatellites[body.OrbitingSatellites.Count - 1];
                            satellite.Sentients.Add(species);
                        }
                        else
                            body.Sentients.Add(species);
                    }
                }
            }

            return sector;
        }
    }
}
