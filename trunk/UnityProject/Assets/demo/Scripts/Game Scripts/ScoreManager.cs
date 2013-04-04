using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
    public GameObject scoreItemPrefab;
    int bonusPoint;
    int numberOfItemPoppedInARow = 0;

	void Start () 
    {
	
	}

    internal void DisplayScorePopup(int score,Transform go,bool makeChild)
    {

        bonusPoint = score * numberOfItemPoppedInARow;
        int points = score + bonusPoint;
		
		GameVariables.score += points;
        numberOfItemPoppedInARow++;
		
		/*GameObject scoreItem = (GameObject)Instantiate(scoreItemPrefab, go.position + new Vector3(0, 0, -1), Quaternion.identity);
        scoreItem.transform.eulerAngles = new Vector3(0, 0, 0);


        scoreItem.GetComponent<ScorePopupItem>().BringItForward(points);
        */
        CancelInvoke("ResetNumberOfItemPopped");
        Invoke("ResetNumberOfItemPopped",.5f);
    }

    private void ResetNumberOfItemPopped()
    {
        numberOfItemPoppedInARow = 0;
        //ScorePopupItem.ResetDelay();
    }
}
