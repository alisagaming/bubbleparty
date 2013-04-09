using UnityEngine;
using System.Collections;

public class PanelTop : MonoBehaviour {
	
	enum State{
		STATE_NONE,
		STATE_COINS_DIAMOND,
		STATE_HEART_START_GAME,
		STATE_LIVES_LEVEL
	};
	
	public GameObject panel_coins_diamond;
	public GameObject panel_heart;
	public GameObject panel_lives_level;
	public GameObject game;
	public GameObject panel_all_bg;
	
	public PanelManager2D panelManager2D;
	
	State currentState;
	State newState;
	
	Vector3 toPosition = new Vector3(0,100,0);
	Vector3 fromPosition = new Vector3(0,0,0);
	
	GameObject currentPanel;
	
	// Use this for initialization
	void Start () {
		//AddAnimations(panel_coins_diamond);
		//AddAnimations(panel_heart);
		AddAnimations(panel_lives_level);
		currentPanel = panel_lives_level;
	}
	
	void AddAnimations(GameObject obj){
		TweenPosition ta = TweenAlpha.Begin<TweenPosition>(obj, PanelManager2D.TIME_SWITCH);
		ta.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator SetNewState(State state){
		switch(state){
		case State.STATE_NONE:
			AnimOut(currentPanel);
			currentPanel = null;
			break;
		case State.STATE_COINS_DIAMOND:
			break;
		case State.STATE_LIVES_LEVEL:
			yield return new WaitForSeconds(AnimOut(currentPanel));
			if(currentPanel != null) currentPanel.SetActive(false);			
			AnimIn(panel_lives_level);			
			break;
		}
			
		yield return new WaitForSeconds(0);
		/*GameObject obj = panel_coins_diamond;
		//iTween.MoveTo(panel_coins_diamond, currentStrikerPosition.position, .1f);
        iTween.MoveTo(obj, iTween.Hash("position", toPosition, "time", timeSwitch,"islocal", true));//, "easetype", iTween.EaseType.easeInOutExpo, "onComplete", EndTween));
		yield return new WaitForSeconds(timeSwitch);
		obj.SetActive(false);
		
		obj = panel_heart;
		obj.SetActive(true);
		obj.transform.localPosition = toPosition;
		iTween.MoveTo(obj, iTween.Hash("position", fromPosition, "time", timeSwitch,"islocal", true));//, "easetype", iTween.EaseType.easeInOutExpo, "onComplete", EndTween));		
        yield return new WaitForSeconds(timeSwitch);
		
		
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
		panel_all_bg.SetActive(false);
		
		panelManager2D.StartGame();
		
		game.SetActive(true);*/
	}
	
	float AnimOut(GameObject obj){
		if(obj == null) return 0;
		TweenPosition anim = obj.GetComponent<TweenPosition>();
		anim.from = fromPosition;
		anim.to = toPosition;
		anim.Reset();
		anim.Play(true);
		return PanelManager2D.TIME_SWITCH;
	}
	
	void AnimIn(GameObject obj){
		if(obj == null) return;
		currentPanel = obj;
		currentPanel.SetActive(true);
		TweenPosition anim = obj.GetComponent<TweenPosition>();
		anim.from = toPosition;
		anim.to = fromPosition;
		anim.Reset();		
		anim.Play(true);		
	}
	
	public void StartGame(){
		StartCoroutine(SetNewState(State.STATE_HEART_START_GAME));
	}
	
	public void StartLivesLevel(){
		StartCoroutine(SetNewState(State.STATE_LIVES_LEVEL));
	}
	
	public void RemoveAll(){
		StartCoroutine(SetNewState(State.STATE_NONE));
	}
}
