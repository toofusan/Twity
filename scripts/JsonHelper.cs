using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class JsonHelper {

	public static string ArrayToObject (string arrayString) {
		if (arrayString.StartsWith ("[")) {
			arrayString = "{\"items\":" + arrayString + "}";
			return arrayString;
		} else {
			return arrayString;
		}
	}
}
