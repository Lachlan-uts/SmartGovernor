using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

	// Note: a lot of what is currently within the TileScript is placeholder functionality

	// private variables
	private int Food;
	private int Production;
	private int Gold;

	// serialized private variables
	[SerializeField]
	private bool randomGen = true; // Control variable for randomly generated resources
	[SerializeField]
	private bool canSet = false; // Control variable for if another source can set the resources for the tile
	[SerializeField]
	private GameObject[] Trees; // Trees!


	// Use this for initialization
	void Awake () {
		if (randomGen) {
			Food = Random.Range (1, 6);
			Production = Random.Range (1, 8);
			Gold = Random.Range (2, 5);
			Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
		} else {
			foreach (GameObject Tree in Trees) {
				Tree.SetActive (false);
			}
		}
	}

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// public methods
	public void SetResources(int newFood, int newProd, int newGold, int maxTreeCount) {
		if (!randomGen && canSet) {
			int treeCount = 0;
			Food = newFood;
			Production = newProd;
			Gold = newGold;
			//Random.InitState (randomSeed);
			foreach (GameObject Tree in Trees) {
				if (Random.Range (0.0f, 10.0f) > 5.0f && treeCount < maxTreeCount) {
					Tree.SetActive (true);
					treeCount++;
				}
			}
			Debug.Log ("Trees Done");
			Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
			randomGen = true;
		}
	}

	public void LoadFromString(string TileString) {
		string[] TileParams = TileString.Split (',');
		int.TryParse (TileParams [0], out Food);
		int.TryParse (TileParams [1], out Production);
		int.TryParse (TileParams [2], out Gold);
		Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
	}

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
