using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniGame.Enums;

namespace MiniGame.Untils
{
    public static class FindBodyPartByStr
    {
        public static BodyPart Main(string bodyPart)
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
