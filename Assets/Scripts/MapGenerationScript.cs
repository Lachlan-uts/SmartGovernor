using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationScript : MonoBehaviour {

	// public variables
	public GameObject baseTile;

	// private variables
	private GameObject T;
	private int curGenStep;
	private float timeMax = 0.1f;
	private float timeCur = 0.0f;
	private bool activeGen; // boolean for active generation of perlin-noise based map generation
	private int genStage; // marker for the stage of development over time
	private int cityX, cityZ; // co-ordinates for initial city
	private int enemyX, enemyZ; // co-ordinates for initial enemy city

	// private perlin noise map seeds
	private float forestPerlinSeed;
	private float heightPerlinSeed;
	private float ariaPerlinSeed;
	private float minePerlinSeed;

	// Serialized private variables
	[SerializeField]
	private float xMax = 160.0f;
	[SerializeField]
	private float zMax = 100.0f;
	[SerializeField]
	private int randomSeed = 10;
	[SerializeField]
	private int maxGenSteps = 20; // How many steps to attempt per attempt
	[SerializeField]
	private float noiseX = 20;
	[SerializeField]
	private float noiseZ = 15;

	//Map Variables (Should they both be public static?
	private GameObject[,] tileList;
	public static List<GameObject> cityList;

	// influence map related calculation variables
	private float[,] influenceBaseMap;
	private float cityCenterInfluence = 2.0f; // City center influence
	private float cityLocalInfluence = 1.0f; // City regional influence
	private float unitInfluence = 5.0f; // Unit influence


	// Use this for initialization
	void Start () {
		cityX = 2;//Random.Range (2, (int)xMax - 3);
		cityZ = 2;//Random.Range (2, (int)zMax - 3);
		enemyX = 24;
		enemyZ = 22;
		Debug.Log ("City Co-Ords: " + cityX + "/" + cityZ +".");
		tileList = new GameObject[(int) xMax, (int) zMax];
		cityList = new List<GameObject>();

		influenceBaseMap = new float[(int)xMax, (int)zMax];
		for (int xCount = 0; xCount < (int)xMax; xCount++) {
			for (int zCount = 0; zCount < (int)zMax; zCount++) {
				influenceBaseMap [xCount, zCount] = 0.0f;
			}
		}

		curGenStep = 0;
		activeGen = true;
		Random.InitState (randomSeed);
		forestPerlinSeed = Random.Range (0, 1500);
		heightPerlinSeed = Random.Range (0, 1500);
		ariaPerlinSeed = Random.Range (0, 1500);
		minePerlinSeed = Random.Range (0, 1500);
		//GenerateNoiseMap ();

		genStage = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (genStage == 0) {
			timeCur += Time.deltaTime;
			if (timeCur > timeMax && activeGen) {
				timeCur -= timeMax;
				GenerateNoiseSegment (curGenStep, maxGenSteps);
				curGenStep += maxGenSteps;
				if (curGenStep >= (xMax * zMax)) {
					activeGen = false;
					genStage = 1;
				}
				//Debug.Log ("step: " + curGenStep + " ; zM*xM: " + (zMax * xMax) + " ; ");
			}
		} else if (genStage == 1) {
			genStage = 2;
			cityList.Add (Instantiate (Resources.Load ("City"), 
				tileList [cityX, cityZ].transform.position, 
				Quaternion.identity, 
				tileList [cityX, cityZ].transform) as GameObject);
		}
	}

	// Perlin Noise Map generation
	void GenerateNoiseMap () {
		float widthCount = baseTile.transform.lossyScale.x;
		float lengthCount = baseTile.transform.lossyScale.z;

		for (float xCount = 0.0f; xCount < xMax; xCount += 1.0f) {
			for (float zCount = 0.0f; zCount < zMax; zCount += 1.0f) {
				T = (GameObject) Instantiate (baseTile, 
					new Vector3(xCount*widthCount, 2.0f, zCount*lengthCount), 
					Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
				tileList [(int)xCount, (int)zCount] = T;
			}
		}


	}

	// Perlin Noise Segment-based generation
	void GenerateNoiseSegment (int startInt, int maxSteps) {
		//Debug.Log (tileList.ToString ());
		float widthCount = baseTile.transform.lossyScale.x;
		float lengthCount = baseTile.transform.lossyScale.z;

		float xCount = 0.0f;
		float zCount = 0.0f;

		int curStepCount = 0;

		int xStepStart = (startInt / (int) xMax);
		xCount = xStepStart;
		int zStepStart = (startInt % (int) xMax);
		zCount = zStepStart;

		while (xCount < xMax) {
			while (zCount < zMax) {
				//Only one which actually affects tile generation.
				var hPerlin = Mathf.PerlinNoise (heightPerlinSeed + xCount / noiseX, heightPerlinSeed + zCount / noiseZ);
				var tPerlin = Mathf.PerlinNoise (forestPerlinSeed + xCount / noiseX, forestPerlinSeed + zCount / noiseZ);
				var aPerlin = Mathf.PerlinNoise (ariaPerlinSeed + xCount / noiseX, ariaPerlinSeed + zCount / noiseZ);
				var mPerlin = Mathf.PerlinNoise (minePerlinSeed + xCount / noiseX, minePerlinSeed + zCount / noiseZ);
				Debug.Log("Before: " + tileList);

				//New method which works
				Vector3 pos = new Vector3(xCount,0.2f * (((int) (Mathf.Lerp(0.0f, 10.0f, hPerlin) * 50.0f)) / 50.0f),zCount);
				tileList [(int)xCount, (int)zCount] = Instantiate (Resources.Load ("TestTile"), pos, Quaternion.identity, transform) as GameObject;
				tileList [(int)xCount, (int)zCount].name = ":Tile: x/z = " + (int)xCount + "/" + (int)zCount + ":";
				tileList [(int)xCount, (int)zCount].GetComponent<TileScriptv2> ().SetStatistics (tPerlin,aPerlin,mPerlin);

				zCount += 1.0f;
				curStepCount++;
				if (curStepCount >= maxSteps) {
					break;
				}
			}
			xCount += 1.0f;
			curStepCount++;
			if (curStepCount >= maxSteps) {
				break;
			}
		}
	}

	//Quick way of getting all the tiles a city has access to.
	public List<GameObject> getSquareRadius(GameObject centerTile, int radius) {
		List<GameObject> tiles = new List<GameObject> ();
		int initialX = centerTile.GetComponent<TileScriptv2> ().getXCoord ();
		int initialZ = centerTile.GetComponent<TileScriptv2> ().getZCoord ();
		for (int x = (initialX - radius); x <= (initialX + radius); x++) {
			for (int z = (initialZ - radius); z <= (initialZ + radius); z++) {
				tiles.Add(getTileAt(x,z));
			}
		}
		//remove the city tile itself from the list
		tiles.Remove(centerTile);
		return tiles;
	}

	public bool buildCity(int xCoord, int zCoord) {
		if (!cityList.Find (city => city.transform.parent.gameObject == tileList [xCoord, zCoord])) {
			cityList.Add (Instantiate (Resources.Load ("City"), 
				tileList [xCoord, zCoord].transform.position, 
				Quaternion.identity, 
				tileList [xCoord, zCoord].transform) as GameObject);
			return true;
		}
		return false;
	}

	//deterministic city building
	public bool expansionBuildCity(GameObject city) {
		List<GameObject> potentialTiles = new List<GameObject>();
		GameObject cityTile = city.GetComponent<CityScriptv2> ().getTileAtOrigin ();
		int cityTileX = cityTile.GetComponent<TileScriptv2> ().getXCoord ();
		int cityTileZ = cityTile.GetComponent<TileScriptv2> ().getZCoord ();

		//Going to get the 4 potential tiles.
		potentialTiles.Add(getTileAt(cityTileX+3, cityTileZ));
		potentialTiles.Add(getTileAt(cityTileX, cityTileZ+3));
		potentialTiles.Add(getTileAt(cityTileX-3, cityTileZ));
		potentialTiles.Add(getTileAt(cityTileX, cityTileZ-3));

		//Remove all null tiles
		potentialTiles.RemoveAll(tile => tile == null);

		//Remove all tiles that already have a city
		cityList.ForEach(Icity => potentialTiles.Remove(Icity.GetComponent<CityScriptv2>().getTileAtOrigin()));

		//Sorts all tiles based off their comparitive value
		potentialTiles.Sort ((x, y) => x.GetComponent<TileScriptv2> ().getTileValue ().CompareTo (y.GetComponent<TileScriptv2> ().getTileValue ()));
		potentialTiles.Reverse ();

		//Build a city at the chosen tile or return false if no valid options
		if (potentialTiles.Count > 0) {
			cityList.Add (Instantiate (Resources.Load ("City"), potentialTiles [0].transform.position, Quaternion.identity, potentialTiles [0].transform) as GameObject);
			return true;
		}
		return false;
	}

	// updating cities, either all or by faction in the future

	public void updateAllCities() {
		using (IEnumerator<GameObject> cityEnum = cityList.GetEnumerator ()) {
			while (cityEnum.MoveNext ()) {
				cityEnum.Current.GetComponent<CityScriptv2> ().CityUpdate ();
			}
		}
//
//		foreach (GameObject city in cityList) {
//			city.GetComponent<CityScriptv2> ().CityUpdate ();
//		}
	}

	//This is an ideal solution, in the short run I'll use a simpler system I've decided <-- Overkill currently. Stretch Goal.
//	public GameObject[,] aStarCalculation() {
//	}

	// simple get methodology
	public GameObject[,] getWholeMap() {
		return tileList;
	}

	//Made it so now this returns either a valid tile when given valid coordinates or null when not.
	public GameObject getTileAt(int xCoord, int zCoord) {
		if (( xCoord > tileList.GetLowerBound (0) && xCoord < tileList.GetUpperBound(0) ) && ( zCoord > tileList.GetLowerBound (1) && zCoord < tileList.GetUpperBound(1) ))
			return tileList [xCoord, zCoord];
		return null;
	}

	public int getXSize() {
		return (int)xMax;
	}

	public int getZSize() {
		return (int)zMax;
	}
}