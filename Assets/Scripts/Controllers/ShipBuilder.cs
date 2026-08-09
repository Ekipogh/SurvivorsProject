using UnityEngine;
using System.Collections.Generic;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject baseBlockPrefab;

    private readonly float blockSize = 64f; // Size of each block in pixels
    private readonly Dictionary<Vector2Int, ShipBlock> shipBlocks = new();

    void Start()
    {
        InitShip();
    }

    public void InitShip()
    {
        if (shipBlocks.Count > 0)
        {
            Debug.LogWarning("Ship already initialized.");
            return;
        }
        // Add an initial ship block to the player's ship
        PlaceBlock(Vector2Int.zero);
    }

    private void PlaceBlock(Vector2Int gridPosition)
    {
        if (shipBlocks.ContainsKey(gridPosition))
        {
            Debug.LogWarning($"Block already exists at {gridPosition}");
            return;
        }

        GameObject newBlock = Instantiate(baseBlockPrefab, player.transform);
        newBlock.transform.localPosition = new Vector3(gridPosition.x * blockSize, gridPosition.y * blockSize, 0);
        ShipBlock shipBlockComponent = newBlock.GetComponent<ShipBlock>();
        shipBlockComponent.Initialize(gridPosition);
        shipBlocks.Add(gridPosition, shipBlockComponent);
    }
}