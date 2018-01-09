using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

	// Note: a lot of what is currently within the TileScript is placeholder functionality

	// private variables
	private int Food;
	private int Production;
	private int Gold;


	// Use this for initialization
	void Start () {
		Food = Random.Range (1, 6);
		Production = Random.Range (1, 8);
		Gold = Random.Range (2, 5);
		Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// public methods
	

	// get methodology
	public int getFood() {
		return Food;
	}

	public int getProduction() {
		return Production;
	}

	public int getGold() {
		return Gold;
	}
}
