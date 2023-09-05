using MiniGame.Enums;
using Newtonsoft.Json;

namespace MiniGame.Models
{
    internal class Save
    {
        [JsonProperty("level")] 
        public int Level { get; set; }

        [JsonProperty("health")]
        public int Health { get; set; }

        [JsonProperty("startHealth")]
        public int StartHealth { get; set; }

        [JsonProperty("armor")]
        public int Armor { get; set; }

        [JsonProperty("damage")]
        public int Damage { get; set; }

        [JsonProperty("completedRooms")]
        public int CompletedRooms { get; set; }

        [JsonProperty("needCompletedRoomsForNextLevel")]
        public int NeedCompletedRoomsForNextLevel { get; set; }

        [JsonProperty("money")]
        public int Money { get; set; }

        [JsonProperty("armorInventory")]
        public List<Dictionary<string, string>> ArmorInventory { get; set; }

        [JsonProperty("weaponInventory")]
        public List<Dictionary<string, string>> WeaponInventory { get; set; }

        [JsonProperty("other")]
        public List<Dictionary<string, string>> Other { get; set; }

        [JsonProperty("equipped")]
        public Dictionary<BodyPart, Dictionary<string, string>> Equipped { get; set; }
    }
}
