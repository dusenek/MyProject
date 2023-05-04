using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GameObject groundTilePrefab;
        [SerializeField] private int gridSize = 3;
        [SerializeField] private float tileSize = 1.0f;

        private Transform playerTransform;
        private PrefabPool pool;
        private GameObject[,] gridTiles;
        private Vector2Int previousPlayerTile;
        private int poolSize = 9;


        private void Start()
        {
            gridTiles = new GameObject[gridSize, gridSize];
            pool = new PrefabPool(groundTilePrefab, poolSize);
            StartCoroutine(GenerateGroundCO());
            previousPlayerTile = GetCurrentPlayerTile();
        }

        private void OnEnable()
        {
            Events.PlayerCreated += OnPlayerCreated;
        }

        private void OnDisable()
        {
            Events.PlayerCreated -= OnPlayerCreated;
        }

        private void OnPlayerCreated(GameObject playerGameObject)
        {
            playerTransform = playerGameObject.transform;
        }

        private void Update()
        {
            Vector2Int currentPlayerTile = GetCurrentPlayerTile();
            if (currentPlayerTile != previousPlayerTile)
            {
                MoveTiles(currentPlayerTile - previousPlayerTile, currentPlayerTile);
                previousPlayerTile = currentPlayerTile;
            }
        }

        private void GenerateGround()
        {
            int halfRow = Mathf.CeilToInt(gridSize * 0.5f);
            int halfCol = Mathf.CeilToInt(gridSize * 0.5f);

            for (int row = -halfRow + 1; row <= gridSize - halfRow; row++)
            {
                for (int col = -halfCol + 1; col <= gridSize - halfCol; col++)
                {
                    Vector3 tilePosition = new Vector3(row * tileSize, 0, col * tileSize);
                    GameObject tile = pool.GetFromPool();
                    tile.transform.position = tilePosition;
                    tile.transform.SetParent(transform);
                    gridTiles[row + halfRow - 1, col + halfCol - 1] = tile;
                }
            }
        }

        IEnumerator GenerateGroundCO()
        {
            int halfRow = Mathf.CeilToInt(gridSize * 0.5f);
            int halfCol = Mathf.CeilToInt(gridSize * 0.5f);

            for (int row = -halfRow + 1; row <= gridSize - halfRow; row++)
            {
                for (int col = -halfCol + 1; col <= gridSize - halfCol; col++)
                {
                    Vector3 tilePosition = new Vector3(row * tileSize, 0, col * tileSize);
                    GameObject tile = pool.GetFromPool();
                    tile.transform.position = tilePosition;
                    tile.transform.SetParent(transform);
                    gridTiles[row + halfRow - 1, col + halfCol - 1] = tile;

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private Vector2Int GetCurrentPlayerTile()
        {
            if (playerTransform == null)
                return Vector2Int.zero;

            float inverseTileSize = 1 / tileSize;
            int playerRow = Mathf.RoundToInt(playerTransform.position.x * inverseTileSize);
            int playerCol = Mathf.RoundToInt(playerTransform.position.z * inverseTileSize);

            int halfRow = Mathf.CeilToInt(gridSize * 0.5f);
            int halfCol = Mathf.CeilToInt(gridSize * 0.5f);

            return new Vector2Int((playerRow % gridSize) + halfRow, (playerCol % gridSize) + halfCol);
        }

        private void MoveTiles(Vector2Int direction, Vector2Int currentPlayerTile)
        {
            int rowDirection = (int)Mathf.Clamp(direction.x, -1, 1);
            int colDirection = (int)Mathf.Clamp(direction.y, -1, 1);

            int halfRow = Mathf.CeilToInt(gridSize * 0.5f);
            int halfCol = Mathf.CeilToInt(gridSize * 0.5f);

            if (rowDirection != 0)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    int targetRow = ((rowDirection > 0) ? previousPlayerTile.x + halfRow : currentPlayerTile.x + halfRow) % gridSize;

                    GameObject tileToMove = gridTiles[targetRow, col];
                    tileToMove.transform.position += new Vector3(rowDirection * tileSize * gridSize, 0, 0);

                    gridTiles[targetRow, col] = tileToMove;
                }
            }

            if (colDirection != 0)
            {
                for (int row = 0; row < gridSize; row++)
                {
                    int targetCol = ((colDirection > 0) ? previousPlayerTile.y + halfCol : currentPlayerTile.y + halfCol) % gridSize;
                    GameObject tileToMove = gridTiles[row, targetCol];
                    tileToMove.transform.position += new Vector3(0, 0, colDirection * tileSize * gridSize);

                    gridTiles[row, targetCol] = tileToMove;
                }
            }
        }
    }
}

