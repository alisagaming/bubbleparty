using UnityEngine;
using System.Collections;

public class PanelManager2D : MonoBehaviour {
	enum State{
		STATE_LOGIN,
		STATE_GAME,
		STATE_GAME_BEGIN,
		STATE_INVITE_FREND,
		STATE_SEND_FREND,
		STATE_TIME_UP_STATICTICS,
		STATE_HELP_STAR,
		STATE_HELP_TIME,
		STATE_HELP_FIREBALL,
		STATE_HELP_PLAZMA,
		STATE_BUY_COINS,
		STATE_BUY_DIAMOND
	};
	
	State currentState;
	State predState;
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
	
	public GameObject anchorHelpStar;
	public GameObject anchorHelpTime;
	public GameObject anchorHelpFireball;
	public GameObject anchorHelpPlazma;
	
	public GameObject anchorBuyCoins;
	public GameObject anchorBuyDiamond;
	
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
		AddAnimations(anchorHelpStar);
		AddAnimations(anchorHelpTime);
		AddAnimations(anchorHelpFireball);
		AddAnimations(anchorHelpPlazma);
		
		AddAnimations(anchorBuyCoins);
		AddAnimations(anchorBuyDiamond);
		
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
		case State.STATE_HELP_STAR:
		case State.STATE_HELP_FIREBALL:
		case State.STATE_HELP_PLAZMA:
		case State.STATE_HELP_TIME:
			panelTop.RemoveAll();	
			//panelDown.RemoveAll();
			yield return new WaitForSeconds(AnimOut(currentObject));
			panelDown.StartBack("Okay");			
			if(currentObject != null) currentObject.SetActive(false);
			switch(state){
			case State.STATE_HELP_STAR:
				AnimIn(anchorHelpStar);	
				break;
			case State.STATE_HELP_FIREBALL:
				AnimIn(anchorHelpFireball);	
				break;
			case State.STATE_HELP_PLAZMA:
				AnimIn(anchorHelpPlazma);	
				break;
			case State.STATE_HELP_TIME:
				AnimIn(anchorHelpTime);	
				break;
			}
		break;	
		case State.STATE_BUY_COINS:
			panelTop.RemoveAll();
			panelDown.RemoveAll();
			yield return new WaitForSeconds(AnimOut(currentObject));
			//panelDown.StartBack("Okay");
			AnimIn(anchorBuyCoins);
			break;			
		case State.STATE_BUY_DIAMOND:
			panelTop.RemoveAll();
			panelDown.RemoveAll();
			yield return new WaitForSeconds(AnimOut(currentObject));
			//panelDown.StartBack("Okay");
			AnimIn(anchorBuyDiamond);
			break;
		}
		
		predState = currentState;
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
		//string str = SystemInfo.deviceUniqueIdentifier;
		//str = "";
		
		StartCoroutine(ClientServer.Sync());
		
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
		case State.STATE_HELP_STAR:
		case State.STATE_HELP_TIME:
		case State.STATE_HELP_FIREBALL:
		case State.STATE_HELP_PLAZMA:
		case State.STATE_BUY_COINS:
		case State.STATE_BUY_DIAMOND:
			StartCoroutine(SetNewState(State.STATE_GAME));
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
	
	public void StartHelpStar(){
		StartCoroutine(SetNewState(State.STATE_HELP_STAR));
	}
	
	public void StartHelpTime(){
		StartCoroutine(SetNewState(State.STATE_HELP_TIME));
	}
	
	public void StartHelpFireball(){
		StartCoroutine(SetNewState(State.STATE_HELP_FIREBALL));
	}
	
	public void StartHelpPlazma(){
		StartCoroutine(SetNewState(State.STATE_HELP_PLAZMA));
	}
	
	public void StartBuyCoins(){
		StartCoroutine(SetNewState(State.STATE_BUY_COINS));
	}
	
	public void StartBuyDiamond(){
		StartCoroutine(SetNewState(State.STATE_BUY_DIAMOND));
	}
}
