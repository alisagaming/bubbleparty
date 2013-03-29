using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PlayingObjectGeneration : MonoBehaviour
{
    public GameObject[] playingObjectsPrefabs;
    int numberOfObjectsInARow = 10;
    public float objectGap = 0;

    internal float rowGap = 0;
    internal float maxX = 2.5f;

    float currentYPos = 0;
	float currentXPos = 0;
	float currentZPos = 0;

    Transform thresoldLineTransform;
    public static bool isBusy = false;
    float rowStartingPos = 0;
    float objectGenerationheight;
    bool isStarting = true;

    float fallDownTime;
    float currentRowAddingInterval;
    int rowCounter = 0;
    int numberOfRowsGenerated = 0;
	LevelsPak levelsPak;

    public GameVariables gameVariables;
	public GameObject burnPrefab;

    void FalsenIsStarting()
    {
        isStarting = false;
    }

    void Start()
    {
		LoadLevels("levels");
        objectGenerationheight = transform.position.y;
		
		currentXPos = transform.position.x;
		currentZPos = transform.position.z;
		
        rowStartingPos = maxX;
        isBusy = false;
        thresoldLineTransform = GameObject.Find("Thresold Line").transform;
        rowGap = objectGap * .9f;
        Invoke("InitiateRowAdd", .1f);
        Invoke("FalsenIsStarting", 2f);

    }
	
	void LoadLevels(string path)
 	{
		
		/*LevelsPak test = new LevelsPak();
		test.levels = new Level[10];
		for(int x=0;x<10;x++){
			test.levels[x] = new Level();
			test.levels[x].rows = new string[5];
			for(int y=0;y<5;y++) test.levels[x].rows[y] = x.ToString();
		}
		
		path = "c:/monsters.xml";		
		var serializer = new XmlSerializer(typeof(LevelsPak));
 		using(var stream = new FileStream(path, FileMode.Create))
 		{
 			serializer.Serialize(stream, test);
 		} */
		
		TextAsset tAsset = Resources.Load(path)as TextAsset; 
		TextReader tr = new StringReader(tAsset.text);
		
		var serializer = new XmlSerializer(typeof(LevelsPak));
		levelsPak = serializer.Deserialize(tr) as LevelsPak;
		//int[] test = levelsPak.levels[0].getRow(0);
		//test[0] = 0;
 	} 

    internal void CheckForMinRowCount(GameObject bottomMostObject)
    {
        int numberOfRowsPresent = 0;

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


    }

    void InitiateRowAdd()
    {
        if (GameUIController.isgameFinish)
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

        gameVariables.totalNumberOfRowsLeft--;
    }
	
	int rowIndex = 0;
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
			levelIndex = /*10;//*/Random.Range(0,levelsPak.levels.Length-1);
			rowIndex = levelsPak.levels[levelIndex].rows.Length-1;
		}
		
		int[] rowIndexs = levelsPak.levels[levelIndex].getRow(rowIndex);
		
        for (int i = 0; i < rowIndexs.Length ; i++)
        {
            int index = rowIndexs[i];//*/Random.Range(0, playingObjectsPrefabs.Length);
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
    }



}
