using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour 
{
	float minY;
	
	void Start()
    {
		minY = transform.position.y - transform.localScale.y/2;
	}

    void OnMouseDown()
    {
        //Vector2 pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //InGameScriptRefrences.strikerManager.Shoot(pos);
    }
	
	void OnMouseDrag(){
		Vector2 pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		if(pos.y < minY) pos.y = minY;
        InGameScriptRefrences.strikerManager.OnMouseDrag(pos);
	}
	
	void OnMouseUp(){
	    Vector2 pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if(pos.y < minY) pos.y = minY;
        InGameScriptRefrences.strikerManager.Shoot(pos);
    }
}
