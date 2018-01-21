using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {


	// Serialized private variables
	[SerializeField]
	private Camera MCamera;

	[SerializeField]
	private GameObject[] unitMenuActions;
	[SerializeField]
	private GameObject[] cityMenuActions;

	[SerializeField]
	private Text foodText;
	[SerializeField]
	private Text prodText;
	[SerializeField]
	private Text goldText;
	[SerializeField]
	private Text heightText;
	[SerializeField]
	private Text coordText;

	private GameObject selectedObject;

	// Use this for initialization
	void Start () {
		selectedObject = null;
		setCityMenuStatus (false);
		setUnitMenuStatus (false);
	}
	
	// Update is called once per frame
	void Update () {


		RaycastHit hit;
		Ray cameraRay = MCamera.ScreenPointToRay (Input.mousePosition);
		//Debug.Log ("hitPoint: " + hit.transform.position);

		if (Physics.Raycast(cameraRay, out hit) && !selectedObject) {
			//Debug.Log ("hitPoint: " + hit.transform.position);
			if (hit.collider.tag == "Tile") {
				foodText.text = "" + hit.collider.GetComponent<TileScript> ().getFood ();
				prodText.text = "" + hit.collider.GetComponent<TileScript> ().getProduction ();
				goldText.text = "" + hit.collider.GetComponent<TileScript> ().getGold ();
				heightText.text = "" + hit.collider.GetComponent<TileScript> ().getHeight ();
				coordText.text = hit.collider.GetComponent<TileScript> ().getXCoord () + "/" + hit.collider.GetComponent<TileScript> ().getZCoord ();
			} else {
				foodText.text = "--";
				prodText.text = "--";
				goldText.text = "--";
				heightText.text = "--";
				coordText.text = "--/--";
			}
		} else if (!(selectedObject)) {
			foodText.text = "--";
			prodText.text = "--";
			goldText.text = "--";
			heightText.text = "--";
			coordText.text = "--/--";
		} else {

			switch (selectedObject.tag) {
			case "City":
				GameObject refTile = selectedObject.GetComponent<CityScript> ().getTileAtOrigin ();
				foodText.text = "" + refTile.GetComponent<TileScript>().getFood();
				prodText.text = "" + refTile.GetComponent<TileScript>().getProduction();
				goldText.text = "" + refTile.GetComponent<TileScript>().getGold();
				heightText.text = "" + refTile.GetComponent<TileScript>().getHeight();
				coordText.text = "" + refTile.GetComponent<TileScript> ().getXCoord () + "/" + selectedObject.GetComponent<TileScript> ().getZCoord ();
				break;
			case "Tile":
				foodText.text = "" + selectedObject.GetComponent<TileScript>().getFood();
				prodText.text = "" + selectedObject.GetComponent<TileScript>().getProduction();
				goldText.text = "" + selectedObject.GetComponent<TileScript>().getGold();
				heightText.text = "" + selectedObject.GetComponent<TileScript>().getHeight();
				coordText.text = "" + selectedObject.GetComponent<TileScript> ().getXCoord () + "/" + selectedObject.GetComponent<TileScript> ().getZCoord ();
				break;
			default:
				foodText.text = "--";
				prodText.text = "--";
				goldText.text = "--";
				heightText.text = "--";
				coordText.text = "--/--";
				break;
			}
		}

		if (Input.GetButtonDown ("Fire1")) {
			if (Physics.Raycast(cameraRay, out hit) && !selectedObject) {
				selectedObject = hit.collider.gameObject;
			} else {
				selectedObject = null;
			}
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

	// private methods



	// public methods

	public void createCity() {
		if (selectedObject) {
			if (selectedObject.CompareTag ("Tile")) {
				selectedObject.GetComponent<TileScript> ().createCity ();
			}
		}
	}

	public void setCityMenuStatus(bool enabled) {
		if (enabled) {
			setUnitMenuStatus (false);

			foreach (GameObject menuItem in cityMenuActions) {
				menuItem.SetActive (true);
			}
		} else {
			foreach (GameObject menuItem in cityMenuActions) {
				menuItem.SetActive (false);
			}
		}
	}

	public void setUnitMenuStatus(bool enabled) {
		if (enabled) {
			setCityMenuStatus (false);

			foreach (GameObject menuItem in unitMenuActions) {
				menuItem.SetActive (true);
			}
		} else {
			foreach (GameObject menuItem in unitMenuActions) {
				menuItem.SetActive (false);
			}
		}
	}

	// get methodology

	public GameObject getSelectedObject() {
		return selectedObject;
	}

}
