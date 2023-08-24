using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    public static class NamesItem
    {
        private static readonly Dictionary<string, string[]> NamesItemDictionary = new Dictionary<string, string[]>
        {
            { "NamesHelmet", new[] { "Cracked Helm", "Leather Cap", "Guardian Visor", "Royal Crown" } },
            { "NamesBody", new[] { "Patchwork Jerkin", "Leatherbound Jerkin", "Granite Surcoat", "Royal Robes" } },
            { "NamesLegs", new[] { "Breezy Legwraps", "Steelthread Legwraps", "Guardian Calf Protectors", "Diamond Shin Guards" } },
            { "NamesShield", new[] { "Gossamer Shield", "Steelplate Buckler", "Paladin's Aegis", "Sovereign's Shield" } },
            { "NamesWeapon", new[] { "Rusty Shank", "Doublethorn Spike", "Ternion Trident", "Eternity's Wrathblade" } },
            { "NamesHeal", new[] { "Small healing flask", "Medium healing flask", "Big healing flask", "Huge healing flask" }}
        };

        public static string GetName(string NameOutwithNames, int index)
        {
            string CorrectName = $"Names{NameOutwithNames}";
            int CorrectIndex = index;

            if (index > 4)
                CorrectIndex = 3;

            return NamesItemDictionary[CorrectName][CorrectIndex];
        }
    }
}
