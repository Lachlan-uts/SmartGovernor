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

	private void pathfind() {
		GameObject Map = GameObject.FindGameObjectWithTag ("Map");
		List<GameObject> closedSet = new List<GameObject> ();

		List<GameObject> openSet = new List<GameObject> ();
		openSet.Add (Map.GetComponent<MapGenerationScript> ().getTileAt (xCoord, zCoord));



	}


}
