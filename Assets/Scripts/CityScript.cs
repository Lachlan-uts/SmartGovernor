using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScript : MonoBehaviour {

	// private variables
	private List<Citizen> Citizens;
	// The Integer value for each citizen denotes the citizen's location in terms of Tiles.
	private GameObject[] Tiles;
	/*
	 * Internal structure of Tiles for representation purposes:
	 * 
	 * 		1	2	3
	 * 		4	0*	5
	 * 		6	7	8
	 * 
	 * Can always restructure this later
	 * Note: 0* denotes the city location relative to it's tiles.
	 */
	private int currentFood; // Temporary variable for future food resource
	private int currentProd; // Temporary variable for future production resource
	private int currentGold; // Temporary variable for future gold accumulation

	private List<Property> Buildings;
	private List<Property> Queue;


	// serialized private fields
	[SerializeField]
	private GameObject BaseTile;

	// void Awake used for purposes of making savestring
	void Awake () {
		Citizens = new List<Citizen>();
		Buildings = new List<Property> ();
		Queue = new List<Property> ();
		Queue.Add (PropertiesList.getList () [0]);
		Tiles = new GameObject[9]; // Eventually will grab Tiles from the map
		// Placeholder
		for ( int i = 0; i < Tiles.Length; i++ ) {
			GameObject Tile = Instantiate (BaseTile, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f)));
			//Debug.Log ("Tile:" + i + "; F:" + Tile.GetComponent<TileScript>().getFood() + "; P:" + Tile.GetComponent<TileScript>().getProduction() + "; G:"
			//	+ Tile.GetComponent<TileScript>().getGold());
			Tiles [i] = Tile;
		}
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
		/*Citizens = new List<int>();
		Buildings = new List<Property> ();
		Queue = new List<Property> ();
		Queue.Add (PropertiesList.getList () [0]);
		Tiles = new GameObject[9]; // Eventually will grab Tiles from the map
		// Placeholder
		for ( int i = 0; i < Tiles.Length; i++ ) {
			GameObject Tile = Instantiate (BaseTile, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f)));
			//Debug.Log ("Tile:" + i + "; F:" + Tile.GetComponent<TileScript>().getFood() + "; P:" + Tile.GetComponent<TileScript>().getProduction() + "; G:"
			//	+ Tile.GetComponent<TileScript>().getGold());
			Tiles [i] = Tile;
		}
		// Placeholder

		int citizen = Random.Range (1, Tiles.Length);
		Citizens.Add (citizen);
		Debug.Log (citizen + ", " + Citizens.Count);

		currentFood = 0;
		currentProd = 0;
		currentGold = 0;
		*/
		NewCitizen ();
		Debug.Log (Citizens[0].getPosition() + ", " + Citizens.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// private methods

	private Citizen getCitizenOnTile(int tileNumber) { // method to find the citizen on a specific tile/position, if there is one
		foreach (Citizen civilian in Citizens) {
			if (civilian.getPosition() == tileNumber) {
				return civilian;
			}
		}

		return null;
	}

	private bool isCitizenOnTile(int tileNumber) { // method to if there is a citizen on a specific tile/position
		foreach (Citizen civilian in Citizens) {
			if (civilian.getPosition() == tileNumber) {
				return true;
			}
		}

		return false;
	}

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
		if (currentFood > FoodRule()) {
			NewCitizen ();
		}
	}

	private int FoodRule() {
		return (int) (Mathf.Exp (1.3f) * (float) Citizens.Count);

	}

	private int getTileValue(int tileNo) {
		return Tiles [tileNo].GetComponent<TileScript> ().getFood ()
			+ Tiles [tileNo].GetComponent<TileScript> ().getProduction ()
			+ Tiles [tileNo].GetComponent<TileScript> ().getGold ();
	}

	// public methods
	public void CityUpdate() {
		currentFood += getTotalFood();
		currentProd += getTotalProd();
		currentGold += getTotalGold();
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
		int nCitizen = 0;
		/* Random placement
		if (Citizens.Count < Tiles.Length - 1) {
			bool passedTest = false;
			while (!passedTest) {
				nCitizen = Random.Range (1, Tiles.Length);
				if (!isCitizenOnTile(nCitizen)) {
					passedTest = true;
				}
			}
		}
		*/

		// Deterministic placement
		if (Citizens.Count < Tiles.Length - 1) {
			int hiTilePos = 1;
			for (int iCount = 1; iCount < Tiles.Length; iCount++) {
				if (getTileValue (iCount) <= getTileValue (hiTilePos)) {
					hiTilePos = iCount;
				}
			}

			for (int iCount = 1; iCount < Tiles.Length; iCount++) {
				//Debug.Log ("icount:" + iCount + "; ictv:" + getTileValue(iCount) + "; htptv:" + getTileValue(hiTilePos));
				if (getTileValue (iCount) > getTileValue (hiTilePos) && !isCitizenOnTile (iCount)) {
					hiTilePos = iCount;
				}
			}
			nCitizen = hiTilePos;
		}


		Citizens.Add (new Citizen(nCitizen));
	}

	public bool MoveCitizen(int oldPos, int newPos) {
		bool done = false;
		if (newPos >= 0 && newPos < Tiles.Length) {
			Citizen civilian;
			if ((civilian = getCitizenOnTile(oldPos)) != null && (!isCitizenOnTile(newPos) || newPos == 0)) {
				civilian.moveTo (newPos);
				done = true;
			}
		}

		return done;
	}

	public bool ReplaceStartOfQueue(string nameOfBuilding) { // returns "true" if the building name was successfully added to queue
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

	public int getTotalFood() { // Note: more of a "get total amount of food produced per turn"
		int totalFood = 0;
		foreach (Citizen citizen in Citizens) {
			if (citizen.getPosition() != 0) {
				totalFood += Tiles [citizen.getPosition()].GetComponent<TileScript> ().getFood ();
				Debug.Log(totalFood);
			}
		}

		foreach (Property Build in Buildings) {
			if (Build.getOutput() == "Food") {
				totalFood += Build.getValue ();
			}
		}

		totalFood += Tiles [0].GetComponent<TileScript> ().getFood ();
		Debug.Log(totalFood);
		totalFood -= Citizens.Count; // food consumed per citizen
		Debug.Log(totalFood);
		return totalFood;
	}

	public int getTotalProd() { // Note: more of a "get total amount of production produced per turn"
		int totalProd = 0;
		foreach (Citizen citizen in Citizens) {
			if (citizen.getPosition() != 0) {
				totalProd += Tiles [citizen.getPosition()].GetComponent<TileScript> ().getProduction ();
			}
		}

		foreach (Property Build in Buildings) {
			if (Build.getOutput() == "Production") {
				totalProd += Build.getValue ();
			}
		}

		totalProd += Tiles [0].GetComponent<TileScript> ().getProduction ();
		return totalProd;
	}

	public int getTotalGold() { // Note: more of a "get total amount of gold produced per turn"
		int totalGold = 0;
		foreach (Citizen citizen in Citizens) {
			if (citizen.getPosition() != 0) {
				totalGold += Tiles [citizen.getPosition()].GetComponent<TileScript> ().getGold ();
			}
		}

		foreach (Property Build in Buildings) {
			if (Build.getOutput () == "Gold") {
				totalGold += Build.getValue ();
			} else {
				totalGold -= 1;
			}
		}

		totalGold += Tiles [0].GetComponent<TileScript> ().getProduction ();
		return totalGold;
	}

	public string getCitizenLocations() {
		string returnString = "";
		foreach (Citizen citizen in Citizens) {
			returnString = returnString + citizen.getPosition() + ", ";
		}

		return returnString;
	}

	public string getTileString() {
		string returnString = "";
		foreach (GameObject Tile in Tiles) { 
			returnString = returnString + "{" + Tile.GetComponent<TileScript>().getFood() + ","
				+ Tile.GetComponent<TileScript>().getProduction() + ","
				+ Tile.GetComponent<TileScript>().getGold() + "}";
			if (Tile != Tiles [Tiles.Length - 1]) {
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
}
