using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CrossbarPlacer : PlacerBase
    {
        [SerializeField, Range(1, 23)] private int _length;
        [SerializeField] private Transform _right;
        [SerializeField] private BoxCollider2D _mainCollider;
        
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

            if (_previousLength == _length)
            {
                return;
            }
            
            var size = CellSize * _length;
                
            _previousLength = _length;
            _spriteRenderer.size = new Vector2(size, CellSize);

            _mainCollider.size = new Vector2(size - CellSize, CellSize / 2);
            _mainCollider.offset = new Vector2(size / 2, CellSize - _mainCollider.size.y / 2);

            _right.localPosition = new Vector2(_spriteRenderer.size.x, _right.localPosition.y);
        }

        [ContextMenu("Recreate crossbar")]
        public override void Recreate()
        {
            base.Recreate();
            
            _previousLength = 0;
        }
    }
}