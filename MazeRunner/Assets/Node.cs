using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] GameObject northWall = null;
    [SerializeField] GameObject eastWall = null;
    [SerializeField] GameObject southWall = null;
    [SerializeField] GameObject westWall = null;

    Vector2Int coordinates = new Vector2Int(0, 0);
    public Vector2Int Coordinates { get { return coordinates; } set { coordinates = value; } }

    bool partOfMaze = false;
    public bool PartOfMaze { get { return partOfMaze; } set { partOfMaze = value; } }

    Vector2Int exploredFrom = new Vector2Int(0,0);
    public Vector2Int ExploredFrom { get { return exploredFrom; } set { exploredFrom = value; } }

    public void DestroyWall(Vector2 direction)
    {
        if (direction == Vector2.up)
            northWall.SetActive(false);
        else if(direction == Vector2.right)
            eastWall.SetActive(false);
        else if(direction == Vector2.down)
            southWall.SetActive(false);
        else
            westWall.SetActive(false);
    }
}