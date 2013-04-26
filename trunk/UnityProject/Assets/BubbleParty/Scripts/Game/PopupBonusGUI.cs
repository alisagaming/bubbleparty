using UnityEngine;
using System.Collections;

public class PopupBonusGUI : MonoBehaviour {
	
	public UILabel score_text;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetScore(string arg){
		score_text.text = arg;
	}
}
