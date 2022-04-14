using Loderunner.Gameplay;

public static class CharacterFilterExtension
{
    public static bool IsCharacterMatch(this ICharacterFilter characterFilter, int characterId)
    {
        return characterFilter.CharacterId == characterId;
    }
}