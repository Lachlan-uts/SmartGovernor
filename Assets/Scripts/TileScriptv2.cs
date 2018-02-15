using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScriptv2 : MonoBehaviour {

	// private variables
	// Resource Limits/Parameters for Map Generation
	private float GoldTop = 12.0f;
	private float ForestDensityParam = 0.3f;
	private float FoodLower = 0.0f;
	private float FoodUpper = 4.0f;


	//Resource Storage for realsies
	private Dictionary<string,Resource> Resources;

	// Resource Storage
	private int Food;
	private int Production;
	private int Gold;

	// Metadata
	private bool hasCity;

	// serialized private variables
	[SerializeField]
	private bool randomGen = true; // Control variable for randomly generated resources
	[SerializeField]
	private bool canSet = false; // Control variable for if another source can set the resources for the tile
	[SerializeField]
	private GameObject[] Trees; // Trees!


	// Use this for initialization
	void Awake () {
		Resources = new Dictionary<string, Resource> ();
		Resources.Add ("Food", new Resource ("Food"));
		Resources.Add ("Production", new Resource ("Production"));
		Resources.Add ("Gold", new Resource ("Gold"));
	}

	void Start () {
	}
		
	// Update is called once per frame
	void Update () {
		
	}

//	// MouseOverFunctions
//
	void OnMouseOver() {
		Debug.Log ("Tile:" + name + "Position:" + transform.position.ToString() + "parent's array: ");
		Debug.Log ("Tile:" + name + "; F:" + Food + "; P:" + Production + "; G:" + Gold);

	}
		
	public void SetStatistics(float forestPerlin, float ariaPerlin, float minePerlin) {
		if (!randomGen && canSet) {
			int numTrees = 4;
//			if (forestPerlin >= ForestDensityParam) {
//				numTrees = (int) (Mathf.Lerp(1.0f, Trees.Length, ((1.0f / (1.0f - ForestDensityParam)) * (forestPerlin - ForestDensityParam))));
//			}
			// Food deterministic algorithm
			Food = (int) (Mathf.Lerp (FoodLower, FoodUpper, ariaPerlin) + (0.5f * numTrees));
			Resources ["Food"].setValue ((int)(Mathf.Lerp (FoodLower, FoodUpper, ariaPerlin) + (0.5f * numTrees)));
			// Production deterministic algorithm
			Production = (int) (Mathf.Lerp(1.0f, 6.0f, getYCoord()) + Mathf.Lerp(0.0f, 4.0f, minePerlin) + (0.5f * numTrees));
			Resources ["Production"].setValue ((int)(Mathf.Lerp (1.0f, 6.0f, getYCoord ()) + Mathf.Lerp (0.0f, 4.0f, minePerlin) + (0.5f * numTrees)));
			// Gold deterministic algorithm
			Gold = (int) (GoldTop - (Mathf.Lerp(0.0f, 4.0f, minePerlin) + numTrees));
			Resources ["Gold"].setValue ((int)(GoldTop - (Mathf.Lerp (0.0f, 4.0f, minePerlin) + numTrees)));
			if (Gold <= 0) {
				Gold = 0;
				Resources ["Gold"].setValue (0);
			}

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
			//Debug.Log ("Tile:" + name + "; F:" + Food + "; P:" + Production + "; G:" + Gold + "; H:" + height);

			randomGen = true;
		}
	}
//
//	public void LoadFromString(string TileString) {
//		string[] TileParams = TileString.Split (',');
//		int.TryParse (TileParams [0], out Food);
//		int.TryParse (TileParams [1], out Production);
//		int.TryParse (TileParams [2], out Gold);
//		Debug.Log ("Tile: F:" + Food + "; P:" + Production + "; G:" + Gold + "; TV:" + (Food + Production + Gold));
//	}

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

	//Painful hack that makes me cry but will suffice for now due to how poorly we've been storing resources.

	//Actually nvm going to do it "right" regardless because I can't help myself <-- this may actually be overkill
	public Resource getResource(string resourceName) {
		return Resources[resourceName];
	}


	public int getTileValue() {
		return getFood () + getProduction () + getGold ();
	}

	public int getXCoord() {
		return (int)Mathf.CeilToInt(transform.position.x);
	}

	public float getYCoord() {
		return Mathf.CeilToInt(transform.position.y);
	}

	public int getZCoord() {
		return (int)Mathf.CeilToInt(transform.position.z);
	}
}
