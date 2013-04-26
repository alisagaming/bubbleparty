using UnityEngine;
using System.Collections;

public class FacebookManager : MonoBehaviour {
	static string APP_ID = "123365184515949";
	
	public Transform panel_users;
	public Transform offline;
	public Transform online;
	
	public UIDraggablePanel scrollPanel;
	
	public GameObject userLinePrefab;
	
	UserData.UserDataRaw me;
	//UserData.UserDataRaw[] friends;
	ArrayList scores = new ArrayList();
	
	string[] fakeFriendName = new string[7]{
		"Name 1",
		"Name 2",
		"Name 3",
		"Name 4",
		"Name 5",
		"Name 6",
		"Name 7"		
	};
	
	// Use this for initialization
	void Start () {
		FacebookAndroid.init();
		//UpdateUsersList();
	}
	
	void UpdateUsersList(){
		offline.gameObject.SetActive(false);
		online.gameObject.SetActive(true);
		LoadFrendsList();		
		/*if(FacebookAndroid.isSessionValid()){
			offline.gameObject.SetActive(false);
			online.gameObject.SetActive(true);
			LoadFrendsList();		
		}else{
			offline.gameObject.SetActive(true);
			online.gameObject.SetActive(false);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void LoadFrendsList(){
		
		scores.Clear();
		UserData.UserDataRaw user;
		for(int x=scores.Count;x<7;x++){
								user = new UserData.UserDataRaw();
								user.ImageLocalFileName = "fakeFriend_0" + (x+1).ToString() + "@2x";
								user.UserName = fakeFriendName[x];
								user.Score = x*100;
								user.Level = 1;
								scores.Add(user);
								scores.Add(user);
							}
		user = new UserData.UserDataRaw();
								user.ImageFileName = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-snc6/275985_100004649500056_1912404844_s.jpg";
								user.UserName = "Test Image URL";
								user.Score = 100000;
								user.Level = 1;
								scores.Add(user);
		scores.Sort(new UserData.UserCompare());
		
		
							CreateFriendScoreList();
		
		//Facebook.instance.debugRequests = true;
		/*Facebook.instance.get( "me?fields=id,name,picture.type(normal)", null, ( error, obj ) =>
		{
			if(error == null){
				scores.Clear();
				me = new UserData.UserDataRaw();
				me.Deserialize((Hashtable)obj);
				me.Score = GameVariables.score;
				scores.Add(me);
				
				Facebook.instance.get( "me/friends?fields=id,name,picture.type(normal)", null, ( error2, obj2 ) =>
				{
					if(error2== null){
						ArrayList arrayFriends = (ArrayList)((Hashtable)obj2)["data"];
						//friends = new UserData.UserDataRaw[array.Count];
						//for(int x=0;x<array.Count;x++){
						//	friends[x] = new UserData.UserDataRaw();
						//	friends[x].Deserialize((Hashtable)array[x]);
						//}
						
						Facebook.instance.get( APP_ID + "/scores?fields=score,user", null, ( error3, obj3 ) =>
						{
							ArrayList scoresList = (ArrayList)((Hashtable)obj3)["data"];
							for(int x=0;x<scoresList.Count;x++){
								
							}
							for(int x=scores.Count;x<7;x++){
								UserData.UserDataRaw user = new UserData.UserDataRaw();
								user.ImageLocalFileName = "fakeFriend_0" + (x+1).ToString() + "@2x";
								user.UserName = fakeFriendName[x];
								user.Score = 0;
								user.Level = 1;
								scores.Add(user);
							}
							
							CreateFriendScoreList();
						});
					}
						
				});
			}
		});*/
		//Facebook.instance.graphRequest( "me?fields=id,name,picture.type(normal)", AddMe );
		//Facebook.instance.restRequest( "me?fields=id,name,picture.type(normal)", Prime31.HTTPVerb.GET, null, AddMe );
		//Facebook.instance.restRequest( "me", Prime31.HTTPVerb.GET, null, AddMe );
		//Facebook.instance.graphRequest( APP_ID + "/scores", completeFrendsScores );
		
	}
	
	void CreateFriendScoreList(){
		Utils.DestroyAllChild(panel_users);
		
		for(int x=0;x<scores.Count;x++){
			UserData.UserDataRaw user = (UserData.UserDataRaw)scores[x];
			user.Index = x;
			GameObject userLine = (GameObject)Instantiate(userLinePrefab, Vector3.zero, Quaternion.identity);
			userLine.transform.parent = panel_users;
			userLine.transform.localPosition = new Vector3(0,-x*102,0);
			userLine.transform.localScale = new Vector3(1,1,1);
			userLine.GetComponent<UserData>().SetUserDataRaw(user);			
		}
		Invoke("SetListPositionToBegin", .01f);
		//scrollPanel.SetDragAmount(0,0,true);		
	}
	
	void SetListPositionToBegin(){
		scrollPanel.SetDragAmount(0,0,false);	
	}
	
	void AddMe(string error, object result){
		string str = result.ToString();
		
		result = null;
		/*if(error == null){
			Hashtable hTable = (Hashtable)result;
			string str = hTable.ToString();
			str = "";
		
			me.UserName = hTable.Values[""];
			me.ID = hTable.Values[""];
		}*/
	}
	
	void completeFrendsScores( string error, object result ){
		if( error != null )
			Debug.LogError( error );
		else{
			AddFrendToList(result);
		}
	}
	
	
	void AddFrendToList(object result){
		
	}
	
	public void Login(){ 
		UpdateUsersList();
		//FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		//Facebook.instance.graphRequest( "123365184515949/scores", completionLogin );
	}
	
	
	
	
	void completionLogin( string error, object result )
	{
		if( error != null )
			Debug.LogError( error );
		else{
			if(FacebookAndroid.isSessionValid()){
				
			}
		}
	}
}
