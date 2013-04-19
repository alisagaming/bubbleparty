using UnityEngine;
using System.Collections;

public class OnFireManager : MonoBehaviour {
	public static int MAX_LEVEL_ON_FIRE = 9;
	
	public tk2dAnimatedSprite fire_l_anim;
	public tk2dAnimatedSprite fire_r_anim;
	public tk2dAnimatedSprite fire_down_anim;
	
	public tk2dSprite fire_l;
	public tk2dSprite fire_r;
	
	public SoundFxManager soundFxManager;
	
	int currentLevel;
	
	float onfireTime;
	int error;
	
	public void Restart(){
		currentLevel = 0;
		fire_l_anim.gameObject.SetActive(false);
		fire_r_anim.gameObject.SetActive(false);
		fire_down_anim.gameObject.SetActive(false);
		soundFxManager.firemode_loop.Stop();
		error = 0;
		SetLevelSprite();
	}
	
	// Use this for initialization
	void Start () {
		Restart();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentLevel == MAX_LEVEL_ON_FIRE){
			onfireTime -= Time.deltaTime;
			if(onfireTime<0) Restart();
		}
	}
	
	void StartOnFire(){
		soundFxManager.onFire.Play();
		fire_l_anim.gameObject.SetActive(true);
		fire_r_anim.gameObject.SetActive(true);
		fire_down_anim.gameObject.SetActive(true);
		soundFxManager.firemode_loop.Play();
		onfireTime = 10;
	}
	
	void SetLevelSprite(){
		if((currentLevel<2) || IsOnFire()){
			fire_l.gameObject.SetActive(false);
			fire_r.gameObject.SetActive(false);	
		}else{
			fire_l.gameObject.SetActive(true);
			fire_r.gameObject.SetActive(true);
			
			int spriteID = fire_l.Collection.GetSpriteIdByName("frameFiremode"+currentLevel);
			fire_l.spriteId = spriteID;
			fire_r.spriteId = spriteID;
		}
	}
	
	public bool IsOnFire(){
		return currentLevel == MAX_LEVEL_ON_FIRE;
	}
	
	public void Shot(bool isSuccessful){
		if(isSuccessful){
			error = 0;
			soundFxManager.firemode[currentLevel].Play();
			if(currentLevel<9){
				currentLevel++;
				if(currentLevel == 9)
					StartOnFire();
				else if(currentLevel>1){
					
				}
				SetLevelSprite();
			}
		}else{
			error++;
			if(error>1) Restart();
		}		
	}
}
