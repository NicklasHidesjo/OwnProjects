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

        RemoveDuplicates();

        BuildWalls();

        // remove comment on line below if you want to generate a maze slowly (for visual porn only) (remember to comment row 45)
        //StartCoroutine(GenerateSlowMaze());

        //GetComponent<MeshCombiner>().GenerateMesh();
        OffsetCube();

        Destroy(this);
    }

    // refactor this
    private void RemoveDuplicates()
    {
        Debug.Log(visitedNodes.Count);

        foreach (var node in visitedNodes)
        {
            Debug.Log("hello2");
            string nodeParent = node.transform.parent.name;
            foreach (var direction in directions)
            {
                //if (!node.GetWall(direction)) { continue; }
                Vector2Int cords = node.Coordinates + direction;
                Node neigbour = TryGetNode(cords, nodeParent);
                if (neigbour == null) { continue; }
                //neigbour.TurnOffWalls(-direction);
                Debug.Log("removing walls");

                Vector2Int dir = direction;
                if (nodeParent == "Back" || nodeParent == "Bottom" || nodeParent == "Right")
                {
                    // manually check what plane and then remove depending on the direction.
                    // it should just be the opposite but for some reason it aint.

                    #region non working stuff
                    /*                    if (cords.y == 9 )
                                        {
                                            if(dir.y != 0)
                                            {
                                                neigbour.TurnOffWalls(Vector2Int.up);
                                                continue;
                                            }

                                        }
                                        else if (cords.y == 0 )
                                        {
                                            if (dir.y != 0)
                                            {
                                                neigbour.TurnOffWalls(Vector2Int.down);
                                                continue;
                                            }
                                        }

                                            //node.TurnOffWalls(dir);
                                            neigbour.TurnOffWalls(-dir);*/
                    #endregion
                }
                else
                {
                    neigbour.TurnOffWalls(-dir);
                    //node.TurnOffWalls(dir);
                }

            }
        }
        Debug.Log("done");
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
            Instantiate(frontTopLeftPole, GetPos(x, y, i), GetRot(x,y,i), sides[i]);

        else
            Instantiate(backBottomRightPole, GetPos(x, y, i), GetRot(x, y, i), sides[i]);
    }

    private void CreateEdges(int i, int x, int y)
    {
        if (y == 0)
        {
            if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
                Instantiate(verticalEdgePoles[0], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
            else
                Instantiate(verticalEdgePoles[2], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
        }
        if (y == cubeSize.y)
        {
            Instantiate(verticalEdgePoles[1], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
        }
        if (x == 0)
        {
            if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
                Instantiate(horizontalEdgePoles[0], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
            else
                Instantiate(horizontalEdgePoles[2], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
        }
        if (x == cubeSize.x)
        {
            if (sides[i].name == "Front" || sides[i].name == "Top" || sides[i].name == "Left")
                Instantiate(horizontalEdgePoles[1], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
            else
                Instantiate(horizontalEdgePoles[3], GetPos(x, y, i), GetRot(x,y,i), sides[i]);
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
        Node newNode = Instantiate(nodePoint, GetPos(x, y, i), GetRot(x,y,i), sides[i]);

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
    private Quaternion GetRot(int x, int y, int i)
    {
        switch (sides[i].name)
        {
            case "Top":
                return Quaternion.Euler(90,0,0);
            case "Front":
                return Quaternion.Euler(0, 0, 0);
            case "Back":
                return Quaternion.Euler(180, 0, 0);
            case "Bottom":
                return Quaternion.Euler(-90, 0, 0);
            case "Right":
                return Quaternion.Euler(0, -90, 90);
            case "Left":
                return Quaternion.Euler(0, 90, 90);
            default:
                Debug.Log("outside the switch plz check");
                return Quaternion.Euler(0, 0, 0);
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
                Debug.LogWarning("Couldn't add to mazemodells as side: "+ sides[i].name + " doesn't exist" );
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

        frontier.Add(GetMazeModellNode(new Vector2Int(x,y), sides[i].name));
    }
    private void HandleFrontierNode()
    {       
        GetNewFrontier();
        AddFrontierCells(); 
        ConnectNewNode(); 
        frontier.Remove(frontierNode);
    }
    private void GetNewFrontier()
    {
        frontierNode = frontier.First();
        frontierParent = frontierNode.transform.parent.name;
        visitedNodes.Add(frontierNode);
    }

    #region methods handling the search and adding of nodes to frontier
    private void AddFrontierCells()
    {        
        foreach (var direction in directions)
            AddToFrontier(direction);
    }
    private void AddToFrontier(Vector2Int direction)
    {
        string parent = frontierParent;
        Vector2Int cords = frontierNode.Coordinates + direction;
        CheckParentAndCords(ref parent, ref cords);

        if (visitedNodes.Contains(TryGetNode(cords, parent))) { return; }
        frontier.Add(TryGetNode(cords, parent));
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
        Vector2Int cords = frontierNode.Coordinates + direction;
        string parent = frontierParent;

        CheckParentAndCords(ref parent, ref cords);

        if (!visitedNodes.Contains(TryGetNode(cords, parent))) { return null; }

        GetMazeModellNode(cords, parent).ExploredFrom = direction;
        return TryGetNode(cords,parent);
    }

    private void HandleWallTurnOff(List<Node> closeVisisted)
    {
        Node bridgeTo = closeVisisted[Random.Range(0, closeVisisted.Count)];

        if (frontierParent == bridgeTo.transform.parent.name)
            TurnOffWalls(bridgeTo);
        else
            TurnOffSideWalls(bridgeTo);
    }
    private void TurnOffWalls(Node bridgeTo)
    {
        Vector2Int dir = bridgeTo.ExploredFrom;
        if (frontierParent == "Back" || frontierParent  == "Bottom" || frontierParent == "Right")
            dir = new Vector2Int(dir.x, -dir.y);
        bridgeTo.TurnOffWalls(-dir);
        frontierNode.TurnOffWalls(dir);
    }
    private void TurnOffSideWalls(Node bridgeTo)
    {
        string bridgeSide = bridgeTo.transform.parent.name;
        bridgeTo.TurnOffWalls(WallDeactivator.DeactivateNeigbourWall(bridgeSide, frontierParent));
        frontierNode.TurnOffWalls(WallDeactivator.DeactivateOwnWall(bridgeSide, frontierParent));
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
