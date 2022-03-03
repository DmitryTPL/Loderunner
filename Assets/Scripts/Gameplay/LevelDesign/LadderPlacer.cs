using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class LadderPlacer : PlacerBase
    {
        [SerializeField, Range(1, 23)] private int _height;
        [SerializeField] private Transform _title;
        [SerializeField] private BoxCollider2D _mainCollider;
        [SerializeField] private BoxCollider2D _leftCollider;
        [SerializeField] private BoxCollider2D _rightCollider;
        
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
                var size = new Vector2(CellSize, CellSize * _height);
                var offset = new Vector2(CellSize / 2, size.y / 2);

                _previousHeight = _height;
                _spriteRenderer.size = size;
                _mainCollider.size = new Vector2(_mainCollider.size.x, size.y);
                _mainCollider.offset = offset;
                
                _leftCollider.size = new Vector2(_leftCollider.size.x, size.y);
                _leftCollider.offset = new Vector2(_leftCollider.size.x / 2, size.y / 2);
                
                _rightCollider.size = new Vector2(_rightCollider.size.x, size.y);
                _rightCollider.offset = new Vector2(CellSize - _rightCollider.size.x / 2, size.y / 2);

                _title.localPosition = new Vector3(_title.localPosition.x, size.y, 0);
            }
        }
    }
}