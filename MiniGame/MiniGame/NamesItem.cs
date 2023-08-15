using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class NamesItem
    {
        private readonly Dictionary<string, string[]> _names = new Dictionary<string, string[]>
        {
            { "NamesHelmet", new[] { "Cracked Helm", "Leather Cap", "Guardian Visor", "Royal Crown" } },
            { "NamesBody", new[] { "Patchwork Jerkin", "Leatherbound Jerkin", "Granite Surcoat", "Royal Robes" } },
            { "NamesLegs", new[] { "Breezy Legwraps", "Steelthread Legwraps", "Guardian Calf Protectors", "Diamond Shin Guards" } },
            { "NamesShield", new[] { "Gossamer Shield", "Steelplate Buckler", "Paladin's Aegis", "Sovereign's Shield" } },
            { "NamesWeapon", new[] { "Rusty Shank", "Doublethorn Spike", "Ternion Trident", "Eternity's Wrathblade" } },
            { "NamesHeal", new[] { "Small healing flask", "Medium healing flask", "Big healing flask", "Huge healing flask" }}
        };

        public string GetName(string NameOutwithNames, int index)
        {
            string CorrectName = $"Names{NameOutwithNames}";
            int CorrectIndex = index;

            if (index > 4)
                CorrectIndex = 4;

            return _names[CorrectName][CorrectIndex];
        }
    }
}
