using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes
{
    public class Checkbox
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public Checkbox() { }
        public Checkbox(int id, string name, bool isChecked)
        {
            ID = id;
            Name = name;
            IsChecked = isChecked;
        }

        public override string ToString()
        {
            return $"IsChecked: {IsChecked} - {ID}, {Name}";
        }
    }
}
