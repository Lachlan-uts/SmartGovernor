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
		int total = 0;
		int decisionCount = 0;

		Dictionary<Enum,int> factorCounts = new Dictionary<Enum, int> ();

		foreach (var factor in question.factors) {
			factorCounts [factor.Value] = 0;
		}

		foreach (var data in playerDecisionFrequency) {
			total += data.Value;
			if (data.Key.decision == question.decision) {
				decisionCount += data.Value;
				foreach (var factor in question.factors) {
					if (data.Key.factors [factor.Key].Equals (factor.Value)) {
						factorCounts[factor.Value] += 1;
					}
				}
			}
		}
		//calculate probability from counts.
		float p = 1.0f;
		foreach (var factor in factorCounts) {
			p /= decisionCount;
			p *= factor.Value;
		}
		return (decisionCount / total) * p;
	}
}
