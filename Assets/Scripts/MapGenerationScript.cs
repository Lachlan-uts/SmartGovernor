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
	[SerializeField]
	private float tileSize = 4.0f;

	private GameObject[,] tileList;

	// influence map related calculation variables
	private float[,] influenceBaseMap;
	private float cityCenterInfluence = 2.0f; // City center influence
	private float cityLocalInfluence = 1.0f; // City regional influence
	private float unitInfluence = 5.0f; // Unit influence


	// Use this for initialization
	void Start () {
		cityX = 5;//Random.Range (2, (int)xMax - 3);
		cityZ = 4;//Random.Range (2, (int)zMax - 3);
		Debug.Log ("City Co-Ords: " + cityX + "/" + cityZ +".");
		tileList = new GameObject[(int) xMax, (int) zMax];

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
			tileList [cityX, cityZ].GetComponent<TileScriptv2> ().createCity ();
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

				//Old method which doesn't
//				GameObject newTile = (GameObject) Instantiate (baseTile, 
//					new Vector3(xCount*widthCount*tileSize, 0.2f * (((int) (Mathf.Lerp(0.0f, 10.0f, hPerlin) * 50.0f)) / 50.0f), zCount*lengthCount*tileSize), 
//					Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
//
//
//				newTile.name = ":Tile: x/z = " + (int)xCount + "/" + (int)zCount + ":";
//
//				newTile.GetComponent<TileScript> ().SetStatistics(hPerlin, tPerlin, aPerlin, mPerlin, (int) xCount, (int) zCount);
//				tileList [(int)xCount, (int)zCount] = newTile;

				Debug.Log ("After:  " + tileList);
				//tileList [(int)xCount, (int)zCount].GetComponent<MeshRenderer> ().material.SetColor("_Color", Color.Lerp (Color.green, Color.gray, perlin));
				//int foodPerlin = (int) Mathf.Lerp(1.0f, 5.0f, hPerlin);
				//int prodPerlin = (int) Mathf.Lerp(2.0f, 6.0f, hPerlin);
				//int goldPerlin = (int) Mathf.Lerp(2.0f, 6.0f, hPerlin);
				//int maxTreePerlin = (int)Mathf.Lerp (0.0f, 4.0f, hPerlin);

				//Debug.Log (tileList [(int)xCount, (int)zCount].ToString ());
				//if ((int) xCount == cityX && (int) zCount == cityZ) {
				//	tileList [(int)xCount, (int)zCount].GetComponent<TileScript> ().createCity ();
				//	Debug.Log ("City Founded!");
				//}



				zCount += 1.0f;
				curStepCount++;
				if (curStepCount >= maxSteps) {
					break;
				}


				//Debug.Log (tileList.ToString ());
			}
			xCount += 1.0f;
			curStepCount++;
			if (curStepCount >= maxSteps) {
				break;
			}
		}


	}

	// get methodology
	public GameObject getTileAt(int XCoord, int ZCoord) {
		return tileList [XCoord, ZCoord];
	}


	public void denoteCity(int xCoord, int zCoord, bool isPlayer) {
		if (isPlayer) {
			influenceBaseMap [xCoord, zCoord] += cityCenterInfluence;
			influenceBaseMap [xCoord - 1, zCoord + 1] += cityLocalInfluence;
			influenceBaseMap [xCoord, zCoord + 1] += cityLocalInfluence;
			influenceBaseMap [xCoord + 1, zCoord + 1] += cityLocalInfluence;
			influenceBaseMap [xCoord - 1, zCoord] += cityLocalInfluence;
			influenceBaseMap [xCoord + 1, zCoord] += cityLocalInfluence;
			influenceBaseMap [xCoord - 1, zCoord - 1] += cityLocalInfluence;
			influenceBaseMap [xCoord, zCoord - 1] += cityLocalInfluence;
			influenceBaseMap [xCoord + 1, zCoord - 1] += cityLocalInfluence;
		} else {
			influenceBaseMap [xCoord, zCoord] -= cityCenterInfluence;
			influenceBaseMap [xCoord - 1, zCoord + 1] -= cityLocalInfluence;
			influenceBaseMap [xCoord, zCoord + 1] -= cityLocalInfluence;
			influenceBaseMap [xCoord + 1, zCoord + 1] -= cityLocalInfluence;
			influenceBaseMap [xCoord - 1, zCoord] -= cityLocalInfluence;
			influenceBaseMap [xCoord + 1, zCoord] -= cityLocalInfluence;
			influenceBaseMap [xCoord - 1, zCoord - 1] -= cityLocalInfluence;
			influenceBaseMap [xCoord, zCoord - 1] -= cityLocalInfluence;
			influenceBaseMap [xCoord + 1, zCoord - 1] -= cityLocalInfluence;
		}


		float cityRadius = 3.0f; // the radius wherein the city can influence





	}

	public void captureCity(int xCoord, int zCoord, bool isPlayer) {
		if (isPlayer) {
			denoteCity (xCoord, zCoord, true);
			denoteCity (xCoord, zCoord, true);
		} else {
			denoteCity (xCoord, zCoord, false);
			denoteCity (xCoord, zCoord, false);
		}
	}

	public float[,] getInfluenceMap() {
		return influenceBaseMap;
	}

	public float[,] getInfluenceMap(int xCoord, int zCoord) {
		return influenceBaseMap;
	}

}
