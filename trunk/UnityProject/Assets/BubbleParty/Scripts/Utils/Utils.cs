using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils  {
	public static void DestroyAllChild(Transform inTransform){
		var children = new List<GameObject>();
		foreach (Transform child in inTransform) children.Add(child.gameObject);
		children.ForEach(child => Object.Destroy(child));
	}
}
