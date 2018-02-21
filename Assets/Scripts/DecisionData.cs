using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

//For now this will only be used for city building.
public class DecisionData {

	//Related Action
	private Buildables buildTarget;

	//Boolean decision
	private bool decision;

	//List of related factors/influencers
	//private List<EnumCategories> factors; <- Ideally somehow make this generic or sumesuch.
	private CitizenCount cCount;
	private TurnCount tCount;

	private Dictionary<EnumCategories, Enum> factors = new Dictionary<EnumCategories, Enum> {
		{ EnumCategories.CitizenCount, CitizenCount.Low },
		{ EnumCategories.TurnCount, TurnCount.Mid }
	};

	public DecisionData (Buildables bT, bool d, int cC, int tC) {
		buildTarget = bT;
		decision = d;
		cCount = getRightEnum<CitizenCount> (cC);
		tCount = getRightEnum<TurnCount> (tC);
	}

	public DecisionData (Buildables bT, bool d, Dictionary<EnumCategories,int> values) {
		buildTarget = bT;
		decision = d;
	}

	private T getRightEnum<T> (int value) where T : IConvertible {
		int closestValue = int.MaxValue;
		foreach (int i in Enum.GetValues(typeof(T))) {
			if (i - value < closestValue)
				closestValue = i;
		}
		return (T)(object)closestValue;
	}

	public override string ToString() {
		return buildTarget.ToString () + ", " + decision.ToString () + ", " + cCount.ToString () + ", " + tCount.ToString ();
	}
}