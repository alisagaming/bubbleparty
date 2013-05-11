using UnityEngine;
using System.Collections;

public class GameUIController : MonoBehaviour 
{
    public static bool isgameFinish = false;
    public UILabel scoreText;
	public UILabel timerText;
	public Transform timerBar;
    //public TextMesh gameStateText;
	public GameObject panelInGameGo;
	
	public PanelManager2D panelManager2D;
	//public GameObject anchorStatistics;
	public GameObject input;

	void Start () 
    {
        isgameFinish = false;
//        gameStateText.text = "";
        InvokeRepeating("UpdateGUI", 0f, .1f);
	
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

    internal void GameIsOver()
    {
    //    gameStateText.text = "Game Over";
    //    isgameFinish = true;
    }

    internal void GameIsFinish()
    {
    //    gameStateText.text = "Level Win";
    //    isgameFinish = true;

    }
	
	void FixedUpdate()
    {
		if((panelInGameGo.activeSelf) || (GameVariables.time == 0)) return;
		GameVariables.time -= Time.fixedDeltaTime;
		if(GameVariables.time<0) GameVariables.time = 0;
		
		if (GameVariables.time == 0) {
			//StartCoroutine(StartStat());
			input.SetActive(false);
			panelManager2D.StartTimeUpStatistic();		
		}
	}
	
	/*IEnumerator StartStat(){
		input.SetActive(false);
		panelManager2D.StartTimeUpStatistic();
		
		anchorTimeLeft.gameObject.SetActive(true);
		yield return new WaitForSeconds(1f);
		anchorTimeLeft.gameObject.SetActive(false);
		
	}*/
    
}
