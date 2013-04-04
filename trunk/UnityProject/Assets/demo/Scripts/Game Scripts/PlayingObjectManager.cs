using UnityEngine;
using System.Collections;

public class PlayingObjectManager : MonoBehaviour 
{
    internal static int brustCounter = 0;
	internal static int burnCounter = 0;
    public static string currentObjectName = "";
    ArrayList playingObjectList;
    public PlayingObject []topRowObjects;
    
	
	void Start () 
    {
        /*brustCounter = 0;
        currentObjectName = "";
        playingObjectList = new ArrayList();        
        RefreshPlayingObjectList();*/
		Restart();
	}
	
	public void Restart(){
		
		brustCounter = 0;
        currentObjectName = "";
        playingObjectList = new ArrayList();        
        RefreshPlayingObjectList();	
	}

    public void RefreshPlayingObjectList()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

        for (int i = 0; i < objects.Length; i++)
        {
            playingObjectList.Add(objects[i].GetComponent<PlayingObject>());
        }
    }

    internal PlayingObject[] allPlayingObjectScripts;

    

    internal void CheckForObjectsFall()
    {
        if ((PlayingObjectManager.brustCounter < 3) && (PlayingObjectManager.burnCounter == 0))
        {
            ResetAllObjects();
            return;
        }

        BrustObjects();
        FallDisconnectedObjects();
    }

    void BrustObjects()
    {
        UpdatePlayingObjectsList();
        for (int i = 0; i < allPlayingObjectScripts.Length; i++)
        {
            if (allPlayingObjectScripts[i].brust)// || allPlayingObjectScripts[i].isConnected == false)
                allPlayingObjectScripts[i].BrustMe(false);
        }
    }

    void FallDisconnectedObjects()
    {
        if (PlayingObjectGeneration.isBusy)
        {
            Invoke("FallDisconnectedObjects", .1f);
            return;
        }

        StartCoroutine(_FallDisconnectedObjects());
    }

    IEnumerator _FallDisconnectedObjects()
    {
        for (int i = 0; i < allPlayingObjectScripts.Length; i++)
        {
            allPlayingObjectScripts[i].isConnected = false;
        }
        yield return new WaitForSeconds(.01f);

        for (int i = 0; i < topRowObjects.Length; i++)
        {
			if(topRowObjects[i] != null)
            topRowObjects[i].TraceForConnection();            
        }

        for (int i = 0; i < allPlayingObjectScripts.Length; i++)
        {
            if (allPlayingObjectScripts[i])
            {
                if (allPlayingObjectScripts[i].isConnected == false)
                    allPlayingObjectScripts[i].BrustMe(true);
            }
        }

        Invoke("ResetAllObjects", .02f); //if cause problem remove invoke
    }

    internal void UpdatePlayingObjectsList()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");
        allPlayingObjectScripts = new PlayingObject[objects.Length];

        GameObject bottomMostObject = null;

        if (objects.Length > 0)
            bottomMostObject = objects[0];

                
        for (int i = 0; i < objects.Length; i++)
        {
            allPlayingObjectScripts[i] = objects[i].GetComponent<PlayingObject>();
            if (objects[i].transform.position.y < bottomMostObject.transform.position.y)
                bottomMostObject = objects[i];
        }

        InGameScriptRefrences.playingObjectGeneration.CheckForMinRowCount(bottomMostObject);
    }

    internal void ResetAllObjects()
    {
        brustCounter = 0;
        currentObjectName = "";

        UpdatePlayingObjectsList();

        for (int i = 0; i < allPlayingObjectScripts.Length; i++)
        {
            allPlayingObjectScripts[i].Reset();
        }

        if (allPlayingObjectScripts.Length == 0 && InGameScriptRefrences.gameVariables.totalNumberOfRowsLeft == 0)
            InGameScriptRefrences.gameUIController.GameIsFinish();
    }

    internal void FallAllPlayingObjects()
    {
        UpdatePlayingObjectsList();

        for (int i = 0; i < allPlayingObjectScripts.Length; i++)
        {
            allPlayingObjectScripts[i].GameOverFall();
        }
    }

    ArrayList currentAvailableObjects = new ArrayList();

    internal ArrayList GetRemainingObjectsNames()
    {
        if (allPlayingObjectScripts == null)
            return null;

        if (allPlayingObjectScripts.Length > 7)
            return null;

        ArrayList currentAvailableObjectsName = new ArrayList();
        currentAvailableObjects = new ArrayList();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

        for (int i = 0; i < objects.Length; i++)
        {
            string tempName = objects[i].name;
            if (!currentAvailableObjectsName.Contains(tempName))
            {
                currentAvailableObjectsName.Add(tempName);
                GetObjectRefrence(tempName);
            }
        }

        if (currentAvailableObjects.Count == 0)
            return null;

        return currentAvailableObjects;

    }

    void GetObjectRefrence(string name)
    {       
        for (int i = 0; i < InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs.Length; i++)
        {            
            if (InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[i].name == name.Substring(0, 3))
            {                
                currentAvailableObjects.Add(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[i]);
            }
        }
    }
}
