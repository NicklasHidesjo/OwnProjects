using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	[SerializeField] GameObject northWall;
	[SerializeField] GameObject eastWall;
	[SerializeField] GameObject southWall;
	[SerializeField] GameObject westWall;

	[SerializeField] GameObject thiccNorthWall;
	[SerializeField] GameObject thiccEastWall;
	[SerializeField] GameObject thiccSouthWall;
	[SerializeField] GameObject thiccWestWall;

	[SerializeField] bool buildNorth = true;
	[SerializeField] bool buildEast = true;
	[SerializeField] bool buildSouth = true;
	[SerializeField] bool buildWest = true;
	[SerializeField] bool buildThiccNorth = false;
	[SerializeField] bool buildThiccEast = false;
	[SerializeField] bool buildThiccSouth = false;
	[SerializeField] bool buildThiccWest = false;



	[SerializeField] Vector2Int coordinates = new Vector2Int(0, 0);
	public Vector2Int Coordinates { get { return coordinates; } set { coordinates = value; } }

	bool partOfMaze = false;
	public bool PartOfMaze { get { return partOfMaze; } set { partOfMaze = value; } }

	Vector2Int exploredFrom = new Vector2Int(0,0);
	public Vector2Int ExploredFrom { get { return exploredFrom; } set { exploredFrom = value; } }

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
			thiccNorthWall.SetActive(true);
		else if (buildNorth)
			northWall.SetActive(true);

		if (buildThiccEast)
			thiccEastWall.SetActive(true);
		else if (buildEast)
			eastWall.SetActive(true);

		if (buildThiccSouth)
			thiccSouthWall.SetActive(true);
		else if (buildSouth)
			southWall.SetActive(true);

		if (buildThiccWest)
			thiccWestWall.SetActive(true);
		else if (buildWest)
			westWall.SetActive(true);
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