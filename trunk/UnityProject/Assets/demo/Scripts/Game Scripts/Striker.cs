using UnityEngine;
using System.Collections;

public enum PowerType
{
    None,
	Fireball,
	Plazma
};

public class Striker : MonoBehaviour 
{
    public PowerType currentPowerType;
    Vector3 currentMovingDirection = Vector3.zero;
    float speed;
    Transform myTransform;

    Transform sliderTransform;
	Vector3 oldPosition;

    internal bool isBusy = false;
    GameObject currentStrikerObject = null;
    public TrailRenderer trail;
	float radius;
	public LayerMask layerMask;
	
	public Transform boundTop;
	public Transform boundDown;
	public Transform boundLeft;
	public Transform boundRight;
	public Transform plazmaBallExpand;
	
	int strikeCount = 0;
	
	public GameObject strikerFireball;
	
	void Start()
    {
        currentPowerType = PowerType.None;
        myTransform = transform;
    }
	
	public void Restart(){
		if(currentStrikerObject != null){
			Destroy(currentStrikerObject);
			currentStrikerObject = null;
		}
	}
	
    internal void Shoot(Vector3 dir)
    {
		if(currentStrikerObject == null) return;
		strikeCount = 0;
        trail.time = 0.3f;
        
        speed = 3000;//10f;  //10
        rigidbody.isKinematic = false;
        currentMovingDirection = dir;
        isBusy = true;
		
		radius = currentStrikerObject.GetComponent<SphereCollider>().radius;//*.5f;		
		if(currentPowerType == PowerType.None) radius *= .5f;
		//layerMask = currentStrikerObject.GetComponent<PlayingObject>().layerMask;
		
		//rigidbody.AddForce(dir*speed*20);
		
      //  Invoke("FreeStriker", 1f);
    }

    void EnableTrail()
    {
        trail.time = 0.3f;
    }

    private void FreeStriker(GameObject collidedObject)
    {
        trail.time = 0f;        
        rigidbody.isKinematic = true;        

        currentStrikerObject.GetComponent<SphereCollider>().enabled = true;
        currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
        currentStrikerObject.tag = "Playing Object";
        currentStrikerObject.GetComponent<PlayingObject>().AdjustPosition(hit);//,oldPosition);
        currentStrikerObject.GetComponent<PlayingObject>().burnPrefab = InGameScriptRefrences.playingObjectGeneration.burnPrefab;
        
		currentStrikerObject = null;
		
        isBusy = false;
        currentPowerType = PowerType.None;
        InGameScriptRefrences.strikerManager.GenerateStriker();
    }
	
	private IEnumerator FreePlazma(GameObject collidedObject)
    {
        trail.time = 0f;        
        rigidbody.isKinematic = true;        
		
		Destroy(currentStrikerObject);
		currentStrikerObject = null;
		
        /*currentStrikerObject.GetComponent<SphereCollider>().enabled = true;
        currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
        currentStrikerObject.tag = "Playing Object";
        currentStrikerObject.GetComponent<PlayingObject>().AdjustPosition(hit);//,oldPosition);
        */
        
		plazmaBallExpand.gameObject.SetActive(true);
		plazmaBallExpand.transform.localScale = new Vector3(0,0,0);
		
		iTween.ScaleTo(plazmaBallExpand.gameObject, new Vector3(1,1,1), 0.5f);
		yield return new WaitForSeconds(0.5f);
		InGameScriptRefrences.playingObjectManager.UpdatePlayingObjectsList();
		plazmaBallExpand.gameObject.SetActive(false);
		
		
		PlayingObject[] allObj = InGameScriptRefrences.playingObjectManager.allPlayingObjectScripts;
		Vector3 currPos = transform.position;
		PlayingObject[] touchObj = new PlayingObject[3];
			
		for (int i = 0; i < allObj.Length; i++)
        {
			if(allObj[i] != null){
	            Vector3 objPos = allObj[i].transform.position;
				float dirt = (objPos - currPos).sqrMagnitude;
				if(dirt < 6084){
					for(int x=0;x<3;x++){
						if(touchObj[x] == null){
							touchObj[x] = allObj[i];
							break;
						}else //if(touchObj[x].name.Equals(allObj[i].name)) break;
							if(touchObj[x].name==allObj[i].name) break;
					}
				}
			}
        }
		
		
		for(int x=0;x<3;x++){
			if(touchObj[x]!=null)
				for(int y=0;y<allObj.Length; y++){
					if(allObj[y] != null)
						if(touchObj[x].name == allObj[y].name) allObj[y].PlazmaMe();							
				}
		}		
		
		yield return new WaitForSeconds(5f);
				
		InGameScriptRefrences.playingObjectManager.CheckForObjectsFall();
		
        isBusy = false;
        currentPowerType = PowerType.None;
        InGameScriptRefrences.strikerManager.GenerateStriker();
    }
	
