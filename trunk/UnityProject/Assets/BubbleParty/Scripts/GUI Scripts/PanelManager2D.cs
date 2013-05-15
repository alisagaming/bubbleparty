using UnityEngine;
using System.Collections;

public class PanelManager2D : MonoBehaviour {
	enum State{
		STATE_LOGIN,
		STATE_GAME,
		STATE_GAME_BEGIN,
		STATE_INVITE_FREND,
		STATE_SEND_FREND,
		STATE_TIME_UP_STATICTICS
	};
	
	State currentState;
	State newState;
	public static float TIME_SWITCH = 0.25f;
	
	public PanelTop panelTop;
	public PanelDown panelDown;
	//public GameObject anchorTopPanel;
	
	public GameObject anchorStartGame;
	public GameObject anchorGamePanel;
	public GameObject anchorInviteFriend;
	public GameObject anchorSendFriend;
	public GameObject anchorLogin;
	public GameObject anchorPause;
	
	public GameObject anchorStatistics;
	public GameObject anchorTimeLeft;
	
	public GameObject panelAllBG;
	public MainGameManager gameManager;
	public GameObject gameCamera;
	
	public SoundFxManager soundFxManager;
	
	public InputScript input;
	
	GameObject currentObject = null;
	
	int isInit = 0;
	
	// Use this for initialization
	void Start () {
		gameCamera.SetActive(false);
		
		AddAnimations(anchorLogin);
		AddAnimations(anchorInviteFriend);
		AddAnimations(anchorSendFriend);
		AddAnimations(anchorStartGame);
		AddAnimations(anchorStatistics);
		
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
		input.gameObject.SetActive(false);
		switch(state){
		case State.STATE_GAME_BEGIN:
			panelTop.RemoveAll();	
			panelDown.RemoveAll();
			yield return new WaitForSeconds(TIME_SWITCH);
			anchorStartGame.SetActive(false);
			panelTop.gameObject.SetActive(false);
			panelDown.gameObject.SetActive(false);
			panelAllBG.SetActive(false);
			
			anchorGamePanel.SetActive(true);
			gameCamera.SetActive(true);
			gameManager.Restart();
		break;
		case State.STATE_GAME:
			panelTop.StartCoinsDiamond();
			panelDown.StartGame();
			yield return new WaitForSeconds(AnimOut(currentObject));
			if(currentObject != null) currentObject.SetActive(false);
			AnimIn(anchorStartGame);	
		break;
		case State.STATE_INVITE_FREND:
			panelTop.RemoveAll();	
			panelDown.RemoveAll();
			yield return new WaitForSeconds(AnimOut(currentObject));
			if(currentObject != null) currentObject.SetActive(false);
			AnimIn(anchorInviteFriend);	
		break;
		case State.STATE_SEND_FREND:
			panelTop.RemoveAll();	
			panelDown.RemoveAll();
			yield return new WaitForSeconds(AnimOut(currentObject));
			if(currentObject != null) currentObject.SetActive(false);
			AnimIn(anchorSendFriend);	
		break;		
		case State.STATE_LOGIN:	
			if(currentState == State.STATE_GAME_BEGIN){
				Time.timeScale = 1;
				panelTop.gameObject.SetActive(true);
				panelDown.gameObject.SetActive(true);
				panelAllBG.SetActive(true);
				
				anchorGamePanel.SetActive(false);
				gameCamera.SetActive(false);
				anchorPause.SetActive(false);
				
				panelTop.StartLivesLevel();
				panelDown.StartLogin();
				AnimIn(anchorLogin);
			}else{			
				yield return new WaitForSeconds(AnimOut(currentObject));
				panelTop.StartLivesLevel();
				panelDown.StartLogin();
				if(currentObject != null) currentObject.SetActive(false);
				AnimIn(anchorLogin);	
			}
		break;
		case State.STATE_TIME_UP_STATICTICS:
			anchorTimeLeft.SetActive(true);
			yield return new WaitForSeconds(1f);
			soundFxManager.firemode_loop.Stop();
			panelTop.gameObject.SetActive(true);
			panelTop.StartCoinsLevel();
			
			panelDown.gameObject.SetActive(true);
			panelDown.StartBackToMain("Okay");
			
			gameCamera.SetActive(false);
			panelAllBG.SetActive(true);			
			anchorTimeLeft.SetActive(false);
			//anchorStatistics.SetActive(true);
			AnimIn(anchorStatistics);	
			
			
			break;
		}
		
		currentState = state;
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
	
	public void OnDownButton(){
		switch(currentState){
		case State.STATE_GAME:
			StartCoroutine(SetNewState(State.STATE_GAME_BEGIN));
			break;
		case State.STATE_LOGIN:
			StartCoroutine(SetNewState(State.STATE_GAME));
			break;
		case State.STATE_TIME_UP_STATICTICS:
			StartCoroutine(SetNewState(State.STATE_LOGIN));
			break;
		}
		/*if(currentState == State.STATE_GAME)
			StartCoroutine(SetNewState(State.STATE_GAME_BEGIN));
		else
			StartCoroutine(SetNewState(State.STATE_GAME));*/
	}
	
	public void StartAchiev(){
		
	}
	
	public void StartInvite(){
		StartCoroutine(SetNewState(State.STATE_INVITE_FREND));
	}
	
	public void StartSend(){
		StartCoroutine(SetNewState(State.STATE_SEND_FREND));
	}
	
	public void StartSettings(){
	}
	
	public void StartTimeUpStatistic(){
		StartCoroutine(SetNewState(State.STATE_TIME_UP_STATICTICS));
	}
}
