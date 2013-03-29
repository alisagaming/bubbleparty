using UnityEngine;
using System.Collections;

public class PlayingObject : MonoBehaviour 
{
    public enum BrustBy
    {
        None,
        PlayingObject,
		Bomb
    };

    public GameObject brustParticle;

    internal BrustBy brustBy = BrustBy.None;

    static int numberOfAdjacentObjects = 6;
    public static float []adjacentObjectAngles = { 0, 60, 120, 180, 240, 300 };

    internal PlayingObject[] adjacentPlayingObjects;
	
	internal GameObject burnPrefab;
    internal bool isTraced = false;
    internal bool brust = false;
    internal bool isConnected = true;
    internal bool isTracedForConnection = false;
	internal bool isBurn = false;
	static Transform thresoldLineTransform;
	float objectGap;
	float sqrBombRadius;
	
	public enum BonusType{
		None,
		Score,
		Bomb,
		FireBall
	}
	
	internal BonusType bonusType = BonusType.None;
	
	internal void AddBonus(BonusType type){
		Object bonusRes = null;
		bonusType = type;
		switch(type){
		case BonusType.Score:
			bonusRes = Resources.Load("bonus_score"); 
			break;
		case BonusType.Bomb:
			bonusRes = Resources.Load("bonus_bomb"); 
			break;
		case BonusType.FireBall:
			bonusRes = Resources.Load("bonus_fireball"); 
			break;
		}
		if(bonusRes!=null){
			GameObject obj = (GameObject)Instantiate(bonusRes); 
			obj.transform.parent = transform;
			obj.transform.localPosition = new Vector3(0,0,-1);
		}
	}
	
    void Start()
    {
        if(thresoldLineTransform == null)
            thresoldLineTransform = GameObject.Find("Thresold Line").transform;
    }
	
	void ReadLevels()
	{
		
	}

    internal void Reset()
    {
        isTracedForConnection = false;
        isTraced = false;
        brust = false;
        
    }

    internal void RefreshAdjacentObjectList()
    {
        adjacentPlayingObjects = new PlayingObject[numberOfAdjacentObjects];
		
		//bool tempFlag = true;
		
        for (int i = 0; i < numberOfAdjacentObjects; i++)
        {
            PlayingObject temp = GetObjectInTheDirection(adjacentObjectAngles[i]);
            adjacentPlayingObjects[i] = temp;
           
            if (temp != null)
            {
                if (i < 3)
                {
                    temp.adjacentPlayingObjects[i + 3] = this;
                }
                else
                {
					//tempFlag = false;
                    temp.adjacentPlayingObjects[i - 3] = this;
                }
            }
        }
		
		//if(tempFlag){
		//	tempFlag = false;
		//	adjacentPlayingObjects[0] = null;
		//}
    }

    public LayerMask layerMask;

    PlayingObject GetObjectInTheDirection(float angle)
    {
        RaycastHit hit;
        float maxDistance = InGameScriptRefrences.playingObjectGeneration.objectGap * .6f;

        float radAngle = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);

        if (Physics.Raycast(transform.position, dir, out hit, maxDistance,layerMask))
        {
            if (hit.collider.gameObject.tag == "Playing Object")
            {
                return hit.collider.gameObject.GetComponent<PlayingObject>();
            }
            
        }
        
