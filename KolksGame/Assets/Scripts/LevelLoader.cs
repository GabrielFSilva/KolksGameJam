using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour 
{
	public event Action <int, int> 			OnSetGridDimensions;
	public event Action <int, List<int>> 	OnSendLayerData;
	public event Action <int> 				OnSetEnergy;
	public event Action <bool, bool> 		OnSetLevelActions;
	public event Action <int, int> 			OnSetStarValues;

	public XmlNodeList layers;
	public XmlNodeList levelInfo;


	public void LoadLevel(int p_level, string p_levelToLoadName, bool p_isOnTestMode)
	{
		string __xmlInfo = "";

		if (p_isOnTestMode)
			//__xmlInfo = File.ReadAllText (Application.dataPath + "/Resources/Levels/" + p_levelToLoadName + ".tmx");
			__xmlInfo = Resources.Load<TextAsset>("Levels/" + p_levelToLoadName).text;
		else
			//__xmlInfo = File.ReadAllText (Application.dataPath + "/Resources/Levels/Level_" + p_level.ToString () + ".tmx");
			__xmlInfo = Resources.Load<TextAsset>("Levels/Level_" + p_level.ToString ()).text;

		if (__xmlInfo.Length == 0)
		{
			Debug.LogWarning("Level Not Found");
			return;
		}
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (__xmlInfo);
		layers = xmlDoc.SelectNodes ("//layer");

		XmlNode __tempNode = GetLayer ("Floor");
		OnSetGridDimensions (int.Parse (__tempNode.Attributes ["width"].Value), int.Parse (__tempNode.Attributes ["height"].Value));
		OnSendLayerData (0, GetTileLayerData (__tempNode));
		__tempNode = GetLayer ("Scenery");
		OnSendLayerData (1, GetTileLayerData (__tempNode));
		__tempNode = GetLayer ("Entities");
		OnSendLayerData (2, GetTileLayerData (__tempNode));
		__tempNode = GetLayer ("IA");
		OnSendLayerData (3, GetTileLayerData (__tempNode));

		levelInfo = xmlDoc.SelectNodes ("//objectgroup/object/properties/property");
		OnSetEnergy(int.Parse(GetNodeWithName(levelInfo,"Energy").Attributes["value"].Value));
		OnSetLevelActions (bool.Parse (GetNodeWithName (levelInfo, "Hello").Attributes ["value"].Value),
			bool.Parse (GetNodeWithName (levelInfo, "ExcuseMe").Attributes ["value"].Value));
		OnSetStarValues (int.Parse (GetNodeWithName (levelInfo, "3Star").Attributes ["value"].Value),
			int.Parse (GetNodeWithName (levelInfo, "2Star").Attributes ["value"].Value));
	}
	public XmlNode GetNodeWithName(XmlNodeList p_nodeList, string p_attributeName)
	{
		foreach (XmlNode __node in p_nodeList)
			if (__node.Attributes ["name"].Value == p_attributeName)
				return __node;
		Debug.LogWarning ("Variable not found: " + p_attributeName);
		return null;
	}
	public XmlNode GetLayer(string p_layerName)
	{
		foreach (XmlNode __node in layers)
			if (__node.Attributes["name"].Value == p_layerName)
				return __node;
		return null;
	}
	public List<int> GetTileLayerData(XmlNode p_layer)
	{
		List<int> __data = new List<int>();
		foreach (XmlNode __node in p_layer.ChildNodes[0].ChildNodes)
			__data.Add(int.Parse(__node.Attributes ["gid"].Value));
		return __data;
	}
}
