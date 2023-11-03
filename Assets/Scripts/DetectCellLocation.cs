using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectCellLocation : MonoBehaviour
{
    [Header("World")]
    public GridLayout gridLayout;
    public Tilemap tilemap;
    private Vector3 _worldPosition;

    [Header("Tiles")]
    public TileBase Origen;
    public TileBase Destino;
    public TileBase Encimado;
    public TileBase Original;

    [Header("Scripts")]
    public FloodFillAlgorithm startpoint;
    public FloodFillAlgorithm endpoint;
    public DijkstrasAlgorithm dikstrafloodfill;
    public HeuristicAlgorithm heuristicfloodfill;
    public AEstrellaAlgorithm aEstrellafloodfill;

    [Header("Booleans")]
    public bool floodFillBool;
    public bool floodFillEarlyExitBool;
    public bool dijkstrasBool;
    public bool heuristicBool; 
    public bool heuristicEarlyExitBool;
    public bool aEstrellaBool;   
    public bool aEstrellaEarlyExitBool;

    private TileBase _originalTileBase;
    private Vector3Int? _origenTile;
    private Vector3Int? _originalTile;
    private Vector3Int? _destinoTile;

    private void Start()
    {
        _origenTile = null;
    }

    private Vector3Int GetPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        _worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPosition = gridLayout.WorldToCell(_worldPosition);
        mousePos = gridLayout.CellToWorld(cellPosition);
        cellPosition.z = 0;
        return cellPosition;
    }

    private void Update()
    {
        if (floodFillBool == true)
        {
            FloodFill();
            dikstrafloodfill.enabled = false;
            heuristicfloodfill.enabled = false;
            aEstrellafloodfill.enabled = false;
        }
        else if (floodFillEarlyExitBool == true)
        {
            FloodFill();
            startpoint.earlyExit = true;
            dikstrafloodfill.enabled = false;
            heuristicfloodfill.enabled = false;
            aEstrellafloodfill.enabled = false;
        }
        else if (dijkstrasBool == true)
        {
            Dijkstras();
            startpoint.enabled = false;
            heuristicfloodfill.enabled = false;
            aEstrellafloodfill.enabled = false;
        } 
        else if (heuristicBool == true)
        {
            Heuristic();
            dikstrafloodfill.enabled = false;
            startpoint.enabled = false;
            aEstrellafloodfill.enabled = false;

        }
        else if (heuristicEarlyExitBool == true)
        {
            Heuristic();
            heuristicfloodfill.earlyExit = true;
            dikstrafloodfill.enabled = false;
            startpoint.enabled = false;
            aEstrellafloodfill.enabled = false;

        }
        else if (aEstrellaBool == true){
            AEstrella();
            startpoint.enabled = false;
            dikstrafloodfill.enabled = false;
            heuristicfloodfill.enabled = false;
        }        
        else if (aEstrellaEarlyExitBool == true){
            AEstrella();
            startpoint.enabled = false;
            aEstrellafloodfill.earlyExit = true;
            dikstrafloodfill.enabled = false;
            heuristicfloodfill.enabled = false;
        }
    }
    private void FloodFill()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), Origen);
            startpoint.startingPoint = GetPosition();
            if (_origenTile != null)
            {
                tilemap.SetTile(_origenTile.Value, Original);
            }
            _origenTile = GetPosition();
        }
        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), Destino);
            Debug.Log("Destino " + GetPosition());
            endpoint.objective = GetPosition();
            if (_destinoTile != null)
            {
                tilemap.SetTile(_destinoTile.Value, Original);
            }
            _destinoTile = GetPosition();
        }

        if (_originalTile != GetPosition())
        {
            if (_originalTile != null && _originalTileBase != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
            {
                tilemap.SetTile(_originalTile.Value, _originalTileBase);
            }
            _originalTile = GetPosition();
            _originalTileBase = tilemap.GetTile(GetPosition());
            if (tilemap.GetSprite(GetPosition()) != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
            {
                tilemap.SetTile(_originalTile.Value, Encimado);
            }
        }
    }
    private void Dijkstras()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), Origen);
            dikstrafloodfill.startingPoint = GetPosition();

            if (_origenTile != null)
            {
                tilemap.SetTile(_origenTile.Value, Original);
            }
            _origenTile = GetPosition();
        }
        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), Destino);
            Debug.Log("Destino " + GetPosition());
            dikstrafloodfill.objective = GetPosition();
            if (_destinoTile != null)
            {
                tilemap.SetTile(_destinoTile.Value, Original);
            }
            _destinoTile = GetPosition();
        }
        if (_originalTile != GetPosition())
        {
            if (_originalTile != null && _originalTileBase != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
            {
                tilemap.SetTile(_originalTile.Value, _originalTileBase);
            }
            _originalTile = GetPosition();
            _originalTileBase = tilemap.GetTile(GetPosition());
            if (tilemap.GetSprite(GetPosition()) != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
            {
                tilemap.SetTile(_originalTile.Value, Encimado);
            }
        }
    }
    private void Heuristic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            Debug.Log("Origen " + GetPosition());
            tilemap.SetTile(GetPosition(), Origen);
            heuristicfloodfill.startingPoint = GetPosition();
            if (_origenTile != null)
            {
                tilemap.SetTile(_origenTile.Value, Original);
            }
            _origenTile = GetPosition();
        }
        if (Input.GetMouseButtonDown(1))
        {
            var actualTile = tilemap.GetTile(GetPosition());
            if (actualTile == null) { return; }
            tilemap.SetTile(GetPosition(), Destino);
            Debug.Log("Destino " + GetPosition());
            heuristicfloodfill.objective = GetPosition();
            if (_destinoTile != null)
            {
                tilemap.SetTile(_destinoTile.Value, Original);
            }
            _destinoTile = GetPosition();
        }
        if (_originalTile != GetPosition())
        {
            if (_originalTile != null && _originalTileBase != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
            {
                tilemap.SetTile(_originalTile.Value, _originalTileBase);
            }
            _originalTile = GetPosition();
            _originalTileBase = tilemap.GetTile(GetPosition());
            if (tilemap.GetSprite(GetPosition()) != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
            {
                tilemap.SetTile(_originalTile.Value, Encimado);
            }
        }
    }
    private void AEstrella()
    {
         if (Input.GetMouseButtonDown(0))
         {
             var actualTile = tilemap.GetTile(GetPosition());
             if (actualTile == null) { return; }
             Debug.Log("Origen " + GetPosition());
             tilemap.SetTile(GetPosition(), Origen);
             aEstrellafloodfill.startingPoint = GetPosition();
             if (_origenTile != null)
             {
                 tilemap.SetTile(_origenTile.Value, Original);
             }
             _origenTile = GetPosition();
         }

         if (Input.GetMouseButtonDown(1))
         {
             var actualTile = tilemap.GetTile(GetPosition());
             if (actualTile == null) { return; }
             tilemap.SetTile(GetPosition(), Destino);
             Debug.Log("Destino " + GetPosition());
             aEstrellafloodfill.objective = GetPosition();
             if (_destinoTile != null)
             {
                 tilemap.SetTile(_destinoTile.Value, Original);
             }
             _destinoTile = GetPosition();
         }
         if (_originalTile != GetPosition())
         {
             if (_originalTile != null && _originalTileBase != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
             {
                 tilemap.SetTile(_originalTile.Value, _originalTileBase);
             }
             _originalTile = GetPosition();
             _originalTileBase = tilemap.GetTile(GetPosition());
             if (tilemap.GetSprite(GetPosition()) != null && _originalTile.Value != _origenTile && _originalTile.Value != _destinoTile)
             {
                 tilemap.SetTile(_originalTile.Value, Encimado);
             }
         }
    }
}

