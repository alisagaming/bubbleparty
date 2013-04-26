using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("model")]
public class BoostRuleset {
	[XmlElement("rule")]
	public BoostRule[] rules;
	
	int[] numBoostsForLevel = new int[30];
	public int[] bonusValueForLevel = new int[30];
	
	int[] startOnRow = null;
	
	public static BoostRuleset LoadFromFile(string fileName){
		TextAsset tAsset = Resources.Load(fileName)as TextAsset; 
		TextReader tr = new StringReader(tAsset.text);
		
		var serializer = new XmlSerializer(typeof(BoostRuleset));
		BoostRuleset boostRuleset = serializer.Deserialize(tr) as BoostRuleset;
		boostRuleset.Prepare();
		return boostRuleset;
	}
	
	public bool IsBoostStart(int rowIndex){
		if(startOnRow == null) return false;
		for(int x=0;x<startOnRow.Length;x++)
			if(startOnRow[x] == rowIndex) return true;
		return false;
	}
	
	public void GenerateForLevel(int level, int levelLength){
		int boostCount = numBoostsForLevel[level-1];
		if(boostCount>0){
			startOnRow = new int[boostCount];
			while(boostCount>0){
				startOnRow[boostCount-1] = Random.Range(0,levelLength-1);
				boostCount--;
			}
		}else startOnRow = null;
	}
	
	void Prepare(){
		for(int x=0;x<rules.Length;x++){
			BoostRule curRule = rules[x];
			for(int y=curRule.levelset_from;y<=curRule.levelset_to;y++){
				numBoostsForLevel[y-1] = curRule.numBoosts;
				bonusValueForLevel[y-1] = curRule.bonusValue;				
			}
		}
	}
}
