using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class BoostRule{
	[XmlAttribute("levelset_from")]
	public int levelset_from;
	[XmlAttribute("levelset_to")]
	public int levelset_to;
	[XmlAttribute("numBoosts")]
	public int numBoosts;
	[XmlAttribute("bonusValue")]
	public int bonusValue;
}
