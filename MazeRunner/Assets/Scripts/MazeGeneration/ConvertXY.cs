using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConvertXY
{
    public static Vector2Int ConvertToLowY(Node node, Vector2Int cubeSize)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                return new Vector2Int(node.Coordinates.x, cubeSize.y - 1);

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
                return new Vector2Int(0, 0); ;
        }
    }
    public static Vector2Int ConvertToHighY(Node node, Vector2Int cubeSize)
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
    public static Vector2Int ConvertToLowX(Node node, Vector2Int cubeSize)
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
    public static Vector2Int ConvertToHighX(Node node, Vector2Int cubeSize)
    {
        switch (node.transform.parent.name)
        {
            case "Top":
                return new Vector2Int(cubeSize.x - 1, node.Coordinates.y);

            case "Front":
                return new Vector2Int(node.Coordinates.y, 0);

            case "Back":
                return new Vector2Int(node.Coordinates.y, cubeSize.y - 1);

            case "Bottom":
                return new Vector2Int(node.Coordinates.x, node.Coordinates.y);

            case "Right":
                return new Vector2Int(cubeSize.x - 1, node.Coordinates.y);

            case "Left":
                return new Vector2Int(0, node.Coordinates.y);

            default:
                Debug.Log("outside the switch plz check");
                return new Vector2Int(0, 0);
        }
    }
}
