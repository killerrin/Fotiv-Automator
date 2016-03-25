﻿using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.ViewModels
{
    public class SpeciesCheckbox
    {
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public bool IsChecked { get; set; }

        public SpeciesCheckbox() { }
        public SpeciesCheckbox(int speciesID, string speciesName, bool isChecked)
        {
            SpeciesID = speciesID;
            SpeciesName = speciesName;
            IsChecked = isChecked;
        }
    }
}
