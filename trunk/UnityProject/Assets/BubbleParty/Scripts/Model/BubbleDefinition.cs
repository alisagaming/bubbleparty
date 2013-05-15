using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("model")]
public class BubbleDefinition {
	[XmlArray("tutorialSets"),XmlArrayItem("integer")]
	public int[] tutorialSets;
	
	[XmlArray("playSets"),XmlArrayItem("integer")]
	public int[] playSets;
	
	public static BubbleDefinition LoadFromFile(string fileName){
		TextAsset tAsset = Resources.Load(fileName)as TextAsset; 
		TextReader tr = new StringReader(tAsset.text);
		
		var serializer = new XmlSerializer(typeof(BubbleDefinition));
		BubbleDefinition bubbleDefinition = serializer.Deserialize(tr) as BubbleDefinition;
		return bubbleDefinition;
	}
}
