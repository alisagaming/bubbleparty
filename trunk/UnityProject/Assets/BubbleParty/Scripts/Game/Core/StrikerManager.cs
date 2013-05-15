using UnityEngine;
using System.Collections;

public class StrikerManager : MonoBehaviour 
{
    GameObject striker;
    Striker strikerScript;
    //public GameObject[] specialStrikerPrefabs;
	
	public GameObject prefabFireball;
	public GameObject prefabPlazma;
	public Tracer tracer;
	public tk2dAnimatedSprite character;

    Transform currentStrikerPosition;
    Transform nextStrikerPosition;

    GameObject currentStrikerObject;
    GameObject nextStrikerObject;
    //Transform thresoldLineTransform;
	
	PowerType nextStrikerType = PowerType.None;
	PowerType currentStrikerType = PowerType.None;
	
	void Start () 
    {
       // return;
        //thresoldLineTransform = GameObject.Find("Thresold Line").transform;
        striker = GameObject.Find("Striker");
        strikerScript = striker.GetComponent<Striker>();

        currentStrikerPosition = GameObject.Find("Current Striker Position").transform;
        nextStrikerPosition = GameObject.Find("Next Striker Position").transform;

        
		//nextStrikerType = PowerType.Plazma;		

        //Invoke("GenerateNextStriker", .1f);
		//GenerateNextStriker();
		//Invoke("GenerateStriker", 1f);
	
	}
	
	public void Restart(){
		Utils.DestroyAllChild(transform);
		strikerScript.Restart();
		GenerateNextStriker();
		Invoke("GenerateStriker", 0.1f);	
	}
	
	internal void SetNextStrikerType(PowerType type){
		nextStrikerType = type;
	}
	
    /*void UpdateThresoldPosition()
    {
        //thresoldLineTransform.position = new Vector3(thresoldLineTransform.position.x, currentStrikerPosition.position.y + .6f, thresoldLineTransform.position.z);
    }*/

    internal bool isFirstObject = true;
	bool inGenerate;

    internal void GenerateStriker()
    {      
		if(inGenerate) return;
		inGenerate = true;
		striker.transform.position = currentStrikerPosition.position;
		
		currentStrikerObject = nextStrikerObject;
		iTween.MoveTo(nextStrikerObject.gameObject, currentStrikerPosition.position, 0.1f);
        
		currentStrikerObject.transform.parent = striker.transform;
        if (isFirstObject)
        {
            currentStrikerObject.transform.localPosition = Vector3.zero;
            isFirstObject = false;
        }
        //strikerScript.currentStrikerObject = currentStrikerObject;
		//currentStrikerType = nextStrikerType;
		strikerScript.PrepareStriker(currentStrikerType, currentStrikerObject);
		//GenerateNextStriker();
		character.Play("get_bubble");	
		CancelInvoke("GenerateNextStriker");        
        Invoke("GenerateNextStriker", .3f);    
    }
	

    void GenerateNextStriker()
    {
		inGenerate = false;
		//yield return new WaitForSeconds(0.3f);
		//yield return new WaitForSeconds(0);
		
        /*ArrayList remainingObjects =  InGameScriptRefrences.playingObjectManager.GetRemainingObjectsNames();
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
			}else if(nextStrikerType == PowerType.Plazma){
				nextStrikerObject = (GameObject)Instantiate(prefabPlazma, nextStrikerPosition.position, Quaternion.identity);
			}else{
				int index = Random.Range(0, objectCount);
            	nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
			}
        }*/
		
		int objectCount = InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs.Length;
            
			if(nextStrikerType == PowerType.Fireball){
				nextStrikerObject = (GameObject)Instantiate(prefabFireball, nextStrikerPosition.position, Quaternion.identity);
			}else if(nextStrikerType == PowerType.Plazma){
				nextStrikerObject = (GameObject)Instantiate(prefabPlazma, nextStrikerPosition.position, Quaternion.identity);
			}else{
				GameObject prefab = InGameScriptRefrences.playingObjectManager.GetRandomBubble();
				if(prefab == null) prefab = InGameScriptRefrences.playingObjectGeneration.GetRandomBubble();
				nextStrikerObject = (GameObject)Instantiate(prefab, nextStrikerPosition.position, Quaternion.identity);
				nextStrikerObject.GetComponent<PlayingObject>().bubblePrefab = prefab;
				//int index = Random.Range(0, objectCount);
            	//nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
			}
		
		
		nextStrikerObject.transform.parent = transform;

        nextStrikerObject.tag = "Striker";
        nextStrikerObject.GetComponent<SphereCollider>().enabled = false;
        nextStrikerObject.GetComponent<SphereCollider>().radius *= .8f;
        iTween.PunchScale(nextStrikerObject, new Vector3(.2f, .2f, .2f), 1f);
		currentStrikerType = nextStrikerType;
		nextStrikerType = PowerType.None;        
    }
	
	internal void ResetStriker(){
		striker.transform.position = currentStrikerPosition.position;
		//iTween.MoveTo(striker.gameObject, currentStrikerPosition.position, .1f);
	}
	
	//Vector3 up = new Vector3(0,1,0);

    internal void Shoot(Vector3 touchedPosition)
    {
		//if(currentStrikerPosition.position != currentStrikerObject.transform.position) return;
		if(inGenerate) return;
        if (GameUIController.isgameFinish == true)
            return;

        if (strikerScript.isBusy)
            return;

        //if (touchedPosition.y < thresoldLineTransform.position.y)
        //    return;

        //iTween.Stop(nextStrikerObject);

        InGameScriptRefrences.soundFxManager.PlayShootingSound();

        Vector3 dir = touchedPosition - currentStrikerPosition.position;
		dir.z = 0;
		
		//Vector3 relativeUp = currentStrikerPosition.TransformDirection (Vector3.forward);
		//currentStrikerPosition.up = up;
		//currentStrikerPosition.rotation = Quaternion.LookRotation(dir,currentStrikerPosition.up);
		//currentStrikerPosition.LookAt(touchedPosition);
		//currentStrikerPosition.up = up;
		
		
		dir.Normalize();		
        strikerScript.Shoot(dir);
        
		
		float rotZ = Mathf.Atan2(-dir.x,dir.y) * Mathf.Rad2Deg;
		currentStrikerPosition.rotation = Quaternion.Euler(0f,0f,rotZ);
		
		
        //iTween.MoveTo(nextStrikerObject.gameObject, currentStrikerPosition.position, .4f);
    }
	
	internal void OnMouseDrag(Vector3 touchedPosition)
    {
        if (GameUIController.isgameFinish == true)
            return;

        if (strikerScript.isBusy)
            return;
		
		Vector3 dir = touchedPosition - currentStrikerPosition.position;
		dir.z = 0;
		dir.Normalize();	
		
		float rotZ = Mathf.Atan2(-dir.x,dir.y) * Mathf.Rad2Deg;
		currentStrikerPosition.rotation = Quaternion.Euler(0f,0f,rotZ);
		//tracer.gameObject.SetActive(true);
		tracer.SetVisible();
	}
	
	
}
