using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScriptv2 : MonoBehaviour {

//	// private variables
//	private List<Citizen> Citizens;
//
//	// The Integer value for each citizen denotes the citizen's location in terms of Tiles.
//	private GameObject[] Tiles;
//	/*
//	 * Internal structure of Tiles for representation purposes:
//	 * 
//	 * 		1	2	3
//	 * 		4	0*	5
//	 * 		6	7	8
//	 * 
//	 * Can always restructure this later
//	 * Note: 0* denotes the city location relative to it's tiles.
//	 */
	private int currentFood; // Temporary variable for future food resource
	private int currentProd; // Temporary variable for future production resource
	private int currentGold; // Temporary variable for future gold accumulation
//
//	private int XCoord = -1;
//	private int ZCoord = -1;
//	// Map coordinates

	private List<GameObject> Citizens;
	private List<GameObject> Tiles;

	private List<Property> Buildings;
	private List<Property> Queue;


	private int randomSeed = 10; //For purposes of consistency in testing the AI


	// serialized private fields
	[SerializeField]
	private bool useMap = true; // Set to "false" for the CLI Scene
	[SerializeField]
	private GameObject BaseTile;



	// void Awake used for purposes of making savestring
	void Awake () {
//		Citizens = new List<Citizen> ();
//		Tiles = new GameObject[9];
		Buildings = new List<Property> ();
		Queue = new List<Property> ();
		Queue.Add (PropertiesList.getList () [0]);
		Citizens = new List<GameObject> ();
		Tiles = GetComponentInParent<MapGenerationScript> ().getSquareRadius (this.transform.parent.gameObject, 1);
		// Placeholder

		//int cPos = Random.Range (1, Tiles.Length);
		//Citizens.Add (new Citizen(cPos));
		//NewCitizen ();

		currentFood = 0;
		currentProd = 0;
		currentGold = 0;

	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	public void establish() {
		NewCitizen ();
	}

	/* What do we want to be able to do with citizens?
	 * 1) Be able to assign them to tiles
	 * 2) Move them between tiles
	 * 3) Get their resources
	 * 4) Create new citizens
	 */

	// private methods

	private void AttemptConstruction() {
		if (currentProd >= Queue [0].getCost ()) {
			currentProd -= Queue [0].getCost ();
			NewBuilding (Queue [0]);
			Queue.RemoveAt (0);
			if (Queue.Count == 0) {
				Queue.Add (PropertiesList.getList () [0]);
			}
		}
	}

	private void AttemptNewCitizen() {
		if (currentFood > FoodRule())
			NewCitizen ();
	}

	private int FoodRule() {
		return (int) (Mathf.Exp (1.3f) * (float) Citizens.Count);
	}

	// public methods
	public void CityUpdate() {
		currentFood += getTotalResource ("Food");
		currentFood -= Citizens.Count;
		currentProd += getTotalResource ("Production");
		currentGold += getTotalResource ("Gold");
		AttemptConstruction ();
		AttemptNewCitizen ();
	}

	public void NewBuilding(Property Build) {
		if (!Build.getUse ()) {
			Buildings.Add (Build);
		} else { // Since this can currently only be used for "coinage", might as well put in the gold until we better develop this
			currentGold += (int) ((1.0 * currentProd) / 2.0);
			currentProd = 0;
		}
	}

	public void NewBuilding(string nameOfBuilding) {
		foreach (Property Build in PropertiesList.properties) {
			if (Build.getName () == nameOfBuilding) {
				if (!Build.getUse ()) {
					Buildings.Add (Build);
				} else { // Since this can currently only be used for "coinage", might as well put in the gold until we better develop this
					currentGold += (int) ((1.0 * currentProd) / 2.0);
					currentProd = 0;
				}
			}
		}
	}

	public void NewCitizen() { // <--- This Function right here behaves interestingly
		
		GameObject chosenParent = this.transform.gameObject;
		// Deterministic placement
		if (Citizens.Count < Tiles.Count) {
			chosenParent = Tiles [0];
			foreach (GameObject currentTile in Tiles) {
				if (chosenParent.GetComponent<TileScriptv2> ().getTileValue () < currentTile.GetComponent<TileScriptv2> ().getTileValue ())
					chosenParent = currentTile;
			}
		}
		GameObject newCitizen = Instantiate (Resources.Load ("Citizen"), chosenParent.transform.position, Quaternion.identity, chosenParent.transform) as GameObject;
		newCitizen.name = this.name + " Citizen " + (Citizens.Count + 1).ToString ();
		Citizens.Add (newCitizen);
	}

//	public bool MoveCitizen(int oldPos, int newPos) {
//		bool done = false;
//		if (newPos >= 0 && newPos < Tiles.Length) {
//			Citizen civilian;
//			if ((civilian = getCitizenOnTile(oldPos)) != null && (!isCitizenOnTile(newPos) || newPos == 0)) {
//				civilian.moveTo (newPos);
//				done = true;
//			}
//		}
//
//		return done;
//	}

	public bool ReplaceStartOfQueue(string nameOfBuilding) { // returns "true" if the building name was successfully added to queue
		//if (Queue.Count == 1) // for discussion later
		//	
		bool addedToQueue = false;
		foreach (Property Build in PropertiesList.properties) {
			if (Build.getName () == nameOfBuilding) {
				addedToQueue = true;
				Queue.RemoveAt (0);
				Queue.Insert (0, Build);
			}
		}
		return addedToQueue;
	}

	public bool AddToQueue(string nameOfBuilding) { // returns "true" if the building name was successfully added to queue
		bool addedToQueue = false;
		foreach (Property Build in PropertiesList.properties) {
			if (Build.getName () == nameOfBuilding) {
				addedToQueue = true;
				Queue.Add (Build);
			}
		}
		return addedToQueue;
	}

	public void RemoveFromQueue() {
		Queue.RemoveAt (Queue.Count-1);
		if (Queue.Count == 0) {
			Queue.Add (PropertiesList.getList () [0]);
		}
	}

	public int getTotalResource(string resource) {
		int resourceCount = this.GetComponentInParent<TileScriptv2>().getResource(resource).getValue();
		foreach (GameObject citizen in Citizens)
			resourceCount += citizen.GetComponent<CitizenScript> ().getResources () [resource];

		foreach (Property Build in Buildings) {
			if (Build.getOutput() == resource) {
				resourceCount += Build.getValue ();
			}
		}
		return resourceCount;
	}

//	public string getCitizenLocations() {
//		string returnString = "";
//		foreach (GameObject citizen in Citizens) {
//			returnString = returnString + citizen.GetComponentInParent<TileScriptv2>().getXCoord() + ", ";
//		}
//
//		return returnString;
//	}

	public string getTileString() {
		string returnString = "";
		foreach (GameObject Tile in Tiles) { 
			returnString = returnString + "{" + Tile.GetComponent<TileScriptv2>().getFood() + ","
				+ Tile.GetComponent<TileScriptv2>().getProduction() + ","
				+ Tile.GetComponent<TileScriptv2>().getGold() + "}";
			if (Tile != Tiles [Tiles.Count - 1]) {
				returnString += ".";
			}
		}


		return returnString;
	}

	public List<Property> getQueue() {
		return Queue;
	}

	public List<Property> getBuildings() {
		return Buildings;
	}

	public int getCurrentFood() {
		return currentFood;
	}

	public int getCurrentProd() {
		return currentProd;
	}

	public int getCurrentGold() {
		return currentGold;
	}

	public GameObject getTileAtOrigin() {
		return Tiles [0];
	}

	public int getXCoord() {
		return GetComponentInParent<TileScriptv2> ().getXCoord ();
	}

	public int getZCoord() {
		return GetComponentInParent<TileScriptv2> ().getZCoord ();
	}
}