        return null;
    }

    void RefreshNeighbourAdjacentList()
    {
        for (int i = 0; i < numberOfAdjacentObjects; i++)
        {
            if (adjacentPlayingObjects[i])
            {
                if (i < 3)
                {
                    adjacentPlayingObjects[i].adjacentPlayingObjects[i + 3] = null;
                }
                else
                {
                    adjacentPlayingObjects[i].adjacentPlayingObjects[i - 3] = null;
                }
            }
        }     
    }
	
	internal void BurnMe(){
		if(!isBurn){
			isBurn = true;
			GameObject tempObject = (GameObject)Instantiate(burnPrefab, Vector3.zero, Quaternion.identity);
			tempObject.transform.parent = transform;
			tempObject.transform.localPosition = new Vector3(0,0,-1);
			//Invoke("stopBurn", .1f);
			PlayingObjectManager.burnCounter++;
			brust = true;
		}
	}
	
	void stopBurn(){
		isBurn = false;
		brust = true;
	}

    internal void BrustMe(bool fall)
    {
		if (bonusType == BonusType.FireBall) InGameScriptRefrences.strikerManager.SetNextStrikerType(PowerType.Fireball);
        Destroy(GetComponent<SphereCollider>());

        RefreshNeighbourAdjacentList();
        gameObject.tag = "Untagged";

        if (fall)
        {
            if (brust == false)
            {
                InGameScriptRefrences.scoreManager.DisplayScorePopup(100, transform, true);
                // iTween.RotateTo(gameObject, new Vector3(0, 0, 180), .5f);
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(new Vector3(0, Random.Range(150f, 250f), 0), ForceMode.VelocityChange);
                GetComponent<RotationScript>().enabled = true;
                Destroy(gameObject, 3f);
            }
        }
        else
        {
            InGameScriptRefrences.scoreManager.DisplayScorePopup(100, transform, false);
            
           
               // InGameScriptRefrences.poppingParticleAudioManager.PlayParticleSound();
               // iTween.ScaleTo(gameObject, Vector3.zero, 1f);
              //  GetComponent<RotationScript>().enabled = true;                
                Instantiate(brustParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
           
        }
        
    }
	
	internal void SetObjectGap(float objectGap){
		this.objectGap = objectGap;
		sqrBombRadius = objectGap * 3;
		sqrBombRadius*=sqrBombRadius;
	}
	
	
	void BombMe(Vector3 posBomb){
		
		//if(InGameScriptRefrences.playingObjectManager.allPlayingObjectScripts!= null)
			foreach(PlayingObject obj in InGameScriptRefrences.playingObjectManager.allPlayingObjectScripts){
				Vector3 dist = obj.transform.position - posBomb;
				if(dist.sqrMagnitude<sqrBombRadius){
					obj.AssignBrust(BrustBy.Bomb);
				}
			}
		
		/*if(brustBy == BrustBy.Bomb) return;
		Vector3 dist = transform.position - posBomb;
		if(dist.sqrMagnitude<sqrBombRadius){
			AssignBrust(BrustBy.Bomb);
			for (int i = 0; i < numberOfAdjacentObjects; i++)
            {
                if (adjacentPlayingObjects[i])
                {
					adjacentPlayingObjects[i].BombMe(posBomb);
				}
			}
		}*/
	}

    void Trace()
    {
        if (!isTraced)
        {
            isTraced = true;
            AssignBrust(BrustBy.PlayingObject);

            PlayingObjectManager.brustCounter++;
            // print(PlayingObjectManager.brustCounter);
            iTween.PunchScale(gameObject, new Vector3(.2f, .2f, .2f), 1f);
			
			if(bonusType == BonusType.Bomb){
				BombMe(transform.position);
			}
			
			for (int i = 0; i < numberOfAdjacentObjects; i++)
            {
                if (adjacentPlayingObjects[i])
                {
                    if (adjacentPlayingObjects[i].name == PlayingObjectManager.currentObjectName)
                    {
                        adjacentPlayingObjects[i].Trace();
                    }
                    else
                    {
                        adjacentPlayingObjects[i].isTraced = true;
						iTween.PunchScale(adjacentPlayingObjects[i].gameObject, new Vector3(.2f, .2f, .2f), 1f);
                    }

                    //   iTween.PunchScale(adjacentPlayingObjects[i].gameObject, new Vector3(.2f, .2f, .2f), 1f);
                    //   print("punch");
                }
            }
        }
    }

    internal void TraceForConnection()
    {
        if ( isTracedForConnection || brust )
            return;

        isTracedForConnection = true;
        
        isConnected = true;

        for (int i = 0; i < numberOfAdjacentObjects; i++)
        {
            if (adjacentPlayingObjects[i])
            {
                adjacentPlayingObjects[i].TraceForConnection();
            }
        }
    }
	
	internal void AdjustPosition(RaycastHit hit)
    {
        PlayingObjectManager.currentObjectName = gameObject.name;
		Vector3 collidedObjectPos = hit.collider.gameObject.transform.localPosition;
		
		float x = 0;
        float y = 0;
		
		if(Mathf.Abs(hit.normal.x) > 0.86){
			y = collidedObjectPos.y;
			if(hit.normal.x<0)
				x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap;
			else
				x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap;
		}else{
			if(hit.normal.x<0)
				x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap*.5f;
			else
				x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap*.5f;
			if(hit.normal.y<0)
				y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
			else
				y = collidedObjectPos.y + InGameScriptRefrences.playingObjectGeneration.rowGap;
		}
		
        /*Vector3 collidedObjectPos = collidedObject.transform.localPosition;

        float x = 0;
        float y = 0;
		Vector3 pos = transform.localPosition;

        if (pos.x < collidedObjectPos.x) //right touched
        {
            if (pos.y > collidedObjectPos.y) //upper part
			//if (pos.y > oldPosition.y)
            {
                x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap;
                y = collidedObjectPos.y;
            }
            else //lower part
            {
                x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
                //if (x < -(InGameScriptRefrences.playingObjectGeneration.maxX + .2f))
                //{
                //    x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                //}
            }
        }
        else //left touched
        {
            if (pos.y > collidedObjectPos.y) //upper part
            //if (pos.y > oldPosition.y)
            {
                x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap;
                y = collidedObjectPos.y;
            }
            else //lower part
            {
                x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
                //if (x > InGameScriptRefrences.playingObjectGeneration.maxX)
                //{
                //    x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                //}
            }
		}*/



        Vector3 newPos = new Vector3(x, y, 0);

        transform.localPosition = newPos;

        GetComponent<SphereCollider>().radius /= .8f;

        if (transform.position.y < thresoldLineTransform.position.y)
        {
            InGameScriptRefrences.gameUIController.GameIsOver();
            return;
        }

        RefreshAdjacentObjectList();

		InGameScriptRefrences.soundFxManager.PlayCollisionSound();
        Invoke("Trace", .02f);

        Invoke("CheckForObjectFall", .2f);
    }
   /* internal void AdjustPosition(GameObject collidedObject)
    {
        PlayingObjectManager.currentObjectName = gameObject.name;

        Vector3 collidedObjectPos = collidedObject.transform.localPosition;

        float x = 0;
        float y = 0;
		Vector3 pos = transform.localPosition;

        if (pos.x < collidedObjectPos.x) //right touched
        {
            if (pos.y > collidedObjectPos.y) //upper part
			//if (pos.y > oldPosition.y)
            {
                x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap;
                y = collidedObjectPos.y;
            }
            else //lower part
            {
                x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
                //if (x < -(InGameScriptRefrences.playingObjectGeneration.maxX + .2f))
                //{
                //    x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                //}
            }
        }
        else //left touched
        {
            if (pos.y > collidedObjectPos.y) //upper part
            //if (pos.y > oldPosition.y)
            {
                x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap;
                y = collidedObjectPos.y;
            }
            else //lower part
            {
                x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
                //if (x > InGameScriptRefrences.playingObjectGeneration.maxX)
                //{
                //    x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
                //}
            }
		}



        Vector3 newPos = new Vector3(x, y, 0);

        transform.localPosition = newPos;

        GetComponent<SphereCollider>().radius /= .8f;

        if (transform.position.y < thresoldLineTransform.position.y)
        {
            InGameScriptRefrences.gameUIController.GameIsOver();
            return;
        }

        RefreshAdjacentObjectList();

		InGameScriptRefrences.soundFxManager.PlayCollisionSound();
        Invoke("Trace", .02f);

        Invoke("CheckForObjectFall", .2f);
    }*/

    void CheckForObjectFall()
    {
        InGameScriptRefrences.playingObjectManager.CheckForObjectsFall();
        
    }

    internal void AssignBrust(BrustBy who)
    {
        brust = true;
        brustBy = who;
    }

    internal void GameOverFall()
    {
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector3(0, Random.Range(2f, 3f), 0), ForceMode.VelocityChange);
        //GetComponent<RotationScript>().enabled = true;
      //  Destroy(gameObject, 3f);
    }

}
