using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AssemblyCSharp;

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

	//Annoying problem with ui elements
	private GraphicRaycaster uiRaycaster;
	private PointerEventData uiPointer;
	private EventSystem eventSystem;

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

		//Trying to fix this nonsense
		uiRaycaster = GetComponent<GraphicRaycaster>();
		eventSystem = GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray cameraRay = MCamera.ScreenPointToRay (Input.mousePosition);
		//Debug.Log ("hitPoint: " + hit.transform.position);

		if (Physics.Raycast(cameraRay, out hit) && !selectedObject) {
			//Debug.Log ("hitPoint: " + hit.transform.position);
			if (hit.collider.tag == "Tile") {
				foodText.text = "F: " + hit.collider.GetComponent<TileScriptv2> ().getFood ();
				prodText.text = "P: " + hit.collider.GetComponent<TileScriptv2> ().getProduction ();
				goldText.text = "G: " + hit.collider.GetComponent<TileScriptv2> ().getGold ();
				heightText.text = "H: " + hit.collider.GetComponent<TileScriptv2> ().getYCoord ();
				coordText.text = hit.collider.GetComponent<TileScriptv2> ().getXCoord () + "/" + hit.collider.GetComponent<TileScriptv2> ().getZCoord ();
			} else if (hit.collider.tag == "City") {
				GameObject refTile = hit.collider.GetComponent<CityScriptv2> ().getTileAtOrigin ();
				foodText.text = "F: " + refTile.GetComponent<TileScriptv2>().getFood();
				prodText.text = "P: " + refTile.GetComponent<TileScriptv2>().getProduction();
				goldText.text = "G: " + refTile.GetComponent<TileScriptv2>().getGold();
				heightText.text = "H: " + refTile.GetComponent<TileScriptv2>().getYCoord();
				coordText.text = "City at: " + hit.collider.GetComponent<CityScriptv2> ().getXCoord () + "/" + hit.collider.GetComponent<CityScriptv2> ().getZCoord ();
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
				GameObject refTile = selectedObject.GetComponent<CityScriptv2> ().getTileAtOrigin ();
				foodText.text = "" + refTile.GetComponent<TileScriptv2> ().getFood ();
				prodText.text = "" + refTile.GetComponent<TileScriptv2> ().getProduction ();
				goldText.text = "" + refTile.GetComponent<TileScriptv2> ().getGold ();
				heightText.text = "" + refTile.GetComponent<TileScriptv2> ().getYCoord ();
				coordText.text = "" + selectedObject.GetComponent<CityScriptv2> ().getXCoord () + "/" + selectedObject.GetComponent<CityScriptv2> ().getZCoord ();
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
			uiPointer = new PointerEventData (eventSystem);
			uiPointer.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult> ();
			uiRaycaster.Raycast (uiPointer, results);

			if (!eventSystem.IsPointerOverGameObject ()) {
				if (Physics.Raycast (cameraRay, out hit) && !selectedObject) {
					sObject = hit.collider.gameObject;
				} else {
					sObject = null;
				}
			}
		}

		if (Input.GetButtonDown ("Fire2")) {
			if (Physics.Raycast(cameraRay, out hit) && selectedObject) {
				Debug.Log ("Order Recieved...?");
				if (selectedObject.tag == "Unit") {
					Debug.Log ("Order Recieved?");
					if (hit.collider.tag == "Tile") {
						Debug.Log ("Order Recieved!");
						selectedObject.GetComponent<UnitScript> ().moveTo (hit.collider.GetComponent<TileScriptv2> ().getXCoord (), 
							hit.collider.GetComponent<TileScriptv2> ().getZCoord ());
					}
				}
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

	// button inputs

	public void addToQueueNewCity() {
		if (selectedObject.tag == "City") {
			selectedObject.GetComponent<CityScriptv2> ().AddToQueue (Buildables.Newcity);
		}
	}

	public void toggleGovernor() {
		if (selectedObject.tag.Equals("City"))
			selectedObject.GetComponent<CityScriptv2>().toggleGovernor();
	}

	// get methodology

	public GameObject getSelectedObject() {
		return selectedObject;
	}

}
