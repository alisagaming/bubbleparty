using UnityEngine;
using System.Collections;

public class GameUIController : MonoBehaviour 
{
    public static bool isgameFinish = false;
    public UILabel scoreText;
	public UILabel timerText;
	public Transform timerBar;
    public GameObject panelInGameGo;
	
	public PanelManager2D panelManager2D;
	//public GameObject input;

	void Start () 
    {
        isgameFinish = false;
        InvokeRepeating("UpdateGUI", 0f, .2f);
	}

    void UpdateGUI()
    {
        string str = GameVariables.score.ToString();
		if(str.Length>3)
			str = str.Insert(str.Length-3,".");
        scoreText.text = str;
        timerText.text = ((int)GameVariables.time).ToString(); 
		timerBar.localPosition = new Vector3(6-(60-GameVariables.time)*290/60,0,1);
    }

	void FixedUpdate()
    {
		if((panelInGameGo.activeSelf) || (GameVariables.time == 0)) return;
		GameVariables.time -= Time.fixedDeltaTime;
		if(GameVariables.time<0) GameVariables.time = 0;
		
		if (GameVariables.time == 0) {
			//input.SetActive(false);
			panelManager2D.StartTimeUpStatistic();		
		}
	}
}
