﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property {

	private string Name;
	private string Output;
	private int Value;
	private int Cost;
	private bool ImmediateUse;
	private string unitName;
	public int coordX;
	public int coordZ;

	public Property() {
		Name = "";
		Output = "";
		Value = 1;
		Cost = 1;
		ImmediateUse = false;
		unitName = "";
		coordX = -1;
		coordZ = -1;
	}

	public Property(string newName, string newOutput, int amount) {
		Name = newName;
		Output = newOutput;
		Value = amount;
		Cost = 1;
		ImmediateUse = false;
		unitName = "";
		coordX = -1;
		coordZ = -1;
	}

	public Property(string newName, string newOutput, int amount, int newCost) {
		Name = newName;
		Output = newOutput;
		Value = amount;
		Cost = newCost;
		ImmediateUse = false;
		unitName = "";
		coordX = -1;
		coordZ = -1;
	}

	public Property(string newName, string newOutput, int amount, int newCost, bool use) {
		Name = newName;
		Output = newOutput;
		Value = amount;
		Cost = newCost;
		ImmediateUse = use;
		unitName = "";
		coordX = -1;
		coordZ = -1;
	}

	public Property(string newName, string newOutput, int amount, int newCost, string unit) {
		Name = newName;
		Output = newOutput;
		Value = amount;
		Cost = newCost;
		ImmediateUse = true;
		unitName = unit;
		coordX = -1;
		coordZ = -1;
	}

	public string getName() {
		return Name;
	}

	public string getOutput() {
		return Output;
	}

	public int getValue() {
		return Value;
	}

	public int getCost() {
		return Cost;
	}

	public bool getUse() {
		return ImmediateUse;
	}

	public string getUnitName() {
		return unitName;
	}

//	public int getCoordX() {
//		return coordX;
//	}

//	public int getCoordZ() {
//		return coordZ;
//	}
}
