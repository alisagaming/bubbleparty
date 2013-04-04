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
		if(panelInGameGo.activeSelf) return;
		GameVariables.time -= Time.fixedDeltaTime;
		if(GameVariables.time<0) GameVariables.time = 0;
	}
    
}
