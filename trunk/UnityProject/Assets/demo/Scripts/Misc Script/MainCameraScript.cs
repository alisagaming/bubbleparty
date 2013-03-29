
	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCameraScript : MonoBehaviour {
    
    void Update () {
        

        RaycastHit hit;
        
        foreach (Touch evt in Input.touches)
        {
        	Ray ray = camera.ScreenPointToRay (Input.GetTouch(0).position);
            if (evt.phase == TouchPhase.Began)
            {
               	if (Physics.Raycast (ray,out hit)) 
            	{
                	hit.transform.gameObject.SendMessage("OnMouseDown");
                }
            }    
            
        }       
    }
 }

