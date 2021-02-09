using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int cubeSize = new Vector2Int(10, 10);
    [SerializeField] int gridSize = 1;

    [SerializeField] Transform[] sides = new Transform[6];
    [SerializeField] Node nodePoint = null;

    Node[,] mazeModellTop;
    Node[,] mazeModellBottom;
    Node[,] mazeModellFront;
    Node[,] mazeModellBack;
    Node[,] mazeModellRight;
    Node[,] mazeModellLeft;

    List<Node> visitedNodes = new List<Node>();
    List<Node> frontier = new List<Node>();


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
        //StartCoroutine(GenerateMaze());
        StartCoroutine(GenerateRealMaze());
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
                SetWallsAlongX(x, y, i, newNode);
                SetWallsAlongY(x, y, i, newNode);
                SaveNodeToMazeModell(x, y, i, newNode);
            }
        }
    }

    private void SetWallsAlongX(int x, int y, int i, Node node)
    {
        string sideName = sides[i].name;

        if (x == cubeSize.x - 1)
        {
            node.TurnOnThiccWall(Vector2Int.right);
            if (y == 0)
            {
                if (sideName == "Bottom" || sideName == "Right" || sideName == "Back")
                {
                    node.TurnOnThiccWall(Vector2Int.up);
                }
                else
                {
                    node.TurnOnThiccWall(Vector2Int.down);
                }
            }
            else if (y == cubeSize.y - 1)
            {
                if (sideName == "Bottom" || sideName == "Right" || sideName == "Back")
                {
                    node.TurnOnThiccWall(Vector2Int.down);
                }
                else
                {
                    node.TurnOnThiccWall(Vector2Int.up);
                }
            }
        }
        else if (x == 0)
        {
            node.TurnOnThiccWall(Vector2Int.zero);
            if (y == 0)
            {
                if (sideName == "Bottom" || sideName == "Right" || sideName == "Back")
                {
                    node.TurnOnThiccWall(Vector2Int.up);
                }
                else
                {
                    node.TurnOnThiccWall(Vector2Int.down);
                }
            }
            else if (y == cubeSize.y - 1)
            {
                if (sideName == "Bottom" || sideName == "Right" || sideName == "Back")
                {
                    node.TurnOnThiccWall(Vector2Int.down);
                }
                else
                {
                    node.TurnOnThiccWall(Vector2Int.up);
                }
            }
        }
    }    
    private void SetWallsAlongY(int x, int y, int i, Node node)
    {
        string sideName = sides[i].name;

        if (y == cubeSize.x - 1)
        {
            
            if(sideName == "Bottom" || sideName == "Right" || sideName == "Back")
            {
                node.TurnOnThiccWall(Vector2Int.down);
            }
            else
            {
                node.TurnOnThiccWall(Vector2Int.up);
            }

            if (x == 0)
            {
                node.TurnOnThiccWall(Vector2Int.zero);

            }
            else if (x == cubeSize.x - 1)
            {
                node.TurnOnThiccWall(Vector2Int.right);
            }

        }
        else if (y == 0)
        {
            if (sideName == "Bottom" || sideName == "Right" || sideName == "Back")
            {
                node.TurnOnThiccWall(Vector2Int.up);
            }
            else
            {
                node.TurnOnThiccWall(Vector2Int.down);
            }

            if (x == 0)
            {
                node.TurnOnThiccWall(Vector2Int.zero);

            }
            else if (x == cubeSize.x - 1)
            {
                node.TurnOnThiccWall(Vector2Int.right);
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

    IEnumerator GenerateRealMaze()
    {
        visitedNodes = new List<Node>();
        frontier = new List<Node>();
        int x = Random.Range(1, cubeSize.x - 1);
        int y = Random.Range(1, cubeSize.y - 1);

        int i = Random.Range(0, sides.Length);

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
            ConnectNewNode(node);

            // Add the non visited nodes to frontier
            AddFrontierCells(node);
            // remove the node from the frontier list
            frontier.Remove(node);

            //yield return new WaitForSeconds(0.5f);
            //yield return new WaitForSeconds(0);

            if (frontier.Count <= 0)
                break;
        }
        //yield return new WaitForSeconds(0);
    }

    #region Methods to find first node (don't touch)
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
    private void AddFrontierCells(Node node, int i)
    {
        // here we need to check what side the node is on and use that to decide how to handle 
        // if we go oustide of x or y in our own modell (if we are at 0 or cubeSize.(x/y) -1.
        // we do different things.
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
    #endregion


    private Node GetNewFrontier()
    {
        int random = Random.Range(0, frontier.Count);
        Node node = frontier[random];
        visitedNodes.Add(node);
        return node;
    }

    private void AddFrontierCells(Node node)
    {        
        foreach (var direction in directions)
        {
            Vector2Int cords = node.Coordinates + direction;
            int x = cords.x;
            int y = cords.y;


            // have a switch with different conditions on how to run the search.
            if (x < 0)
            {
                cords = ConvertToHighX(node);
                string parent = GetLowXSide(node);
                if (IsValidFrontier(cords.x, cords.y, parent))
                {
                    Node newNode = GetMazeModellNode(cords.x, cords.y, parent);
                    if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
                        frontier.Add(newNode);
                }
            }
            else if (x > cubeSize.x - 1)
            {
                cords = ConvertToLowX(node);
                string parent = GetHighXSide(node);
                if (IsValidFrontier(cords.x, cords.y, parent))
                {
                    Node newNode = GetMazeModellNode(cords.x, cords.y, parent);
                    if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
                        frontier.Add(newNode);
                }
            }
            else if(y < 0)
            {
                cords = ConvertToHighY(node);// if front we should make y = 0 and then check valid forntier on top side x should remain the same
                string parent = GetLowYSide(node);
                if (IsValidFrontier(cords.x, cords.y, parent))
                {
                    Node newNode = GetMazeModellNode(cords.x, cords.y, parent);
                    if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
                        frontier.Add(newNode);
                }
            }
            else if (y > cubeSize.y -1)
            {
                cords = ConvertToLowY(node);// if front we should make y = 0 and then check valid forntier on top side x should remain the same
                string parent = GetHighYSide(node);
                if (IsValidFrontier(cords.x, cords.y, parent))
                {
                    Node newNode = GetMazeModellNode(cords.x, cords.y, parent);
                    if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
                        frontier.Add(newNode);
                }
            }
            else
            {
                if (IsValidFrontier(cords.x, cords.y, node))
                {
                    Node newNode = GetMazeModellNode(cords.x, cords.y, node);
                    if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
                        frontier.Add(newNode);
                }
            }
        }
    }

    private void ConnectNewNode(Node node)
    {
        List<Node> closeVisisted = new List<Node>();
        foreach (var direction in directions)
        {
            Vector2Int cords = node.Coordinates + direction;
            int x = cords.x;
            int y = cords.y;


            // have a switch with different conditions on how to run the search.
            if (x < 0)
            {
                cords = ConvertToHighX(node);
                string parent = GetLowXSide(node);

                if (IsValidFrontier(cords.x, cords.y, parent) && visitedNodes.Contains(GetMazeModellNode(cords.x, cords.y, parent)))
                {
                    closeVisisted.Add(GetMazeModellNode(cords.x, cords.y, parent));
                    GetMazeModellNode(cords.x, cords.y, parent).ExploredFrom = direction;
                }
            }
            else if (x > cubeSize.x - 1)
            {
                cords = ConvertToLowX(node);
                string parent = GetHighXSide(node);

                if (IsValidFrontier(cords.x, cords.y, parent) && visitedNodes.Contains(GetMazeModellNode(cords.x, cords.y, parent)))
                {
                    closeVisisted.Add(GetMazeModellNode(cords.x, cords.y, parent));
                    GetMazeModellNode(cords.x, cords.y, parent).ExploredFrom = direction;
                }
            }
            else if (y < 0)
            {
                cords = ConvertToHighY(node);
                string parent = GetLowYSide(node);
                if (IsValidFrontier(cords.x, cords.y, parent) && visitedNodes.Contains(GetMazeModellNode(cords.x, cords.y, parent)))
                {
                    closeVisisted.Add(GetMazeModellNode(cords.x, cords.y, parent));
                    GetMazeModellNode(cords.x, cords.y, parent).ExploredFrom = direction;
                }
            }
            else if (y > cubeSize.y - 1)
            {
                cords = ConvertToLowY(node);
                string parent = GetHighYSide(node);
                if (IsValidFrontier(cords.x, cords.y, parent) && visitedNodes.Contains(GetMazeModellNode(cords.x, cords.y, parent)))
                {
                    closeVisisted.Add(GetMazeModellNode(cords.x, cords.y, parent));
                    GetMazeModellNode(cords.x, cords.y, parent).ExploredFrom = direction;
                }
            }
            else
            {
                if (IsValidFrontier(cords.x, cords.y, node) && visitedNodes.Contains(GetMazeModellNode(cords.x, cords.y, node)))
                {
                    closeVisisted.Add(GetMazeModellNode(cords.x, cords.y, node));
                    GetMazeModellNode(cords.x, cords.y, node).ExploredFrom = direction;
                }
            }
        }

        if (closeVisisted.Count < 1)
            return;


        int random = Random.Range(0, closeVisisted.Count);

        Node bridgeTo = closeVisisted[random];



        Vector2Int dir = bridgeTo.ExploredFrom;

        if (node.transform.parent.name != bridgeTo.transform.parent.name)
        {
            DestroyWalls(node, bridgeTo, dir);
        }
        else
        {
            string parentname = node.transform.parent.name;
            if (parentname == "Back" || parentname == "Bottom" || parentname == "Right")
                dir = new Vector2Int(dir.x, -dir.y);
            bridgeTo.DestroyWall(-dir);
            node.DestroyWall(dir);
        }

    }

    private void DestroyWalls(Node node, Node bridgeTo, Vector2Int dir)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                switch(bridgeTo.transform.parent.name)
                {
                    case "Front":
                        bridgeTo.DestroyWall(Vector2Int.up);
                        node.DestroyWall(Vector2Int.down);
                        break;
                    case "Left":
                        bridgeTo.DestroyWall(Vector2Int.right);
                        node.DestroyWall(Vector2Int.zero);
                        break;
                    case "Back":
                        bridgeTo.DestroyWall(Vector2Int.down);
                        node.DestroyWall(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.DestroyWall(Vector2Int.right);
                        node.DestroyWall(Vector2Int.right);
                        break;
                }
                break;
            case "Front":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.DestroyWall(Vector2Int.down);
                        node.DestroyWall(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.DestroyWall(Vector2Int.up);
                        node.DestroyWall(Vector2Int.right);
                        break;
                    case "Bottom":
                        bridgeTo.DestroyWall(Vector2Int.up);
                        node.DestroyWall(Vector2Int.down);
                        break;
                    case "Left":
                        bridgeTo.DestroyWall(Vector2Int.down);
                        node.DestroyWall(Vector2Int.zero);
                        break;
                }
                break;

            case "Back":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.DestroyWall(Vector2Int.up);
                        node.DestroyWall(Vector2Int.down);
                        break;
                    case "Left":
                        bridgeTo.DestroyWall(Vector2Int.up);
                        node.DestroyWall(Vector2Int.zero);
                        break;
                    case "Bottom":
                        bridgeTo.DestroyWall(Vector2Int.down);
                        node.DestroyWall(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.DestroyWall(Vector2Int.down);
                        node.DestroyWall(Vector2Int.right);
                        break;
                }
                break;

            case "Bottom":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Left":
                        bridgeTo.DestroyWall(Vector2Int.zero);
                        node.DestroyWall(Vector2Int.zero);
                        break;
                    case "Front":
                        bridgeTo.DestroyWall(Vector2Int.down);
                        node.DestroyWall(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.DestroyWall(Vector2Int.zero);
                        node.DestroyWall(Vector2Int.right);
                        break;
                    case "Back":
                        bridgeTo.DestroyWall(Vector2Int.up);
                        node.DestroyWall(Vector2Int.down);
                        break;
                }
                break;

            case "Right":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.DestroyWall(Vector2Int.right);
                        node.DestroyWall(Vector2Int.right);
                        break;
                    case "Back":
                        bridgeTo.DestroyWall(Vector2Int.right);
                        node.DestroyWall(Vector2Int.down);
                        break;
                    case "Bottom":
                        bridgeTo.DestroyWall(Vector2Int.right);
                        node.DestroyWall(Vector2Int.zero);
                        break;
                    case "Front":
                        bridgeTo.DestroyWall(Vector2Int.right);
                        node.DestroyWall(Vector2Int.up);
                        break;
                }
                break;

            case "Left":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.DestroyWall(Vector2Int.zero);
                        node.DestroyWall(Vector2Int.right);
                        break;
                    case "Front":
                        bridgeTo.DestroyWall(Vector2Int.zero);
                        node.DestroyWall(Vector2Int.down);
                        break;
                    case "Bottom":
                        bridgeTo.DestroyWall(Vector2Int.zero);
                        node.DestroyWall(Vector2Int.zero);
                        break;
                    case "Back":
                        bridgeTo.DestroyWall(Vector2Int.zero);
                        node.DestroyWall(Vector2Int.up);
                        break;
                }
                break;

            default:
                Debug.Log("outside the switch plz check");
                break;
        }
    }

    private bool IsValidFrontier(int x, int y, string nodeParent)
    {
        try
        {
            Node tryNode = GetMazeModellNode(x, y, nodeParent);
            return true;
        }
        catch
        {
            return false;
        }
    }
    private Node GetMazeModellNode(int x, int y, string nodeParent)
    {
        switch (nodeParent)
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

    private Node GetMazeModellNode(int x, int y, Node node)
    {
        switch (node.transform.parent.name)
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
    private bool IsValidFrontier(int x, int y, Node node)
    {
        try
        {
            Node tryNode = GetMazeModellNode(x, y, node);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #region Get Next Side Methods
    private string GetHighYSide(Node node)
    {
        string nodeParent = node.transform.parent.name;
        switch (nodeParent)
        {
            case "Top":
                return "Back";

            case "Front":
                return "Top";

            case "Back":
                return "Top";

            case "Bottom":
                return "Back";

            case "Right":
                return "Back";

            case "Left":
                return "Back";

            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }    
    private string GetLowYSide(Node node)
    {
        string nodeParent = node.transform.parent.name;
        switch (nodeParent)
        {
            case "Top":
                return "Front";

            case "Front":
                return "Bottom";

            case "Back":
                return "Bottom";

            case "Bottom":
                return "Front";

            case "Right":
                return "Front";

            case "Left":
                return "Front";

            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }    
    private string GetHighXSide(Node node)
    {
        string nodeParent = node.transform.parent.name;
        switch (nodeParent)
        {
            case "Top":
                return "Right";

            case "Front":
                return "Right";

            case "Back":
                return "Right";

            case "Bottom":
                return "Right";

            case "Right":
                return "Top";

            case "Left":
                return "Top";

            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }    
    private string GetLowXSide(Node node)
    {
        string nodeParent = node.transform.parent.name;
        switch (nodeParent)
        {
            case "Top":
                return "Left";

            case "Front":
                return "Left";

            case "Back":
                return "Left";

            case "Bottom":
                return "Left";

            case "Right":
                return "Bottom";

            case "Left":
                return "Bottom";

            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }
    #endregion


    private Vector2Int ConvertToLowY(Node node)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.x,cubeSize.y - 1);

            case "Front":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Back":
                return new Vector2Int(node.Coordinates.x, node.Coordinates.y);

            case "Bottom":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Right":
                return new Vector2Int(node.Coordinates.y, node.Coordinates.x);

            case "Left":
                return new Vector2Int(0, node.Coordinates.x);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0,0); ;
        }
    }
    private Vector2Int ConvertToHighY(Node node)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.x, cubeSize.y - 1);

            case "Front":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Back":
                return new Vector2Int(node.Coordinates.x, cubeSize.y - 1);

            case "Bottom":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Right":
                return new Vector2Int(cubeSize.x - 1, node.Coordinates.x);

            case "Left":
                return new Vector2Int(0, node.Coordinates.x);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }    
    private Vector2Int ConvertToLowX(Node node)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.x, node.Coordinates.y);

            case "Front":
                return new Vector2Int(node.Coordinates.y, 0);

            case "Back":
                return new Vector2Int(node.Coordinates.y, node.Coordinates.x);

            case "Bottom":
                return new Vector2Int(0, node.Coordinates.y);

            case "Right":
                return new Vector2Int(node.Coordinates.x, node.Coordinates.y);

            case "Left":
                return new Vector2Int(0, node.Coordinates.y);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }
    private Vector2Int ConvertToHighX(Node node)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                return new Vector2Int(cubeSize.x -1, node.Coordinates.y);

            case "Front":
                return new Vector2Int(node.Coordinates.y, 0);

            case "Back":
                return new Vector2Int(node.Coordinates.y, cubeSize.y - 1);

            case "Bottom":
                return new Vector2Int(node.Coordinates.x, node.Coordinates.y);

            case "Right":
                return new Vector2Int(cubeSize.x -1, node.Coordinates.y);

            case "Left":
                return new Vector2Int(0, node.Coordinates.y);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }




}
