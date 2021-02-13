using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallDeactivator
{
    public static Vector2Int DeactivateNeigbourWall(string connect, string side)
    {
        switch (side)
        {
            case "Top":
                switch (connect)
                {
                    case "Front":
                        return Vector2Int.up;

                    case "Left":
                        return Vector2Int.up;

                    case "Back":
                        return Vector2Int.down;

                    case "Right":
                        return Vector2Int.up;

                }
                break;
            case "Front":
                switch (connect)
                {
                    case "Top":
                        return Vector2Int.down;

                    case "Right":
                        return Vector2Int.zero;

                    case "Bottom":
                        return Vector2Int.up;

                    case "Left":
                        return Vector2Int.right;
                }
                break;
            case "Back":
                switch (connect)
                {
                    case "Top":
                        return Vector2Int.up;
 
                    case "Left":
                        return Vector2Int.zero;

                    case "Bottom":
                        return Vector2Int.down;

                    case "Right":
                        return Vector2Int.right;

                }
                break;
            case "Bottom":
                switch (connect)
                {
                    case "Left":
                        return Vector2Int.down;

                    case "Front":
                        return Vector2Int.down;

                    case "Right":
                        return Vector2Int.down;

                    case "Back":
                        return Vector2Int.up;

                }
                break;
            case "Right":
                switch (connect)
                {
                    case "Top":
                        return Vector2Int.right;

                    case "Back":
                        return Vector2Int.right;

                    case "Bottom":
                        return Vector2Int.right;

                    case "Front":
                        return Vector2Int.right;

                }
                break;
            case "Left":
                switch (connect)
                {
                    case "Top":
                        return Vector2Int.zero;

                    case "Front":
                        return Vector2Int.zero;

                    case "Bottom":
                        return Vector2Int.zero;

                    case "Back":
                        return Vector2Int.zero;

                }
                break;
            default:
                Debug.Log("Outside Deactivate Connect Wall switch do fix!");
                break;
        }

        return new Vector2Int(2,2);
    }

    public static Vector2Int DeactivateOwnWall( string nodeParent, string side)
    {
        switch (side)
        {
            case "Top":
                switch (nodeParent)
                {
                    case "Front":
                        return (Vector2Int.down);
                    case "Left":
                        return Vector2Int.zero;
                    case "Back":
                        return (Vector2Int.up);
                    case "Right":
                        return (Vector2Int.right);
                }
                break;
            case "Front":
                switch (nodeParent)
                {
                    case "Top":
                        return (Vector2Int.up);
                    case "Right":
                        return (Vector2Int.right);
                    case "Bottom":
                        return (Vector2Int.down);
                    case "Left":
                        return Vector2Int.zero;
                }
                break;
            case "Back":
                switch (nodeParent)
                {
                    case "Top":
                        return (Vector2Int.down);
                    case "Left":
                        return Vector2Int.zero;
                    case "Bottom":
                        return (Vector2Int.up);
                    case "Right":
                        return (Vector2Int.right);
                }
                break;
            case "Bottom":
                switch (nodeParent)
                {
                    case "Left":
                        return (Vector2Int.zero);
                    case "Front":
                        return (Vector2Int.up);
                    case "Right":
                        return (Vector2Int.right);
                    case "Back":
                        return (Vector2Int.down);
                }
                break;
            case "Right":
                switch (nodeParent)
                {
                    case "Top":
                        return (Vector2Int.up);
                    case "Back":
                        return (Vector2Int.right);
                    case "Bottom":
                        return (Vector2Int.down);
                    case "Front":
                        return Vector2Int.zero;
                }
                break;
            case "Left":
                switch (nodeParent)
                {
                    case "Top":
                        return (Vector2Int.up);
                    case "Front":
                        return (Vector2Int.right);
                    case "Bottom":
                        return (Vector2Int.down);
                    case "Back":
                        return Vector2Int.zero;
                }
                break;
            default:
                Debug.Log("Outside Deactivate Own Wall switch do fix!");
                break;
        }
        return new Vector2Int(2, 2);
    }
}
