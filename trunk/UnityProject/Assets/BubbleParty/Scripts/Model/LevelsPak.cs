using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("model")]
public class LevelsPak {
	//[XmlArray("levels"),XmlArrayItem("level")]
	//public List<Level> levels = new List<Level>();
	[XmlElement("level")]
	public Level[] levels;
}
