using UnityEngine;
using System.Collections;

public enum PowerType
{
    None,
	Fireball
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
    public GameObject currentStrikerObject = null;
    public TrailRenderer trail;
	float radius;
	public LayerMask layerMask;
	
	public Transform boundTop;
	public Transform boundDown;
	public Transform boundLeft;
	public Transform boundRight;
	int strikeCount = 0;
	
	public GameObject strikerFireball;
	
	void Start()
    {
        currentPowerType = PowerType.None;
        myTransform = transform;
    }

    internal void Shoot(Vector3 dir)
    {
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
        
        
        isBusy = false;
        currentPowerType = PowerType.None;
        InGameScriptRefrences.strikerManager.GenerateStriker();
    }
	
	private void FreeFireBall(){
		trail.time = 0f; 
		rigidbody.isKinematic = true;        
		Destroy(currentStrikerObject);
		
		InGameScriptRefrences.playingObjectManager.CheckForObjectsFall();
		
		isBusy = false;
        currentPowerType = PowerType.None;
        InGameScriptRefrences.strikerManager.GenerateStriker();
	}

	RaycastHit hit = new RaycastHit();
		
	void FixedUpdate()
    {
        if (isBusy == false)
            return;

        //speed += 2000 * Time.deltaTime;

		oldPosition = myTransform.localPosition;
		
		float dist = speed * Time.fixedDeltaTime;//deltaTime;
		
		if(currentPowerType == PowerType.None){		
			if (Physics.SphereCast(myTransform.position,radius,currentMovingDirection,out hit, dist, layerMask)){
				//dist = dist;
				myTransform.Translate(currentMovingDirection * hit.distance);
				OnCollisionHit();
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

    bool hasBombPierced = false;

    internal void Skip()
    {
        rigidbody.isKinematic = true;
        Destroy(currentStrikerObject);
        isBusy = false;
        InGameScriptRefrences.strikerManager.isFirstObject = true;
        InGameScriptRefrences.strikerManager.GenerateStriker();
    }

    internal void ReplaceObject(GameObject newObjectPrefab)
    {
        if (isBusy)
            return;

        Destroy(currentStrikerObject);

        GameObject tempObject = (GameObject)Instantiate(newObjectPrefab, transform.position, Quaternion.identity);

        tempObject.tag = "Striker";
        tempObject.GetComponent<SphereCollider>().enabled = false;
        tempObject.GetComponent<SphereCollider>().radius *= .5f;        
		
        tempObject.transform.parent = transform;
        tempObject.transform.localPosition = Vector3.zero;
        currentStrikerObject = tempObject;

        iTween.PunchScale(tempObject, new Vector3(.2f, .2f, .2f), 1f);
    }

    internal void PrepareStriker(PowerType _currentPowerType)
    {
        
        currentPowerType = _currentPowerType;

    }
}
