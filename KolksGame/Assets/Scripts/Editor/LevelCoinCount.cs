using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class LevelCoinCount : MonoBehaviour 
{

	[MenuItem("Util/UpdateStarCountFile")] 
	static void UpdateStarCountFile()
	{
		//Level TextAsset
		TextAsset textAsset;
		//XmlDoc made from the TextAsset
		XmlDocument xmlDoc = new XmlDocument();
		//XmlDoc layers
		XmlNodeList layers;
		//Data from the Entities Layer
		List<int> data = new List<int>();

		//Counter of levels on the game
		int fileCount = 1;

		//List of coins of all levels
		List<int> coinsCountList = new List<int> ();
		//Counter of coins for the current level
		int coinsCount = 0;

		while (true) 
		{
			//Reset variables
			coinsCount = 0;
			data = new List<int>();
			textAsset = Resources.Load<TextAsset>("Levels/Level_" + fileCount.ToString());

			//If don't find the file, end the search
			if (textAsset == null)
			{
				Debug.Log((fileCount-1) + " levels");
				break;
			}
			//Get file text, convert to XmlDoc and get the layers
			xmlDoc.LoadXml (textAsset.text);
			layers = xmlDoc.SelectNodes ("//layer");

			//Find the Entities layer
			foreach (XmlNode __node in layers)
				if (__node.Attributes["name"].Value == "Entities")
				{
					//Create a list of the Entities layer data
					foreach (XmlNode __nodeChild in __node.ChildNodes[0].ChildNodes)
						data.Add(int.Parse(__nodeChild.Attributes ["gid"].Value));
				}

			//Count the amount of coins on the layer
			for (int i = 0; i < data.Count; i ++)
				if (data[i] - 129 >= 8 && data[i] - 129 <= 12)
					coinsCount ++;

			//Add info to the list then change to next file
			coinsCountList.Add(coinsCount);
			fileCount ++;
		}

		//string to be the target file text
		string countListString = (fileCount-1).ToString();

		//Add a space and the coint amount for each stage
		for (int i = 0; i < coinsCountList.Count; i ++)
		{
			countListString += " ";
			countListString += coinsCountList[i].ToString();
		}
		//Write file and save
		File.WriteAllText(Application.dataPath + "/Resources/Levels/LevelsCoinInfo.txt", countListString);
		AssetDatabase.Refresh();
	}
}
