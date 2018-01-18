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
	private bool activeGen;

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

	// Use this for initialization
	void Start () {
		tileList = new GameObject[(int) xMax, (int) zMax];
		curGenStep = 0;
		activeGen = true;
		Random.InitState (randomSeed);
		forestPerlinSeed = Random.Range (0, 1500);
		heightPerlinSeed = Random.Range (0, 1500);
		ariaPerlinSeed = Random.Range (0, 1500);
		minePerlinSeed = Random.Range (0, 1500);
		//GenerateNoiseMap ();
	}
	
	// Update is called once per frame
	void Update () {
		timeCur += Time.deltaTime;
		if (timeCur > timeMax && activeGen) {
			timeCur -= timeMax;
			GenerateNoiseSegment (curGenStep, maxGenSteps);
			curGenStep += maxGenSteps;
			if (curGenStep >= (xMax * zMax)) {
				activeGen = false;
			}
			//Debug.Log ("step: " + curGenStep + " ; zM*xM: " + (zMax * xMax) + " ; ");
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
				var hPerlin = Mathf.PerlinNoise (heightPerlinSeed + xCount / noiseX, heightPerlinSeed + zCount / noiseZ);
				var tPerlin = Mathf.PerlinNoise (forestPerlinSeed + xCount / noiseX, forestPerlinSeed + zCount / noiseZ);
				var aPerlin = Mathf.PerlinNoise (ariaPerlinSeed + xCount / noiseX, ariaPerlinSeed + zCount / noiseZ);
				var mPerlin = Mathf.PerlinNoise (minePerlinSeed + xCount / noiseX, minePerlinSeed + zCount / noiseZ);
				T = (GameObject) Instantiate (baseTile, 
					new Vector3(xCount*widthCount*tileSize, 0.2f * (((int) (Mathf.Lerp(0.0f, 10.0f, hPerlin) * 50.0f)) / 50.0f), zCount*lengthCount*tileSize), 
					Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
				//T.GetComponent<Material> ().SetColor("ColourGrad:" + perlin, Color.Lerp (Color.green, Color.gray, perlin));
				tileList [(int)xCount, (int)zCount] = T;
				tileList [(int)xCount, (int)zCount].name = ":Tile: x/z = " + (int)xCount + "/" + (int)zCount + ":";
				//tileList [(int)xCount, (int)zCount].GetComponent<MeshRenderer> ().material.SetColor("_Color", Color.Lerp (Color.green, Color.gray, perlin));
				//int foodPerlin = (int) Mathf.Lerp(1.0f, 5.0f, hPerlin);
				//int prodPerlin = (int) Mathf.Lerp(2.0f, 6.0f, hPerlin);
				//int goldPerlin = (int) Mathf.Lerp(2.0f, 6.0f, hPerlin);
				//int maxTreePerlin = (int)Mathf.Lerp (0.0f, 4.0f, hPerlin);
				tileList [(int)xCount, (int)zCount].GetComponent<TileScript> ().SetStatistics(hPerlin, tPerlin, aPerlin, mPerlin, (int) xCount, (int) zCount);
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

	// get methodology
	public GameObject getTileAt(int XCoord, int ZCoord) {
		return tileList [XCoord, ZCoord];
	}

}
