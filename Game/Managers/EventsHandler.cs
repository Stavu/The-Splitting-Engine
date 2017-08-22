using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public static class EventsHandler
{


	// Input Manager 


	public static Action cb_inputStateChanged; 

	public static void Invoke_cb_inputStateChanged()
	{
		if(cb_inputStateChanged != null)
		{
			cb_inputStateChanged ();
		}
	}



	public static Action<Direction> cb_keyPressed; 

	public static void Invoke_cb_keyPressed(Direction direction)
	{
		if(cb_keyPressed != null)
		{
			cb_keyPressed (direction);
		}
	}


	public static Action<Direction> cb_keyPressedDown;

	public static void Invoke_cb_keyPressedDown(Direction direction)
	{
		if(cb_keyPressedDown != null)
		{
			cb_keyPressedDown (direction);
		}
	}


	public static Action<Direction> cb_noKeyPressed;

	public static void Invoke_cb_noKeyPressed(Direction direction)
	{
		if(cb_noKeyPressed != null)
		{
			cb_noKeyPressed (direction);
		}

	}


	public static Action cb_spacebarPressed;

	public static void Invoke_cb_spacebarPressed()
	{
		if(cb_spacebarPressed != null)
		{
			cb_spacebarPressed ();
		}

	}


	public static Action cb_escapePressed;

	public static void Invoke_cb_escapePressed()
	{
		if(cb_escapePressed != null)
		{
			cb_escapePressed ();
		}

	}


	// UI

	public static Action cb_key_i_pressed;

	public static void Invoke_cb_key_i_pressed()
	{
		if(cb_key_i_pressed != null)
		{
			cb_key_i_pressed ();
		}

	}


	public static Action cb_key_p_pressed;

	public static void Invoke_cb_key_p_pressed()
	{
		if(cb_key_p_pressed != null)
		{
			cb_key_p_pressed ();
		}

	}



	public static Action cb_key_m_pressed;

	public static void Invoke_cb_key_m_pressed()
	{
		if(cb_key_m_pressed != null)
		{
			cb_key_m_pressed ();
		}

	}




	// Player Manager 


	public static Action<Player> cb_playerCreated; 

	public static void Invoke_cb_playerCreated(Player player)
	{
		if(cb_playerCreated != null)
		{
			cb_playerCreated (player);
		}

	}



	public static Action<PhysicalInteractable> cb_playerHitPhysicalInteractable; 

	public static void Invoke_cb_playerHitPhysicalInteractable(PhysicalInteractable physicalInt)
	{
		if(cb_playerHitPhysicalInteractable != null)
		{
			cb_playerHitPhysicalInteractable (physicalInt);
		}

	}



	public static Action cb_playerLeavePhysicalInteractable; 

	public static void Invoke_cb_playerLeavePhysicalInteractable()
	{
		if(cb_playerLeavePhysicalInteractable != null)
		{
			cb_playerLeavePhysicalInteractable ();
		}

	}


	public static Action<Tile> cb_playerHitTileInteraction;

	public static void Invoke_cb_playerHitTileInteraction(Tile tile)
	{
		if(cb_playerHitTileInteraction != null)
		{
			cb_playerHitTileInteraction (tile);
		}

	}


	public static Action cb_playerLeaveTileInteraction;

	public static void Invoke_cb_playerLeaveTileInteraction()
	{
		if(cb_playerLeaveTileInteraction != null)
		{
			cb_playerLeaveTileInteraction ();
		}

	}


	// Shadow state


	public static Action<bool> cb_shadowStateChanged;

	public static void Invoke_cb_shadowStateChanged(bool intoShadows)
	{
		if(cb_shadowStateChanged != null)
		{
			cb_shadowStateChanged (intoShadows);
		}
	}



	// Room Manager 


	public static Action<Room> cb_roomCreated; 

	public static void Invoke_cb_roomCreated(Room room)
	{
		if(cb_roomCreated != null)
		{
			cb_roomCreated (room);
		}

	}


	public static Action<Room> cb_entered_room; 

	public static void Invoke_cb_entered_room(Room room)
	{
		if(cb_entered_room != null)
		{
			cb_entered_room (room);
		}
	}


	public static Action<Furniture> cb_furnitureChanged; 

	public static void Invoke_cb_furnitureChanged(Furniture furniture)
	{
		if(cb_furnitureChanged != null)
		{
			cb_furnitureChanged (furniture);
		}

		Invoke_cb_tileLayoutChanged ();	

	}


	public static Action<Character> cb_characterChanged; 

	public static void Invoke_cb_characterChanged(Character character)
	{
		if(cb_characterChanged != null)
		{
			cb_characterChanged (character);
		}

		Invoke_cb_tileLayoutChanged ();	
	}


	public static Action<Player> cb_playerChanged; 

	public static void Invoke_cb_playerChanged(Player player)
	{
		if(cb_playerChanged != null)
		{
			cb_playerChanged (player);
		}

		Invoke_cb_tileLayoutChanged ();	
	}


	public static Action<TileInteraction> cb_tileInteractionChanged; 

	public static void Invoke_cb_tileInteractionChanged(TileInteraction tileInt)
	{
		if(cb_tileInteractionChanged != null)
		{
			cb_tileInteractionChanged (tileInt);
		}

		Invoke_cb_tileLayoutChanged ();	

	}


	// Cutscene events

	public static Action cb_dialogueEnded;
	public static Action cb_characterFinishedPath;
	public static Action cb_isClearToContinue;

	public static void Invoke_Callback (Action callback) 
	{
		if (callback != null)
		{
			callback ();
		}
	}


	// Animation Events


	public static Action<PhysicalInteractable, string> cb_newAnimationState; 

	public static void Invoke_cb_newAnimationState(PhysicalInteractable physicalInteractable, string state)
	{
		if(cb_newAnimationState != null)
		{
			cb_newAnimationState (physicalInteractable, state);
		}
	}


	// Player 

	public static Action<Player> cb_playerSwitched;

	public static void Invoke_cb_playerSwitched(Player player)
	{
		if(cb_playerSwitched != null)
		{
			cb_playerSwitched (player);
		}

	}


	public static Action<Player> cb_playerMove; 

	public static void Invoke_cb_playerMove(Player player)
	{
		if(cb_playerMove != null)
		{
			cb_playerMove (player);
		}

	}



	// Inventory


	public static Action<Inventory> cb_inventoryChanged;

	public static void Invoke_cb_inventoryChanged(Inventory inventory)
	{
		if(cb_inventoryChanged != null)
		{
			cb_inventoryChanged (inventory);
		}

	}

	public static Action<InventoryItem> cb_itemAddedToInventory;

	public static void Invoke_cb_itemAddedToInventory(InventoryItem inventoryItem)
	{
		if(cb_itemAddedToInventory != null)
		{
			cb_itemAddedToInventory (inventoryItem);
		}

	}



	/* ------- EDITOR -------- */



	// Editor Room Manager 


	public static Action<Room> cb_editorNewRoomCreated;

	public static void Invoke_cb_editorNewRoomCreated(Room room)
	{
		if(cb_editorNewRoomCreated != null)
		{
			cb_editorNewRoomCreated (room);
		}

	}


	/*
	public static Action<string> cb_editorBgNameChanged;

	public static void Invoke_cb_editorBgNameChanged(string bgName)
	{
		if(cb_editorBgNameChanged != null)
		{
			cb_editorBgNameChanged (bgName);
		}

	}
	*/



	// Action for tile manager - coloring tiles


	public static Action cb_tileLayoutChanged;

	public static void Invoke_cb_tileLayoutChanged()
	{		

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}


	public static Action<Furniture> cb_editorFurnitureModelChanged;

	public static void Invoke_cb_editorFurnitureModelChanged(Furniture furniture)
	{
		if(cb_editorFurnitureModelChanged != null)
		{
			cb_editorFurnitureModelChanged (furniture);

		}

		Invoke_cb_tileLayoutChanged ();

	}


	public static Action<Character> cb_editorCharacterModelChanged;


	public static void Invoke_cb_editorCharacterModelChanged(Character character)
	{
		if(cb_editorCharacterModelChanged != null)
		{
			cb_editorCharacterModelChanged (character);

		}

		Invoke_cb_tileLayoutChanged ();

	}



	public static Action<TileInteraction> cb_editorTileInteractioneModelChanged;

	public static void Invoke_cb_editorTileInteractioneModelChanged(TileInteraction tileInteraction)
	{
		if(cb_editorTileInteractioneModelChanged != null)
		{
			cb_editorTileInteractioneModelChanged (tileInteraction);
		}

		Invoke_cb_tileLayoutChanged ();

	}


	public static Action<Furniture> cb_editorFurniturePlaced;

	public static void Invoke_cb_editorFurniturePlaced(Furniture furniture)
	{
		if(cb_editorFurniturePlaced != null)
		{
			cb_editorFurniturePlaced (furniture);
		}

		Invoke_cb_tileLayoutChanged ();

	}




	// Condition Inspector 


	public static Action cb_conditionAdded;

	public static void Invoke_cb_conditionAdded()
	{
		if(cb_conditionAdded != null)
		{
			cb_conditionAdded ();
		}

	}


	// Subinteraction Inspector 

	public static Action cb_subinteractionChanged;

	public static void Invoke_cb_subinteractionChanged()
	{
		if(cb_subinteractionChanged != null)
		{
			cb_subinteractionChanged ();
		}
	}




	// Mouse Controller 

	public static Action<Tile> cb_editorTilesSelected;

	public static void Invoke_cb_editorTilesSelected(Tile tile)
	{
		if(cb_editorTilesSelected != null)
		{
			cb_editorTilesSelected (tile);
		}

	}






}
