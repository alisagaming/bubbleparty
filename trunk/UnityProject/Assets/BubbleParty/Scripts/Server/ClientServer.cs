using UnityEngine;
using System.Collections;
using System.Text;

using Pathfinding.Serialization.JsonFx;

public class ClientServer{
	static string ServerUrl = "http://192.168.1.129:8080/";
	
	public class Test{
		//public int id = 1;
		public string name = "И";
	}
	
	public static IEnumerator Sync(){
		string url = ServerUrl + "players";///" + SystemInfo.deviceUniqueIdentifier;
		
		/*Test test = new Test(); 
		string str = JsonWriter.Serialize(test);
		
		str = WWW.UnEscapeURL("Привет",Encoding.ASCII);
		
		str = "Pi(\u03a0)";
		
		byte[] unicodeBytes = Encoding.Unicode.GetBytes(str);
		byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
		char[] asciiChars = new char[Encoding.ASCII.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
        Encoding.ASCII.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
        string asciiString = new string(asciiChars);
		asciiString = "";
		
		/*byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
		str = System.Text.Encoding.Unicode.GetString(arr);
		str = "";*/
		//Test test = new Test();
		string str = JsonWriter.Serialize(GameVariables.playerParameters);
		//WWW www = new WWW(url);
		//www.InitWWW(
		
		//WWWForm form = new WWWForm();
		//form.AddField("", str, Encoding.);
		/*form.AddField("op","download1");
		form.AddField("usr_login", "");
		form.AddField("id", "clruomb0apvq");
		form.AddField("fname", "");
		form.AddField("referer", "");
		form.AddField("method_free", "Slow Download");
		form.AddField("method_premium", "High Speed Download");*/
		
		//WWW www = new WWW(url, form);
		/*str="ЙЦУКЕН";*/
		byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
		int l = str.Length;
		
		WWW www = new WWW(url, arr);
		yield return www;
		str = www.text;
		str = "";
	}
		
}
