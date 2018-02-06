using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScriptv2 : MonoBehaviour {

	// Metadata
	private bool hasCity;
 	private float height;
	private int XCoord = 0; // Locally stored reference data for GUI streamlining
	private int ZCoord = 0; // Locally stored reference data for GUI streamlining

	// serialized private variables
	[SerializeField]
	private bool randomGen = true; // Control variable for randomly generated resources
	[SerializeField]
	private bool canSet = false; // Control variable for if another source can set the resources for the tile
	[SerializeField]
	private GameObject[] Trees; // Trees!


	// Use this for initialization
	void Awake () {
//		hasCity = false;
//		if (randomGen) {
//			height = 0.0f;
//			Food = Random.Range (1, 6);
//			Production = Random.Range (1, 8);
//			Gold = Random.Range (2, 5);
//			Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
//		} else {
//			foreach (GameObject Tree in Trees) {
//				Tree.SetActive (false);
//			}
//			height = 0.0f;
//		}
	}

	public void init(int xCoord, int yCoord, float height, int rndSeed) {

	}

	void Start () {
		XCoord = 0;
		ZCoord = 0;
		height = 0.0f;
	}

	void Start (int xCoord, int zCoord, float height, int rndSeed) {
		//Vector3 ();
		XCoord = xCoord;
		ZCoord = zCoord;
		height = height;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	// MouseOverFunctions
//
	void OnMouseOver() {
		Debug.Log ("Tile:" + name + "Position:" + transform.position.ToString());

	}
//
//	// public methods
//	public void SetResources(int newFood, int newProd, int newGold, int maxTreeCount) {
//		if (!randomGen && canSet) {
//			int treeCount = 0;
//			Food = newFood;
//			Production = newProd;
//			Gold = newGold;
//			//Random.InitState (randomSeed);
//			foreach (GameObject Tree in Trees) {
//				if (Random.Range (0.0f, 10.0f) > 5.0f && treeCount < maxTreeCount) {
//					Tree.SetActive (true);
//					treeCount++;
//				}
//			}
//			Debug.Log ("Trees Done");
//			Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
//			randomGen = true;
//		}
//	}
//
//	public void SetStatistics(float heightPerlin, float forestPerlin, float ariaPerlin, float minePerlin, int newX, int newZ) {
//		if (!randomGen && canSet) {
//			height = 50.0f * ((int) Mathf.Lerp(0.0f, 10.0f, heightPerlin));
//			int numTrees = 0;
//			if (forestPerlin >= ForestDensityParam) {
//				numTrees = (int) (Mathf.Lerp(1.0f, Trees.Length, ((1.0f / (1.0f - ForestDensityParam)) * (forestPerlin - ForestDensityParam))));
//			}
//			// Food deterministic algorithm
//			Food = (int) (Mathf.Lerp (FoodLower, FoodUpper, ariaPerlin) + (0.5f * numTrees));
//			// Production deterministic algorithm
//			Production = (int) (Mathf.Lerp(1.0f, 6.0f, heightPerlin) + Mathf.Lerp(0.0f, 4.0f, minePerlin) + (0.5f * numTrees));
//			// Gold deterministic algorithm
//			Gold = (int) (GoldTop - (Mathf.Lerp(0.0f, 4.0f, minePerlin) + numTrees));
//			if (Gold <= 0) {
//				Gold = 0;
//			}
//
//			// While loop for turning "trees" "on"
//			int curTrees = 0;
//			while (curTrees < numTrees) {
//				foreach (GameObject Tree in Trees) {
//					if (Random.Range (0.0f, 10.0f) > 5.0f && curTrees < numTrees && Tree.activeSelf == false) {
//						Tree.SetActive (true);
//						curTrees++;
//					}
//				}
//			}
//
//			XCoord = newX;
//			ZCoord = newZ;
//
//			//Debug.Log ("Tile:" + name + "; F:" + Food + "; P:" + Production + "; G:" + Gold + "; H:" + height);
//
//			randomGen = true;
//		}
//	}
//
//	public void LoadFromString(string TileString) {
//		string[] TileParams = TileString.Split (',');
//		int.TryParse (TileParams [0], out Food);
//		int.TryParse (TileParams [1], out Production);
//		int.TryParse (TileParams [2], out Gold);
//		Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
//	}
//
//	public void createCity() {
//		if (!hasCity) {
//			hasCity = true;
//			// Code to create a city on this tile
//			GameObject.FindWithTag("InputManager").GetComponent<InputManagementScript>().colonize(transform.position, XCoord, ZCoord);
//		}
//	}
//
//	// get methodology
//	public int getFood() {
//		return Food;
//	}
//
//	public int getProduction() {
//		return Production;
//	}
//
//	public int getGold() {
//		return Gold;
//	}
//
//	public float getHeight() {
//		return height;
//	}
//
//	public int getXCoord() {
//		return XCoord;
//	}
//
//	public int getZCoord() {
//		return ZCoord;
//	}
}
