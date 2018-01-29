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

	private void setCoords(int newX, int newZ) {
		xCoord = newX;
		zCoord = newZ;
	}

	private void pathfind(int goalXCoord, int goalZCoord) {
		GameObject Map = GameObject.FindGameObjectWithTag ("Map");
		List<GameObject> closedSet = new List<GameObject> ();

		List<GameObject> openSet = new List<GameObject> ();
		openSet.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (xCoord, zCoord));

		// Setup the cameFrom list here

		int xSize = Map.GetComponent<MapGenerationScript> ().getXSize ();
		int zSize = Map.GetComponent<MapGenerationScript> ().getZSize ();

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

		cScore [xCoord, zCoord] = heuristicCostEstimate(xCoord, zCoord); // Estimated cost of getting to the goal

		while (openSet.Count != 0) {
			GameObject current = openSet[0]; // Need to change this such that the algorithm grabs the tile with the lowest cScore assosciated

			closedSet.Add (current);
			openSet.Remove (current);

			List<GameObject> neighbours = new List<GameObject>();

			if (current.GetComponent<TileScript> ().getXCoord () > 0) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScript> ().getXCoord () - 1, 
					current.GetComponent<TileScript> ().getZCoord ()));
			}

			if (current.GetComponent<TileScript> ().getZCoord () > 0) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScript> ().getXCoord (), 
					current.GetComponent<TileScript> ().getZCoord () - 1));
			}

			if (current.GetComponent<TileScript> ().getXCoord () < xSize) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScript> ().getXCoord () + 1, 
					current.GetComponent<TileScript> ().getZCoord ()));
			}

			if (current.GetComponent<TileScript> ().getZCoord () < zSize) {
				neighbours.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (current.GetComponent<TileScript> ().getXCoord (), 
					current.GetComponent<TileScript> ().getZCoord () + 1));
			}

			foreach (GameObject neighbour in neighbours) {
				if (closedSet.Contains (neighbour)) {
					continue;
				}

				if (!openSet.Contains (neighbour)) {
					openSet.Add (neighbour);
				}

				int tentativeCScore = cScore [current.GetComponent<TileScript> ().getXCoord (), current.GetComponent<TileScript> ().getZCoord ()] + 1;
				if (tentativeCScore >= cScore [neighbour.GetComponent<TileScript> ().getXCoord (), neighbour.GetComponent<TileScript> ().getZCoord ()]) {
					continue;
				}

				// Continue the neighbour chain logic here

			}

		}

	}



	private int heuristicCostEstimate(int tileXCoord, int tileZCoord) {
		int HCE = 0;

		int MovementCostToTile = Mathf.Abs (xCoord - tileXCoord) + Mathf.Abs (zCoord - tileZCoord);

		int MovementCostToGoal = 3; // Estimate

		HCE = MovementCostToTile + MovementCostToGoal;
		return HCE;
	}

}
