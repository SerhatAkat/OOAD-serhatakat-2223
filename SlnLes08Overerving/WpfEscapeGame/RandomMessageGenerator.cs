using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public enum MessageType
    {
        Info,
        Pickup,
        Error
    }

    internal static class RandomMessageGenerator
    {
        private static string[] infoMessages = new[]
        {
            "Ik denk dat ik op de goede weg ben.",
            "Hmm, dit ziet er interessant uit.",
            "Laten we doorgaan met verkennen."
        };

        private static string[] pickupMessages = new[]
        {
            "Wat mooi!.",
            "Weer een stuk voor mijn verzameling",
            "Interessant..., hier kan ik wat mee"
        };

        private static string[] errorMessages = new[]
        {
            "Oeps, dat had ik niet moeten doen.",
            "Dit is niet goed, ik moet een andere manier vinden.",
            "Dat was zeker niet de bedoeling."
        };

        private static Random random = new Random();

        public static string GetRandomMessage(MessageType t)
        {
            switch (t)
            {
                case MessageType.Info:
                    return infoMessages[random.Next(infoMessages.Length)];
                case MessageType.Pickup:
                    return pickupMessages[random.Next(pickupMessages.Length)];
                case MessageType.Error:
                    return errorMessages[random.Next(errorMessages.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }
    }
}
