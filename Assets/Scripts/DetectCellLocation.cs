using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectCellLocation : MonoBehaviour
{
    private Vector3 worldPosition;
    public GridLayout gridLayout;
    public Tilemap tilemap;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPosition = gridLayout.WorldToCell(worldPosition);
        mousePos = gridLayout.CellToWorld(cellPosition);

        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log(worldPosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(cellPosition);
            var actualTile = tilemap.GetTile(cellPosition);
        }
    }
}
