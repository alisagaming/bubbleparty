using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class ConfigLine {
	[XmlAttribute("amount")]
	public int amount;
	[XmlAttribute("score")]
	public string score;
}