	private void FreeFireBall(){
		trail.time = 0f; 
		rigidbody.isKinematic = true;        
		Destroy(currentStrikerObject);
		currentStrikerObject = null;
				
		InGameScriptRefrences.playingObjectManager.CheckForObjectsFall();
		
		isBusy = false;
        currentPowerType = PowerType.None;
        InGameScriptRefrences.strikerManager.GenerateStriker();
	}

	RaycastHit hit = new RaycastHit();
		
	void FixedUpdate()
    {
        if ((isBusy == false) || (currentStrikerObject==null))
            return;

        //speed += 2000 * Time.deltaTime;

		oldPosition = myTransform.localPosition;
		
		float dist = speed * Time.fixedDeltaTime;//deltaTime;
		
		if(currentPowerType == PowerType.None){		
			if (Physics.SphereCast(myTransform.position,radius,currentMovingDirection,out hit, dist, layerMask)){
				//dist = dist;
				myTransform.Translate(currentMovingDirection * hit.distance);
				//OnCollisionHit();
				
				Collider other = hit.collider;
        		if (other.gameObject.tag == "Playing Object" && isBusy)
		        {
		            FreeStriker(other.gameObject);           	
		        }
				
				return;
			} 
		}else if(currentPowerType == PowerType.Plazma){
			if (Physics.SphereCast(myTransform.position,radius,currentMovingDirection,out hit, dist, layerMask)){
				myTransform.Translate(currentMovingDirection * hit.distance);
				Collider other = hit.collider;
	        	if (other.gameObject.tag == "Playing Object" && isBusy)
			    {
			          StartCoroutine(FreePlazma(other.gameObject));           	
			    }
				return;
			}				
		}else{
			RaycastHit[] hints = Physics.SphereCastAll(myTransform.position,radius,currentMovingDirection, dist, layerMask);
			foreach(RaycastHit hint in hints){
				hint.collider.GetComponent<PlayingObject>().BurnMe();
			}
		}
		myTransform.Translate(currentMovingDirection * dist);
		
		Vector3 pos = myTransform.position;
		
		if(pos.x<boundLeft.position.x){
			pos.x = boundLeft.position.x;
			currentMovingDirection.x = - currentMovingDirection.x;
			strikeCount++;
		}
		if(pos.x>boundRight.position.x){
			pos.x = boundRight.position.x;
			currentMovingDirection.x = - currentMovingDirection.x;
			strikeCount++;
		}
		if(pos.y>boundTop.position.y){
			pos.y = boundTop.position.y;
			currentMovingDirection.y = - currentMovingDirection.y;
			strikeCount++;
		}
		myTransform.position = pos;
		if((pos.y<boundDown.position.y) && (currentMovingDirection.y<0)){
			if(currentPowerType == PowerType.Fireball){
				FreeFireBall();
			}else{			
				rigidbody.isKinematic = true;
	            isBusy = false;
	            trail.time = 0f;
				pos.y = boundDown.position.y;
	            InGameScriptRefrences.strikerManager.ResetStriker();
			}
		}
		
		if((strikeCount>2) && (currentPowerType == PowerType.Fireball)) FreeFireBall();
		
		
		/*if(rigidbody.SweepTest(currentMovingDirection,out hit, dist)){
			dist = dist;
		}*/
		
		/*float dx = 1;
		
		while(dist>dx){
        	//myTransform.Translate(currentMovingDirection * dx);
			dist -= dx;
			rigidbody.MovePosition(rigidbody.position + currentMovingDirection * dx);
		}
		//if(dist>0)	myTransform.Translate(currentMovingDirection * dist);*/	
    }
	
