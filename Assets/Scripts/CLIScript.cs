using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CLIScript : MonoBehaviour {

	// Public variables
	public GameObject City; // Used to select a particular city, for the moment only a singular city needs to be used
	public InputField CLITextInput;
	public Text CLITextOutput;
	public GameObject QueuePanel; // Used to reference the queue, by default does nothing unless the queue panel is assigned


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
		//SaveString = "" + Random.state + ":";
		SaveString = "";
		Debug.Log ("SaveString: " + SaveString);
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

	// Save/Load stuff
	void WriteTo() {
		string path = "Assets/Resources/test.txt";
		System.IO.File.WriteAllText (path, "");
		StreamWriter writer = new StreamWriter (path, true);
		//writer.Flush ();
		writer.WriteLine (SaveString);
		writer.Close ();
	}

	string LoadFrom() {
		SaveString = "";
		string path = "Assets/Resources/test.txt";
		StreamReader reader = new StreamReader (path);
		string returnString = reader.ReadLine ();
		reader.Close ();
		return returnString;
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
		City.GetComponent<CityScriptv2> ().CityUpdate ();

		CLIStrings.Add ("City Updated...");
		//SaveString += "";
	}

	void AddCitizen() {
		City.GetComponent<CityScriptv2> ().NewCitizen ();

		CLIStrings.Add ("Citizen Added, I guess...");
	}

	void MoveCitizen(int oldPos, int newPos) {
		if (City.GetComponent<CityScript> ().MoveCitizen (oldPos, newPos)) {
			CLIStrings.Add ("Citizen moved from " + oldPos + " to " + newPos + ".");
		} else {
			CLIStrings.Add ("Citizen might not exist, or exists outside the valid range of tiles");
		}
	}

	void MoveCitizenv2(int oldX, int oldZ, int newX, int newZ) {
		if (City.GetComponent<CityScriptv2>().MoveCitizen(oldX,oldZ,newX,newZ)) {
			CLIStrings.Add ("Citizen moved from " + oldX + "," + oldZ + " to " + newX + "," + newZ + ".");
		} else {
			CLIStrings.Add ("Citizen might not exist, or exists outside the valid range of tiles");
		}
	}

	void GetCitizens() {
		string citizenLocations = City.GetComponent<CityScriptv2> ().getCitizenLocations ();

		CLIStrings.Add ("Locations: " + citizenLocations);
	}

	void GetFoodProduction() {
		int totalFoodOut = City.GetComponent<CityScriptv2> ().getTotalResource ("Food");

		CLIStrings.Add ("Food Production for City: " + totalFoodOut);
	}

	void GetProdProduction() { // Need to think of a better name for "Production"
		int totalProdOut = City.GetComponent<CityScriptv2> ().getTotalResource ("Production");

		CLIStrings.Add ("Production Capacity for City: " + totalProdOut);
	}

	void GetGoldProduction() { // Need to think of a better name for "Production"
		int totalGoldOut = City.GetComponent<CityScriptv2> ().getTotalResource ("Gold");

		CLIStrings.Add ("Gold Production for City: " + totalGoldOut);
	}

	void GetQueueOfCity() {
		string StrQueue = "";
		foreach (Property Build in City.GetComponent<CityScriptv2>().getQueue()) {
			StrQueue = StrQueue + Build.getName () + "; ";
		}

		CLIStrings.Add ("Queue: " + StrQueue);
	}

	void ReplaceQueueStart(string nameOfItem) {
		if (City.GetComponent<CityScriptv2> ().ReplaceStartOfQueue (FirstLetterToCapital(nameOfItem))) {
			CLIStrings.Add ("Item added: " + nameOfItem);
		} else {
			CLIStrings.Add ("Item not added: either non-existant or requisites not met!");
		}
	}

	void ReplaceQueueStart(string nameOfItem, int xCoord, int zCoord) {
		if (City.GetComponent<CityScriptv2> ().ReplaceStartOfQueue (FirstLetterToCapital(nameOfItem), xCoord, zCoord)) {
			CLIStrings.Add ("Item added: " + nameOfItem);
		} else {
			CLIStrings.Add ("Item not added: either non-existant or requisites not met!");
		}
	}

	void AddToCityQueue(string nameOfItem) {
		if (City.GetComponent<CityScriptv2> ().AddToQueue (FirstLetterToCapital(nameOfItem))) {
			CLIStrings.Add ("Item added: " + nameOfItem);
		} else {
			CLIStrings.Add ("Item not added: either non-existant or requisites not met!");
		}
	}

	void AddToCityQueue(string nameOfItem, int xCoord, int zCoord) {
		if (City.GetComponent<CityScriptv2> ().AddToQueue (FirstLetterToCapital(nameOfItem), xCoord, zCoord)) {
			CLIStrings.Add ("Item added: " + nameOfItem);
		} else {
			CLIStrings.Add ("Item not added: either non-existant or requisites not met!");
		}
	}

	void RemoveLastItemFromCityQueue() {
		City.GetComponent<CityScriptv2> ().RemoveFromQueue ();

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
		bool addToSS = true;
		string InputCommand = CLITextInput.text;
		string[] CommandParams = InputCommand.Split(' ');
		//CLIStrings.Add ("Command: " + InputCommand);

		switch (CommandParams [0].ToLower()) {
		case "save":
			WriteTo ();
			addToSS = false;
			break;
		case "load":
			if (CommandParams.Length > 1) {
				int curTurns = 0;
				int maxTurns = 0;
				int.TryParse (CommandParams [1], out maxTurns);
				string LoadedString = LoadFrom ();
				string[] newCommandParams = LoadedString.Split (':');
				foreach (string commandPhrase in newCommandParams) {
					if (curTurns <= maxTurns) {
						command (commandPhrase);
					}

					if ((commandPhrase.Split(' '))[0] == "update" ) {
						curTurns++;
					}
				}
			} else {
				string LoadedString = LoadFrom ();
				string[] newCommandParams = LoadedString.Split (':');
				foreach (string commandPhrase in newCommandParams) {
					command (commandPhrase);
				}
			}
			addToSS = false;
			break;
		case "move":
			int x1, x2 = 0;
			bool x1d = int.TryParse(CommandParams [1], out x1);
			bool x2d = int.TryParse(CommandParams [2], out x2);
			if ((x1d) && (x2d)) {
				MoveCitizen (x1, x2);
			}
			break;
		case "movecoords":
			int y1, y2, z1, z2 = 0;
			string coord1 = CommandParams [1];
			string coord2 = CommandParams [2];
			string[] coords1 = coord1.Split ('/');
			string[] coords2 = coord2.Split ('/');
			if (int.TryParse (coords1[0], out y1) && int.TryParse (coords1[1], out z1) && int.TryParse (coords2[0], out y2) && int.TryParse (coords2[1], out z2))
				MoveCitizenv2 (y1, z1, y2, z2);
			break;
		case "update":
			UpdateCity ();
			break;
		case "remove":
			RemoveLastItemFromCityQueue ();
			break;
		case "add":
			if (CommandParams.Length > 2) {
				int xCo, zCo = -1;
				if ((int.TryParse (CommandParams [2], out xCo)) && (int.TryParse (CommandParams [3], out zCo))) {
					AddToCityQueue (CommandParams [1].ToLower (), xCo, zCo);
				}
			} else {
				AddToCityQueue (CommandParams [1].ToLower ());
			}
			break;
		case "replace":
			if (CommandParams.Length > 2) {
				int xCo, zCo = -1;
				if ((int.TryParse (CommandParams [2], out xCo)) && (int.TryParse (CommandParams [3], out zCo))) {
					ReplaceQueueStart (CommandParams [1].ToLower (), xCo, zCo);
				}
			} else {
				ReplaceQueueStart (CommandParams [1].ToLower ());
			}
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
			addToSS = false;
			break;
		}

		if (QueuePanel != null) {
			QueuePanel.GetComponent<QueuePanelScript> ().UpdateQueue ();
		}

		if (addToSS) {
			SaveString = SaveString + ":" + InputCommand;
		}
		Debug.Log (SaveString);

		CLITextInput.text = "";
		CLITextInput.ActivateInputField ();
	}

	// Alternative 'command', used normally through loading. Likewise, cannot execute save/load functionality by design
	public void command(string InputCommand) {
		bool addToSS = true;
		//string InputCommand = CLITextInput.text;
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
			addToSS = false;
			break;
		}

		if (QueuePanel != null) {
			QueuePanel.GetComponent<QueuePanelScript> ().UpdateQueue ();
		}

		if (addToSS) {
			SaveString = SaveString + ":" + InputCommand;
		}
		Debug.Log (SaveString);

		CLITextInput.text = "";
		CLITextInput.ActivateInputField ();
	}

	public void pressInputField() {
		CLITextInput.ActivateInputField ();
	}




}
