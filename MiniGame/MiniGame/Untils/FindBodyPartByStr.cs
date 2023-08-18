using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame.Untils
{
    internal class FindBodyPartByStr
    {
        public BodyPart Main(string bodyPart)
        {
            return bodyPart switch
            {
                "Helmet" => BodyPart.Helmet,
                "Body" => BodyPart.Body,
                "Legs" => BodyPart.Legs,
                "Weapon" => BodyPart.Weapon,
                "Shield" => BodyPart.Shield,
                _ => throw new ArgumentException("Invalid body part string"),
            };
        }
    }
}
