using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models
{
    public class UnitTypes
    {
        public static List<Checkbox> GetUnitTypesCheckboxes()
        {
            var unitTypes = new List<Checkbox>();
            unitTypes.Add(new Checkbox(1, "Ground Unit", false));
            unitTypes.Add(new Checkbox(2, "Boat", false));
            unitTypes.Add(new Checkbox(3, "Plane", false));
            unitTypes.Add(new Checkbox(4, "Spaceship", false));
            return unitTypes;
        }

        public static bool IsGroundUnit(string type)
        {
            if (type == "Ground Unit")
                return true;
            return false;
        }
        public static bool IsBoat(string type)
        {
            if (type == "Boat")
                return true;
            return false;
        }
        public static bool IsPlane(string type)
        {
            if (type == "Plane")
                return true;
            return false;
        }
        public static bool IsSpaceship(string type)
        {
            if (type == "Spaceship")
                return true;
            return false;
        }
    }
}
