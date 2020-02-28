using System.Collections.Generic;

namespace SailTally.Classes
{
    public class Races : List<Race>
    {
        public new bool Contains(Race race)
        {
            foreach (var raceScan in this)
            {
                if (raceScan == race)
                {
                    return true;
                }
            }
            return false;
        }
    }
}