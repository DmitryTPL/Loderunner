namespace Loderunner.Gameplay
{
    public interface ICharacterStateContext
    {
        public StateResultData GetStateData(StateInitialData data);
    }
}