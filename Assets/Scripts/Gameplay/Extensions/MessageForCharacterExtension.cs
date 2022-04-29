using Loderunner.Gameplay;

public static class MessageForCharacterExtension
{
    public static bool IsCharacterMatch(this IMessageForCharacter message, int characterId)
    {
        return message.CharacterId == characterId;
    }
}