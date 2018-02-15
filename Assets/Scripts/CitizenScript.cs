using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// This should return either the tile the citizen is attached to or the city itself if it's not on a tile.
//	public GameObject getPosition() {
//		return transform.parent;
//	}

	private int getFood() {
		return transform.GetComponentInParent<TileScriptv2> ().getFood ();
	}

	private int getProduction() {
		return transform.GetComponentInParent<TileScriptv2> ().getProduction ();
	}

	private int getGold() {
		return transform.GetComponentInParent<TileScriptv2> ().getGold ();
	}
	//The idea here is to return an array or equivlent data structure containing all of the resources this citizen collected
	public Dictionary<string,int> getResources() {
		
		if (transform.parent.name.Contains("City"))
			return new Dictionary<string,int>() 
			{
				{"Food",0},
				{"Production",0},
				{"Gold",2}
			};
		else
			return new Dictionary<string,int>()
			{
				{"Food", getFood()},
				{"Production", getProduction()},
				{"Gold", getGold()}
			};
	}
}
