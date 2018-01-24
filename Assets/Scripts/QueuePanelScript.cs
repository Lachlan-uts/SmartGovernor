using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePanelScript : MonoBehaviour {

	// public variables
	[SerializeField]
	private GameObject ListElement;

	public GameObject CityRef; // Used to store the reference to the city whose queue is being viewed

	private List<Property> LocalQueue; // Used to store equivalent data, ensures that the queue dynamically updates

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateQueue() {
		List<Property> QueueList = CityRef.GetComponent<CityScript> ().getQueue ();

		List<Transform> ItemsInQueue = new List<Transform> ();

		//ItemsInQueue.AddRange(gameObject.GetComponentsInChildren<Transform> ()); // Seems to include the parent object as well

		foreach (Transform child in transform) {
			ItemsInQueue.Add (child);
		}

		Debug.Log ("QLC: " + QueueList.Count + "; IQC: " + ItemsInQueue.Count);

		int iLocalCounter = 0;
		while (iLocalCounter < QueueList.Count || iLocalCounter < ItemsInQueue.Count) {
			if (iLocalCounter <= QueueList.Count - 1) {
				if (QueueList.Count > ItemsInQueue.Count) {
					// Addition of a new queue item, should only happen at the end of the queue, but easier/better to design for all cases
					GameObject newItem = Instantiate (ListElement);
					string NameOfItem = QueueList [iLocalCounter].getName();
					newItem.GetComponent<QueuePanelItemScript> ().setItemName (NameOfItem);
					ItemsInQueue.Add (newItem.transform);
					newItem.transform.parent = gameObject.transform;
				} else if (ItemsInQueue [iLocalCounter].GetComponent<QueuePanelItemScript> ().getItemName () != QueueList [iLocalCounter].getName()){
					// Replacement/Reallocation of item, should only happen at the start of the queue, but easier/better to design for all cases
					ItemsInQueue [iLocalCounter].GetComponent<QueuePanelItemScript> ().setItemName (QueueList [iLocalCounter].getName());
				}
			} else if (iLocalCounter > QueueList.Count - 1) {
				// Removal of item, should only happen at the end of the queue, but easier/better to design for all cases
				ItemsInQueue.RemoveAt(iLocalCounter);
				iLocalCounter--;
			} 



			iLocalCounter++;
		}

	}


}
