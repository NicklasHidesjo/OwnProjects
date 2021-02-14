using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableNode : ScriptableObject
{
	[SerializeField] GameObject verticalWall;
	[SerializeField] GameObject horizontalWall;

	[SerializeField] GameObject thiccHorizontal;
	[SerializeField] GameObject thiccVertical;

	bool buildNorth = true;
	bool buildEast = true;
	bool buildSouth = true;
	bool buildWest = true;
	bool buildThiccNorth = false;
	bool buildThiccEast = false;
	bool buildThiccSouth = false;
	bool buildThiccWest = false;

	Vector2Int coordinates = new Vector2Int(0, 0);
	public Vector2Int Coordinates { get { return coordinates; } set { coordinates = value; } }

	bool partOfMaze = false;
	public bool PartOfMaze { get { return partOfMaze; } set { partOfMaze = value; } }

	Vector2Int exploredFrom = new Vector2Int(0, 0);
	public Vector2Int ExploredFrom { get { return exploredFrom; } set { exploredFrom = value; } }

	Vector3 position;
	int size;
    Transform parent;

    public ScriptableNode(Vector3 pos, Vector2Int cords, int size, Transform parent)
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

	public void BuildWalls()
	{
		if (buildThiccNorth)
        {
			Vector3 truePos = new Vector3(position.x, position.y + (size / 2),0);
			Instantiate(thiccHorizontal, truePos, Quaternion.identity,parent);
		}
		else if (buildNorth)
        {
			Vector3 truePos = new Vector3(position.x, position.y + (size / 2),0);
			Instantiate(horizontalWall, truePos, Quaternion.identity, parent);
        }

		if (buildThiccEast)
        {
			Vector3 truePos = new Vector3(position.x + (size / 2), position.y,0);
			Instantiate(thiccVertical, truePos, Quaternion.identity, parent);
		}
		else if (buildEast)
		{
			Vector3 truePos = new Vector3(position.x + (size / 2), position.y,0);
			Instantiate(verticalWall, truePos, Quaternion.identity, parent);
		}

		if (buildThiccSouth)
		{
			Vector3 truePos = new Vector3(position.x, position.y - (size / 2), 0);
			Instantiate(thiccHorizontal, truePos, Quaternion.identity, parent);
		}
		else if (buildSouth)
		{
			Vector3 truePos = new Vector3(position.x, position.y - (size / 2), 0);
			Instantiate(horizontalWall, truePos, Quaternion.identity, parent);
		}

		if (buildThiccWest)
		{
			Vector3 truePos = new Vector3(position.x - (size / 2), position.y,0);
			Instantiate(thiccVertical, truePos, Quaternion.identity, parent);
		}
		else if (buildWest)
		{
			Vector3 truePos = new Vector3(position.x - (size / 2), position.y,0);
			Instantiate(verticalWall, truePos, Quaternion.identity, parent);
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
