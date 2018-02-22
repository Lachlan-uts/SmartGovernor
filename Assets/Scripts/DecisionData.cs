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
//		{ EnumCategories.CitizenCount, CitizenCount.Low },
//		{ EnumCategories.TurnCount, TurnCount.Mid }
	};

	public DecisionData (Buildables bT, bool d, int cC, int tC) {
		Type type = Type.GetType (EnumCategories.CitizenCount.ToString ());

		//typeof(EnumCategories).Namespace;

		//Type typ = AssemblyCSharp.CitizenCount.High.GetType ();

		buildTarget = bT;
		decision = d;
		factors.Add (EnumCategories.CitizenCount, getRightEnum<CitizenCount> (cC));
		factors.Add (EnumCategories.TurnCount, getRightEnum<TurnCount> (tC));

		//cCount = getClosestEnum (CitizenCount, cC);

		//cCount = getRightEnum<CitizenCount> (cC);
		//tCount = getRightEnum<TurnCount> (tC);

		Debug.Log ("The type of citizencount is " + CitizenCount.High.GetType ());
		Debug.Log ("The attempted to get a type from string is " + Type.GetType (typeof(EnumCategories).Namespace + "." + EnumCategories.CitizenCount.ToString ()));

	}

	public DecisionData (Buildables bT, bool d, Dictionary<EnumCategories,int> values) {
		buildTarget = bT;
		decision = d;

		foreach (KeyValuePair<EnumCategories,int> pair in values) {
//			getClosestEnum (Type.GetType (typeof(EnumCategories).Namespace + "." + pair.Key.ToString ()), pair.Value);
//
//			var currentType = Type.GetType (typeof(EnumCategories).Namespace + "." + pair.Key.ToString ());
//			Debug.Log (currentType.GetType ());
			//factors.Add(pair.Key, getRightEnum<
		}
	}

//	private Enum getClosestEnum(Enum e, int value) {
//		int closestValue = int.MaxValue;
//		foreach (int i in Enum.GetValues(e.GetType())) {
//			if (i - value < closestValue)
//				closestValue = i;
//		}
//		return (e)closestValue;
//	}

	private T getRightEnum<T> (int value) where T : IConvertible {
		int closestValue = int.MaxValue;
		foreach (int i in Enum.GetValues(typeof(T))) {
			if (i - value < closestValue)
				closestValue = i;
		}
		return (T)(object)closestValue;
	}

	//Don't like these get methods, want to make it more generic
	public bool getDecision() {
		return decision;
	}

	public Enum getFactor(EnumCategories enm) {
		return factors [enm];
	}

	public CitizenCount getCitizenCount() {
		return cCount;
	}

	public TurnCount getTurnCount() {
		return tCount;
	}

	public override string ToString() {
		return buildTarget.ToString () + ", " + decision.ToString () + ", " + cCount.ToString () + ", " + tCount.ToString ();
	}
}