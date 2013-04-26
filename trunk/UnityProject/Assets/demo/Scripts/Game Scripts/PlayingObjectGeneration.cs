using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class PlayingObjectGeneration : MonoBehaviour
{
	int numberOfObjectsInARow = 10;
    
    public GameObject[] playingObjectsPrefabs;
    public float objectGap = 0;
	public Transform thresholdLineTransform;
    public GameVariables gameVariables;
	public GameObject burnPrefab;
    
	BoostRuleset boostRulesetTime;
	
	internal bool enableStar = true;	
	internal bool enableTime = true;
	internal bool enableFireball = false;
	internal bool enablePlazma = false;
	
	
	internal static bool isBusy = false;
	internal float rowGap = 0;
    internal float maxX = 0;
	
	Vector3 startPosition;
	int rowIndex = 0;
	int levelIndex = 0;
	int levelCount = 0;
	LevelsPak levelsPak;
	
	int rowCounter = 0;
	int numberOfRowsGenerated = 0;
	float rowStartingPos;
	
	float currentYPos = 0;

	void Start()
    {
		boostRulesetTime = BoostRuleset.LoadFromFile("time_boost_ruleset");
		startPosition = transform.localPosition;
		maxX = rowGap/4;
		if(true){
			numberOfObjectsInARow = 10;
    		LoadLevels("levels");
		}else{
		}
    }
	
	void LoadLevels(string path)
 	{
		TextAsset tAsset = Resources.Load(path)as TextAsset; 
		TextReader tr = new StringReader(tAsset.text);
		
		var serializer = new XmlSerializer(typeof(LevelsPak));
		levelsPak = serializer.Deserialize(tr) as LevelsPak;	
 	} 
	
	void ResetBoostCounts(){
		if(levelCount > 0){
			boostRulesetTime.GenerateForLevel(levelCount, rowIndex);
		}
	}
	
	void AddBoostToRow(int rowLenght){
		PlayingObject[] row = InGameScriptRefrences.playingObjectManager.topRowObjects;
		if(enableTime && boostRulesetTime.IsBoostStart(rowIndex)){
			int pos = Random.Range(1,rowLenght);
			int posX = 0;
			while(pos>0){
				if(row[posX]!=null) pos--; 
				posX++;
			}
			row[posX-1].GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.TIME, boostRulesetTime.bonusValueForLevel[levelCount-1]);
		}
	}
	
	public void Restart(){
		
		Utils.DestroyAllChild(transform);
		transform.localPosition = startPosition;
		
		ResetBoostCounts();
		
		levelCount = 0;
		rowIndex = 0;
		levelIndex = 0;
		rowCounter = 0;
    	rowGap = objectGap * .9f;
		numberOfRowsGenerated = 0;
		rowStartingPos = maxX;
		currentYPos = 0;
		
		iTween.Stop(gameObject, "mov");
		//CancelInvoke("InitiateRowAdd");
		isBusy = false;
        
		InitiateRowAdd();
		/*gameVariables.totalNumberOfRowsLeft = 1000;
		var children = new List<GameObject>();
		foreach (Transform child in transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		
		transform.localPosition = new Vector3(0,objectGenerationheight,0);
		
		currentYPos = 0;
		currentXPos = 0;
		currentZPos = 0;
		
		rowIndex = 0;
		rowCounter = 0;
    	numberOfRowsGenerated = 0;
		rowStartingPos = 0;
  
		currentXPos = transform.position.x;
		currentZPos = transform.position.z;
		
        rowStartingPos = maxX;
        isBusy = false;
        thresoldLineTransform = GameObject.Find("Thresold Line").transform;
        rowGap = objectGap * .9f;
		CancelInvoke("InitiateRowAdd");
		CancelInvoke("FalsenIsStarting");
		CancelInvoke("CheckForGameOver");
		
		iTween.Stop(gameObject, "mov");
		
        Invoke("InitiateRowAdd", .1f);
        Invoke("FalsenIsStarting", 2f);*/
	}
	
	/*void DestroyAllChild(Transform inTransform){
		var children = new List<GameObject>();
		foreach (Transform child in inTransform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
	}*/

    internal void CheckForMinRowCount(GameObject bottomMostObject)
    {
		int numberOfRowsPresent = 0;

        if (bottomMostObject == null)
            numberOfRowsPresent = 0;
        else
			numberOfRowsPresent = Mathf.RoundToInt((currentYPos - bottomMostObject.transform.localPosition.y)/ rowGap) + 1;
            //numberOfRowsPresent = Mathf.RoundToInt((objectGenerationheight - bottomMostObject.transform.position.y) / rowGap);*/
		
		rowCounter = numberOfRowsPresent;
		
        InitiateRowAdd();
		
		/*int numberOfRowsPresent = 0;

        if (bottomMostObject == null)
        {
            numberOfRowsPresent = 0;
        }
        else
            numberOfRowsPresent = Mathf.RoundToInt((objectGenerationheight - bottomMostObject.transform.position.y) / rowGap);

        rowCounter = numberOfRowsPresent;

        if (rowCounter < Mathf.Min(6, gameVariables.minimumNumberOfRows) && gameVariables.totalNumberOfRowsLeft > 0)
        {
            CancelInvoke("InitiateRowAdd");
            InitiateRowAdd();
        }
		
		/*int numberOfRowsPresent = 0;

        if (bottomMostObject == null)
        {
            numberOfRowsPresent = 0;
        }
        else
            numberOfRowsPresent = Mathf.RoundToInt((objectGenerationheight - bottomMostObject.transform.position.y) / rowGap);

        if (rowCounter < Mathf.Min(6, gameVariables.minimumNumberOfRows) && gameVariables.totalNumberOfRowsLeft > 0)
        {
            CancelInvoke("InitiateRowAdd");
            InitiateRowAdd();
        }*/
		
		/*int numberOfRowsPresent = 0;

        if (bottomMostObject == null)
        {
            numberOfRowsPresent = 0;
        }
        else
            numberOfRowsPresent = Mathf.RoundToInt((objectGenerationheight - bottomMostObject.transform.position.y) / rowGap);

        rowCounter = numberOfRowsPresent;

        if (rowCounter < Mathf.Min(6, gameVariables.minimumNumberOfRows) && gameVariables.totalNumberOfRowsLeft > 0)
        {
            CancelInvoke("InitiateRowAdd");
            InitiateRowAdd();
        }
		*/	
    }

    void InitiateRowAdd()
    {
		if(gameVariables.minimumNumberOfRows<=rowCounter) return;
		
		for(int x=rowCounter;x<gameVariables.minimumNumberOfRows;x++){
			AddRow();
		}
        
		iTween.MoveBy(gameObject, new Vector3(0, -rowGap*(gameVariables.minimumNumberOfRows - rowCounter), 0), 0.5f);
		rowCounter = gameVariables.minimumNumberOfRows;
			
        /*if (GameUIController.isgameFinish)
            return;
		
        if (gameVariables.totalNumberOfRowsLeft == 0)
        {
            currentRowAddingInterval = gameVariables.rowAddingInterval;
            iTween.MoveBy(gameObject, new Vector3(0, -rowGap, 0), fallDownTime);
            Invoke("InitiateRowAdd", currentRowAddingInterval);
            Invoke("CheckForGameOver", .5f);
            return;
        }

        if (!isBusy)
            StartCoroutine(AddRow());

        gameVariables.totalNumberOfRowsLeft--;*/
    }
	
	void AddRow(){
		InGameScriptRefrences.playingObjectManager.topRowObjects = new PlayingObject[numberOfObjectsInARow];
		float x;
        if (rowStartingPos == maxX)
            rowStartingPos = maxX - objectGap * .5f;
        else
            rowStartingPos = maxX;
		x = rowStartingPos;
		
		currentYPos = (int)(numberOfRowsGenerated * rowGap);
		numberOfRowsGenerated++;
		GameObject tempObject;
		
		rowIndex--;
		if(rowIndex<0){
			levelIndex = Random.Range(0,levelsPak.levels.Length-1);
			rowIndex = levelsPak.levels[levelIndex].rows.Length-1;
			levelCount++;
			ResetBoostCounts();
		}
		
		int[] rowIndexs = levelsPak.levels[levelIndex].getRow(rowIndex);
		
		int bubbleCount = 0;
		
        for (int i = 0; i < rowIndexs.Length ; i++)
        {
            int index = rowIndexs[i];
			//index = 0;
			if(index>-1){
	            Vector3 pos = new Vector3(x + numberOfObjectsInARow * objectGap * .5f, currentYPos, 0);
	            float rand = Random.value;
	            tempObject = (GameObject)Instantiate(playingObjectsPrefabs[index], Vector3.zero, Quaternion.identity);
	
	
	            tempObject.transform.parent = transform;
	            tempObject.transform.localPosition = pos;
				
				//if( Random.Range(0,100)>90) tempObject.GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.Score);
	            //if( Random.Range(0,100)>90) tempObject.GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.Bomb);
				//if( Random.Range(0,100)>90) tempObject.GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.FireBall);
				PlayingObject po = tempObject.GetComponent<PlayingObject>();
				po.SetObjectGap(objectGap);
	            po.RefreshAdjacentObjectList();
				po.burnPrefab = burnPrefab;
	            
	            InGameScriptRefrences.playingObjectManager.topRowObjects[i] = tempObject.GetComponent<PlayingObject>();
				bubbleCount++;
	            //if (i % numberOfObjectsInARow == 0)
	            //    yield return new WaitForSeconds(.01f);*/
			}
			x -= objectGap;	
        }
		AddBoostToRow(bubbleCount);
	}
	
	/*int rowIndex = 0;
	int levelIndex = 0;
	
    IEnumerator AddRow()
    {
        isBusy = true;
        InGameScriptRefrences.playingObjectManager.topRowObjects = new PlayingObject[numberOfObjectsInARow];

        float x;
        if (rowStartingPos == maxX)
            rowStartingPos = maxX - objectGap * .5f;
        else
            rowStartingPos = maxX;

        x = rowStartingPos;

        numberOfRowsGenerated++;

        GameObject tempObject;
		
		rowIndex--;
		if(rowIndex<0){
			levelIndex = Random.Range(0,levelsPak.levels.Length-1);
			rowIndex = levelsPak.levels[levelIndex].rows.Length-1;
		}
		
		int[] rowIndexs = levelsPak.levels[levelIndex].getRow(rowIndex);
		
        for (int i = 0; i < rowIndexs.Length ; i++)
        {
            int index = rowIndexs[i];
			//index = 0;
			if(index>-1){
	            Vector3 pos = new Vector3(x + numberOfObjectsInARow * objectGap * .5f, currentYPos, 0);
	            float rand = Random.value;
	            tempObject = (GameObject)Instantiate(playingObjectsPrefabs[index], Vector3.zero, Quaternion.identity);
	
	
	            tempObject.transform.parent = transform;
	            tempObject.transform.localPosition = pos;
				//if( Random.Range(0,100)>90) tempObject.GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.Score);
	            //if( Random.Range(0,100)>90) tempObject.GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.Bomb);
				if( Random.Range(0,100)>90) tempObject.GetComponent<PlayingObject>().AddBonus(PlayingObject.BonusType.FireBall);
				PlayingObject po = tempObject.GetComponent<PlayingObject>();
				po.SetObjectGap(objectGap);
	            po.RefreshAdjacentObjectList();
				po.burnPrefab = burnPrefab;
	            
	            InGameScriptRefrences.playingObjectManager.topRowObjects[i] = tempObject.GetComponent<PlayingObject>();
	            //if (i % numberOfObjectsInARow == 0)
	            //    yield return new WaitForSeconds(.01f);
			}
			x -= objectGap;	
        }

        isBusy = false;
        iTween.Defaults.easeType = iTween.EaseType.linear;

        currentYPos = numberOfRowsGenerated * rowGap;
        iTween.MoveTo(gameObject, new Vector3(currentXPos, objectGenerationheight - currentYPos, currentZPos), fallDownTime);


        rowCounter++;
        if (rowCounter >= gameVariables.minimumNumberOfRows)
        {
            currentRowAddingInterval = gameVariables.rowAddingInterval;
            fallDownTime = .5f;
        }
        else
        {
            currentRowAddingInterval = .01f;

            if (isStarting)
                fallDownTime = .2f;
            else
                fallDownTime = .5f;
        }
		
		yield return new WaitForSeconds(.01f);
		
        Invoke("InitiateRowAdd", currentRowAddingInterval);

        Invoke("CheckForGameOver", .5f);
    }

    private void CheckForGameOver()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");


        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.position.y < thresoldLineTransform.position.y)
            {
                InGameScriptRefrences.gameUIController.GameIsOver();
                break;
            }
        }
    }*/



}
