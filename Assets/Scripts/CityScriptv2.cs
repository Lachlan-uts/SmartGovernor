using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScriptv2 : MonoBehaviour {


	private int currentFood; // Temporary variable for future food resource
	private int currentProd; // Temporary variable for future production resource
	private int currentGold; // Temporary variable for future gold accumulation

	private List<GameObject> Citizens;
	private List<GameObject> Tiles;
	private List<GameObject> availableTiles;

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
		Buildings = new List<Property> ();
		Queue = new List<Property> ();
		Queue.Add (PropertiesList.getList () [0]);
		Citizens = new List<GameObject> ();
		Tiles = GetComponentInParent<MapGenerationScript> ().getSquareRadius (this.transform.parent.gameObject, 1);
		availableTiles = new List<GameObject> (Tiles);

		currentFood = 0;
		currentProd = 0;
		currentGold = 0;

	}

	// Use this for initialization
	void Start () {
		establish ();
	}

	// Update is called once per frame
	void Update () {
	}

	public void establish() {
		for (int i = 0; i < 5; i++) {
			NewCitizen ();
		}
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

	private void removeCitizen() {
		GameObject citizen = Citizens [Citizens.Count - 1];
		Citizens.Remove (citizen);
		Debug.Log (this.transform);
		Debug.Log (citizen.transform.parent);
		if (!citizen.transform.parent.Equals(this.transform)) {
			Debug.Log ("the tile was added.");
			availableTiles.Add (citizen.transform.parent.gameObject);
		}
		Destroy (citizen);
	}

	private void AttemptNewCitizen() {
		if (currentFood > FoodRule())
			NewCitizen ();
	}

	private int FoodRule() {
		return (int) (Mathf.Exp (1.3f) * (float) Citizens.Count);
	}

	private GameObject getTile(int x, int z) {
		GameObject attemptTile = GetComponentInParent<MapGenerationScript> ().getTileAt (x, z);
		if (attemptTile == getTileAtOrigin() || Tiles.Contains (attemptTile))
			return attemptTile;
		return null;
	}

	// public methods

	public void CityUpdate() {
		currentFood += getTotalResource ("Food");
		currentFood -= Citizens.Count;
		currentProd += getTotalResource ("Production");
		currentGold += getTotalResource ("Gold");
		AttemptNewCitizen ();
		AttemptConstruction ();
	}

	public void NewBuilding(Property Build) {
		if (!Build.getUse ()) {
			Buildings.Add (Build);
		} else { // Since this can currently only be used for "coinage", might as well put in the gold until we better develop this
			Debug.Log(Build.getUnitName());
			if (Build.getUnitName() == "") {
				currentGold += (int)((1.0 * currentProd) / 2.0);
				currentProd = 0;
			} else if (Build.getUnitName ().Contains("newCity")) {
				Debug.Log ("reached the city build.");
				GetComponentInParent<MapGenerationScript> ().expansionBuildCity (this.gameObject);
				//GetComponentInParent<MapGenerationScript> ().buildCity (Build.coordX, Build.coordZ);
				removeCitizen ();
			} else {
				GameObject newUnit = Resources.Load<GameObject> (Build.getUnitName());
				Instantiate (newUnit, this.transform);
			}
		}
	}

	public void NewBuilding(string nameOfBuilding) {
		foreach (Property Build in PropertiesList.properties) {
			if (Build.getName () == nameOfBuilding) {
				if (!Build.getUse ()) {
					Buildings.Add (Build);
				} else { // Since this can currently only be used for "coinage", might as well put in the gold until we better develop this
					if (Build.getUnitName() == "") {
						currentGold += (int)((1.0 * currentProd) / 2.0);
						currentProd = 0;
					} else {
						GameObject newUnit = Resources.Load<GameObject> (Build.getUnitName());
						Instantiate (newUnit, this.transform);
					}
				}
			}
		}
	}

	public void NewCitizen() { // <--- This Function right here behaves interestingly
		GameObject chosenParent = this.transform.gameObject;
		// Deterministic placement
		Debug.Log (availableTiles.Count);
		if (availableTiles.Count > 0) {
			chosenParent = availableTiles [0];
			foreach (GameObject currentTile in availableTiles) {
				if (chosenParent.GetComponent<TileScriptv2> ().getTileValue () < currentTile.GetComponent<TileScriptv2> ().getTileValue ())
					chosenParent = currentTile;
			}
			availableTiles.Remove (chosenParent);
		}
		GameObject newCitizen = Instantiate (Resources.Load ("Citizen"), chosenParent.transform.position, Quaternion.identity, chosenParent.transform) as GameObject;
		newCitizen.name = this.name + " Citizen " + (Citizens.Count + 1).ToString ();
		Citizens.Add (newCitizen);
	}

	public bool MoveCitizen(int oldX, int oldZ, int newX, int newZ) {
		GameObject tileOld = getTile (oldX, oldZ);
		GameObject tileNew = getTile (newX, newZ);

		if (!availableTiles.Contains (tileOld) && availableTiles.Contains(tileNew)) {
//			GameObject citizen = tileOld.GetComponentInChildren<CitizenScript> ();
			tileOld.GetComponentInChildren<CitizenScript> ().transform.SetParent (tileNew.transform, false);
			availableTiles.Add (tileOld);
			availableTiles.Remove (tileNew);
			return true;
		}
		if (getTileAtOrigin() == tileOld && availableTiles.Contains (tileNew)) {
			(Citizens.Find (citizen => citizen.transform.parent == this.transform)).transform.SetParent (tileNew.transform, false);
			availableTiles.Remove (tileNew);
			return true;
		}
		if (getTileAtOrigin() == tileNew) {
			tileOld.GetComponentInChildren<CitizenScript> ().transform.SetParent (this.transform, false);
			availableTiles.Add (tileOld);
			return true;
		}
		return false;
	}

//	public bool MoveCitizen(int xCoord, int zCoord) {
//
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

	public bool ReplaceStartOfQueue(string nameOfBuilding, int xCoord, int zCoord) { // returns "true" if the building name was successfully added to queue
		//if (Queue.Count == 1) // for discussion later
		//	
		bool addedToQueue = false;
		foreach (Property Build in PropertiesList.properties) {
			if (Build.getName () == nameOfBuilding) {
				addedToQueue = true;
				Property buildCopy = Build;
				buildCopy.coordX = xCoord;
				buildCopy.coordZ = zCoord;
				Queue.RemoveAt (0);
				Queue.Insert (0, buildCopy);
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

	public bool AddToQueue(string nameOfBuilding, int xCoord, int zCoord) { // returns "true" if the building name was successfully added to queue
		bool addedToQueue = false;
		foreach (Property Build in PropertiesList.properties) {
			if (Build.getName () == nameOfBuilding) {
				addedToQueue = true;
				Property buildCopy = Build;
				buildCopy.coordX = xCoord;
				buildCopy.coordZ = zCoord;
				Queue.Add (buildCopy);
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

	public string getCitizenLocations() {
		string returnString = "";
		foreach (GameObject citizen in Citizens) {
			returnString = returnString + "[" + citizen.GetComponentInParent<TileScriptv2>().getXCoord() + "," + citizen.GetComponentInParent<TileScriptv2>().getYCoord() + "]";
		}
		return returnString;
	}

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

	public int getXCoord() {
		return GetComponentInParent<TileScriptv2> ().getXCoord ();
	}

	public int getZCoord() {
		return GetComponentInParent<TileScriptv2> ().getZCoord ();
	}

	public GameObject getTileAtOrigin() {
		return this.transform.parent.gameObject;
	}
}
