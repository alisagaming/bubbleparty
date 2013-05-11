using UnityEngine;
using System.Collections;

public class PanelInGameGo : MonoBehaviour {
	
	public UILabel txtReady;
	public UILabel txtGo;
	
	//public GameObject bonusScore;
	//public GameObject bonusTime;
	//public GameObject bonusFireball;
	//public GameObject bonusSplash;
	
	public AudioSource soundReady;
	public AudioSource soundGo;

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable() {
		//bonusScore.SetActive(false);
		//bonusTime.SetActive(false);
		//bonusSplash.SetActive(false);
        //bonusFireball.SetActive(true);
		
		txtGo.gameObject.SetActive(false);
		txtReady.gameObject.SetActive(true);
		
		StartCoroutine(RunAnim());
	}
	
	public IEnumerator RunAnim(){
		soundReady.Play();
		yield return new WaitForSeconds(1f);
		txtGo.gameObject.SetActive(true);
		txtReady.gameObject.SetActive(false);
		soundGo.Play();
		
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}
}
