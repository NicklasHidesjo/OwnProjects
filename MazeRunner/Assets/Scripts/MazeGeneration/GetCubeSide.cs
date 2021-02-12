using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetCubeSide
{
    public static string GetHighYSide(Node node)
    {
        switch (node.transform.parent.name)
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
    public static string GetLowYSide(Node node)
    {
        switch (node.transform.parent.name)
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
    public static string GetHighXSide(Node node)
    {
        switch (node.transform.parent.name)
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
    public static string GetLowXSide(Node node)
    {
        switch (node.transform.parent.name)
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
}
