using UnityEngine;
using System;
using System.IO;

using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour 
{
	public XmlNodeList layers;

	public GridManager gridManager;
	void Start () 
	{
		

	}
	public void LoadLevel(int p_level)
	{
		Debug.Log ("Levels/Levels_" + p_level.ToString ());
		string __xmlInfo =  File.ReadAllText(Application.dataPath + "/Resources/Levels/Level_" + p_level.ToString() + ".tmx");
		if (__xmlInfo.Length == 0)
		{
			Debug.Log("Nop");
			return;
		}
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (__xmlInfo);
		layers = xmlDoc.SelectNodes ("//layer");

		XmlNode __tempNode = GetLayer ("Floor");
		gridManager.LoadTiles (int.Parse(__tempNode.Attributes ["width"].Value), 
			int.Parse(__tempNode.Attributes ["height"].Value),
			GetLayerData (__tempNode));
		
		__tempNode = GetLayer ("Scenery");
		gridManager.LoadScenery(int.Parse(__tempNode.Attributes ["width"].Value), 
			int.Parse(__tempNode.Attributes ["height"].Value),
			GetLayerData (__tempNode));
	}

	public XmlNode GetLayer(string p_layerName)
	{
		foreach (XmlNode __node in layers)
			if (__node.Attributes["name"].Value == p_layerName)
				return __node;
		return null;
	}
	public List<int> GetLayerData(XmlNode p_layer)
	{
		List<int> __data = new List<int>();
		foreach (XmlNode __node in p_layer.ChildNodes[0].ChildNodes)
			__data.Add(int.Parse(__node.Attributes ["gid"].Value));
		return __data;
	}
}
