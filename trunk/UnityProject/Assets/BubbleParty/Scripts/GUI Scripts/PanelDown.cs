using UnityEngine;
using System.Collections;

public class PanelDown : MonoBehaviour {
	
	public GameObject panel_btn;
	public GameObject down_panel_menu;
	
	enum State{
		STATE_NONE,
		STATE_LOGIN,
		STATE_START_GAME
	};
	
	Vector3 pos1 = new Vector3(0,166,0);
	Vector3 pos2 = new Vector3(0,75,0);
	Vector3 pos3 = new Vector3(0,-100,0);
	
	Vector3 pos4 = new Vector3(0,42,0);
	Vector3 pos5 = new Vector3(0,-224,0);
	
	State currentState;
	
	
	// Use this for initialization
	void Start () {
		AddAnimations(panel_btn);
		AddAnimations(down_panel_menu);
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
			if(currentState == State.STATE_START_GAME){
				AnimStart(panel_btn,pos2,pos3);
			}else{
				AnimStart(panel_btn,pos1,pos3);
				AnimStart(down_panel_menu,pos4,pos5);
			}
			break;	
		case State.STATE_LOGIN:
			if(currentState == State.STATE_START_GAME)
				AnimStart(panel_btn,pos2,pos1);
			else
				AnimStart(panel_btn,pos3,pos1);
			AnimStart(down_panel_menu,pos5,pos4);
			break;
		case State.STATE_START_GAME:
			AnimStart(panel_btn,pos1,pos2);
			AnimStart(down_panel_menu,pos4,pos5);
			break;
		}
		currentState = state;
		yield return new WaitForSeconds(0);		
	}
	
	float AnimStart(GameObject obj, Vector3 fromPos, Vector3 toPos){
		if(obj == null) return 0;
		TweenPosition anim = obj.GetComponent<TweenPosition>();
		anim.from = fromPos;
		anim.to = toPos;
		anim.Reset();
		anim.Play(true);
		return PanelManager2D.TIME_SWITCH;
	}
	
	public void StartLogin(){
		StartCoroutine(SetNewState(State.STATE_LOGIN));
	}
	
	public void StartGame(){
		StartCoroutine(SetNewState(State.STATE_START_GAME));
	}
	
	public void RemoveAll(){
		StartCoroutine(SetNewState(State.STATE_NONE));
	}
}
