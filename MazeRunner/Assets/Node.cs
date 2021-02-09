using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] GameObject northWall = null;
    [SerializeField] GameObject eastWall = null;
    [SerializeField] GameObject southWall = null;
    [SerializeField] GameObject westWall = null;

    [SerializeField] GameObject thiccNorthWall = null;
    [SerializeField] GameObject thiccEastWall = null;
    [SerializeField] GameObject thiccSouthWall = null;
    [SerializeField] GameObject thiccWestWall = null;
 
    Vector2Int coordinates = new Vector2Int(0, 0);
    public Vector2Int Coordinates { get { return coordinates; } set { coordinates = value; } }

    bool partOfMaze = false;
    public bool PartOfMaze { get { return partOfMaze; } set { partOfMaze = value; } }

    Vector2Int exploredFrom = new Vector2Int(0,0);
    public Vector2Int ExploredFrom { get { return exploredFrom; } set { exploredFrom = value; } }

    public void DestroyWall(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            northWall.SetActive(false);
            thiccNorthWall.SetActive(false);
        }
        else if (direction == Vector2Int.right)
        {
            eastWall.SetActive(false);
            thiccEastWall.SetActive(false);
        }
        else if (direction == Vector2Int.down)
        {
            thiccSouthWall.SetActive(false);
            southWall.SetActive(false);
        }
        else
        {
            westWall.SetActive(false);
            thiccWestWall.SetActive(false);
        }
    }
    public void TurnOnThiccWall(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            northWall.SetActive(false);
            thiccNorthWall.SetActive(true);
        }
        else if (direction == Vector2Int.right)
        {
            eastWall.SetActive(false);
            thiccEastWall.SetActive(true);
        }
        else if (direction == Vector2Int.down)
        {
            southWall.SetActive(false);
            thiccSouthWall.SetActive(true);
        }
        else
        {
            westWall.SetActive(false);
            thiccWestWall.SetActive(true);
        }
    }
}