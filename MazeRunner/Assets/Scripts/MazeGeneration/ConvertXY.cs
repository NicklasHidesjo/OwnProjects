using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConvertXY
{
    // refactor this with either a default or falling cases

    public static Vector2Int ConvertToLowY(ScriptableNode node, Vector2Int cubeSize)
    {
        int x = cubeSize.x - 1;
        int y = cubeSize.y - 1;

        switch (node.Parent)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Front":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Back":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Bottom":
                return new Vector2Int(node.Coordinates.x, 0);

            case "Right":
                return new Vector2Int(node.Coordinates.y, node.Coordinates.x);

            case "Left":
                return new Vector2Int(0, y - node.Coordinates.x);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0); ;
        }
    }
    public static Vector2Int ConvertToHighY(ScriptableNode node, Vector2Int cubeSize)
    {
        int x = cubeSize.x - 1;
        int y = cubeSize.y - 1;

        switch (node.Parent)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.x, y);

            case "Front":
                return new Vector2Int(node.Coordinates.x, y);

            case "Back":
                return new Vector2Int(node.Coordinates.x, y);

            case "Bottom":
                return new Vector2Int(node.Coordinates.x, y);

            case "Right":
                return new Vector2Int(x, y - node.Coordinates.x);

            case "Left":
                return new Vector2Int(0, node.Coordinates.x);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }
    public static Vector2Int ConvertToLowX(ScriptableNode node, Vector2Int cubeSize)
    {
        int x = cubeSize.x - 1;
        int y = cubeSize.y - 1;

        switch (node.Parent)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.y, node.Coordinates.x);

            case "Front":
                return new Vector2Int(0 , node.Coordinates.y);

            case "Back":
                return new Vector2Int(node.Coordinates.x,node.Coordinates.y);

            case "Bottom":
                return new Vector2Int(x - node.Coordinates.y, 0);

            case "Right":
                return new Vector2Int(node.Coordinates.x, y - node.Coordinates.y);

            case "Left":
                return new Vector2Int(0, node.Coordinates.y);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }
    public static Vector2Int ConvertToHighX(ScriptableNode node, Vector2Int cubeSize)
    {
        int x = cubeSize.x - 1;
        int y = cubeSize.y - 1;

        switch (node.Parent)
        {
            case "Top":
                return new Vector2Int(x - node.Coordinates.y, y);

            case "Front":
                return new Vector2Int(x, node.Coordinates.y);

            case "Back":
                return new Vector2Int(node.Coordinates.x, node.Coordinates.y);

            case "Bottom":
                return new Vector2Int(node.Coordinates.y, node.Coordinates.x);

            case "Right":
                return new Vector2Int(x, node.Coordinates.y);

            case "Left":
                return new Vector2Int(0, y - node.Coordinates.y);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }
}
