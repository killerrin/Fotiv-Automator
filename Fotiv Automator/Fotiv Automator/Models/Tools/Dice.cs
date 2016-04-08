using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.Tools
{
    public class Dice
    {
        Random m_random;

        public Dice() { m_random = new Random(); }
        public Dice(int seed) { ChangeSeed(seed); }
        public void ChangeSeed(int seed) { m_random = new Random(seed); }

        public bool FlipCoin() { return m_random.Next(2) == 0; }

        public int RollBetween(int min, int max) { return m_random.Next(min, max); }
        public int Roll(int sidesOnDice) { return m_random.Next(1, sidesOnDice + 1); }
        public int Roll(int numberOfDice, int sidesOnDice)
        {
            int totalRoll = 0;
            for (int i = 0; i < numberOfDice; i++)
                totalRoll += Roll(sidesOnDice);
            return totalRoll;
        }

        public List<int> RollMultiple(int numberOfDice, int sidesOnDice)
        {
            List<int> rolls = new List<int>();

            for (int i = 0; i < numberOfDice; i++)
            {
                rolls.Add(Roll(sidesOnDice));
            }

            return rolls;
        }
        public List<int> RollMultiple(string dieString)
        {
            string[] dieSplit = dieString.Split('d');

            int numberOfDice;
            int sidesOnDice;

            if (!int.TryParse(dieSplit[0], out numberOfDice))
                return new List<int>();
            if (!int.TryParse(dieSplit[1], out sidesOnDice))
                return new List<int>();

            return RollMultiple(numberOfDice, sidesOnDice);
        }
    }
}
