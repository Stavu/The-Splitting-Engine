using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class InventoryItem
{

	public string titleName;
	public string fileName;

	[NonSerialized]
	public string lookAtText;

	[NonSerialized]
	public List<Interaction> inventoryItemInteractionList;



	public void Initialize()
	{

		inventoryItemInteractionList = new List<Interaction>();


		// Look at interaction

		Interaction interactionLookAt = new Interaction ();
		interactionLookAt.myVerb = "Look At";
		SubInteraction subInt_displayInventoryText = new SubInteraction ("showInventoryText");
		subInt_displayInventoryText.textList = new List<string> ();

		// take the correct textList from the itemLookAtMap, and put it in the textList of the subinteraction 

		if (GameManager.gameData.itemLookAtMap.ContainsKey (fileName))
		{
			subInt_displayInventoryText.textList = GameManager.gameData.itemLookAtMap [fileName];
		}

		interactionLookAt.subInteractionList.Add (subInt_displayInventoryText);


		// Combine interaction
	
		Interaction interactionCombine = new Interaction ();
		interactionCombine.myVerb = "Combine";
		SubInteraction subInt_combine = new SubInteraction ("combine");
		interactionCombine.subInteractionList.Add (subInt_combine);



		Interaction interactionOpen = new Interaction ();
		interactionOpen.myVerb = "Open";

		inventoryItemInteractionList.Add (interactionLookAt);
		inventoryItemInteractionList.Add (interactionCombine);
		inventoryItemInteractionList.Add (interactionOpen);

	}



	public InventoryItem(string fileName, string titleName)
	{

		this.fileName = fileName;
		this.titleName = titleName;


	}



}