	void OnCollisionHit()
    {
		if(true) return;
		Collider other = hit.collider;
        /*if (other.gameObject.name == "Left" || other.gameObject.name == "Right" || other.gameObject.name == "Top")
        {
            InGameScriptRefrences.soundFxManager.PlayWallCollisionSound();
            currentMovingDirection = Vector3.Reflect(currentMovingDirection, hit.normal).normalized;           
        }
		
        /*if ((other.gameObject.name == "Top" || other.gameObject.name == "Top Down")  && isBusy)
        {
            rigidbody.isKinematic = true;
            Destroy(currentStrikerObject);
            isBusy = false;
            trail.time = 0f;
            hasBombPierced = false;
            InGameScriptRefrences.strikerManager.GenerateStriker();
            InGameScriptRefrences.playingObjectManager.ResetAllObjects();

            
        }*/
		
		/*if(other.gameObject.name == "Top Down" && isBusy){
			rigidbody.isKinematic = true;
            //Destroy(currentStrikerObject);
            isBusy = false;
            trail.time = 0f;
            hasBombPierced = false;            
			//InGameScriptRefrences.strikerManager.GenerateStriker();
            //InGameScriptRefrences.playingObjectManager.ResetAllObjects();
			InGameScriptRefrences.strikerManager.ResetStriker();

		}*/
		
        if (other.gameObject.tag == "Playing Object" && isBusy)
        {
            FreeStriker(other.gameObject);           	
        }

    }
	
    /*void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Left" || other.gameObject.name == "Right")
        {
            InGameScriptRefrences.soundFxManager.PlayWallCollisionSound();
            currentMovingDirection = Vector3.Reflect(currentMovingDirection, other.contacts[0].normal).normalized;

           
        }

        if ((other.gameObject.name == "Top" || other.gameObject.name == "Top Down")  && isBusy)
        {
            rigidbody.isKinematic = true;
            Destroy(currentStrikerObject);
            isBusy = false;
            trail.time = 0f;
            hasBombPierced = false;
            InGameScriptRefrences.strikerManager.GenerateStriker();
            InGameScriptRefrences.playingObjectManager.ResetAllObjects();

            
        }
        if (other.gameObject.tag == "Playing Object" && isBusy)
        {

            FreeStriker(other.gameObject);
           	
        }

    }*/

    //bool hasBombPierced = false;

    internal void Skip()
    {
        rigidbody.isKinematic = true;
        Destroy(currentStrikerObject);
		currentStrikerObject = null;		
        isBusy = false;
        InGameScriptRefrences.strikerManager.isFirstObject = true;
        InGameScriptRefrences.strikerManager.GenerateStriker();
    }

    internal void ReplaceObject(GameObject newObjectPrefab)
    {
        if (isBusy)
            return;

        Destroy(currentStrikerObject);
		currentStrikerObject = null;		

        GameObject tempObject = (GameObject)Instantiate(newObjectPrefab, transform.position, Quaternion.identity);

        tempObject.tag = "Striker";
        tempObject.GetComponent<SphereCollider>().enabled = false;
        tempObject.GetComponent<SphereCollider>().radius *= .5f;        
		
        tempObject.transform.parent = transform;
        tempObject.transform.localPosition = Vector3.zero;
        currentStrikerObject = tempObject;

        iTween.PunchScale(tempObject, new Vector3(.2f, .2f, .2f), 1f);
    }

    internal void PrepareStriker(PowerType _currentPowerType, GameObject _currentStrikerObject)
    {   
        currentPowerType = _currentPowerType;
		currentStrikerObject = _currentStrikerObject;
    }
}
