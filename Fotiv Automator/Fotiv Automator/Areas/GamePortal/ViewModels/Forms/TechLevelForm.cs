﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Forms
{
    public class TechLevelForm
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int AttackDetriment { get; set; } = 0;
    }
}



