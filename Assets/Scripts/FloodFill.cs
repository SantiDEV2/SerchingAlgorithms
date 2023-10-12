using System.Collections.Generic;
using UnityEngine;

public class FloodFill : MonoBehaviour
{
    public Queue<Vector3> frontier = new();
    public Vector3 startingPoint;
    public Set reached = new Set();

    private void Start()
    {
        frontier.Enqueue(startingPoint);
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            var neighbours = getNeighbours(current);
            foreach(Vector3Int neighbour in neighbours )
            {
                /*if(!neighbour)
                {

                }*/
            }

            
        }
    }

    public List<Vector3Int> getNeighbours(Vector3 current)
    {
        return new List<Vector3Int>();
    }
}
