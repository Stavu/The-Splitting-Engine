using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



[Serializable]
public class SubInteraction : IConditionable {


	public string interactionType;

	public List<string> textList;
	public List<Condition> conditionList;

	public Direction direction;

	public string destinationRoomName;
	public Vector2 entrancePoint;

	public string ItemToUseName;
	public bool ItemToUseRemoveBool;
	//public List<InventoryItem> inventoryItems;

	public InventoryItem inventoryItem;

	public string conversationName;
	public string dialogueOptionTitle;
	public string dialogueTreeName;

	public string animationToPlay;
	public string targetFurniture;

	public string soundToPlay;
	public int numberOfPlays;

	public string soundToStop;

	public string eventToAdd;
	public string eventToRemove;

	public string newPlayer;

	public string rawText;
	public string RawText 
	{
		get
		{ 
			return rawText;
		}

		set 
		{
			rawText = value;
			textList = Utilities.SeparateText (rawText);
		}
	}


	public List<Condition> ConditionList 
	{
		get
		{ 
			return conditionList;
		}

		set 
		{
			conditionList = value;
		}
	}




	// move to room interaction


	public SubInteraction (string interactionType)
	{
		this.interactionType = interactionType;
		conditionList = new List<Condition>();
	}



	public void RemoveConditionFromList(Condition condition)
	{
		if (condition == null) 
		{
			Debug.LogError ("condition is null");
			return;
		}

		if (conditionList.Contains (condition) == false) 
		{
			Debug.LogError ("condition is not in list");
			return;
		}

		conditionList.Remove (condition);
	}




	// ----- SUBINTERACT ----- //



	public void SubInteract ()
	{
		
		switch (interactionType) 
		{
			
			case "showMonologue":

				//Debug.Log ("SubInteract: Show monologue");

				InteractionManager.instance.DisplayText (Utilities.CreateSentenceList(PlayerManager.myPlayer, textList));

				break;


			case "showDialogue":

				//Debug.Log ("SubInteract: Show dialogue");

				InteractionManager.instance.DisplayDialogueOption (this.dialogueOptionTitle);

				break;


			case "showDialogueTree":

				DialogueTree dialogueTree = GameManager.gameData.nameDialogueTreeMap [this.dialogueTreeName];

				if (dialogueTree.currentConversation == null) 
				{					
					if (dialogueTree.conversationList.Count == 0) 
					{					
						Debug.LogError ("There are no conversations");
					}

					dialogueTree.currentConversation = dialogueTree.conversationList [0];				
				}

				DialogueManager.instance.ActivateDialogueTree (dialogueTree);

				break;

			
			case "PlayAnimation":

				PI_Handler.instance.SetPIAnimationState (targetFurniture, animationToPlay);
				EventsHandler.Invoke_cb_inputStateChanged ();
				//GameManager.instance.inputState = InputState.Character;

				break;


			case "PlaySound":

				SoundManager.Invoke_cb_playSound (soundToPlay, numberOfPlays);
				EventsHandler.Invoke_cb_inputStateChanged ();
				//GameManager.instance.inputState = InputState.Character;

				break;

			
			case "StopSound":

				SoundManager.Invoke_cb_stopSound (soundToStop);
				EventsHandler.Invoke_cb_inputStateChanged ();

				break;


			case "playCutscene":

				//CutsceneManager.instance.c

				break;				


			case "moveToRoom":

				InteractionManager.instance.MoveToRoom (destinationRoomName, entrancePoint);

				break;


			case "intoShadows":

				InteractionManager.instance.ChangeShadowState (true);

				break;


			case "outOfShadows":

				InteractionManager.instance.ChangeShadowState (false);

				break;


			case "pickUpItem":

				InteractionManager.instance.PickUpItem (inventoryItem);
				ActionBoxManager.instance.CloseFurnitureFrame ();			

				EventsHandler.Invoke_cb_inputStateChanged ();

				break;


			case "useItem":

				InteractionManager.instance.OpenInventory_UseItem (ActionBoxManager.instance.currentPhysicalInteractable);

				break;


			case "changeConversation":

				DialogueManager.instance.SetConversation (conversationName);

				break;


			case "endDialogueTree":

				DialogueManager.instance.DestroyDialogueTree ();

				break;


			case "showInventoryText":
				
				InteractionManager.instance.DisplayInventoryText (textList);

				break;


			case "changeInventoryItemBigPicture":

				//InteractionManager.instance.ChangeInventoryItemBigPicture (fileName);

				break;


			case "combine":
				
				InteractionManager.instance.OpenInventory_CombineItem ();

				break;
			

			case "addEvent":

				GameManager.userData.AddEventToList (eventToAdd);

				break;


			case "removeEvent":

				GameManager.userData.RemoveEventFromList (eventToRemove);

				break;


			case "switchPlayer":

				PlayerManager.instance.SwitchPlayer (newPlayer);
				break;
		}
	}


	public void ResetDataFields()
	{

		this.RawText = string.Empty;
		this.destinationRoomName = string.Empty;

		this.inventoryItem = null;
	}


}




// Interface

public interface ISubinteractable
{	
	List<SubInteraction> SubIntList { get; set; }
}
