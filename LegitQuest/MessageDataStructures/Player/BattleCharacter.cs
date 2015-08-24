using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Player
{
    public class BattleCharacter
    {
        public string name { get; set; }
        public CharacterClass characterClass { get; set; }
        public int hp { get; set; }
        public int maxHp { get; set; }
        public int strength { get; set; } //Physical attack
        public int dexterity { get; set; } //Physical attack for speed-based moves, increases accuracy, crit, and dodge
        public int vitality { get; set; } //Physical resistance, also contributes to some status resistance
        public int magic { get; set; } //Magic Attack
        public int mind { get; set; } //Healing strength
        public int resistance { get; set; } //Magic Defense
        public int accuracy { get; set; } //Higher hit rate, generally useful when fighting enemies much higher than you
        public int critical { get; set; } //Increases critical hit rate
        public int dodge { get; set; } //Increases chance to dodge
    }
}
