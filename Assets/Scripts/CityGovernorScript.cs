using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CityGovernorScript : MonoBehaviour {

	//Better data storage
	//public static Dictionary<Buildables,List<DecisionData>> playerBuildDecisions = new Dictionary<Buildables, List<DecisionData>> ();

	//Player decision Dataset
	public static List<DecisionData> playerDecisions = new List<DecisionData> ();

	//Frequency Dataset
	public static Dictionary<DecisionData, int> playerDecisionFrequency = new Dictionary<DecisionData, int>();

	// Use this for initialization
	void Start () {
		//cityDecisions = new List<DecisionData> ();
	}

	//Naive Bayes stuff
	private int frequencyCalculator(bool choice) {
		return (playerDecisions.FindAll (dec => dec.getDecision().Equals (choice))).Count;
	}

	private int frequencyCalculator(CitizenCount cCount, TurnCount tCount ) {
		return (playerDecisions.FindAll (dec => dec.getCitizenCount().Equals (cCount) && dec.getTurnCount().Equals (tCount))).Count;
	}

//	private int naiveProbability(DecisionData choice) {
//
//	}

	public void updateDataset() {
		foreach (GameObject City in MapGenerationScript.cityList) {
			if (!City.GetComponent<CityScriptv2> ().getGovernorStatus ()) {
				playerDecisions.AddRange (City.GetComponent<CityScriptv2> ().getDecisionData ());
			}
		}
	}

	//Idea to build a better data structure with frequency easily accessable.
	//		foreach (DecisionData decision in playerDecisions) {
	//			int currentCount;
	//			playerDecisionFrequency.TryGetValue(decision, out currentCount);
	//			playerDecisionFrequency [decision] = currentCount + 1;
	//		}

}
