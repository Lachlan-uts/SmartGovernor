using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CityGovernorScript : MonoBehaviour {

	//Player decision Dataset
	public static List<DecisionData> playerDecisions = new List<DecisionData> ();

	// Use this for initialization
	void Start () {
		//cityDecisions = new List<DecisionData> ();
	}

	public void updateDataset() {
		foreach (GameObject City in MapGenerationScript.cityList) {
			if (!City.GetComponent<CityScriptv2> ().getGovernorStatus ()) {
				playerDecisions.AddRange (City.GetComponent<CityScriptv2> ().getDecisionData ());
			}
		}
	}

}
