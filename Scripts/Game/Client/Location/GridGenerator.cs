using UnityEngine;

namespace Game.Client.Location
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private MapTile _tile;
        [SerializeField] private int _gridHeight;
        [SerializeField] private int _gridWidth;
        [SerializeField] private float _tileSize;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    var newTile = Instantiate(_tile, _parent);
                    var posX = (x * _tileSize + y * _tileSize) / 2;
                    var posY = (x * _tileSize - y * _tileSize) / 4;

                    newTile.gameObject.name = $"Tile {x} {y}";
                    newTile.SetupTile(new Vector3(posX, posY, 0f));
                }
            }
        }

        public void ClearField()
        {
            if (_parent.transform.childCount == 0)
            {
                return;
            }

            foreach (var child in _parent.transform.GetComponentsInChildren<Transform>(true))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
