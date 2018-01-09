using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen {

	private int position;

	public Citizen () {
		position = 0;
	}

	public Citizen (int newPos) {
		position = newPos;
	}

	public void moveTo(int newPos) {
		position = newPos;
	}

	public int getPosition() {
		return position;
	}

}
