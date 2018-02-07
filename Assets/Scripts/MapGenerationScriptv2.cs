using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationScriptv2 : MonoBehaviour {

	public Transform TestTile;

	//public GameObject TestTile;

	private GameObject[,] tiles;

	private GameObject testTile;

	[SerializeField]
	private int xLength;
	[SerializeField]
	private int zLength;



	// Use this for initialization
	void Start () {

		tiles = new GameObject[xLength, zLength];

		for (int x = 0; x < xLength; x++) {
			for (int z= 0; z < zLength; z++) {
				Vector3 pos = new Vector3 (x, 4, z);
				tiles[x,z] = Instantiate(Resources.Load("TestTile"), pos, Quaternion.identity, transform) as GameObject;
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	public GameObject getTile(int x, int z) {
		return tiles [x, z];
	}
}
