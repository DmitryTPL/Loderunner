using UnityEngine;

namespace Loderunner.Gameplay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class LadderPlacer : PlacerBase
    {
        [SerializeField, Range(1, 23)] protected int _height;
        [SerializeField] private Transform _title;
        [SerializeField] private Transform _bottomCenter;
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

            if (_previousHeight == _height)
            {
                return;
            }

            ChangeLadder();
        }

        protected virtual void ChangeLadder()
        {
            var size = new Vector2(CellSize, CellSize * _height);
            var offset = new Vector2(CellSize / 2, size.y / 2);

            _previousHeight = _height;
            _spriteRenderer.size = size;
            _mainCollider.size = new Vector2(CellSize / 4, size.y);
            _mainCollider.offset = offset;

            var fallSideSize = new Vector2((CellSize - _mainCollider.size.x) / 2, size.y);

            _leftCollider.size = fallSideSize;
            _leftCollider.offset = new Vector2(fallSideSize.x / 2, fallSideSize.y / 2);

            _rightCollider.size = fallSideSize;
            _rightCollider.offset = new Vector2(CellSize - fallSideSize.x / 2, fallSideSize.y / 2);

            _title.localPosition = new Vector2(0, size.y);
            _bottomCenter.localPosition = new Vector2(CellSize / 2, 0);
        }

        [ContextMenu("Recreate ladder")]
        public override void Recreate()
        {
            base.Recreate();

            _previousHeight = 0;
        }
    }
}