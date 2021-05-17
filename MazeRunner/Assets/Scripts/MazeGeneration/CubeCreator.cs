using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CubeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int cubeSize;
    [SerializeField] Transform[] sides;

    [SerializeField] int seed = 0;

    [SerializeField] ScriptableNode nodePoint;
    [SerializeField] GameObject pole;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject[] verticalEdgePoles;
    [SerializeField] GameObject[] horizontalEdgePoles;
    [SerializeField] GameObject[] walls;


    ScriptableNode[,] mazeModellTop;
    ScriptableNode[,] mazeModellBottom;
    ScriptableNode[,] mazeModellFront;
    ScriptableNode[,] mazeModellBack;
    ScriptableNode[,] mazeModellRight;
    ScriptableNode[,] mazeModellLeft;

    HashSet<ScriptableNode> visitedNodes;
    HashSet<ScriptableNode> frontier;

    ScriptableNode currentNode;
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

    /* IEnumerator Start()
         {
             Debug.Log("Initializing");
             Init();
             Debug.Log("Done initializing");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Placing poles");
             PlacePoles();
             Debug.Log("Done placing poles");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Generating cube");
             GenerateCube();
             Debug.Log("Done generating cube");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Generating maze");
             GenerateMaze();
             Debug.Log("Done generating maze");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Removing duplicates");
             RemoveDuplicates();
             Debug.Log("Done removing duplicates");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Building walls");
             BuildWalls();
             Debug.Log("Done building walls");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Offsetting sides");
             OffSetSides();
             Debug.Log("Done offsetting sides");
             yield return new WaitForSeconds(0.5f);
             Debug.Log("Combining meshes");
             GetComponent<MeshCombiner>().GenerateMesh();
             Debug.Log("Done combining meshses");
             Destroy(this);
         }*/

    void Start()
    {
        Debug.Log("Initializing");
        Init();
        Debug.Log("Done initializing");

        Debug.Log("Placing poles");
        PlacePoles();
        Debug.Log("Done placing poles");

        Debug.Log("Generating cube");
        GenerateCube();
        Debug.Log("Done generating cube");

        Debug.Log("Generating maze");
        GenerateMaze();
        Debug.Log("Done generating maze");

        Debug.Log("Removing duplicates");
        RemoveDuplicates();
        Debug.Log("Done removing duplicates");

        Debug.Log("Building walls");
        BuildWalls();
        Debug.Log("Done building walls");

        Debug.Log("Offsetting sides");
        OffSetSides();
        Debug.Log("Done offsetting sides");

        Debug.Log("Combining meshes");
        GetComponent<MeshCombiner>().GenerateMesh();
        Debug.Log("Done combining meshses");

        //Destroy(this);
    }

    /*    void Start()
    {
        Init();

        PlacePoles();

        GenerateCube();

        GenerateMaze();

        RemoveDuplicates();

        BuildWalls();

        OffSetSides();

        //GetComponent<MeshCombiner>().GenerateMesh();

        Destroy(this);
    }*/


    private void Init()
    {
        Random.InitState(seed);
        FindObjectOfType<RotationScript>().SetScale(cubeSize);
        gridSize = 1;
        ground.transform.localScale = new Vector3(cubeSize.x, cubeSize.y, cubeSize.x);
        SetNodeHastSets();
        SetMazeModellArrays();
        GenerateOffset();
    }
    private void SetNodeHastSets()
    {
        visitedNodes = new HashSet<ScriptableNode>();
        frontier = new HashSet<ScriptableNode>();
    }
    private void SetMazeModellArrays()
    {
        mazeModellTop = new ScriptableNode[cubeSize.x, cubeSize.y];
        mazeModellBottom = new ScriptableNode[cubeSize.x, cubeSize.y];
        mazeModellFront = new ScriptableNode[cubeSize.x, cubeSize.y];
        mazeModellBack = new ScriptableNode[cubeSize.x, cubeSize.y];
        mazeModellRight = new ScriptableNode[cubeSize.x, cubeSize.y];
        mazeModellLeft = new ScriptableNode[cubeSize.x, cubeSize.y];
    }
    private void GenerateOffset()
    {
        offset = new Vector3(cubeSize.x / 2, cubeSize.y / 2, 0);

        if (cubeSize.x % 2 == 1) { return; }

        float x = (cubeSize.x / 2) - ((float)gridSize / 2);
        float y = cubeSize.y / 2 - ((float)gridSize / 2);
        offset = new Vector3(x, y, 0);
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
        else if (y == cubeSize.y)
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
        if (y == 0)
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
            string nodeParent = node.Parent;
            currentNode = node;

            foreach (var direction in directions)
            {
                if (!node.GetWall(direction)) { continue; }

                string parent = nodeParent;
                Vector2Int cords = node.Coordinates + direction;

                //CheckParentAndCords(ref parent, ref cords);
                ScriptableNode connect = TryGetNode(cords, parent);

                if (connect == null) { continue; }
/*                string connectParent = connect.Parent;

                if (connectParent != nodeParent)
                {
                    connect.TurnOffWalls(WallDeactivator.DeactivateNeigbourWall(connectParent, nodeParent));
                }
                else
                {
                    connect.TurnOffWalls(-direction);
                }*/

                connect.TurnOffWalls(-direction);
            }
        }
    }

    private void BuildWalls()
    {
        foreach (var node in visitedNodes)
        {
            node.BuildWalls(walls);
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
        ScriptableNode newNode = (ScriptableNode)ScriptableObject.CreateInstance("ScriptableNode");
        newNode.Initialize(GetPos(x, y, i), new Vector2Int(x, y), gridSize, sides[i]);

        CreateEdgeWalls(x, y, newNode);
        SaveNodeToMazeModell(x, y, i, newNode);
    }
    private Vector3 GetPos(int x, int y, int i)
    {
        return new Vector3(gridSize * x, gridSize * y, 0) - offset;
    }


    private void CreateEdgeWalls(int x, int y, ScriptableNode node)
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

    private void SaveNodeToMazeModell(int x, int y, int i, ScriptableNode node)
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
                Debug.LogWarning("Couldn't add to mazemodells as side: " + sides[i].name + " doesn't exist");
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

        frontier.Add(TryGetNode(new Vector2Int(x, y), sides[i].name));
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
        currentNode = frontier.ElementAt(Random.Range(0, frontier.Count));
        currentNodeParent = currentNode.Parent;
        visitedNodes.Add(currentNode);
    }

    private void AddFrontierCells()
    {
        foreach (var direction in directions)
            AddToFrontier(direction);
    }
    private void AddToFrontier(Vector2Int direction)
    {
        ScriptableNode node = FindNode(direction);
        if (node == null) return;
        if (visitedNodes.Contains(node)) return;

        frontier.Add(node);
    }

    #endregion

    #region methods handling the connection of nodes
    private void ConnectNewNode()
    {
        List<ScriptableNode> closeVisisted = new List<ScriptableNode>();

        foreach (var direction in directions)
        {
            ScriptableNode nodeToAdd = FindVisitedNode(direction);
            if (nodeToAdd != null)
                closeVisisted.Add(nodeToAdd);
        }

        if (closeVisisted.Count < 1)
            return;

        HandleWallTurnOff(closeVisisted);

    }
    private ScriptableNode FindVisitedNode(Vector2Int direction)
    {
        ScriptableNode node = FindNode(direction);
        if (node == null) return null;
        if (!visitedNodes.Contains(node)) { return null; }

        node.ExploredFrom = direction;
        return node;
    }

    private void HandleWallTurnOff(List<ScriptableNode> closeVisisted)
    {
        ScriptableNode bridgeTo = closeVisisted[Random.Range(0, closeVisisted.Count)];

        if (currentNodeParent == bridgeTo.Parent)
            TurnOffWalls(bridgeTo);
        else
            TurnOffSideWalls(bridgeTo);
    }
    private void TurnOffWalls(ScriptableNode bridgeTo)
    {
        Vector2Int dir = bridgeTo.ExploredFrom;
        bridgeTo.TurnOffWalls(-dir);
        currentNode.TurnOffWalls(dir);
    }
    private void TurnOffSideWalls(ScriptableNode bridgeTo)
    {
        string bridgeSide = bridgeTo.Parent;
        bridgeTo.TurnOffWalls(WallDeactivator.DeactivateNeigbourWall(bridgeSide, currentNodeParent));
        currentNode.TurnOffWalls(WallDeactivator.DeactivateOwnWall(bridgeSide, currentNodeParent));
    }
    #endregion


    private ScriptableNode FindNode(Vector2Int direction)
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

    private ScriptableNode TryGetNode(Vector2Int cords, string nodeParent)
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

    private ScriptableNode GetMazeModellNode(Vector2Int cords, string name)
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
