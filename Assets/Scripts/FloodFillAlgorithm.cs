using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloodFillAlgorithm: MonoBehaviour
{
    public Queue<Vector3Int> frontier = new();
    public Vector3Int startingPoint;
    public Vector3Int objective;
    private Set _reached = new Set();
    public Tilemap tilemap;
    public TileBase pintador;
    public TileBase camino;
    public float delay;
    public Dictionary<Vector3Int, Vector3Int> cameFrom = new();
    public bool canRun = true;
    public bool earlyExit;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !canRun) return;
        StarterCoroutine();
        canRun = false;
    }

    private void StarterCoroutine()
    {
        frontier.Enqueue(startingPoint);
        cameFrom.Add(startingPoint, Vector3Int.zero);
        StartCoroutine(FloodFillCoroutine());
    }

    private IEnumerator FloodFillCoroutine()
    {
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            var neighbours = GetNeighbours(current);
            if (current == objective && earlyExit) break;
            foreach (Vector3Int next in neighbours)
            {
                if (!_reached.set.Contains(next) && tilemap.GetSprite(next) != null)
                {
                    if (next != startingPoint && next != objective)
                    {
                        tilemap.SetTile(next, pintador);
                    }
                    _reached.Add(next);
                    frontier.Enqueue(next);
                    cameFrom.TryAdd(next, current);
                }
            }
            yield return new WaitForSeconds(delay);
        }
        Pathing();
    }

    private List<Vector3Int> GetNeighbours(Vector3Int current)
    {
        var neighbours = new List<Vector3Int>
        {
            current + new Vector3Int(0, 1, 0),
            current + new Vector3Int(0, -1, 0),
            current + new Vector3Int(1, 0, 0),
            current + new Vector3Int(-1, 0, 0)
        };
        return neighbours;
    }

    private void Pathing()
    {
        var tile = cameFrom[objective];
        while (tile != startingPoint)
        {
            tilemap.SetTile(tile, camino);
            tile = cameFrom[tile];
        }
    }
}