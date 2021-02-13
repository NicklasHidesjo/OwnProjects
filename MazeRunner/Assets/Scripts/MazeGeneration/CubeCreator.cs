using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CubeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int cubeSize;
    [SerializeField] Transform[] sides;

    [SerializeField] Node nodePoint;
    [SerializeField] GameObject pole;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject[] verticalEdgePoles;
    [SerializeField] GameObject[] horizontalEdgePoles;


    Node[,] mazeModellTop;
    Node[,] mazeModellBottom;
    Node[,] mazeModellFront;
    Node[,] mazeModellBack;
    Node[,] mazeModellRight;
    Node[,] mazeModellLeft;

    HashSet<Node> visitedNodes;
    HashSet<Node> frontier;

    Node currentNode;
    string currentNodeParent;
    int gridSize;

    Vector3 offset;

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    void Start()
    {
        Init();

        PlacePoles();

        GenerateCube();

        GenerateMaze();

        RemoveDuplicates();
        
        BuildWalls();

        OffSetSides();

        GetComponent<MeshCombiner>().GenerateMesh();

        Destroy(this);
    }

    private void Init()
    {
        gridSize = (int)nodePoint.transform.localScale.x;
        ground.transform.localScale = new Vector3(cubeSize.x, cubeSize.y, cubeSize.x);
        SetNodeHastSets();
        SetMazeModellArrays();
        GenerateOffset();
    }
    private void SetNodeHastSets()
    {
        visitedNodes = new HashSet<Node>();
        frontier = new HashSet<Node>();
    }
    private void SetMazeModellArrays()
    {
        mazeModellTop = new Node[cubeSize.x, cubeSize.y];
        mazeModellBottom = new Node[cubeSize.x, cubeSize.y];
        mazeModellFront = new Node[cubeSize.x, cubeSize.y];
        mazeModellBack = new Node[cubeSize.x, cubeSize.y];
        mazeModellRight = new Node[cubeSize.x, cubeSize.y];
        mazeModellLeft = new Node[cubeSize.x, cubeSize.y];
    }
    private void GenerateOffset()
    {
        offset = new Vector3(cubeSize.x / 2, cubeSize.y / 2, 0);

        if (cubeSize.x % 2 == 1) { return; }

        float x = (cubeSize.x / 2) - ((float)gridSize / 2);
        float y = cubeSize.y / 2 - ((float)gridSize / 2);
        offset = new Vector3(x,y, 0);
    }

    #region methods for spawning inbetween poles and edges
    private void PlacePoles()
    {
        for (int i = 0; i < sides.Length; i++)
            GeneratePolesOnSides(i);
    }
    private void GeneratePolesOnSides(int i)
    {
        for (int x = 0; x <= cubeSize.x; x++)
            for (int y = 0; y <= cubeSize.y; y++)
                InitializeNewPole(i, x, y);
    }
    private void InitializeNewPole(int i, int x, int y)
    {
        Instantiate(pole, GetPos(x, y, i), Quaternion.identity, sides[i]);

        if (sides[i].name == "Right" || sides[i].name == "Left")
        {
            CreateEdges(i, x, y);
        }
        else if(y == cubeSize.y)
        {
            Instantiate(verticalEdgePoles[1], GetPos(x, y, i), Quaternion.identity, sides[i]);
        }
    }
    private void CreateEdges(int i, int x, int y)
    {
        if (y == cubeSize.y)
        {
            Instantiate(verticalEdgePoles[1], GetPos(x, y, i), Quaternion.identity, sides[i]);
        }
        if (y == 0 )
        {
            Instantiate(verticalEdgePoles[0], GetPos(x, y, i), Quaternion.identity, sides[i]);
        }
        if (x == cubeSize.x)
        {
            Instantiate(horizontalEdgePoles[1], GetPos(x, y, i), Quaternion.identity, sides[i]);
        }
        if (x == 0)
        {
            Instantiate(horizontalEdgePoles[0], GetPos(x, y, i), Quaternion.identity, sides[i]);
        }
    }
    #endregion

    // refactor this
    private void RemoveDuplicates()
    {
        foreach (var node in visitedNodes)
        {
            string nodeParent = node.transform.parent.name;
            currentNode = node;

            foreach (var direction in directions)
            {
                if (!node.GetWall(direction)) { continue; }

                string parent = nodeParent;
                Vector2Int cords = node.Coordinates + direction;

                CheckParentAndCords(ref parent, ref cords);
                Node connect = TryGetNode(cords, parent);

                if (connect == null) { continue; }
                string connectParent = connect.transform.parent.name;

                if (connectParent != nodeParent)
                {
                    connect.TurnOffWalls(WallDeactivator.DeactivateNeigbourWall(connectParent, nodeParent));
                }
                else
                {
                   connect.TurnOffWalls(-direction);
                }
            }
        }
    }

    private void BuildWalls()
    {
        foreach (var node in visitedNodes)
        {
            node.BuildWalls();
        }
    }

    // look into making this run in a separate script and then call a 
    // mazeGeneration method handling the Generation of mazes
    #region Cube Generation Methods
    private void GenerateCube()
    {
        for (int i = 0; i < sides.Length; i++)
            GenerateSide(i);
    }

    private void GenerateSide(int i)
    {
        for (int x = 0; x < cubeSize.x; x++)
            for (int y = 0; y < cubeSize.y; y++)
                InitializeNewNode(i, x, y);
    }

    private void InitializeNewNode(int i, int x, int y)
    {
        Node newNode = Instantiate(nodePoint, GetPos(x, y, i), Quaternion.identity, sides[i]);

        newNode.Coordinates = new Vector2Int(x, y);

        CreateEdgeWalls(x, y, newNode);
        SaveNodeToMazeModell(x, y, i, newNode);
    }    
    private Vector3 GetPos(int x, int y, int i)
    {
        return new Vector3(gridSize * x, gridSize * y, 0) - offset;
    }


    private void CreateEdgeWalls(int x, int y, Node node)
    {
        if (x == cubeSize.x - 1)
        {
            node.TurnOnThiccWall(Vector2Int.right);
        }
        if (x == 0)
        {
            node.TurnOnThiccWall(Vector2Int.zero);
        }
        if (y == cubeSize.y - 1)
        {
            node.TurnOnThiccWall(Vector2Int.up);
        }
        if (y == 0)
        {
            node.TurnOnThiccWall(Vector2Int.down);
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
                Debug.LogWarning("Couldn't add to mazemodells as side: "+ sides[i].name + " doesn't exist" );
                break;
        }
    }

    private void OffSetSides()
    {
        float offset = (float)cubeSize.x / 2;
        foreach (var side in sides)
        {
            switch (side.name)
            {
                case "Top":
                    side.transform.rotation = Quaternion.Euler(90, 0, 0);
                    side.position = new Vector3(0, offset, 0);

                    break;
                case "Front":
                    side.transform.rotation = Quaternion.Euler(0, 0, 0);
                    side.position = new Vector3(0, 0, -offset);
 
                    break;
                case "Back":
                    side.transform.rotation = Quaternion.Euler(180, 0, 0);
                    side.position = new Vector3(0, 0, offset);

                    break;
                case "Bottom":
                    side.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    side.position = new Vector3(0, -offset, 0);

                    break;
                case "Right":
                    side.transform.rotation = Quaternion.Euler(0, -90, 0);
                    side.position = new Vector3(offset, 0, 0);

                    break;
                case "Left":
                    side.transform.rotation = Quaternion.Euler(0, 90, 0);
                    side.position = new Vector3(-offset, 0, 0);

                    break;
                default:
                    Debug.Log("outside the switch plz check");
                    break;
            }
        }
    }
    #endregion


    private void GenerateMaze()
    {
        GetFirstNode();
        while (frontier.Count > 0)
            HandleFrontierNode();
    }
    private void GetFirstNode()
    {
        int x = Random.Range(1, cubeSize.x - 1);
        int y = Random.Range(1, cubeSize.y - 1);
        int i = Random.Range(0, sides.Length);

        frontier.Add(TryGetNode(new Vector2Int(x,y), sides[i].name));
    }

    #region methods handling what happens to each maze node (frontierCell)

    private void HandleFrontierNode()
    {       
        GetNewFrontier();
        AddFrontierCells(); 
        ConnectNewNode(); 
        frontier.Remove(currentNode);
    }
    private void GetNewFrontier()
    {
        currentNode = frontier.First();
        currentNodeParent = currentNode.transform.parent.name;
        visitedNodes.Add(currentNode);
    }

    private void AddFrontierCells()
    {        
        foreach (var direction in directions)
            AddToFrontier(direction);
    }
    private void AddToFrontier(Vector2Int direction)
    {
        Node node = FindNode(direction);
        if (node == null) return;
        if (visitedNodes.Contains(node)) return;

        frontier.Add(node);
    }

    #endregion

    #region methods handling the connection of nodes
    private void ConnectNewNode()
    {
        List<Node> closeVisisted = new List<Node>();

        foreach (var direction in directions)
        {
            Node nodeToAdd = FindVisitedNode(direction);
            if (nodeToAdd != null)
                closeVisisted.Add(nodeToAdd);
        }

        if (closeVisisted.Count < 1)
            return;

        HandleWallTurnOff(closeVisisted);

    }
    private Node FindVisitedNode(Vector2Int direction)
    {
        Node node = FindNode(direction);
        if (node == null) return null;
        if (!visitedNodes.Contains(node)) { return null; }

        node.ExploredFrom = direction;
        return node;
    }

    private void HandleWallTurnOff(List<Node> closeVisisted)
    {
        Node bridgeTo = closeVisisted[Random.Range(0, closeVisisted.Count)];

        if (currentNodeParent == bridgeTo.transform.parent.name)
            TurnOffWalls(bridgeTo);
        else
            TurnOffSideWalls(bridgeTo);
    }
    private void TurnOffWalls(Node bridgeTo)
    {
        Vector2Int dir = bridgeTo.ExploredFrom;
        bridgeTo.TurnOffWalls(-dir);
        currentNode.TurnOffWalls(dir);
    }
    private void TurnOffSideWalls(Node bridgeTo)
    {
        string bridgeSide = bridgeTo.transform.parent.name;
        bridgeTo.TurnOffWalls(WallDeactivator.DeactivateNeigbourWall(bridgeSide, currentNodeParent));
        currentNode.TurnOffWalls(WallDeactivator.DeactivateOwnWall(bridgeSide, currentNodeParent));
    }
    #endregion


    private Node FindNode(Vector2Int direction)
    {
        string parent = currentNodeParent;
        Vector2Int cords = currentNode.Coordinates + direction;

        CheckParentAndCords(ref parent, ref cords);

        return TryGetNode(cords, parent);
    }
    private void CheckParentAndCords(ref string parent, ref Vector2Int cords)
    {
        if (cords.x < 0)
        {
            cords = ConvertXY.ConvertToHighX(currentNode, cubeSize);
            parent = GetCubeSide.GetLowXSide(currentNode);
        }
        else if (cords.x > cubeSize.x - 1)
        {
            cords = ConvertXY.ConvertToLowX(currentNode, cubeSize);
            parent = GetCubeSide.GetHighXSide(currentNode);
        }
        if (cords.y < 0)
        {
            cords = ConvertXY.ConvertToHighY(currentNode, cubeSize);
            parent = GetCubeSide.GetLowYSide(currentNode);
        }
        else if (cords.y > cubeSize.y - 1)
        {
            cords = ConvertXY.ConvertToLowY(currentNode, cubeSize);
            parent = GetCubeSide.GetHighYSide(currentNode);
        }
    }

    private Node TryGetNode(Vector2Int cords, string nodeParent)
    {
        try
        {
            return GetMazeModellNode(cords, nodeParent);
        }
        catch
        {
            Debug.LogWarning("Outside the MazeModells");
            return null;
        }
    }

    private Node GetMazeModellNode(Vector2Int cords, string name)
    {
        switch (name)
        {
            case "Top":
                return mazeModellTop[cords.x, cords.y];

            case "Front":
                return mazeModellFront[cords.x, cords.y];

            case "Back":
                return mazeModellBack[cords.x, cords.y];

            case "Bottom":
                return mazeModellBottom[cords.x, cords.y];

            case "Right":
                return mazeModellRight[cords.x, cords.y];

            case "Left":
                return mazeModellLeft[cords.x, cords.y];

            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }
}
