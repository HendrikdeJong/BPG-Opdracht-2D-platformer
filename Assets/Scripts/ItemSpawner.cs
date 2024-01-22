using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockSpawnBlock : MonoBehaviour{
    [SerializeField] private bool hasSpawnedItem = false;
    [SerializeField] private Tilemap ItemTileMap;
    [SerializeField] private TileBase CoinTile;
    [SerializeField] private TileBase HeartTile;
    [SerializeField] private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other){
            Debug.Log("block saw player");
            Vector3Int cellPosition = ItemTileMap.WorldToCell(transform.position);
            TileBase tile = ItemTileMap.GetTile(cellPosition);

            if (!hasSpawnedItem && tile == null){
                hasSpawnedItem = true;
                Debug.Log("should spawn block");
                // Randomly choose between CoinTile and HeartTile
                TileBase selectedTile = Random.Range(0, 2) == 0 ? CoinTile : HeartTile;

                ItemTileMap.SetTile(cellPosition + new Vector3Int(0, 1, 0), selectedTile);
            }
    }
}
