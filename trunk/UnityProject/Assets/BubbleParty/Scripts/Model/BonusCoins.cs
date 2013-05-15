using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("model")]
public class BonusCoins {
	[XmlArray("bonusCoins"),XmlArrayItem("item")]
	public ConfigLine[] bonusCoins;
	[XmlArray("reducedCoins"),XmlArrayItem("item")]
	public ConfigLine[] reducedCoins;
	
	
	public static BonusCoins LoadFromFile(string fileName){
		TextAsset tAsset = Resources.Load(fileName)as TextAsset; 
		TextReader tr = new StringReader(tAsset.text);
		
		var serializer = new XmlSerializer(typeof(BonusCoins));
		BonusCoins bonusCoins = serializer.Deserialize(tr) as BonusCoins;
		return bonusCoins;
	}
		
}
