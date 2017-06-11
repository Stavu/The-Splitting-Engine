using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public class GameData {



	public Dictionary <string,List<string>> itemLookAtMap;
	public Dictionary <string,List<ItemData_CombineInteractions>> itemCombineMap;
	public Dictionary <string,DialogueOption> nameDialogueOptionMap;
	public Dictionary <string,DialogueTree> nameDialogueTreeMap;

	ItemData itemData;


	public GameData()
	{		
		CreateItemLookAtMap ();
		CreateDialogueOptionData ();
		CreateDialogueTreesData ();
	}


	public void CreateItemLookAtMap()
	{
		
		itemLookAtMap = new Dictionary<string, List<string>> ();
		itemCombineMap = new Dictionary <string,List<ItemData_CombineInteractions>> ();


		//itemData = JsonUtility.FromJson<ItemData> (Resources.LoadAll ("Jsons/Data/itemData"));


		System.Object[] myTextAssets = Resources.LoadAll ("Jsons/Data/");

		foreach (TextAsset txt in myTextAssets) 
		{
			itemData = JsonUtility.FromJson<ItemData> (txt.text);
		}

		/* populate itemLookAtMap - go through itemData, for each itemData_lookAt in the lookAtList
		 insert the itemName and textList as the key and value of itemLookAtMap */

		foreach (ItemData_LookAt lookAt in itemData.lookAtList) 
		{
			itemLookAtMap.Add (lookAt.itemName, lookAt.textList);			
		}

		foreach (ItemData_Combine combine in itemData.combineList) 
		{
			itemCombineMap.Add (combine.itemName, combine.itemsToCombineList);
		}


	}


	public void CreateDialogueOptionData()
	{

		nameDialogueOptionMap = new Dictionary<string, DialogueOption> ();

		UnityEngine.Object[] objs = Resources.LoadAll<UnityEngine.Object> ("Jsons/DialogueOptions");

		foreach (TextAsset textAsset in objs) 
		{	
			DialogueOptionData dialogueOptionData = JsonUtility.FromJson<DialogueOptionData> (textAsset.text);
					
			// populating the map 

			foreach (DialogueOption dialogueOption in dialogueOptionData.optionList) 
			{
				nameDialogueOptionMap.Add (dialogueOption.myTitle, dialogueOption);
			}
		}	

	}



	public void CreateDialogueTreesData()
	{

		nameDialogueTreeMap = new Dictionary<string, DialogueTree> ();

		System.Object[] myTextAssets = Resources.LoadAll ("Jsons/DialogueTrees");

		foreach (TextAsset txt in myTextAssets) 
		{
			
			DialogueTree dialogueTree = JsonUtility.FromJson<DialogueTree> (txt.text);
			nameDialogueTreeMap.Add (dialogueTree.myName, dialogueTree);
		}
	}





}




// ------ DATA CLASSES ------ //


// GENERAL //

[Serializable]
public class ItemData {

	public List<ItemData_LookAt> lookAtList;
	public List<ItemData_Combine> combineList;

}


// LOOK AT //

[Serializable]
public class ItemData_LookAt {

	public string itemName;
	public List<string> textList;
}



// COMBINE //

[Serializable]
public class ItemData_Combine {

	public string itemName;
	public List<ItemData_CombineInteractions> itemsToCombineList;

}

[Serializable]
public class ItemData_CombineInteractions {

	public string targetName;
	public List<SubInteraction> subInteractionList;
	

}

