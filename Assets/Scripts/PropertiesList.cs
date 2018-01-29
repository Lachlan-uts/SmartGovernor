using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesList {

	public static Property[] properties = new Property[4] {
		new Property ("Coinage", "Gold", 0, 0, true), 
		new Property ("Factory", "Production", 2, 16),
		new Property ("Granary", "Food", 2, 12),
		new Property ("Mine", "Gold", 4, 15)
	};

	public static Property getProperty(int position) {
		return properties [position];
	}

	public static Property[] getList() {
		return properties;
	}

}
