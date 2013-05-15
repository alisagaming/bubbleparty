using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class ScoreManager : MonoBehaviour 
{
	public GameObject scoreGUI;
	public GameObject popup_time;
	public GameObject popup_star;
	public GameObject thresholdLine;
	
    //public GameObject scoreItemPrefab;
    //int bonusPoint;
    //int numberOfItemPoppedInARow = 0;
	ScoreConfig scoreConfig;
	
	int fallenCount;
	int explodedCount;
	
	Vector3 fPos;
	Vector3 ePos;
	

	void Start () 
    {
		LoadScoreConfig("config_score");
	}
	
	public void Restart(){
		CancelInvoke("ResetNumberOfItemPopped");
        fallenCount = 0;
		explodedCount = 0;
		Utils.DestroyAllChild(transform);		
	}
	
	public void PopupTime(Vector3 pos, int timeValue){
		GameObject score = (GameObject)Instantiate(popup_time, pos, Quaternion.identity);
		score.transform.parent = transform;
		score.GetComponent<PopupBonusGUI>().SetScore("+"+timeValue);		
	}	
	
	public void PopupStar(Vector3 pos){
		GameObject score = (GameObject)Instantiate(popup_time, pos, Quaternion.identity);
		score.transform.parent = transform;
		score.GetComponent<PopupBonusGUI>().SetScore(scoreConfig.boostScore.ToString());
		GameVariables.score += scoreConfig.boostScore;
	}
	
	void LoadScoreConfig(string path)
 	{
		TextAsset tAsset = Resources.Load(path)as TextAsset; 
		TextReader tr = new StringReader(tAsset.text);
		
		var serializer = new XmlSerializer(typeof(ScoreConfig));
		scoreConfig = serializer.Deserialize(tr) as ScoreConfig;	
		
		//scoreConfig = null;
		/*tAsset = Resources.Load("config_score")as TextAsset; 
		tr = new StringReader(tAsset.text);
		
		serializer = new XmlSerializer(typeof(ScoreConfig));
		ScoreConfig scoreConfig = serializer.Deserialize(tr) as ScoreConfig;
		scoreConfig = null;*/
 	} 

    internal void AddScore(Transform pos,bool isFallen)
    {
		if(isFallen){
			if(fallenCount == 0) fPos = pos.position;
			else fPos += pos.position; 
			fallenCount++;
		}else{
			if(explodedCount == 0) ePos = pos.position;
			else ePos += pos.position; 
			explodedCount++;
		}
		/*int score = 100;
        bonusPoint = score * numberOfItemPoppedInARow;
        int points = score + bonusPoint;
		
		GameVariables.score += points;
        numberOfItemPoppedInARow++;
		
		GameObject scoreItem = (GameObject)Instantiate(scoreItemPrefab, go.position + new Vector3(0, 0, -1), Quaternion.identity);
        scoreItem.transform.eulerAngles = new Vector3(0, 0, 0);


        scoreItem.GetComponent<ScorePopupItem>().BringItForward(points);
        */
        CancelInvoke("ResetNumberOfItemPopped");
        Invoke("ResetNumberOfItemPopped",.2f);
    }

    private void ResetNumberOfItemPopped()
    {
		ePos /= explodedCount;
		int scoreExploded = scoreConfig.getValueInt(explodedCount, scoreConfig.explodedScores);
		scoreExploded += explodedCount*scoreConfig.explodedScorePerBubble;
		
		int scoreFallen = scoreConfig.getValueInt(fallenCount, scoreConfig.droppedScore);
		scoreFallen += fallenCount*scoreConfig.droppedScorePerBubble;
		
		GameVariables.score += scoreExploded + scoreFallen;        
		
		GameObject score = (GameObject)Instantiate(scoreGUI, ePos, Quaternion.identity);
		score.transform.parent = transform;
		score.GetComponent<ScoreGUI>().SetScore(scoreExploded, ScoreGUI.Type.EXPLOSE);
		
		if(scoreFallen>0){
			fPos /= fallenCount;
			fPos.y = thresholdLine.transform.position.y;
			score = (GameObject)Instantiate(scoreGUI, fPos, Quaternion.identity);
			score.transform.parent = transform;
			score.GetComponent<ScoreGUI>().SetScore(scoreFallen, ScoreGUI.Type.FALLEN);
		}
		
		fallenCount = 0;
		explodedCount = 0;				
		
        //numberOfItemPoppedInARow = 0;
        //ScorePopupItem.ResetDelay();
    }
}
