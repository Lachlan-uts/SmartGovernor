using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionScript : MonoBehaviour {

	// private variables
	private List<GameObject> CitiesList;
	private List<GameObject> UnitList;
	[SerializeField]
	private bool ownership;
	private bool hasTurn;

	// Faction AI variables
	private float timeMax = 3.0f;
	private float timeCur = 0.0f;

	// Use this for initialization
	void Start () {
		hasTurn = false;
		CitiesList = new List<GameObject> ();
		UnitList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!ownership && hasTurn) { // Placeholder AI
			endTurn();
			Debug.Log ("TurnEnded");
			//timeCur += Time.deltaTime;
			//if (timeCur >= timeMax) {
			//	timeCur = 0.0f;
			//	endTurn ();
			//}
		}
	}

	public void giveTurn() {
		hasTurn = true;
	}

	public void endTurn() {
		GameObject GameManager = GameObject.FindWithTag ("GameController");
		GameManager.GetComponent<GameManagerScript> ().endTurn (ownership);
		hasTurn = false;
	}

	public void UpdateAssets() {
		foreach (GameObject city in CitiesList) {
			city.GetComponent<CityScriptv2> ().CityUpdate ();
		}


	}

	public void addCity(GameObject newCity) {
		CitiesList.Add (newCity);
	}

	public GameObject removeCity(GameObject oldCity) {
		GameObject remCity;
		if (CitiesList.Contains(oldCity)) {
			remCity = CitiesList [CitiesList.IndexOf (oldCity)];
			CitiesList.Remove (oldCity);
			return remCity;
		}
		return null;
	}

	public void addUnit(GameObject newUnit) {
		UnitList.Add (newUnit);
	}

	public bool getOwnership() {
		return ownership;
	}

}
