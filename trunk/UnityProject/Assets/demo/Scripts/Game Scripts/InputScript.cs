using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour 
{

    void OnMouseDown()
    {
        Vector2 pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        InGameScriptRefrences.strikerManager.Shoot(pos);
    }
}
