using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectCellLocation : MonoBehaviour
{
    [Header("World")]
    public GridLayout gridLayout;
    private Vector3 worldPosition;

    [Header("Tiles")]
    public Tilemap tilemap;
    public TileBase origen;
    public TileBase destino;
    public TileBase encimado;
    public TileBase original;

    [Header("Scripts")]
    public FloodFill startpoint;
    public FloodFill endpoint;
    public DikstrasAlgorithm Dikstrafloodfill;
    public Heuristic Heuristicfloodfill;
    public AEstrella AEstrellafloodfill;

    [Header("Booleans")]
    public bool FloodFillBool;
    public bool FloodFillEarlyExitBool;
    public bool DijkstrasBool;
    public bool HeuristicBool;
    public bool AEstrellaBool;

    private TileBase originalTileBase;
    private Vector3Int? origenTile;
    private Vector3Int? originalTile;
    private Vector3Int? destinoTile;

    private void Start()
    {
        origenTile = null;
    }

    private Vector3Int GetPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPosition = gridLayout.WorldToCell(worldPosition);
        mousePos = gridLayout.CellToWorld(cellPosition);
        cellPosition.z = 0;
        return cellPosition;
    }

    private void Update()
    {
        if (FloodFillBool == true)
        {
            FloodFill();
            Dikstrafloodfill.enabled = false;
            Heuristicfloodfill.enabled = false;
            AEstrellafloodfill.enabled = false;
        }
        else if (FloodFillEarlyExitBool == true)
        {
            FloodFill();
            startpoint.canstop = true;
            Dikstrafloodfill.enabled = false;
            Heuristicfloodfill.enabled = false;
            AEstrellafloodfill.enabled = false;
        }
        else if (DijkstrasBool == true)
        {
            Dijkstras();
            startpoint.enabled = false;
            Heuristicfloodfill.enabled = false;
            AEstrellafloodfill.enabled = false;
        }
        else if (HeuristicBool == true)
        {
            Heuristic();
            Dikstrafloodfill.enabled = false;
            startpoint.enabled = false;
            AEstrellafloodfill.enabled = false;

        }
        else if (AEstrellaBool == true){
            AEstrella();
            startpoint.canstop = true;
            Dikstrafloodfill.enabled = false;
            Heuristicfloodfill.enabled = false;
        }
    }

    public void FloodFill()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), origen);

            startpoint.startingPoint = GetPosition();

            if (origenTile != null)
            {
                tilemap.SetTile(origenTile.Value, original);
            }
            origenTile = GetPosition();
        }

        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), destino);
            Debug.Log("Destino " + GetPosition());
            endpoint.objective = GetPosition();

            if (destinoTile != null)
            {
                tilemap.SetTile(destinoTile.Value, original);
            }
            destinoTile = GetPosition();
        }

        if (originalTile != GetPosition())
        {
            if (originalTile != null && originalTileBase != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, originalTileBase);
            }

            originalTile = GetPosition();
            originalTileBase = tilemap.GetTile(GetPosition());

            if (tilemap.GetSprite(GetPosition()) != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, encimado);
            }
        }
    }

    public void Dijkstras()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), origen);
            Dikstrafloodfill.startingPoint = GetPosition();

            if (origenTile != null)
            {
                tilemap.SetTile(origenTile.Value, original);
            }
            origenTile = GetPosition();
        }

        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), destino);
            Debug.Log("Destino " + GetPosition());
            Dikstrafloodfill.objective = GetPosition();

            if (destinoTile != null)
            {
                tilemap.SetTile(destinoTile.Value, original);
            }
            destinoTile = GetPosition();
        }

        if (originalTile != GetPosition())
        {
            if (originalTile != null && originalTileBase != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, originalTileBase);
            }

            originalTile = GetPosition();
            originalTileBase = tilemap.GetTile(GetPosition());

            if (tilemap.GetSprite(GetPosition()) != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, encimado);
            }
        }
    }

    public void Heuristic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), origen);
            Heuristicfloodfill.startingPoint = GetPosition();

            if (origenTile != null)
            {
                tilemap.SetTile(origenTile.Value, original);
            }
            origenTile = GetPosition();
        }

        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), destino);
            Debug.Log("Destino " + GetPosition());
            Heuristicfloodfill.objective = GetPosition();

            if (destinoTile != null)
            {
                tilemap.SetTile(destinoTile.Value, original);
            }
            destinoTile = GetPosition();
        }

        if (originalTile != GetPosition())
        {
            if (originalTile != null && originalTileBase != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, originalTileBase);
            }

            originalTile = GetPosition();
            originalTileBase = tilemap.GetTile(GetPosition());

            if (tilemap.GetSprite(GetPosition()) != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, encimado);
            }
        }
    }

    public void AEstrella()
    {
         if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), origen);
            AEstrellafloodfill.startingPoint = GetPosition();

            if (origenTile != null)
            {
                tilemap.SetTile(origenTile.Value, original);
            }
            origenTile = GetPosition();
        }

        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), destino);
            Debug.Log("Destino " + GetPosition());
            AEstrellafloodfill.objective = GetPosition();

            if (destinoTile != null)
            {
                tilemap.SetTile(destinoTile.Value, original);
            }
            destinoTile = GetPosition();
        }

        if (originalTile != GetPosition())
        {
            if (originalTile != null && originalTileBase != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, originalTileBase);
            }

            originalTile = GetPosition();
            originalTileBase = tilemap.GetTile(GetPosition());

            if (tilemap.GetSprite(GetPosition()) != null && originalTile.Value != origenTile && originalTile.Value != destinoTile)
            {
                tilemap.SetTile(originalTile.Value, encimado);
            }
        }
    }
}

