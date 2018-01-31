using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

	public int turnNumber;
	private int turnPhase;
	private List<GameObject> FactionManagers;
	private GameObject GUI;

	// Use this for initialization
	void Start () {
		FactionManagers = new List<GameObject> ();
		/*
		foreach (GameObject faction in GameObject.FindGameObjectsWithTag("Faction")) {
			FactionManagers.Add (faction);
		}
		GUI = GameObject.FindWithTag("GUI");
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (!GUI) {
			foreach (GameObject faction in GameObject.FindGameObjectsWithTag("Faction")) {
				FactionManagers.Add (faction);
			}
			GUI = GameObject.FindWithTag("GUI");
		}
	}

	public void endTurn(bool ownership) {
		


		// Upkeep phase coding here



		turnPhase++;


		// End of 'turn' coding here
		if (turnPhase >= FactionManagers.Count) {
			foreach (GameObject faction in FactionManagers) {
				faction.GetComponent<FactionScript> ().UpdateAssets ();
			}
			turnPhase = 0;
			turnNumber++;
		}

		// End of faction turn actions stuff here
		//if (ownership) {
		//	GUI.SetActive (false);
		//} else {
		//	GUI.SetActive (true);
		//}
		FactionManagers [turnPhase].GetComponent<FactionScript> ().giveTurn ();

	}

	// Planned turn structure (for a player and bot)
	/* - Initial
	 * Map Generation
	 * City Generation
	 * Factions added to GameManager for local reference
	 * 
	 * - Turns
	 * Player 1 Turn -> receive message of end turn
	 * Upkeep...?
	 * Player 2 Turn -> receive message of end turn
	 * Upkeep...?
	 * ResetTurnPhase, increment TurnNumber
	 */

	public void newCityMade(GameObject newCity, bool ownership) {
		foreach (GameObject faction in FactionManagers) {
			if (faction.GetComponent<FactionScript> ().getOwnership () == ownership) {
				faction.GetComponent<FactionScript> ().addCity (newCity);
			}
		}
	}
}
