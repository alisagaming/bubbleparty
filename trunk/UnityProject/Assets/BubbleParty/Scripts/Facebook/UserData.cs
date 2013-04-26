using UnityEngine;
using System.Collections;

public class UserData : MonoBehaviour {
	public class UserDataRaw{
		public string UserName;
		public string ID;
		public string ImageFileName;
		public string ImageLocalFileName;
		public int Score;
		public int Level;
		public int Index;
		
		public void Deserialize(Hashtable obj){
			UserName = (string)obj["name"];
			ID = (string)obj["id"]; 
			//var img = ()obj["picture"];
			ImageFileName = (string)(((Hashtable)((Hashtable)obj["picture"])["data"])["url"]);
			ImageLocalFileName = null;
		}		
	}
	
	public class UserCompare : IComparer{
			public int Compare (object x, object y){
				return ((UserDataRaw)y).Score - ((UserDataRaw)x).Score;
			}
	}
	
	public UISprite avatar_icon;
	public UISprite avatar_bg;
	public UILabel txt_user_name;
	public UILabel txt_user_number;
	public UILabel txt_user_score;
	public UILabel txt_user_level;
	
	UserDataRaw userDataRaw;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetUserDataRaw(UserDataRaw dataRaw){
		userDataRaw = dataRaw;
		txt_user_name.text = dataRaw.UserName;
		txt_user_level.text = dataRaw.Level.ToString();
		if(dataRaw.Index<9){
			switch(dataRaw.Index){
			case 0:	avatar_bg.spriteName = "frame_gold@2x"; break;
			case 1:	avatar_bg.spriteName = "frame_silver@2x"; break;
			case 2:	avatar_bg.spriteName = "frame_bronze@2x"; break;
			default:avatar_bg.spriteName = "frame_profile@2x"; break;
			}
			txt_user_number.text = "#" + (dataRaw.Index+1);
		}
		else{
			txt_user_number.gameObject.SetActive(false);
			avatar_bg.spriteName = "frame_profile@2x";
		}
		txt_user_score.text = dataRaw.Score.ToString();
		
		if(dataRaw.ImageLocalFileName != null)
			avatar_icon.spriteName = dataRaw.ImageLocalFileName;
		else{
			StartCoroutine (DownloadImage(dataRaw.ImageFileName));
		}
	}
	
	IEnumerator DownloadImage(string imageURL){
		// Wait for the Caching system to be ready
		//while (!Caching.ready)
		//	yield return null;
		
		//using(WWW image = WWW.LoadFromCacheOrDownload (imageURL, 1)){
		using(WWW image = new WWW(imageURL))
		{
		
			yield return image;
			
			Texture2D texture = new Texture2D(128, 128);
			image.LoadImageIntoTexture(texture);
			//avatar_icon.mainTexture = texture;
			//avatar_icon.mainTexture = texture;
				
			//if (www.error != null)
			//	throw new Exception("WWW download had an error:" + www.error);
			//www.LoadImageIntoTexture(avatar_icon.mainTexture);
			//Texture texture = image.texture;
			//object texture = image.assetBundle.Load(imageURL);
			//texture = null;
		}
	}
}
