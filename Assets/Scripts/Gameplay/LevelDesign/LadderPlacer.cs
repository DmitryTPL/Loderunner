using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class LadderPlacer : PlacerBase
    {
        [SerializeField, Range(1, 23)] private int _height;

        private int _previousHeight;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _previousHeight = _height;
        }

        protected override void Update()
        {
            base.Update();

            if (_previousHeight != _height)
            {
                _previousHeight = _height;
                _spriteRenderer.size = new Vector2(GlobalConstant.CellValue, GlobalConstant.CellValue * _height);
            }
        }
    }
}