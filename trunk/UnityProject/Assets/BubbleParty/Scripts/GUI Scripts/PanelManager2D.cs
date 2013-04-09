using UnityEngine;
using System.Collections;

public class PanelManager2D : MonoBehaviour {
	enum State{
		STATE_LOGIN,
		STATE_GAME,
		STATE_INVITE_FREND
		
	};
	
	State currentState;
	State newState;
	public static float TIME_SWITCH = 0.25f;
	
	public PanelTop panelTop;
	//public GameObject anchorTopPanel;
	
	public GameObject anchorStartGame;
	public GameObject anchorDownPanel;
	public GameObject anchorGamePanel;
	public GameObject anchorInviteFriend;
	public GameObject anchorLogin;
	
	public GameObject panelAllBG;
	public MainGameManager gameManager;
	public GameObject gameCamera;
	
	GameObject currentObject = null;
	
	int isInit = 0;
	
	TweenAlpha tweenAlpha;
	
	// Use this for initialization
	void Start () {
		gameCamera.SetActive(false);
		tweenAlpha = new TweenAlpha();
		tweenAlpha.from = 1;
		tweenAlpha.to = 0;
		tweenAlpha.duration = 1;
		
		AddAnimations(anchorLogin);
		AddAnimations(anchorInviteFriend);
		
		StartLogin();		
	}
	
	void AddAnimations(GameObject obj){
		TweenAlpha ta = TweenAlpha.Begin<TweenAlpha>(obj, TIME_SWITCH);
		ta.from = 1;
		ta.to = 0;
		ta.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(isInit<1000){
			isInit++;
			if(isInit ==1000)
				gameCamera.SetActive(false);
		}*/		
	}
	
	IEnumerator SetNewState(State state){
		if(state == State.STATE_GAME){
			anchorStartGame.SetActive(false);
			panelTop.gameObject.SetActive(false);
			anchorDownPanel.SetActive(false);
			panelAllBG.SetActive(false);
			
			anchorGamePanel.SetActive(true);
			gameCamera.SetActive(true);
			gameManager.Restart();
		}
		
		if(state == State.STATE_INVITE_FREND){
			panelTop.RemoveAll();			
			yield return new WaitForSeconds(AnimOut(currentObject));
			if(currentObject != null) currentObject.SetActive(false);
			AnimIn(anchorInviteFriend);	
		}
		
		if(state == State.STATE_LOGIN){			
			yield return new WaitForSeconds(AnimOut(currentObject));
			panelTop.StartLivesLevel();
			if(currentObject != null) currentObject.SetActive(false);
			AnimIn(anchorLogin);	
		}
		yield return new WaitForSeconds(0);
	}
	
	float AnimOut(GameObject obj){
		if(obj == null) return 0;
		TweenAlpha anim = obj.GetComponent<TweenAlpha>();
		anim.from = 1;
		anim.to = 0;
		anim.Reset();
		anim.Play(true);
		return TIME_SWITCH;
	}
	
	void AnimIn(GameObject obj){
		if(obj == null) return;
		currentObject = obj;
		currentObject.SetActive(true);
		TweenAlpha anim = obj.GetComponent<TweenAlpha>();
		anim.from = 0;
		anim.to = 1;
		anim.Reset();		
		anim.Play(true);		
	}
	
	public void StartLogin(){
		StartCoroutine(SetNewState(State.STATE_LOGIN));
	}
	
	public void StartGame(){
		StartCoroutine(SetNewState(State.STATE_GAME));
	}
	
	public void StartAchiev(){
		
	}
	
	public void StartInvite(){
		StartCoroutine(SetNewState(State.STATE_INVITE_FREND));
	}
	
	public void StartSend(){
	}
	
	public void StartSettings(){
	}
}
