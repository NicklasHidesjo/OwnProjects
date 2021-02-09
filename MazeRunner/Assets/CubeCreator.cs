using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int cubeSize = new Vector2Int(10, 10);
    [SerializeField] int gridSize = 1;

    [SerializeField] Transform top = null;
    [SerializeField] Transform[] sides = new Transform[6];



    [SerializeField] Node nodePoint = null;

    [SerializeField] Node[,] mazeModellTop;
    [SerializeField] Node[,] mazeModellBottom;
    [SerializeField] Node[,] mazeModellFront;
    [SerializeField] Node[,] mazeModellBack;
    [SerializeField] Node[,] mazeModellRight;
    [SerializeField] Node[,] mazeModellLeft;

    [SerializeField] List<Node> visitedNodes = new List<Node>();
    [SerializeField] List<Node> frontier = new List<Node>();


    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };


    void Start()
    {
        InitArrays();
        GenerateCube();
        OffSetSides();
        //GenerateGrid();
        StartCoroutine(GenerateMaze());
    }

    private void InitArrays()
    {
        mazeModellTop = new Node[cubeSize.x, cubeSize.y];
        mazeModellBottom = new Node[cubeSize.x, cubeSize.y];
        mazeModellFront = new Node[cubeSize.x, cubeSize.y];
        mazeModellBack = new Node[cubeSize.x, cubeSize.y];
        mazeModellRight = new Node[cubeSize.x, cubeSize.y];
        mazeModellLeft = new Node[cubeSize.x, cubeSize.y];

        // if we want to make funny looking cubes/rectangles we need to use this but also 
        // make a switch when generating sides so that if the side is left or right we use another grid generation tecnique.
        /*        int sideSizes = 0;
                if (cubeSize.x > cubeSize.y)
                    sideSizes = cubeSize.x;
                else
                    sideSizes = cubeSize.y;

                mazeModellRight = new Node[sideSizes, sideSizes];
                mazeModellLeft = new Node[sideSizes, sideSizes];
            */
    }
    private void GenerateCube()
    {
        for (int i = 0; i < sides.Length; i++)
        {
            GenerateGrid(i);
        }
    }

    private void GenerateGrid(int i)
    {
        for (int x = 0; x < cubeSize.x; x++)
        {
            for (int y = 0; y < cubeSize.y; y++)
            {
                Vector3 position = GetPos(x, y, i);

                Node newNode = Instantiate(nodePoint, position, new Quaternion(0, 0, 0, 0), sides[i]);
                newNode.name = "Node" + x + " , " + y;
                newNode.Coordinates = new Vector2Int(x, y);
                SaveNodeToMazeModell(x, y, i, newNode);
            }
        }
    }

    private Vector3 GetPos(int x, int y, int i)
    {
        switch (sides[i].name)
        {
            case "Top":
                return new Vector3(gridSize * x, gridSize * cubeSize.y - 1, gridSize * y);
            case "Front":
                return new Vector3(gridSize * x, gridSize * y, 0);
            case "Back":
                return new Vector3(gridSize * x, gridSize * y, gridSize * cubeSize.y - 1);
            case "Bottom":
                return new Vector3(gridSize * x, 0, gridSize * y);
            case "Right":
                return new Vector3(gridSize * cubeSize.y - 1, gridSize * x, gridSize * y);
            case "Left":
                return new Vector3(0, gridSize * x, gridSize * y);
            default:
                Debug.Log("outside the switch plz check");
                return new Vector3(0, 0, 0);
        }
    }
    private Quaternion GetRotation(int i)
    {
        switch (sides[i].name)
        {
            case "Top":
                return new Quaternion(90,0,0,0);
            case "Front":
                return new Quaternion(0,0,0,0);
            case "Back":
                return new Quaternion(0,180,0,0);
            case "Bottom":
                return new Quaternion(-90,0,0,0);
            case "Right":
                return new Quaternion(0,-90,0,0);
            case "Left":
                return new Quaternion(0,90,0,0);
            default:
                Debug.Log("outside the switch plz check");
                return new Quaternion(0,0,0,0);
        }
    }

    // name this way better it's the method for choosing what two dimensional array the node gets put into (depending on its parent side)
    private void SaveNodeToMazeModell(int x, int y, int i, Node node)
    {
        switch (sides[i].name)
        {
            case "Top":
                mazeModellTop[x, y] = node;
                break;
            case "Front":
                mazeModellFront[x, y] = node;
                break;
            case "Back":
                mazeModellBack[x, y] = node;
                break;
            case "Bottom":
                mazeModellBottom[x, y] = node;
                break;
            case "Right":
                mazeModellRight[x, y] = node;
                break;
            case "Left":
                mazeModellLeft[x, y] = node;
                break;
            default:
                Debug.Log("outside the switch plz check");
                break;
        }
    }

    private void OffSetSides()
    {
        foreach (var side in sides)
        {
            switch (side.name)
            {
                case "Top":
                    side.position = new Vector3(0, (float)gridSize / 2, 0);
                    break;
                case "Front":
                    side.position = new Vector3(0, 0, (float)-gridSize / 2);
                    break;
                case "Back":
                    side.position = new Vector3(0, 0, (float)gridSize / 2);
                    break;
                case "Bottom":
                    side.position = new Vector3(0, (float)-gridSize / 2, 0);
                    break;
                case "Right":
                    side.position = new Vector3((float)gridSize / 2, 0, 0);
                    break;
                case "Left":
                    side.position = new Vector3(-(float)gridSize / 2, 0, 0);
                    break;
                default:
                    Debug.Log("outside the switch plz check");
                    break;
            }
        }
    }

    private Node GetMazeModellNode(int x, int y, int i)
    {
        switch (sides[i].name)
        {
            case "Top":
                return mazeModellTop[x, y];

            case "Front":
                return mazeModellFront[x, y];

            case "Back":
                return mazeModellBack[x, y];

            case "Bottom":
                return mazeModellBottom[x, y];

            case "Right":
                return mazeModellRight[x, y];

            case "Left":
                return mazeModellLeft[x, y];

            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }

    IEnumerator GenerateMaze()
    {
        for (int i = 0; i < sides.Length; i++)
        {
            visitedNodes = new List<Node>();
            frontier = new List<Node>();
            int x = Random.Range(1, cubeSize.x - 1);
            int y = Random.Range(1, cubeSize.y - 1);
            frontier.Add(GetMazeModellNode(x, y, i));
            visitedNodes.Add(GetMazeModellNode(x, y, i));
            AddFrontierCells(GetMazeModellNode(x, y, i), i);
            while (frontier.Count > 0)
            {
                //yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0);

                // get random node from frontier
                Node node = GetNewFrontier();

                // connect to one of the visitedNodes
                ConnectNewNode(node, i);

                // Add the non visited nodes to frontier
                AddFrontierCells(node, i);
                // remove the node from the frontier list
                frontier.Remove(node);

                //yield return new WaitForSeconds(0.5f);
                //yield return new WaitForSeconds(0);

                if (frontier.Count <= 0)
                    break;
            }
            //yield return new WaitForSeconds(0);
        }


    }

    private void ConnectNewNode(Node node, int i)
    {
        List<Node> closeVisisted = new List<Node>();
        foreach (var direction in directions)
        {
            Vector2Int cords = node.Coordinates + direction;
            int x = cords.x;
            int y = cords.y;
            if (IsValidFrontier(x, y, i) && visitedNodes.Contains(GetMazeModellNode(x, y, i)))
            {
                closeVisisted.Add(GetMazeModellNode(x, y, i));
                GetMazeModellNode(x, y, i).ExploredFrom = direction;
            }
        }

        if (closeVisisted.Count < 1)
            return;


        int random = Random.Range(0, closeVisisted.Count);

        Node bridgeTo = closeVisisted[random];
        Vector2Int dir = bridgeTo.ExploredFrom;

        string parentname = node.transform.parent.name;
        if (parentname == "Back" || parentname == "Bottom" || parentname == "Right")
            dir = new Vector2Int(dir.x, -dir.y);

        bridgeTo.DestroyWall(-dir);
        node.DestroyWall(dir);

    }

    private Node GetNewFrontier()
    {
        int random = Random.Range(0, frontier.Count);
        Node node = frontier[random];
        visitedNodes.Add(node);
        return node;
    }

    private void AddFrontierCells(Node node, int i)
    {
        foreach (var direction in directions)
        {
            Vector2Int cords = node.Coordinates + direction;
            int x = cords.x;
            int y = cords.y;

            if (IsValidFrontier(x, y, i))
            {
                Node newNode = GetMazeModellNode(x, y, i);
                if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
                    frontier.Add(newNode);
            }
        }
    }

    private bool IsValidFrontier(int x, int y, int i)
    {
        try
        {
            Node node = GetMazeModellNode(x, y, i);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
