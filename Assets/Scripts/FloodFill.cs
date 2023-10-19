using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloodFill : MonoBehaviour
{
    public Queue<Vector3Int> frontier = new();

    public Vector3Int startingPoint;
    public Vector3Int objective;

    public Set reached = new Set();

    public Tilemap tilemap;

    public TileBase tile1;

    public TileBase tile2;

    public float delay;

    public Dictionary<Vector3Int, Vector3Int> cameFrom = new();

    public bool canRun = true;

    public bool earlyExit;
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

        frontier.Enqueue(startingPoint);
        cameFrom.Add(startingPoint, Vector3Int.zero);


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

                if (!reached.set.Contains(next) && tilemap.GetSprite(next) != null)
                {


                    if (next != startingPoint && next != objective) { tilemap.SetTile(next, tile2); }
                    reached.Add(next);
                    frontier.Enqueue(next);
                    if (!cameFrom.ContainsKey(next))
                    {
                        cameFrom.Add(next, current);

                    }


                }

            }
            yield return new WaitForSeconds(delay);
        }
        DrawPath();

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
            tilemap.SetTile(tile, tile1);
            tile = cameFrom[tile];
        }

    }
}