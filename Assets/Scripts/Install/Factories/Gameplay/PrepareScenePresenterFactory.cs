using System;
using Loderunner.Gameplay;
using UnityEngine;

namespace Loderunner.Install
{
    public class PrepareScenePresenterFactory : FactoryBase<PrepareScenePresenter>
    {
        private readonly Func<Transform, PlayerView> _createPlayer;
        private readonly Func<Transform, int, LevelView> _createLevel;

        public PrepareScenePresenterFactory(Func<Transform, PlayerView> createPlayerFactory, Func<Transform, int, LevelView> createLevelFactory)
        {
            _createPlayer = createPlayerFactory;
            _createLevel = createLevelFactory;
        }

        protected override PrepareScenePresenter CreateEntryWithDependencies()
        {
            return new PrepareScenePresenter(_createPlayer, _createLevel);
        }
    }
}