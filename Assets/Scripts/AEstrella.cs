using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ESarkis;

public class AEstrella : MonoBehaviour
{
    public PriorityQueue<Vector3Int> frontier = new();

    public Vector3Int startingPoint;
    public Vector3Int objective;

    public Set reached = new Set();

    public Tilemap tilemap;

    public TileBase tilePurple;

    public TileBase tileBlue;

    public float delay;

    public Dictionary<Vector3Int, Vector3Int> cameFrom = new();

    public Dictionary<Vector3Int, int> costSoFar = new();

    public bool canRun = true;

    public bool earlyExit;

    public TileBase cost0;
    public TileBase cost1;
    public TileBase cost2;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canRun && !earlyExit)
        {
            FloodFillStartCoroutine();
            canRun = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && canRun && earlyExit)
        {
            FloodFillStartCoroutine();
            canRun = false;
        }
    }
    public void FloodFillStartCoroutine()
    {

        frontier.Enqueue(startingPoint, 0);
        cameFrom.Add(startingPoint, Vector3Int.zero);
        costSoFar.Add(startingPoint, 0);

        StartCoroutine(FloodFillCoroutine());


    }

    IEnumerator FloodFillCoroutine()
    {
        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            Debug.Log(frontier.Count);


            List<Vector3Int> neighbours = GetNeighbours(current);

            if (earlyExit && current == objective) break;

            foreach (Vector3Int next in neighbours)
            {

                if (tilemap.GetSprite(next) != null)
                {
                    //if (next != startingPoint && next != objective) tilemap.SetTile(next, tilePurple);

                    int new_cost = costSoFar[current] + GetCost(tilemap.GetTile(next));


                    if (!costSoFar.ContainsKey(next) || new_cost < costSoFar[next])
                    {
                        costSoFar[next] = new_cost;
                        if (next != startingPoint && next != objective) { tilemap.SetTile(next, tilePurple); }
                        int priority = new_cost + HeuristicMethod(objective, next);
                        frontier.Enqueue(next, priority);
                        if (!cameFrom.ContainsKey(next))
                        {
                            cameFrom.Add(next, current);

                        }
                    }

                }

            }
            yield return new WaitForSeconds(delay);
        }
        DrawPath();

    }

    private int GetCost(TileBase tile)
    {
        int cost = 0;
        if (tile == cost0)
        {
            cost = 0;
        }
        else if (tile == cost1)
        {
            cost = 1;
        }
        else if (tile == cost2)
        {
            cost = 2000;
        }
        return cost;
    }

    private List<Vector3Int> GetNeighbours(Vector3Int current)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        neighbours.Add(current + new Vector3Int(0, 1, 0));
        neighbours.Add(current + new Vector3Int(0, -1, 0));
        neighbours.Add(current + new Vector3Int(1, 0, 0));
        neighbours.Add(current + new Vector3Int(-1, 0, 0));

        return neighbours;
    }

    private void DrawPath()
    {
        Vector3Int tile = cameFrom[objective];
        while (tile != startingPoint)
        {
            tilemap.SetTile(tile, tileBlue);
            tile = cameFrom[tile];
        }

    }

    private int HeuristicMethod(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}