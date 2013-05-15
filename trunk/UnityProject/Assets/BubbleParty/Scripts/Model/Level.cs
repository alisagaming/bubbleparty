using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Level{
	//[XmlArrayItem("row")]
	[XmlElement("row")]
	public string[] rows;
	
	
	public int[] getRow(int index){
		string[] spl = rows[index].Split(',');
		int[] result = new int[spl.Length];
		for(int x=0;x<spl.Length;x++){
			string val = spl[x].Trim();
			if(val.Equals("x"))
				result[x] = -1;
			else
				result[x] = int.Parse(val);
		}
		return result;
	}
}
