using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CubeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int cubeSize = new Vector2Int(10, 10);


    [SerializeField] Transform[] sides = new Transform[6];
    [SerializeField] Node nodePoint = null;
    [SerializeField] GameObject frontTopLeftPole;
    [SerializeField] GameObject backBottomRightPole;
    [SerializeField] GameObject[] verticalEdgePoles;
    [SerializeField] GameObject[] horizontalEdgePoles;

    [SerializeField] GameObject edgePole;


    Node[,] mazeModellTop;
    Node[,] mazeModellBottom;
    Node[,] mazeModellFront;
    Node[,] mazeModellBack;
    Node[,] mazeModellRight;
    Node[,] mazeModellLeft;

    HashSet<Node> visitedNodes = new HashSet<Node>();
    HashSet<Node> frontier = new HashSet<Node>();

    Node frontierNode;
    string frontierParent;
    int gridSize;

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    [SerializeField] int divide = 3;

    void Start()
    {
        InitArrays();
        //PlacePoles();

        GenerateCube();

        OffSetSides();

        GenerateMazeQuick();

        Debug.Log(visitedNodes.Count);

/*        foreach (var node in visitedNodes)
        {
            Debug.Log("hello2");
            string nodeParent = node.transform.parent.name;
            foreach (var direction in directions)
            {
                if(!node.GetWall(direction)) { continue; }
                Vector2Int cords = node.Coordinates + direction;
                Node neigbour = TryGetNode(cords, nodeParent);
                if(neigbour == null) { continue; }
                //neigbour.TurnOffWalls(-direction);
                Debug.Log("removing walls");

                Vector2Int dir = direction;
                if (frontierParent == "Back" || frontierParent == "Bottom" || frontierParent == "Right")
                {
                    if (cords.y == 0)
                        dir = Vector2Int.right;
                }
                neigbour.TurnOffWalls(-dir);
                node.TurnOffWalls(dir);
            }
        }
        Debug.Log("done");*/
        BuildWalls();

        // remove comment on line below if you want to generate a maze slowly (for visual porn only) (remember to comment row 45)
        //StartCoroutine(GenerateSlowMaze());

        //GetComponent<MeshCombiner>().GenerateMesh();
        OffsetCube();

        Destroy(this);
    }


    private void BuildWalls()
    {
        foreach (var node in visitedNodes)
        {
            node.BuildWalls();
        }
    }

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

    private Vector3 GetPolePos(int x, int y, int i)
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
    private void InitializeNewPole(int i, int x, int y)
    {
        if (y == 0 || y == cubeSize.y || x == 0 || x == cubeSize.x)
            CreateEdges(i, x, y);
        if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
            Instantiate(frontTopLeftPole, GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);

        else
            Instantiate(backBottomRightPole, GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
    }

    private void CreateEdges(int i, int x, int y)
    {
        if (y == 0)
        {
            if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
                Instantiate(verticalEdgePoles[0], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
            else
                Instantiate(verticalEdgePoles[2], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
        }
        if (y == cubeSize.y)
        {
            Instantiate(verticalEdgePoles[1], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
        }
        if (x == 0)
        {
            if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
                Instantiate(horizontalEdgePoles[0], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
            else
                Instantiate(horizontalEdgePoles[2], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
        }
        if (x == cubeSize.x)
        {
            if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
                Instantiate(horizontalEdgePoles[1], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
            else
                Instantiate(horizontalEdgePoles[3], GetPolePos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
        }
    }

    private void InitArrays()
    {
        gridSize = (int)nodePoint.transform.localScale.x;
        mazeModellTop = new Node[cubeSize.x, cubeSize.y];
        mazeModellBottom = new Node[cubeSize.x, cubeSize.y];
        mazeModellFront = new Node[cubeSize.x, cubeSize.y];
        mazeModellBack = new Node[cubeSize.x, cubeSize.y];
        mazeModellRight = new Node[cubeSize.x, cubeSize.y];
        mazeModellLeft = new Node[cubeSize.x, cubeSize.y];
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
        Node newNode = Instantiate(nodePoint, GetPos(x, y, i), new Quaternion(0, 0, 0, 0), sides[i]);
        newNode.Coordinates = new Vector2Int(x, y);

        SetWallsAlongX(x, y, i, newNode);
        SetWallsAlongY(x, y, i, newNode);
        SaveNodeToMazeModell(x, y, i, newNode);
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

    #region Refactoring this should be easy (Low prio)
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

    #endregion

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
    private void OffsetCube()
    {
        float offset = -cubeSize.x / 2;
        transform.position = new Vector3(offset,offset,offset);
    }
    #endregion

    private void GenerateMazeQuick()
    {
        GetFirstNode();
        while (frontier.Count > 0)
            HandleFrontierNode();
    }
    IEnumerator GenerateSlowMaze()
    {
        GetFirstNode();
        int index = 0;
        while (frontier.Count > 0)
        {
            if (index % divide == 0)
                yield return new WaitForSeconds(0);
            HandleFrontierNode();
            index++;
        }
    }

    private void GetFirstNode()
    {
        int x = Random.Range(1, cubeSize.x - 1);
        int y = Random.Range(1, cubeSize.y - 1);
        int i = Random.Range(0, sides.Length);

        i = 2;

        frontier.Add(GetMazeModellNode(new Vector2Int(x,y), sides[i].name));
    }
    private void HandleFrontierNode()
    {       
        GetNewFrontier(); // gets a random node from frontier       
        AddFrontierCells(); // Adds the non visited nodes to frontier     
        ConnectNewNode(); // connects to one of the visitedNodes
        frontier.Remove(frontierNode); // remove the node from the frontier list
    }
    private void GetNewFrontier()
    {
        //frontierNode = frontier[Random.Range(0, frontier.Count)];
        frontierNode = frontier.First();
        frontierParent = frontierNode.transform.parent.name;
        visitedNodes.Add(frontierNode);
    }
    #region methods handling the search and adding of nodes to frontier
    private void AddFrontierCells()
    {        
        foreach (var direction in directions)
            HandleAddingToFrontier(frontierNode, direction);
    }
    private void HandleAddingToFrontier(Node node, Vector2Int direction)
    {
        string parent = "";
        Vector2Int cords = frontierNode.Coordinates + direction;
        CheckParentAndCords(ref parent, ref cords);
        CheckHowToAdd(parent, cords);
    }
    private void CheckHowToAdd(string parent, Vector2Int cords)
    {
        if (parent != "")
        {
            if (IsValidNode(cords, parent))
                AddNodeToFrontier(parent, cords);
        }
        else
        {
            if (IsValidNode(cords, frontierParent))
                AddNodeToFrontier(cords);
        }
    }
    private void AddNodeToFrontier(Vector2Int cords)
    {
        Node newNode = GetMazeModellNode(cords, frontierParent);
        if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
            frontier.Add(newNode);
    }
    private void AddNodeToFrontier(string parent, Vector2Int cords)
    {
        Node newNode = GetMazeModellNode(cords, parent);
        if (!visitedNodes.Contains(newNode) && !frontier.Contains(newNode))
            frontier.Add(newNode);
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

        HandleDestructionOfWalls(closeVisisted);

    }
    private Node FindVisitedNode(Vector2Int direction)
    {
        Vector2Int cords = frontierNode.Coordinates + direction;
        string parent = "";
        Node node = null;

        CheckParentAndCords(ref parent, ref cords);

        if (parent != "")
        {
            if (IsValidNode(cords, parent) && visitedNodes.Contains(GetMazeModellNode(cords, parent)))
                node = GetVisitedNode(direction, cords, parent);
        }
        else
        {
            if (IsValidNode(cords, frontierParent) && visitedNodes.Contains(GetMazeModellNode(cords, frontierParent)))
                node = GetVisitedNode(direction, cords);
        }

        return node;
    }
    private Node GetVisitedNode(Vector2Int direction, Vector2Int cords, string parent)
    {
        Node node = GetMazeModellNode(cords, parent);
        GetMazeModellNode(cords, parent).ExploredFrom = direction;
        return node;
    }
    private Node GetVisitedNode(Vector2Int direction, Vector2Int cords)
    {
        Node node = GetMazeModellNode(cords, frontierParent);
        GetMazeModellNode(cords, frontierParent).ExploredFrom = direction;
        return node;
    }
    private void HandleDestructionOfWalls(List<Node> closeVisisted)
    {
        Node bridgeTo = closeVisisted[Random.Range(0, closeVisisted.Count)];

        if (frontierParent != bridgeTo.transform.parent.name)
            DestroyWallsOnOtherSide(bridgeTo);
        else
            DestroyWallsOnSameSide(bridgeTo);
    }
    private void DestroyWallsOnSameSide(Node bridgeTo)
    {
        Vector2Int dir = bridgeTo.ExploredFrom;
        if (frontierParent == "Back" || frontierParent  == "Bottom" || frontierParent == "Right")
            dir = new Vector2Int(dir.x, -dir.y);
        bridgeTo.TurnOffWalls(-dir);
        frontierNode.TurnOffWalls(dir);
    }
    private void DestroyWallsOnOtherSide(Node bridgeTo)
    {
        switch (frontierParent)
        {
            case "Top":
                switch(bridgeTo.transform.parent.name)
                {
                    case "Front":
                        bridgeTo.TurnOffWalls(Vector2Int.up);
                        frontierNode.TurnOffWalls(Vector2Int.down);
                        break;
                    case "Left":
                        bridgeTo.TurnOffWalls(Vector2Int.right);
                        frontierNode.TurnOffWalls(Vector2Int.zero);
                        break;
                    case "Back":
                        bridgeTo.TurnOffWalls(Vector2Int.down);
                        frontierNode.TurnOffWalls(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.TurnOffWalls(Vector2Int.right);
                        frontierNode.TurnOffWalls(Vector2Int.right);
                        break;
                }
                break;
            case "Front":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.TurnOffWalls(Vector2Int.down);
                        frontierNode.TurnOffWalls(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.TurnOffWalls(Vector2Int.up);
                        frontierNode.TurnOffWalls(Vector2Int.right);
                        break;
                    case "Bottom":
                        bridgeTo.TurnOffWalls(Vector2Int.up);
                        frontierNode.TurnOffWalls(Vector2Int.down);
                        break;
                    case "Left":
                        bridgeTo.TurnOffWalls(Vector2Int.down);
                        frontierNode.TurnOffWalls(Vector2Int.zero);
                        break;
                }
                break;
            case "Back":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.TurnOffWalls(Vector2Int.up);
                        frontierNode.TurnOffWalls(Vector2Int.down);
                        break;
                    case "Left":
                        bridgeTo.TurnOffWalls(Vector2Int.up);
                        frontierNode.TurnOffWalls(Vector2Int.zero);
                        break;
                    case "Bottom":
                        bridgeTo.TurnOffWalls(Vector2Int.down);
                        frontierNode.TurnOffWalls(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.TurnOffWalls(Vector2Int.down);
                        frontierNode.TurnOffWalls(Vector2Int.right);
                        break;
                }
                break;
            case "Bottom":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Left":
                        bridgeTo.TurnOffWalls(Vector2Int.zero);
                        frontierNode.TurnOffWalls(Vector2Int.zero);
                        break;
                    case "Front":
                        bridgeTo.TurnOffWalls(Vector2Int.down);
                        frontierNode.TurnOffWalls(Vector2Int.up);
                        break;
                    case "Right":
                        bridgeTo.TurnOffWalls(Vector2Int.zero);
                        frontierNode.TurnOffWalls(Vector2Int.right);
                        break;
                    case "Back":
                        bridgeTo.TurnOffWalls(Vector2Int.up);
                        frontierNode.TurnOffWalls(Vector2Int.down);
                        break;
                }
                break;
            case "Right":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.TurnOffWalls(Vector2Int.right);
                        frontierNode.TurnOffWalls(Vector2Int.right);
                        break;
                    case "Back":
                        bridgeTo.TurnOffWalls(Vector2Int.right);
                        frontierNode.TurnOffWalls(Vector2Int.down);
                        break;
                    case "Bottom":
                        bridgeTo.TurnOffWalls(Vector2Int.right);
                        frontierNode.TurnOffWalls(Vector2Int.zero);
                        break;
                    case "Front":
                        bridgeTo.TurnOffWalls(Vector2Int.right);
                        frontierNode.TurnOffWalls(Vector2Int.up);
                        break;
                }
                break;
            case "Left":
                switch (bridgeTo.transform.parent.name)
                {
                    case "Top":
                        bridgeTo.TurnOffWalls(Vector2Int.zero);
                        frontierNode.TurnOffWalls(Vector2Int.right);
                        break;
                    case "Front":
                        bridgeTo.TurnOffWalls(Vector2Int.zero);
                        frontierNode.TurnOffWalls(Vector2Int.down);
                        break;
                    case "Bottom":
                        bridgeTo.TurnOffWalls(Vector2Int.zero);
                        frontierNode.TurnOffWalls(Vector2Int.zero);
                        break;
                    case "Back":
                        bridgeTo.TurnOffWalls(Vector2Int.zero);
                        frontierNode.TurnOffWalls(Vector2Int.up);
                        break;
                }
                break;
            default:
                Debug.Log("outside the switch plz check");
                break;
        }
    }
    #endregion

    private void CheckParentAndCords(ref string parent, ref Vector2Int cords)
    {
        if (cords.x < 0)
        {
            cords = ConvertXY.ConvertToHighX(frontierNode, cubeSize);
            parent = GetCubeSide.GetLowXSide(frontierNode);
        }
        else if (cords.x > cubeSize.x - 1)
        {
            cords = ConvertXY.ConvertToLowX(frontierNode, cubeSize);
            parent = GetCubeSide.GetHighXSide(frontierNode);
        }
        else if (cords.y < 0)
        {
            cords = ConvertXY.ConvertToHighY(frontierNode, cubeSize);
            parent = GetCubeSide.GetLowYSide(frontierNode);
        }
        else if (cords.y > cubeSize.y - 1)
        {
            cords = ConvertXY.ConvertToLowY(frontierNode, cubeSize);
            parent = GetCubeSide.GetHighYSide(frontierNode);
        }
    }

    private bool IsValidNode(Vector2Int cords, string nodeParent)
    {
        try
        {
            Node tryNode = GetMazeModellNode(cords, nodeParent);
            return true;
        }
        catch
        {
            Debug.LogWarning("Outside the MazeModells");
            return false;
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
