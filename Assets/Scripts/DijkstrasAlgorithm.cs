using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ESarkis;

public class DijkstrasAlgorithm : MonoBehaviour
{
    private PriorityQueue<Vector3Int> _frontier = new();
    public Vector3Int startingPoint;
    public Vector3Int objective;
    public Tilemap tilemap;
    public TileBase pintador;
    public float delay;
    public Dictionary<Vector3Int, Vector3Int> cameFrom = new();
    public Dictionary<Vector3Int, int> costSoFar = new();
    public bool canRun = true;
    public TileBase cost1;
    public TileBase cost2;
    public TileBase cost3;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !canRun) return;
        StarterCoroutine();
        canRun = false;
    }

    private void StarterCoroutine()
    {
        _frontier.Enqueue(startingPoint, 0);
        cameFrom.Add(startingPoint, Vector3Int.zero);
        costSoFar.Add(startingPoint, 0);
        StartCoroutine(DijkstrasCoroutine());
    }

    private IEnumerator DijkstrasCoroutine()
    {
        while (_frontier.Count > 0)
        {
            var current = _frontier.Dequeue();
            var neighbours = GetNeighbours(current);
            foreach (Vector3Int next in neighbours)
            {
                if (tilemap.GetSprite(next) != null)
                {
                    var newCost = costSoFar[current] + GetCost(tilemap.GetTile(next));
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        var priority = newCost;
                        _frontier.Enqueue(next, priority);
                        cameFrom.TryAdd(next, current);
                    }
                }
            }
            yield return new WaitForSeconds(delay);
        }
        Pathing();
    }

    private int GetCost(TileBase tile)
    {
        int cost = 0;
        if (tile == cost1)
        {
            cost = 0;
        }
        else if (tile == cost2)
        {
            cost = 1;
        }
        else if (tile == cost3)
        {
            cost = 2;
        }

        return cost;
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
            tilemap.SetTile(tile, pintador);
            tile = cameFrom[tile];
        }
    }
}