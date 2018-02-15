using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {

	// Note: This will be an abstract resource class for allowing consistent method access and definition
	private string Name;
	private int Value;

	public Resource() {
		Name = "Nothing!";
		Value = 0;
	}

	public Resource(string name) {
		Name = name;
		Value = 0;
	}

	public Resource(string name, int value) {
		Name = name;
		Value = value;
	}

	public string getName() {
		return Name;
	}

	public int getValue() {
		return Value;
	}

	public void setName(string newName) {
		Name = newName;
	}

	public void setValue(int newValue) {
		Value = newValue;
	}
}
