using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

//For now this will only be used for city building.
public class DecisionData : IEquatable<DecisionData> {

	//Boolean decision
	public bool decision {get; private set;}

	//List of factors relevent to decision.
	public Dictionary<EnumCategories, Enum> factors {get; private set;}

	public DecisionData (bool d, int cC, int tC) {
		//initialise the dictionary
		factors = new Dictionary<EnumCategories, Enum>();

		decision = d;

		factors.Add (EnumCategories.CitizenCount, getRightEnum<CitizenCount> (cC));
		factors.Add (EnumCategories.TurnCount, getRightEnum<TurnCount> (tC));
	}
		
	private T getRightEnum<T> (int value) where T : IConvertible {
		int closestValue = int.MaxValue;
		foreach (int i in Enum.GetValues(typeof(T))) {
			if ((i - value) < closestValue)
				closestValue = i;
		}
		return (T)(object)closestValue;
	}

	//Don't like these get methods, want to make it more generic
//	public bool getDecision() {
//		return decision;
//	}

	public Enum getFactor(EnumCategories enm) {
		return factors [enm];
	}

	public bool Equals(DecisionData other) {
		//check if the other is null
		if (other == null)
			return false;
		// check if the other is of the same decision
		if (decision != other.decision)
			return false;
		// check if the other has the same number of factors
		if (factors.Count != other.factors.Count)
			return false;
		//check if the factors are equal
		bool equal = true;
		foreach (var pair in factors) {
			Enum value;
			equal = other.factors.TryGetValue (pair.Key, out value);
			if (!equal)
				return equal;
			if (!value.Equals(pair.Value))
				return equal;
		} return equal;
	}
		
	public override bool Equals(object obj) {
		if (obj is DecisionData) {
			DecisionData dat = (DecisionData)obj;
			return Equals (dat);
		} return false;
	}

	public override int GetHashCode() {
		unchecked {
			int hash = (int)2166136261;
			hash = (hash * 16777619) ^ decision.GetHashCode ();
			foreach (var pair in factors) {
				hash = (hash * 16777619) ^ pair.Key.GetHashCode ();
				hash = (hash * 16777619) ^ pair.Value.GetHashCode ();
			}
			return hash;
		}
	}

	public override string ToString() {
		return decision.ToString () + ", " + string.Join(" ! ", factors.Values);
	}
}