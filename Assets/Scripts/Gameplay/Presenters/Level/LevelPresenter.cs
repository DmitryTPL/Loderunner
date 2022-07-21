﻿using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LevelPresenter: Presenter
    {
        private readonly LevelData _levelData;
        private readonly IAsyncEnumerablePublisher _publisher;

        public LevelPresenter(LevelData levelData, IAsyncEnumerablePublisher publisher)
        {
            _levelData = levelData;
            _publisher = publisher;
        }

        public void SetCameraBounds(Bounds bounds)
        {
            _levelData.CameraBounds = bounds;
        }

        public void LevelCreated(int levelNumber, Matrix<int> map, LevelConfig levelConfig)
        {
            _levelData.LevelNumber = levelNumber;
            _levelData.Map = map;
            _levelData.Config = levelConfig;
            _publisher.Publish(new LevelCreatedMessage(levelNumber));
        }
    }
}