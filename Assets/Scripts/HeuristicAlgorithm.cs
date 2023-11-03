using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ESarkis;

public class HeuristicAlgorithm : MonoBehaviour
{
    private PriorityQueue<Vector3Int> _frontier = new();
    public Vector3Int startingPoint;
    public Vector3Int objective;
    public Tilemap tilemap;
    public TileBase pintador;
    public TileBase camino;
    public float delay;
    public Dictionary<Vector3Int, Vector3Int> cameFrom = new();
    public Dictionary<Vector3Int, int> costSoFar = new();
    public bool canRun = true;
    public bool earlyExit;
    public TileBase cost1;
    public TileBase cost2;
    public TileBase cost3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canRun && !earlyExit)
        {
            StarterCoroutine();
            canRun = false;
        }

        if (!Input.GetKeyDown(KeyCode.Space) || !canRun || !earlyExit) return;
        StarterCoroutine();
        canRun = false;
    }

    private void StarterCoroutine()
    {
        _frontier.Enqueue(startingPoint, 0);
        cameFrom.Add(startingPoint, Vector3Int.zero);
        costSoFar.Add(startingPoint, 0);
        StartCoroutine(HeuristicCoroutine());
    }

    IEnumerator HeuristicCoroutine()
    {
        while (_frontier.Count > 0)
        {
            var current = _frontier.Dequeue();
            var neighbours = GetNeighbours(current);
            if (earlyExit && current == objective) break;
            foreach (var next in neighbours)
            {
                if (tilemap.GetSprite(next) == null) continue;
                var newCost = costSoFar[current] + GetCost(tilemap.GetTile(next));
                if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                costSoFar[next] = newCost;
                if (next != startingPoint && next != objective)
                {
                    tilemap.SetTile(next, pintador);
                }
                var priority = Heuristic(objective, next);
                _frontier.Enqueue(next, priority);
                cameFrom.TryAdd(next, current);
            }
            yield return new WaitForSeconds(delay);
        }
        Pathing();
    }

    private int GetCost(TileBase tile)
    {
        var cost = 0;
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
            cost = 4000;
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
            tilemap.SetTile(tile, camino);
            tile = cameFrom[tile];
        }
    }

    private int Heuristic(Vector3Int first, Vector3Int second)
    {
        return Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y);
    }
}