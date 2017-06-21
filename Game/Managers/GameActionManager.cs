using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoBehaviour {


	// Singleton //

	public static GameActionManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public TileInteraction currentTileInteraction;


	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_spacebarPressed += OnSpacebarPressed;
		EventsHandler.cb_escapePressed += OnEscapePressed;

		EventsHandler.cb_playerHitTileInteraction += OnHitTileInteraction;
		EventsHandler.cb_playerLeaveTileInteraction += OnLeaveTileInteraction;

		EventsHandler.cb_inputStateChanged += ManageInputState;

	}


	public void OnDestroy()
	{
		EventsHandler.cb_spacebarPressed -= OnSpacebarPressed;
		EventsHandler.cb_escapePressed -= OnEscapePressed;

		EventsHandler.cb_playerHitTileInteraction -= OnHitTileInteraction;
		EventsHandler.cb_playerLeaveTileInteraction -= OnLeaveTileInteraction;

		EventsHandler.cb_inputStateChanged -= ManageInputState;

	}


	
	// Update is called once per frame
	void Update () 
	{
		
	}



	// SPACEBAR PRESSED // 


	public void OnSpacebarPressed ()
	{
		switch (GameManager.instance.inputState) 
		{

			case InputState.ActionBox:

				// If there is already an actionbox, activate interaction

				if (ActionBoxManager.instance.currentActionBox != null) 
				{
					ActionBoxManager.instance.ActivateInteraction ();
					return;
				}

				break;



			case InputState.Inventory:

				
				switch (GameManager.userData.GetCurrentPlayerData().inventory.myState) 
				{

					case InventoryState.UseItem:

						InventoryUI.instance.SelectItemToUse ();

						break;				


					case InventoryState.Browse:

						InventoryUI.instance.ActivateInteraction ();

						break;	


					case InventoryState.Combine:

						InventoryUI.instance.CombineItems ();

						break;
				}	

				return;		



			//case InputState.Cutscene:
			case InputState.DialogueBox:
				
				
				if (DialogueManager.instance.currentDialogueOption != null)
				{						
					DialogueManager.instance.ActivateDialogueOption ();
					return;
				}

				break;



			case InputState.Character:

				// if there's no action box, create one

				if (ActionBoxManager.instance.currentPhysicalInteractable != null) 
				{
					ActionBoxManager.instance.OpenActionBox ();
				}


				break;

		}

	}





	public void OnEscapePressed()
	{

		if (GameManager.instance.inputState == InputState.Inventory) 
		{
			InventoryUI.instance.CloseInventory();
			return;
		}

		if (GameManager.instance.inputState == InputState.Settings) 
		{
			SettingsUI.instance.CloseSettings();
			return;
		}

		if (GameManager.instance.inputState == InputState.Map) 
		{
			MapUI.instance.CloseMap();
			return;
		}

		ActionBoxManager.instance.CloseActionBox ();
	}


	// manage input stat

	public void ManageInputState()
	{
		if (NavigationManager.navigationInProcess == true) 
		{
			GameManager.instance.inputState = InputState.NoInput;
			return;
		}


		if (CutsceneManager.inCutscene == true) 
		{
			GameManager.instance.inputState = InputState.Cutscene;
			return;
		}


		if (GameManager.actionBoxActive == true) 
		{
			GameManager.instance.inputState = InputState.ActionBox;
			return;
		}


		if (GameManager.dialogueTreeBoxActive == true) 
		{
			if (DialogueManager.instance.dialogueTreeObject.gameObject.active == true) 
			{
				GameManager.instance.inputState = InputState.DialogueBox;

			} else {
				
				GameManager.instance.inputState = InputState.Dialogue;
			}

			return;
		}


		if (GameManager.textBoxActive == true) 
		{
			GameManager.instance.inputState = InputState.Dialogue;
			return;
		}

		if (GameManager.inventoryOpen == true) 
		{
			GameManager.instance.inputState = InputState.Inventory;
			return;
		}

		if (GameManager.settingsOpen == true) 
		{
			GameManager.instance.inputState = InputState.Settings;
			return;
		}

		if (GameManager.mapOpen == true) 
		{
			GameManager.instance.inputState = InputState.Map;
			return;
		}


		GameManager.instance.inputState = InputState.Character;

	}


	// Tile Interaction //

	public void OnHitTileInteraction(Tile tile)
	{		
		TileInteraction tileInt = tile.myTileInteraction;

		if (currentTileInteraction != tileInt) 
		{
			currentTileInteraction = tileInt;

			if (currentTileInteraction != null)
			{	
				// Check if passed the conditions
			
				if (Utilities.EvaluateConditions (tileInt.mySubInt.ConditionList))
				{
					tileInt.mySubInt.SubInteract ();
				}
			}
		}
	}



	public void OnLeaveTileInteraction ()
	{
		currentTileInteraction = null;
	}

}
