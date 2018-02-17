using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

	private List<GameObject> TileList; // List of tiles to be pathfound through

	private int xCoord, zCoord;

	[SerializeField]
	private int movement = 1; 



	// Use this for initialization
	void Start () {
		xCoord = -1;
		zCoord = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCoords(int newX, int newZ) {
		xCoord = newX;
		zCoord = newZ;
	}

	private List<GameObject> pathfind(int goalXCoord, int goalZCoord) {
		GameObject Map = GameObject.FindGameObjectWithTag ("Map");
		List<GameObject> closedSet = new List<GameObject> ();

		List<GameObject> openSet = new List<GameObject> ();
		openSet.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (xCoord, zCoord));

		int xSize = Map.GetComponent<MapGenerationScript> ().getXSize ();
		int zSize = Map.GetComponent<MapGenerationScript> ().getZSize ();

		// Setup the cameFrom list here
		GameObject[,] cameFromList = new GameObject[xSize, zSize];

		int[,] pScore = new int[xSize, zSize];
		for (int xCount = 0; xCount < xSize; xCount++) {
			for (int zCount = 0; zCount < zSize; zCount++) {
				pScore [xCount, zCount] = 1000; // Equivalent of infinite for the purposes of pathfinding
			}
		}

		pScore [xCoord, zCoord] = 0; // Cost of going start to start is 0


		int[,] cScore = new int[xSize, zSize];
		for (int xCount = 0; xCount < xSize; xCount++) {
			for (int zCount = 0; zCount < zSize; zCount++) {
				cScore [xCount, zCount] = 1000; // Equivalent of infinite for the purposes of pathfinding
			}
		}

		cScore [xCoord, zCoord] = heuristicCostEstimate(xCoord, zCoord, goalXCoord, goalZCoord); // Estimated cost of getting to the goal

		while (openSet.Count != 0) {
			 // Need to change this such that the algorithm grabs the tile with the lowest cScore assosciated

			int lowestCScorePos = 0;
			for (int LCSCount = 1; LCSCount < openSet.Count - 1; LCSCount++) { // No need to compare the first openSet item to itself
				int lowX = openSet [lowestCScorePos].GetComponent<TileScriptv2> ().getXCoord();
				int lowZ = openSet [lowestCScorePos].GetComponent<TileScriptv2> ().getZCoord ();
				int comX = openSet [LCSCount].GetComponent<TileScriptv2> ().getXCoord ();
				int comZ = openSet [LCSCount].GetComponent<TileScriptv2> ().getZCoord ();
				if (cScore [lowX, lowZ] > cScore [comX, comZ]) {
					lowestCScorePos = LCSCount;
				}
			}

			GameObject current = openSet[lowestCScorePos];
			Debug.Log (current.GetComponent<TileScriptv2> ().getXCoord () + "," + current.GetComponent<TileScriptv2> ().getZCoord ());

			int curX = current.GetComponent<TileScriptv2> ().getXCoord ();
			int curZ = current.GetComponent<TileScriptv2> ().getZCoord ();
			if (curX == goalXCoord && curZ == goalZCoord) {
				// Return a reconstructed path based upon the cameFromList
				return reconstructPath(cameFromList, current);
			}

			closedSet.Add (current);
			openSet.Remove (current);

			List<GameObject> neighbours = new List<GameObject>();

			if (current.GetComponent<TileScriptv2> ().getXCoord () > 0) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScriptv2> ().getXCoord () - 1, 
					current.GetComponent<TileScriptv2> ().getZCoord ()));
			}

			if (current.GetComponent<TileScriptv2> ().getZCoord () > 0) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScriptv2> ().getXCoord (), 
					current.GetComponent<TileScriptv2> ().getZCoord () - 1));
			}

			if (current.GetComponent<TileScriptv2> ().getXCoord () < xSize) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScriptv2> ().getXCoord () + 1, 
					current.GetComponent<TileScriptv2> ().getZCoord ()));
			}

			if (current.GetComponent<TileScriptv2> ().getZCoord () < zSize) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScriptv2> ().getXCoord (), 
					current.GetComponent<TileScriptv2> ().getZCoord () + 1));
			}

			foreach (GameObject neighbour in neighbours) {
				if (closedSet.Contains (neighbour)) {
					continue;
				}

				if (!openSet.Contains (neighbour)) {
					openSet.Add (neighbour);
				}

				int tentativeCScore = cScore [current.GetComponent<TileScriptv2> ().getXCoord (), current.GetComponent<TileScriptv2> ().getZCoord ()] + 1;
				if (tentativeCScore >= cScore [neighbour.GetComponent<TileScriptv2> ().getXCoord (), neighbour.GetComponent<TileScriptv2> ().getZCoord ()]) {
					continue;
				}

				// Continue the neighbour chain logic here
				// cameFrom list here
				cameFromList[neighbour.GetComponent<TileScriptv2>().getXCoord(), neighbour.GetComponent<TileScriptv2>().getZCoord()] = current;
				pScore[neighbour.GetComponent<TileScriptv2>().getXCoord(), neighbour.GetComponent<TileScriptv2>().getZCoord()] = tentativeCScore;
				cScore [neighbour.GetComponent<TileScriptv2> ().getXCoord (), neighbour.GetComponent<TileScriptv2> ().getZCoord ()] 
				= pScore [neighbour.GetComponent<TileScriptv2> ().getXCoord (), neighbour.GetComponent<TileScriptv2> ().getZCoord ()]
				+ heuristicCostEstimate (neighbour.GetComponent<TileScriptv2> ().getXCoord (), neighbour.GetComponent<TileScriptv2> ().getZCoord (),
					goalXCoord, goalZCoord);

			}

		}

		return null;

	}

	private List<GameObject> reconstructPath(GameObject[,] cameFromList, GameObject current) {
		GameObject tCurrent = current;
		List<GameObject> totalPath = new List<GameObject> ();
		totalPath.Add (tCurrent);
		while (arrayContains (cameFromList, tCurrent)) {
			tCurrent = cameFromList [tCurrent.GetComponent<TileScriptv2> ().getXCoord (), tCurrent.GetComponent<TileScriptv2> ().getZCoord ()];
			totalPath.Add (tCurrent);
		}

		return totalPath;
	}

	private int heuristicCostEstimate(int tileXCoord, int tileZCoord) {
		int HCE = 0;

		int MovementCostToTile = Mathf.Abs (xCoord - tileXCoord) + Mathf.Abs (zCoord - tileZCoord);

		int MovementCostToGoal = 3; // Estimate

		HCE = MovementCostToTile + MovementCostToGoal;
		return HCE;
	}

	private int heuristicCostEstimate(int tileXCoord, int tileZCoord, int goalXCoord, int goalZCoord) {
		int HCE = 0;

		int MovementCostToTile = Mathf.Abs (xCoord - tileXCoord) + Mathf.Abs (zCoord - tileZCoord);

		int MovementCostToGoal = Mathf.Abs (goalXCoord - tileXCoord) + Mathf.Abs (goalZCoord - tileZCoord);

		HCE = MovementCostToTile + MovementCostToGoal;
		return HCE;
	}

	private bool arrayContains(GameObject[,] Array, GameObject Item) {
		for (int xCount = 0; xCount < Array.GetLength (0); xCount++) {
			for (int zCount = 0; zCount < Array.GetLength (1); zCount++) {
				if (Item.Equals (Array [xCount, zCount])) {
					return true;
				}
			}
		}

		return false;
	}

	public void moveTo(int newX, int newZ) {
		Debug.Log ("Acting on order");
		TileList = pathfind (newX, newZ);
		string debugString = "";
		if (TileList != null) {
			foreach (GameObject Tile in TileList) {
				debugString = debugString + "(" + Tile.GetComponent<TileScriptv2> ().getXCoord () + "," + Tile.GetComponent<TileScriptv2> ().getZCoord () + "),";
			}
		}
		Debug.Log (debugString);

	}
}
