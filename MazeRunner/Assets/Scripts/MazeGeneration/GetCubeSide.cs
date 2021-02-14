using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetCubeSide
{
    public static string GetHighYSide(ScriptableNode node)
    {
        switch (node.Parent)
        {
            case "Top":
                return "Back";
            case "Front":
                return "Top";
            case "Back":
                return "Bottom";
            case "Bottom":
                return "Front";
            case "Right":
                return "Top";
            case "Left":
                return "Top";
            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }
    public static string GetLowYSide(ScriptableNode node)
    {
        switch (node.Parent)
        {
            case "Top":
                return "Front";
            case "Front":
                return "Bottom";
            case "Back":
                return "Top";
            case "Bottom":
                return "Back";
            case "Right":
                return "Bottom";
            case "Left":
                return "Bottom";
            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }
    public static string GetHighXSide(ScriptableNode node)
    {
        switch (node.Parent)
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
                return "Back";
            case "Left":
                return "Front";
            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }
    public static string GetLowXSide(ScriptableNode node)
    {
        switch (node.Parent)
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
                return "Front";
            case "Left":
                return "Back";
            default:
                Debug.Log("outside the switch plz check");
                return null;
        }
    }
}
