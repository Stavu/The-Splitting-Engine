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
	public string itemToRemove;

	public string conversationName;
	public string dialogueOptionTitle;
	public string dialogueTreeName;

	public string animationToPlay;
	public string animationToPlayOn;
	public string targetFurniture;

	public string soundToPlay;
	public int numberOfPlays;

	public string soundToStop;

	public string eventToAdd;
	public string eventToRemove;

	public string cutsceneToPlay;

	public string PItoHide;
	public string PItoShow;

	public string specialSubInt;
	public string roomToReset;

	public string newPlayer;

	public bool isImportant;

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
		//Debug.Log ("subinteract");

		switch (interactionType) 
		{
			
			case "showMonologue":

				InteractionManager.instance.DisplayText (Utilities.CreateSentenceList(PlayerManager.myPlayer, textList), isImportant);

				break;


			case "showDialogue":

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

				break;


			case "PlayAnimationOn":

				PI_Handler.instance.SetPIAnimationState (targetFurniture, animationToPlayOn);
				EventsHandler.Invoke_cb_inputStateChanged ();

				break;


			case "PlaySound":

				SoundManager.Invoke_cb_playSound (soundToPlay, numberOfPlays);
				EventsHandler.Invoke_cb_inputStateChanged ();

				break;

			
			case "StopSound":

				SoundManager.Invoke_cb_stopSound (soundToStop);
				EventsHandler.Invoke_cb_inputStateChanged ();

				break;


			case "PlayCutscene":

				Debug.Log ("subinteract" + "play cut scene");

				CutsceneManager.instance.PlayCutscene(cutsceneToPlay);

				break;				


			case "moveToRoom":

				InteractionManager.instance.MoveToRoom (destinationRoomName, entrancePoint);

				break;


			case "intoShadows":

				Debug.Log ("into shadows");

				InteractionManager.instance.ChangeShadowState (true);
			
				break;


			case "outOfShadows":

				Debug.Log ("out of shadows");

				InteractionManager.instance.ChangeShadowState (false);
				InteractionManager.instance.ResetRoom (RoomManager.instance.myRoom.myName);

				break;


			case "pickUpItem":

				InteractionManager.instance.PickUpItem (inventoryItem);
				ActionBoxManager.instance.CloseFurnitureFrame ();			

				EventsHandler.Invoke_cb_inputStateChanged ();

				break;


			case "removeItem":

				InteractionManager.instance.RemoveItem (itemToRemove);
				ActionBoxManager.instance.CloseFurnitureFrame ();			

				EventsHandler.Invoke_cb_inputStateChanged ();

				break;


			case "useItem":

				InteractionManager.instance.OpenInventory_UseItem (ActionBoxManager.instance.currentPhysicalInteractable);

				break;


			case "changeConversation":


				Debug.Log ("change conversation");
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


			case "hidePI":
				

				PI_Handler.instance.Hide_PI (PItoHide);
				break;



			case "showPI":

				PhysicalInteractable tempPI = RoomManager.instance.getFurnitureByName (PItoShow);

				PI_Handler.instance.UnHide_PI (tempPI);
				break;

			case "special":
							
				InteractionManager.instance.SpecialInteraction (specialSubInt);

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
