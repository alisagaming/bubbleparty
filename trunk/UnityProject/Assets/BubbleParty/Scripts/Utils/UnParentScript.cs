using UnityEngine;
using System.Collections;

public class UnParentScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        gameObject.transform.parent = null;
	
	}
	
	
}
