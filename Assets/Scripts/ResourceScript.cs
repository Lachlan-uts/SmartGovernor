using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour {

	// Note: This will be an abstract resource class for allowing consistent method access and definition
	[SerializeField]
	private string Name = "Nothing!";
	private int Value = 0;

	public string getName() {
		return Name;
	}

	public int getValue() {
		return Value;
	}

	public void setName(string newName) {
		Name = newName;
	}

	public void setName(int newValue) {
		Value = newValue;
	}
}
