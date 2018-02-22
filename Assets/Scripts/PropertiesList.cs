using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PropertiesList {

	//Newest Dictionary method.
	public static Dictionary<Buildables,Property> dictProperties = new Dictionary<Buildables, Property> {
		{ Buildables.Coinage, new Property ("Coinage", "Gold", 0, 0, true) },
		{ Buildables.Factory, new Property ("Factory", "Production", 2, 16) },
		{ Buildables.Granary, new Property ("Granary", "Food", 2, 12) },
		{ Buildables.Mine, new Property ("Mine", "Gold", 4, 15) },
		{ Buildables.Testunit, new Property ("Testunit", "none", 0, 20, "TestUnit") },
		{ Buildables.Newcity, new Property ("Newcity", "none", 0, 40, "newCity") }
	};

	//New Dictionary methods.
	public static Dictionary<string,Property> dictPropertiesString = new Dictionary<string, Property> {
		{ "Coinage", new Property ("Coinage", "Gold", 0, 0, true) },
		{ "Factory", new Property ("Factory", "Production", 2, 16) },
		{ "Granary", new Property ("Granary", "Food", 2, 12) },
		{ "Mine", new Property ("Mine", "Gold", 4, 15) },
		{ "Testunit", new Property ("Testunit", "none", 0, 20, "TestUnit") },
		{ "Newcity", new Property ("Newcity", "none", 0, 40, "newCity") }
	};

//	public static T getProperty<T>(string propertyName) {
//		return dictProperties.TryGetValue (PropertyName);
//	}


	//old Array methods.
	public static Property[] properties = new Property[6] {
		new Property ("Coinage", "Gold", 0, 0, true), 
		new Property ("Factory", "Production", 2, 16),
		new Property ("Granary", "Food", 2, 12),
		new Property ("Mine", "Gold", 4, 15),
		new Property ("Testunit", "none", 0, 20, "TestUnit"),
		new Property ("Newcity", "none", 0, 40, "newCity")};

	public static Property getProperty(int position) {
		return properties [position];
	}

	public static Property[] getList() {
		return properties;
	}

}
