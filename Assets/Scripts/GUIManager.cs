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
	private GameObject CLIManagerRef; // hacky workaround for CLI, might be improved depending on need
	[SerializeField]
	private GameObject cityQueuePanel; // hacky workaroudn for the dynamic queue, might be improved later

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

	// Setting up a theoretical event system based on whether this variable updates
	private GameObject selectedObject;
	public GameObject sObject {
		get { return selectedObject; }
		set {
			if (selectedObject == value) {
				return;
			}
			selectedObject = value;
			if (selectedObject != null) {
				switch (selectedObject.tag) {
				case "City":
					setCityMenuStatus (true);
					CLIManagerRef.GetComponent<CLIScript> ().City = selectedObject;
					break;
				default:
					disableMenus ();
					break;
				}
			}
		}
	}

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
				foodText.text = "" + hit.collider.GetComponent<TileScriptv2> ().getFood ();
				prodText.text = "" + hit.collider.GetComponent<TileScriptv2> ().getProduction ();
				goldText.text = "" + hit.collider.GetComponent<TileScriptv2> ().getGold ();
				heightText.text = "" + hit.collider.GetComponent<TileScriptv2> ().getYCoord ();
				coordText.text = hit.collider.GetComponent<TileScriptv2> ().getXCoord () + "/" + hit.collider.GetComponent<TileScriptv2> ().getZCoord ();
			} else if (hit.collider.tag == "City") {
				GameObject refTile = hit.collider.GetComponent<CityScript> ().getTileAtOrigin ();
				foodText.text = "" + refTile.GetComponent<TileScriptv2>().getFood();
				prodText.text = "" + refTile.GetComponent<TileScriptv2>().getProduction();
				goldText.text = "" + refTile.GetComponent<TileScriptv2>().getGold();
				heightText.text = "" + refTile.GetComponent<TileScriptv2>().getYCoord();
				coordText.text = "" + hit.collider.GetComponent<CityScript> ().getXCoord () + "/" + hit.collider.GetComponent<CityScript> ().getZCoord ();
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
				foodText.text = "" + refTile.GetComponent<TileScriptv2> ().getFood ();
				prodText.text = "" + refTile.GetComponent<TileScriptv2> ().getProduction ();
				goldText.text = "" + refTile.GetComponent<TileScriptv2> ().getGold ();
				heightText.text = "" + refTile.GetComponent<TileScriptv2> ().getYCoord ();
				coordText.text = "" + selectedObject.GetComponent<CityScript> ().getXCoord () + "/" + selectedObject.GetComponent<CityScript> ().getZCoord ();
				cityQueuePanel.GetComponent<QueuePanelScript> ().CityRef = selectedObject;
				cityQueuePanel.GetComponent<QueuePanelScript> ().UpdateQueue (); // Should be replaced by observable event system later
				break;
			case "Tile":
				foodText.text = "" + selectedObject.GetComponent<TileScriptv2>().getFood();
				prodText.text = "" + selectedObject.GetComponent<TileScriptv2>().getProduction();
				goldText.text = "" + selectedObject.GetComponent<TileScriptv2>().getGold();
				heightText.text = "" + selectedObject.GetComponent<TileScriptv2>().getYCoord();
				coordText.text = "" + selectedObject.GetComponent<TileScriptv2> ().getXCoord () + "/" + selectedObject.GetComponent<TileScriptv2> ().getZCoord ();
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
				sObject = hit.collider.gameObject;
			} else {
				sObject = null;
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
				selectedObject.GetComponent<TileScriptv2> ().createCity ();
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

	public void disableMenus() {
		setUnitMenuStatus (false);
		setCityMenuStatus (false);
	}

	// get methodology

	public GameObject getSelectedObject() {
		return selectedObject;
	}

}
