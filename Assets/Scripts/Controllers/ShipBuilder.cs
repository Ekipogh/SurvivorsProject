using UnityEngine;
using System.Collections.Generic;

public class ShipBuilder : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject baseBlockPrefab;
    [SerializeField] private GameObject baseBlockPreviewPrefab;
    [SerializeField] private BuildManager buildManager;

    private bool _isBuildingMode;
    private bool _isPlacingBlock;
    private float baseBlockCost;

    private readonly Dictionary<Vector2Int, ShipBlock> shipBlocks = new();

    private GameObject _currentPreviewBlock;

    void Start()
    {
        InitShip();
        InitPreviewBlock();
    }

    void Update()
    {
        if (_isBuildingMode && _isPlacingBlock)
        {
            UpdatePreview();
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                Vector2Int gridPosition = GetGridPositionFromMouse();
                TryPlacePurchasedBlock(gridPosition);
            }
            if (Input.GetMouseButtonDown(1)) // Right mouse button
            {
                CancelCurrentPlacement();
            }
        }
    }

    public void InitShip()
    {
        if (shipBlocks.Count > 0)
        {
            Debug.LogWarning("Ship already initialized.");
            return;
        }
        CreateBlock(Vector2Int.zero, applyHealthBonus: false);
    }

    private void InitPreviewBlock()
    {
        if (_currentPreviewBlock == null)
        {
            _currentPreviewBlock = Instantiate(baseBlockPreviewPrefab, player.transform);
            _currentPreviewBlock.SetActive(false);
        }
    }

    private void TryPlacePurchasedBlock(Vector2Int gridPosition)
    {
        if (!IsValidPlacement(gridPosition))
        {
            Debug.LogWarning($"Invalid placement at {gridPosition}");
            return;
        }

        if (!player.TrySpendPoints(baseBlockCost))
        {
            Debug.LogWarning($"Not enough points to place a ship block. Required: {baseBlockCost}");
            return;
        }

        CreateBlock(gridPosition, applyHealthBonus: true);
        _isPlacingBlock = false;
        _currentPreviewBlock?.SetActive(false);
        buildManager.OnBlockPlaced();
    }

    private ShipBlock CreateBlock(Vector2Int gridPosition, bool applyHealthBonus)
    {
        GameObject newBlock = Instantiate(baseBlockPrefab, player.transform);
        newBlock.transform.localPosition = new Vector3(gridPosition.x, gridPosition.y, 0);
        ShipBlock shipBlockComponent = newBlock.GetComponent<ShipBlock>();
        shipBlockComponent.Initialize(gridPosition);
        shipBlocks.Add(gridPosition, shipBlockComponent);
        if (applyHealthBonus)
        {
            player.ApplyHealthBonus(shipBlockComponent.HPBonus);
        }

        return shipBlockComponent;
    }

    public void BeginBuildMode()
    {
        // Implement logic to begin build mode
        Debug.Log("Build mode started.");
        _isBuildingMode = true;
    }

    public void EndBuildMode()
    {
        // Implement logic to end build mode
        Debug.Log("Build mode ended.");
        _isBuildingMode = false;
        _currentPreviewBlock?.SetActive(false);
        _isPlacingBlock = false;
    }

    public void BeginPlacingBlock(float baseBlockCost)
    {
        if (!_isBuildingMode)
        {
            Debug.LogWarning("Cannot place block when not in build mode.");
            return;
        }
        this.baseBlockCost = baseBlockCost;
        _isPlacingBlock = true;
        _currentPreviewBlock?.SetActive(true);
    }

    private void UpdatePreview()
    {
        if (_isBuildingMode && _isPlacingBlock)
        {
            Vector2Int gridPosition = GetGridPositionFromMouse();
            _currentPreviewBlock ??= Instantiate(baseBlockPreviewPrefab, player.transform);
            _currentPreviewBlock.SetActive(IsValidPlacement(gridPosition));
            _currentPreviewBlock.transform.localPosition = new Vector3(gridPosition.x, gridPosition.y, 0);
        }
    }

    private Vector2Int GetGridPositionFromMouse()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found for grid position calculation.");
            return Vector2Int.zero;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;

        // Project the cursor onto the player's Z plane so perspective cameras produce stable results.
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z - player.transform.position.z);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // Blocks are placed in player-local space, so convert world position to local before snapping.
        Vector3 mouseLocalPosition = player.transform.InverseTransformPoint(mouseWorldPosition);
        return new Vector2Int(Mathf.RoundToInt(mouseLocalPosition.x), Mathf.RoundToInt(mouseLocalPosition.y));
    }

    private bool IsValidPlacement(Vector2Int gridPosition)
    {
        // Check if the grid position is already occupied
        if (shipBlocks.ContainsKey(gridPosition))
        {
            return false;
        }

        if (shipBlocks.Count == 0)
        {
            return gridPosition == Vector2Int.zero;
        }

        // Check if the new block is adjacent to an existing block
        foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up,
                                                            Vector2Int.down,
                                                            Vector2Int.left,
                                                            Vector2Int.right })
        {
            if (shipBlocks.ContainsKey(gridPosition + direction))
            {
                return true;
            }
        }

        return false;
    }

    public void CancelCurrentPlacement()
    {
        if (_isPlacingBlock)
        {
            _isPlacingBlock = false;
            _currentPreviewBlock?.SetActive(false);
            buildManager.EnableBuildingMode(true);
        }
    }
}
