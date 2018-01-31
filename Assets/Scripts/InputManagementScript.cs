using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagementScript : MonoBehaviour {

	// serialized private variable
	[SerializeField]
	private GameObject City;
	[SerializeField]
	private GameObject MCamera;
	[SerializeField]
	private float MCSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (KeyCode.BackQuote)) {
			GameObject CLIManager;
			if (CLIManager = GameObject.FindGameObjectWithTag("CLI")) {
				CLIManager.GetComponent<CLIScript>().pressInputField();
			}
		}

		if (MCamera != null) {
			float relZChange = Input.GetAxis ("Vertical") * MCSpeed * Time.deltaTime;
			float relXChange = Input.GetAxis ("Horizontal") * MCSpeed * Time.deltaTime;

			MCamera.transform.Translate (new Vector3 (relXChange, 0.0f, relZChange));

			if (Input.GetAxis ("Mouse ScrollWheel") > 0.01f) {
				MCamera.transform.Translate (new Vector3 (0.0f, -(0.2f * MCSpeed), (0.2f * MCSpeed)));
			} else if (Input.GetAxis ("Mouse ScrollWheel") < -0.01f) {
				MCamera.transform.Translate (new Vector3 (0.0f, (0.2f * MCSpeed), -(0.2f * MCSpeed)));
			}

		}


	}

	// public methods
	public void colonize(Vector3 position, int XCoord, int ZCoord) {
		GameObject newCity = Instantiate (City, position, Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f)));
		Debug.Log ("City Instantiated");
		newCity.GetComponent<CityScript> ().setCoordinates (XCoord, ZCoord);
	}

	public void colonize(Vector3 position, int XCoord, int ZCoord, bool ownership) {
		GameObject newCity = Instantiate (City, position, Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f)));
		Debug.Log ("City Instantiated");
		newCity.GetComponent<CityScript> ().setCoordinates (XCoord, ZCoord);
		if (!ownership) {
			newCity.GetComponent<CityScript> ().changeOwnership();
		}
		GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ().newCityMade (newCity, ownership);
	}

}
