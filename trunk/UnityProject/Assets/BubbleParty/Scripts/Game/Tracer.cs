using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {
	
	public GameObject tracePrefab;
	public Transform boundLeft;
	public Transform boundRight;
	
	
	GameObject[] traces;
	int maxCount = 15;
	float dd = 40;
	
	float xLeft;
	float xRight;
	
	float timer;
	float visibleTimer;
	
	// Use this for initialization
	void Start () {
		xLeft = boundLeft.transform.position.x;
		xRight = boundRight.transform.position.x;
		traces = new GameObject[maxCount];
		for(int x=0;x<maxCount;x++){
			traces[x] = (GameObject)Instantiate(tracePrefab, Vector3.zero, Quaternion.identity);
			traces[x].transform.parent = transform;
			traces[x].transform.localPosition =  new Vector3(0,x*dd,0);
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		visibleTimer -= Time.deltaTime;
		if(visibleTimer<0){
			gameObject.SetActive(false);
		}
		
		timer+=Time.deltaTime*dd*2;
		timer%= dd;
		for(int x=0;x<maxCount;x++){
			Vector3 pos = traces[x].transform.localPosition;
			pos.x = 0;
			pos.y = x*dd + timer;
			traces[x].transform.localPosition = pos;
			pos = traces[x].transform.position;
			if(xLeft>pos.x){
				pos.x = xLeft - (pos.x - xLeft);
				traces[x].transform.position = pos;
			}
			if(xRight<pos.x){
				pos.x = xRight - (pos.x - xRight);
				traces[x].transform.position = pos;
			}
			pos = traces[x].transform.localScale;
			pos.x = (maxCount-x)/(float)maxCount + 0.5f;
			pos.y = pos.x;
			traces[x].transform.localScale = pos;
		}
	}
	
	public void SetVisible(){
		visibleTimer = 0.1f;
		gameObject.SetActive(true);
	}
}
