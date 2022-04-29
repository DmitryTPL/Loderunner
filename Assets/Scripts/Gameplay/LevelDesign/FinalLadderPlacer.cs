using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FinalLadderPlacer : LadderPlacer
    {
        [SerializeField] private Transform _foldingScreen;

        protected override void ChangeLadder()
        {
            base.ChangeLadder();

            _foldingScreen.localScale = new Vector3(1, _height, 1);
        }
    }
}