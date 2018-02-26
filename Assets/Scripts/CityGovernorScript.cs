using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CityGovernorScript : MonoBehaviour {

	//Better data storage
	//public static Dictionary<Buildables,List<DecisionData>> playerBuildDecisions = new Dictionary<Buildables, List<DecisionData>> ();

	//Frequency Dataset
	public static Dictionary<DecisionData, int> playerDecisionFrequency = new Dictionary<DecisionData, int>();

}
