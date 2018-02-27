using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CityScriptv2 : MonoBehaviour {

	private int currentFood; // Temporary variable for future food resource
	private int currentProd; // Temporary variable for future production resource
	private int currentGold; // Temporary variable for future gold accumulation

	public List<GameObject> Citizens { get; private set; }


	private List<GameObject> Tiles;
	private List<GameObject> availableTiles;

	private List<Property> Buildings;
	private List<Property> Queue;

	//List of turn actions
	private List<DecisionData> Decisions;

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
		Queue.Add (PropertiesList.dictProperties[Buildables.Coinage]);
		//Queue.Add (PropertiesList.getList () [0]);
		Citizens = new List<GameObject> ();
		Tiles = GetComponentInParent<MapGenerationScript> ().getSquareRadius (this.transform.parent.gameObject, 1);
		availableTiles = new List<GameObject> (Tiles);

		currentFood = 0;
		currentProd = 0;
		currentGold = 0;

		//and the player decisions should start empty
		Decisions = new List<DecisionData> ();
	}

	// Use this for initialization
	void Start () {
		establish ();
	}

	// Update is called once per frame
	void Update () {
	}

	public void establish() {
		for (int i = 0; i < 1; i++) {
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
		if (!citizen.transform.parent.Equals(this.transform)) {
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

	//Should update the list of player actions when there is no governor enabled on this city.
	private void updatePlayerActions() {
		if (!this.gameObject.GetComponent<CityGovernorScript> ().isActiveAndEnabled) {
			if (Decisions.Count == 0) {
				Decisions.Add (new DecisionData (false, this.Citizens.Count, GameManagerScript.turnNumber));
				Decisions.Add (new DecisionData (false, this.Citizens.Count, GameManagerScript.turnNumber));
				Debug.Log (Decisions [0].Equals (Decisions [1]));
//
//				Dictionary<EnumCategories,int> passedEnums = new Dictionary<EnumCategories, int> {
//					{ EnumCategories.CitizenCount, Citizens.Count },
//					{ EnumCategories.TurnCount, GameManagerScript.turnNumber }
//				};
//				Decisions.Add (new DecisionData (Buildables.Newcity, false, passedEnums));
			}
//			CityGovernorScript.playerDecisions.AddRange (Decisions);

			foreach (DecisionData decision in Decisions) {
				int currentCount;
				Debug.Log (CityGovernorScript.playerDecisionFrequency.ContainsKey (decision));
				CityGovernorScript.playerDecisionFrequency.TryGetValue(decision, out currentCount);
				CityGovernorScript.playerDecisionFrequency [decision] = currentCount + 1;
			}

			//add directly to a frequency dictionary

			Debug.Log (string.Join(" ; ", CityGovernorScript.playerDecisionFrequency.Keys));
		}
	}

	private void governorActions() {
		if (this.gameObject.GetComponent<CityGovernorScript> ().isActiveAndEnabled) {
			Debug.Log ("Governor enabled!");
			this.gameObject.GetComponent<CityGovernorScript> ().governorActions ();
		}
	}

	// public methods

	public void CityUpdate() {
		/*
		 * Need a method to pass negative events (turns where a user decided not to add a city to the queue or anything else that might be situationally helpful)
		 */
		updatePlayerActions ();
		governorActions ();
		currentFood += getTotalResource ("Food");
		currentFood -= Citizens.Count;
		currentProd += getTotalResource ("Production");
		currentGold += getTotalResource ("Gold");
		AttemptNewCitizen ();
		AttemptConstruction ();
		Decisions.Clear ();
	}

	public void NewBuilding(Property Build) {
		if (!Build.getUse ()) {
			Buildings.Add (Build);
		} else { // Since this can currently only be used for "coinage", might as well put in the gold until we better develop this
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

	public void NewCitizen() { // <--- This Function right here behaves interestingly
		GameObject chosenParent = this.transform.gameObject;
		// Deterministic placement
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

	public bool AddToQueue(Buildables buildy) {
		Property attemptedProp = null;
		if (PropertiesList.dictProperties.TryGetValue (buildy, out attemptedProp)) {
			if (buildy == Buildables.Newcity) {
				/*
				 * Pass relevent information here.
				 * This included but is not limited to:
				 * Citizen count
				 * Food production rate
				 * Food rule
				 */
				Decisions.Add(new DecisionData(true, this.Citizens.Count, GameManagerScript.turnNumber));
			}
			Queue.Add (attemptedProp);
			return true;
		}
		return false;
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

	//All public get methods

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

	//Give the A.I player data
	public List<DecisionData> getDecisionData() {
		return Decisions;
	}

	public bool getGovernorStatus() {
		return this.gameObject.GetComponent<CityGovernorScript> ().isActiveAndEnabled;
	}

	//public toggle methods
	public bool toggleGovernor() {
		Debug.Log (this.gameObject.GetComponent<CityGovernorScript> ().enabled);
		return this.gameObject.GetComponent<CityGovernorScript> ().enabled = !this.gameObject.GetComponent<CityGovernorScript> ().isActiveAndEnabled;
	}

	//User action and information store method(s)
//	public T storePlayerAction<T>(T input) {
//		
//	}

}
