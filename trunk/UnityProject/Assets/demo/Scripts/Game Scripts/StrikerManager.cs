using UnityEngine;
using System.Collections;

public class StrikerManager : MonoBehaviour 
{
    GameObject striker;
    Striker strikerScript;
    //public GameObject[] specialStrikerPrefabs;
	
	public GameObject prefabFireball;

    Transform currentStrikerPosition;
    Transform nextStrikerPosition;

    GameObject currentStrikerObject;
    GameObject nextStrikerObject;
    Transform thresoldLineTransform;
	
	PowerType nextStrikerType = PowerType.None;
	PowerType currentStrikerType = PowerType.None;

	void Start () 
    {
       // return;
        thresoldLineTransform = GameObject.Find("Thresold Line").transform;
        striker = GameObject.Find("Striker");
        strikerScript = striker.GetComponent<Striker>();

        currentStrikerPosition = GameObject.Find("Current Striker Position").transform;
        nextStrikerPosition = GameObject.Find("Next Striker Position").transform;

        GenerateNextStriker();

        Invoke("UpdateThresoldPosition", .2f);
        Invoke("GenerateStriker", .2f);
	
	}
	
	internal void SetNextStrikerType(PowerType type){
		nextStrikerType = type;
	}
	
    void UpdateThresoldPosition()
    {
        thresoldLineTransform.position = new Vector3(thresoldLineTransform.position.x, currentStrikerPosition.position.y + .6f, thresoldLineTransform.position.z);
    }

    internal bool isFirstObject = true;


    internal void GenerateStriker()
    {        
        striker.transform.position = currentStrikerPosition.position;
		
		currentStrikerObject = nextStrikerObject;
		iTween.MoveTo(nextStrikerObject.gameObject, currentStrikerPosition.position, .1f);
        
		currentStrikerObject.transform.parent = striker.transform;
        if (isFirstObject)
        {
            currentStrikerObject.transform.localPosition = Vector3.zero;
            isFirstObject = false;
        }
        strikerScript.currentStrikerObject = currentStrikerObject;
		strikerScript.PrepareStriker(currentStrikerType);
		currentStrikerType = nextStrikerType;
		GenerateNextStriker();
        nextStrikerType = PowerType.None;
        //Invoke("GenerateNextStriker", .1f);       

    }

    void GenerateNextStriker()
    {
        ArrayList remainingObjects =  InGameScriptRefrences.playingObjectManager.GetRemainingObjectsNames();
        if (remainingObjects != null)
        {
            int index = Random.Range(0, remainingObjects.Count);
            nextStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[index], nextStrikerPosition.position, Quaternion.identity);
        }
        else
        {
            int objectCount = InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs.Length;
            
			if(nextStrikerType == PowerType.Fireball){
				nextStrikerObject = (GameObject)Instantiate(prefabFireball, nextStrikerPosition.position, Quaternion.identity);
			}else{
				int index = Random.Range(0, objectCount);
            	nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
			}
        }

        nextStrikerObject.tag = "Striker";
        nextStrikerObject.GetComponent<SphereCollider>().enabled = false;
        nextStrikerObject.GetComponent<SphereCollider>().radius *= .8f;
        iTween.PunchScale(nextStrikerObject, new Vector3(.2f, .2f, .2f), 1f);
    }
	
	internal void ResetStriker(){
		//striker.transform.position = currentStrikerPosition.position;
		iTween.MoveTo(striker.gameObject, currentStrikerPosition.position, .01f);
	}
	
    internal void Shoot(Vector3 touchedPosition)
    {
        if (GameUIController.isgameFinish == true)
            return;

        if (strikerScript.isBusy)
            return;

        if (touchedPosition.y < thresoldLineTransform.position.y)
            return;

        //iTween.Stop(nextStrikerObject);

        InGameScriptRefrences.soundFxManager.PlayShootingSound();

        Vector3 dir = touchedPosition - currentStrikerPosition.position;
		dir.z = 0;
		dir.Normalize();		
        strikerScript.Shoot(dir);
        

        //iTween.MoveTo(nextStrikerObject.gameObject, currentStrikerPosition.position, .4f);
    }
}
