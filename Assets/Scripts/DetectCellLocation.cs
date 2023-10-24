using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectCellLocation : MonoBehaviour
{
    private Vector3 worldPosition;
    public GridLayout gridLayout;
    public Tilemap tilemap;
    public TileBase origin;
    public TileBase end;

    public FloodFill startpoint, endpoint;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPosition = gridLayout.WorldToCell(worldPosition);
        mousePos = gridLayout.CellToWorld(cellPosition);
        cellPosition.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(cellPosition);
            if (actualTile == null) { return; }
            startpoint.startingPoint = cellPosition;
            TileFlags flags = tilemap.GetTileFlags(cellPosition);
            tilemap.SetTile(cellPosition, origin);
            tilemap.SetTileFlags(cellPosition, flags);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(cellPosition);
            if (actualTile == null) { return; }
            endpoint.objective = cellPosition;
            TileFlags flags = tilemap.GetTileFlags(cellPosition);
            tilemap.SetTile(cellPosition, end);
            tilemap.SetTileFlags(cellPosition, flags);
        }
    }
}
