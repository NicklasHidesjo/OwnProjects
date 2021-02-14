using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableNode : ScriptableObject
{
	bool buildNorth = true;
	bool buildEast = true;
	bool buildSouth = true;
	bool buildWest = true;
	bool buildThiccNorth = false;
	bool buildThiccEast = false;
	bool buildThiccSouth = false;
	bool buildThiccWest = false;

	Vector2Int coordinates = new Vector2Int(0, 0);
	public Vector2Int Coordinates { get { return coordinates; } }

	bool partOfMaze = false;
	public bool PartOfMaze { get { return partOfMaze; } set { partOfMaze = value; } }

	Vector2Int exploredFrom = new Vector2Int(0, 0);
	public Vector2Int ExploredFrom { get { return exploredFrom; } set { exploredFrom = value; } }

	Vector3 position;
	float size;
    Transform parent;
	public string Parent { get { return parent.name; } }

    public void Initialize(Vector3 pos, Vector2Int cords, int size, Transform parent)
    {
		position = pos;
		coordinates = cords;
		this.size = size;
		this.parent = parent;
    }

	public void TurnOffWalls(Vector2Int direction)
	{
		if (direction == Vector2Int.up)
		{
			buildNorth = false;
			buildThiccNorth = false;
		}
		else if (direction == Vector2Int.right)
		{
			buildEast = false;
			buildThiccEast = false;
		}
		else if (direction == Vector2Int.down)
		{
			buildSouth = false;
			buildThiccSouth = false;
		}
		else
		{
			buildWest = false;
			buildThiccWest = false;
		}
	}
	public void TurnOnThiccWall(Vector2Int direction)
	{
		if (direction == Vector2Int.up)
		{
			buildNorth = false;
			buildThiccNorth = true;
		}
		else if (direction == Vector2Int.right)
		{
			buildEast = false;
			buildThiccEast = true;
		}
		else if (direction == Vector2Int.down)
		{
			buildSouth = false;
			buildThiccSouth = true;
		}
		else
		{
			buildWest = false;
			buildThiccWest = true;
		}
	}

	public void BuildWalls(GameObject[] walls)
	{
		if (buildThiccNorth)
        {
			Vector3 truePos = new Vector3(position.x, position.y + (size - 0.025f), -size/2);
			Instantiate(walls[1], truePos, Quaternion.Euler(90, 0, 0), parent);
		}
		else if (buildNorth)
        {
			Vector3 truePos = new Vector3(position.x, position.y + (size / 2), -size / 2);
			Instantiate(walls[0], truePos, Quaternion.Euler(90, 0, 0), parent);
        }

		if (buildThiccEast)
        {
			Vector3 truePos = new Vector3(position.x + (size - 0.025f), position.y, -size / 2);
			Instantiate(walls[3], truePos, Quaternion.Euler(0, 180, 270), parent);
		}
		else if (buildEast)
		{
			Vector3 truePos = new Vector3(position.x + (size / 2), position.y, -size / 2);
			Instantiate(walls[2], truePos, Quaternion.Euler(90,0,0), parent);
		}

		if (buildThiccSouth)
		{
			Vector3 truePos = new Vector3(position.x, position.y - (size - 0.025f), -size / 2);
			Instantiate(walls[1], truePos, Quaternion.Euler(90, 0, 0), parent);
		}
		else if (buildSouth)
		{
			Vector3 truePos = new Vector3(position.x, position.y - (size / 2), -size / 2);
			Instantiate(walls[0], truePos, Quaternion.Euler(90, 0, 0), parent);
		}

		if (buildThiccWest)
		{
			Vector3 truePos = new Vector3(position.x - (size - 0.025f), position.y, -size / 2);
			Instantiate(walls[3], truePos, Quaternion.Euler(0, 180, 270), parent);
		}
		else if (buildWest)
		{
			Vector3 truePos = new Vector3(position.x -(size/2), position.y, -size / 2);
			Instantiate(walls[2], truePos, Quaternion.Euler(90, 0, 0), parent);
		}
	}

	public bool GetWall(Vector2Int direction)
	{
		if (direction == Vector2Int.up)
		{
			if (buildNorth)
				return true;
			if (buildThiccNorth)
				return true;
			return false;
		}
		if (direction == Vector2Int.right)
		{
			if (buildEast)
				return true;
			if (buildThiccEast)
				return true;
			return false;

		}
		if (direction == Vector2Int.down)
		{
			if (buildSouth)
				return true;
			if (buildThiccSouth)
				return true;
			return false;
		}
		if (direction == Vector2Int.left)
		{
			if (buildWest)
				return true;
			if (buildThiccWest)
				return true;
			return false;
		}

		Debug.LogWarning("No valid direction");
		return false;
	}
}
