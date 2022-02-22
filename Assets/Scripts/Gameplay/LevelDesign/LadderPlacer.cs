using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class LadderPlacer : PlacerBase
    {
        [SerializeField, Range(1, 23)] private int _height;
        [SerializeField] private Transform _title;

        private int _previousHeight;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
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
                var size = new Vector2(CellSize, CellSize * _height);

                _previousHeight = _height;
                _spriteRenderer.size = size;
                _boxCollider.size = size;
                _boxCollider.offset = new Vector2(CellSize * 0.5f, size.y * 0.5f);

                _title.localPosition = new Vector3(_title.localPosition.x, size.y, 0);
            }
        }
    }
}