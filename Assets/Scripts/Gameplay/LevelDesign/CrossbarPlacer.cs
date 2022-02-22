using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CrossbarPlacer : PlacerBase
    {
        [SerializeField, Range(1, 23)] private int _length;

        private int _previousLength;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _previousLength = _length;
        }

        protected override void Update()
        {
            base.Update();

            if (_previousLength != _length)
            {
                _previousLength = _length;
                _spriteRenderer.size = new Vector2(CellSize * _length, CellSize);
            }
        }
    }
}