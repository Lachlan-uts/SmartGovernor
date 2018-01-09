using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CLIScript : MonoBehaviour {

	// Public variables
	public GameObject City; // Used to select a particular city, for the moment only a singular city needs to be used
	public InputField CLITextInput;
	public Text CLITextOutput;


	// Private variables
	private List<string> CLIStrings;
	private int CLILineCount = 20;

	// Private saving variables
	private string SaveString; // Used for storing the player's actions in a string format, as well as city details


	// Use this for initialization
	void Start () {
		CLIStrings = new List<string>();
		CLIStrings.Add ("Unity CLI Version 0.2a: Awaiting Input...");
		//CLITextInput.ActivateInputField ();
		SaveString = "";
	}
	
	// Update is called once per frame
	void Update () {
		CLITextOutput.text = CompileCLIText();
	}

	// Private methods
	string CompileCLIText() {
		string CLIText = "";
		if (CLIStrings.Count < CLILineCount) {
			foreach (string Line in CLIStrings) {
				CLIText = CLIText + Line + "\n";
			}
		} else {
			for (int CLICount = CLIStrings.Count - CLILineCount; CLICount < CLIStrings.Count; CLICount++) {
				CLIText = CLIText + CLIStrings [CLICount] + "\n";
			}
		}

		return CLIText;
	}


	// Commands
	void Print(string[] CommandParams) {
		string Output = "";
		foreach (string Line in CommandParams) {
			if (!Line.Equals(CommandParams[0])) {
				Output = Output + Line + " ";
			}
		}
		CLIStrings.Add (Output);
	}

	void UpdateCity() {
		City.GetComponent<CityScript> ().CityUpdate ();

		CLIStrings.Add ("City Updated...");
		SaveString += "";
	}

	void AddCitizen() {
		City.GetComponent<CityScript> ().NewCitizen ();

		CLIStrings.Add ("Citizen Added, I guess...");
	}

	void MoveCitizen(int oldPos, int newPos) {
		if (City.GetComponent<CityScript> ().MoveCitizen (oldPos, newPos)) {
			CLIStrings.Add ("Citizen moved from " + oldPos + " to " + newPos + ".");
		} else {
			CLIStrings.Add ("Citizen might not exist, or exists outside the valid range of tiles");
		}
	}

	void GetCitizens() {
		string citizenLocations = City.GetComponent<CityScript> ().getCitizenLocations ();

		CLIStrings.Add ("Locations: " + citizenLocations);
	}

	void GetFoodProduction() {
		int totalFoodOut = City.GetComponent<CityScript> ().getTotalFood ();

		CLIStrings.Add ("Food Production for City: " + totalFoodOut);
	}

	void GetProdProduction() { // Need to think of a better name for "Production"
		int totalProdOut = City.GetComponent<CityScript> ().getTotalProd ();

		CLIStrings.Add ("Production Capacity for City: " + totalProdOut);
	}

	void GetGoldProduction() { // Need to think of a better name for "Production"
		int totalGoldOut = City.GetComponent<CityScript> ().getTotalGold ();

		CLIStrings.Add ("Gold Production for City: " + totalGoldOut);
	}

	void GetQueueOfCity() {
		string StrQueue = "";
		foreach (Property Build in City.GetComponent<CityScript>().getQueue()) {
			StrQueue = StrQueue + Build.getName () + "; ";
		}

		CLIStrings.Add ("Queue: " + StrQueue);
	}

	void ReplaceQueueStart(string nameOfItem) {
		if (City.GetComponent<CityScript> ().ReplaceStartOfQueue (FirstLetterToCapital(nameOfItem))) {
			CLIStrings.Add ("Item added: " + nameOfItem);
		} else {
			CLIStrings.Add ("Item not added: either non-existant or requisites not met!");
		}
	}

	void AddToCityQueue(string nameOfItem) {
		if (City.GetComponent<CityScript> ().AddToQueue (FirstLetterToCapital(nameOfItem))) {
			CLIStrings.Add ("Item added: " + nameOfItem);
		} else {
			CLIStrings.Add ("Item not added: either non-existant or requisites not met!");
		}
	}

	void RemoveLastItemFromCityQueue() {
		City.GetComponent<CityScript> ().RemoveFromQueue ();

		CLIStrings.Add ("Last Item removed...");
	}

	// Private methods for working out input commands

	string FirstLetterToCapital(string str) {
		if (str == null) {
			return null;
		} else if (str.Length > 1) {
			return char.ToUpper (str [0]) + str.Substring(1);
		}
		return str.ToUpper ();
	}

	// Public methods
	public void command() {
		string InputCommand = CLITextInput.text;
		string[] CommandParams = InputCommand.Split(' ');
		//CLIStrings.Add ("Command: " + InputCommand);

		switch (CommandParams [0].ToLower()) {
		case "move":
			int x1, x2 = 0;
			bool x1d = int.TryParse(CommandParams [1], out x1);
			bool x2d = int.TryParse(CommandParams [2], out x2);
			if ((x1d) && (x2d)) {
				MoveCitizen (x1, x2);
			}
			break;
		case "update":
			UpdateCity ();
			break;
		case "remove":
			RemoveLastItemFromCityQueue ();
			break;
		case "add":
			AddToCityQueue (CommandParams [1].ToLower ());
			break;
		case "replace":
			ReplaceQueueStart (CommandParams [1].ToLower ());
			break;
		case "getcitizens":
			GetCitizens ();
			break;
		case "newcitizen":
			AddCitizen ();
			break;
		case "getqueue":
			GetQueueOfCity ();
			break;
		case "getgold":
			GetGoldProduction ();
			break;
		case "getprod":
			GetProdProduction ();
			break;
		case "getfood":
			GetFoodProduction ();
			break;
		case "print":
			Print (CommandParams);
			break;
		default:
			CLIStrings.Add ("Command: " + InputCommand);
			break;
		}


		CLITextInput.text = "";
		CLITextInput.ActivateInputField ();
	}

	public void pressInputField() {
		CLITextInput.ActivateInputField ();
	}




}
