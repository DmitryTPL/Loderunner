using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PrepareScenePresenter : Presenter
    {
        public Func<Transform, PlayerView> CreatePlayer { get; }
        public Func<Transform, int, LevelView> CreateLevel { get; }
        
        public PrepareScenePresenter(Func<Transform, PlayerView> createPlayer, Func<Transform, int, LevelView> createLevel)
        {
            CreatePlayer = createPlayer;
            CreateLevel = createLevel;
        }
    }
}