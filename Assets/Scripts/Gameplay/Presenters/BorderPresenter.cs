using Loderunner.Service;
using UniTaskPubSub;

namespace Loderunner.Gameplay
{
    public class BorderPresenter: Presenter
    {
        private readonly IAsyncPublisher _publisher;

        public BorderPresenter(IAsyncPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public void EnterBorderTrigger(ICharacterView characterView, BorderType borderType)
        {
            _publisher.PublishAsync(new BorderReachedMessage(characterView, borderType));
        }
        
        public void ExitBorderTrigger(ICharacterView characterView, BorderType borderType)
        {
            _publisher.PublishAsync(new MovedAwayFromBorderMessage(characterView));
        }
    }
}