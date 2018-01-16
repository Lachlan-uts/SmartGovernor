using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {


	// Serialized private variables
	[SerializeField]
	private Camera MCamera;

	[SerializeField]
	private Text foodText;
	[SerializeField]
	private Text prodText;
	[SerializeField]
	private Text goldText;
	[SerializeField]
	private Text heightText;

	private GameObject selectedObject;

	// Use this for initialization
	void Start () {
		selectedObject = null;
	}
	
	// Update is called once per frame
	void Update () {


		RaycastHit hit;
		Ray cameraRay = MCamera.ScreenPointToRay (Input.mousePosition);
		//Debug.Log ("hitPoint: " + hit.transform.position);

		if (Physics.Raycast(cameraRay, out hit)) {
			//Debug.Log ("hitPoint: " + hit.transform.position);
			if (hit.collider.tag == "Tile") {
				foodText.text = "" + hit.collider.GetComponent<TileScript> ().getFood ();
				prodText.text = "" + hit.collider.GetComponent<TileScript> ().getProduction ();
				goldText.text = "" + hit.collider.GetComponent<TileScript> ().getGold ();
				heightText.text = "" + hit.collider.GetComponent<TileScript> ().getHeight ();
			}
		} /*else if (!(selectedObject)) {
			foodText.text = "";
			prodText.text = "";
			goldText.text = "";
			heightText.text = "";
		}*/ else {
			foodText.text = "Uh";
			prodText.text = "Uh";
			goldText.text = "Uh";
			heightText.text = "Uh";
		}

		/*
		// Food Text Management
		if (foodNumber == null) {
			foodText.text = "";
		} else {
			foodText.text = "" + foodNumber;
		}

		// Production Text Management
		if (prodNumber == null) {
			prodText.text = "";
		} else {
			prodText.text = "" + prodNumber;
		}

		// Gold Text Management
		if (goldNumber == null) {
			goldText.text = "";
		} else {
			goldText.text = "" + goldNumber;
		}

		// Height Text Management
		if (heightNumber == null) {
			heightText.text = "";
		} else {
			heightText.text = "" + heightNumber;
		}*/


	}

	// public methods



}
