using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagementScript : MonoBehaviour {

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
	}
}
