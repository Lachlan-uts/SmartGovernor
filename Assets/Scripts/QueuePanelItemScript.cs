using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueuePanelItemScript : MonoBehaviour {

	// serialized private variables
	[SerializeField]
	private Text ItemName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setItemName(string Name) {
		ItemName.text = Name;
	}

	public string getItemName() {
		return ItemName.text;
	}

	public void DeleteSelf() {
		Destroy (this);
	}
}
