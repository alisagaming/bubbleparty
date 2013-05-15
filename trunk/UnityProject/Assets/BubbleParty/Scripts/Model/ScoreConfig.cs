using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("model")]
public class ScoreConfig{
	public int boostScore;
	[XmlArray("droppedScore"),XmlArrayItem("item")]
	public ConfigLine[] droppedScore;
	
	public int droppedScorePerBubble;
	public int explodedScorePerBubble;
	
	[XmlArray("explodedScores"),XmlArrayItem("item")]
	public ConfigLine[] explodedScores;
	
	public float fireballScoreMultiplier;
	public float plasmaScoreMultiplier;
	
	public int fixedFireballScore;
	public int fixedPlasmaScore;
	[XmlArray("rowScores"),XmlArrayItem("item")]
	public ConfigLine[] rowScores;
	[XmlArray("soundsByScore"),XmlArrayItem("item")]
	public ConfigLine[] soundsByScore;
	public int speedModeScore;	
	
	
	public string getValue(int pos, ConfigLine[] arrays){
		for(int x=0;x<arrays.Length;x++){
			if(pos<arrays[x].amount){
				if(x==0) return "";
				else return arrays[x-1].score;
			}
		}
		return arrays[arrays.Length-1].score;
	}
	
	public int getValueInt(int pos, ConfigLine[] arrays){		
		string str = getValue(pos,arrays);
		if(str.Equals("")) return 0;
		else return int.Parse(str);
	}
}
