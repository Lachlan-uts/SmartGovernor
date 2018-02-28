using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CityGovernorScript : MonoBehaviour {

	//Better data storage
	//public static Dictionary<Buildables,List<DecisionData>> playerBuildDecisions = new Dictionary<Buildables, List<DecisionData>> ();

	//Frequency Dataset
	public static Dictionary<DecisionData, int> playerDecisionFrequency = new Dictionary<DecisionData, int>();

	public CityScriptv2 attachedCity { get; private set; }


	void Awake() {
		attachedCity = this.gameObject.GetComponent<CityScriptv2> ();
	}

	void Update() {
	}

	public void governorActions() {
		float cityBuild = calculateNaiveBayes (getQuestion());
		Debug.Log("The probability of building a city is." + cityBuild);
		if (cityBuild >= UnityEngine.Random.Range (0.0f, 1.0f)) {
			//trigger build city
			Debug.Log("The city build was triggered");
			Debug.Log("The city build was triggered");
		}
	}

	private DecisionData getQuestion() {
		//currently hardcoded to a single question, but could be easily adjusted in future.
		return new DecisionData (true, attachedCity.Citizens.Count, GameManagerScript.turnNumber);
	}

	private float calculateNaiveBayes(DecisionData question) {
		int decisionCount = 0;
		int total = 0;

		Dictionary<Enum,int> factorCounts = new Dictionary<Enum, int> ();
		Dictionary<Enum,int> factorTotals = new Dictionary<Enum, int> ();

		Dictionary<Enum,Dictionary<string,int>> factorData = new Dictionary<Enum, Dictionary<string, int>> ();

		foreach (var factor in question.factors) {
			factorCounts [factor.Value] = 0;
			factorTotals [factor.Value] = 0;
			//factorData [factor.Value] ["Count"] = 0;
			//factorData [factor.Value] ["Total"] = 0;
		}
			

		foreach (var data in playerDecisionFrequency) {
			total += data.Value;
			foreach (var factorT in question.factors) {
				if (data.Key.factors [factorT.Key].Equals (factorT.Value)) {
					factorTotals [factorT.Value] += data.Value;
				}
			}
			if (data.Key.decision == question.decision) {
				decisionCount += data.Value;
				foreach (var factor in question.factors) {
					if (data.Key.factors [factor.Key].Equals (factor.Value)) {
						factorCounts[factor.Value] += data.Value;
					}
				}
			}
		}
		Debug.Log ("The total is " + total);
		Debug.Log ("Decision count is " + decisionCount);
		//calculate probability from counts.
		float p = 1.0f;
		float fP = 1.0f;
		foreach (var factor in factorCounts) {
			Debug.Log (factor.Key.ToString () + ": " + factor.Value);
			Debug.Log (factor.Key.ToString () + " total: " + factorTotals[factor.Key]);
			p /= factorTotals[factor.Key];
			p *= factor.Value;
			Debug.Log ("p now is " + p);
			fP /= total;
			fP *= factorTotals[factor.Key];

		}
		Debug.Log ("The final value should be " + ((float)decisionCount / (float)total) + " * " + p + " * 1/" + fP);
		return ( (float)decisionCount / (float)total * p * (1.0f/fP));
	}
}
