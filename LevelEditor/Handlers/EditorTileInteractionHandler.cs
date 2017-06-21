using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTileInteractionHandler : MonoBehaviour {

	// Use this for initialization

	public void Initialize ()
	{
		
	}

	
	// Update is called once per frame
	void Update () 
	{
		
	}




	// ----- Tile Interaciton ----- //



	public void PlaceTileInteraction(Tile tile)
	{

		Room myRoom = EditorRoomManager.instance.room;


		// If there's already a tileInteraction on this tile, destroy it before creating a new tileInteraction

		if (tile.myTileInteraction != null)
		{
			TileInteraction oldTileInt = tile.myTileInteraction;

			if (EditorRoomManager.instance.room.RoomState == RoomState.Real) 
			{			
				// Real
				EditorRoomManager.instance.room.myTileInteractionList.Remove (oldTileInt);

			} else {

				if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
				{			
					// Shadow
					EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Shadow.Remove (oldTileInt);

				} else {

					// Mirror
					EditorRoomManager.instance.room.myTileInteractionList.Remove (oldTileInt);
				}
			}

			foreach (Tile oldTile in myRoom.MyGrid.gridArray) 
			{
				if (oldTile.myTileInteraction == oldTileInt) 
				{
					oldTile.myTileInteraction = null;
				}
			}
		}


		// create tileInteraction

		TileInteraction tileInteraction = new TileInteraction (tile.x, tile.y);


		// set default size

		tileInteraction.mySize = Vector2.one;


		// According to state, add to list

		if (EditorRoomManager.instance.room.RoomState == RoomState.Real) 
		{			
			// Real

			EditorRoomManager.instance.room.myTileInteractionList.Add (tileInteraction);
			tile.myTileInteraction = tileInteraction;

		} else {

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{			
				// Shadow

				EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Shadow.Add (tileInteraction);
				tile.myTileInteraction = tileInteraction;

			} else {

				// Mirror

				EditorRoomManager.instance.room.myTileInteractionList.Add (tileInteraction);
				tile.myTileInteraction = tileInteraction;
			}
		}

		EventsHandler.Invoke_cb_editorTileInteractioneModelChanged (tileInteraction);
		PlaceTileIntInTiles (tileInteraction, myRoom, myRoom.MyGrid);	
	}



	public void PlaceTileIntInTiles(TileInteraction tileInt, Room room, Grid grid)
	{
		List<Tile> tempTileList = room.GetMyTiles (grid, tileInt.mySize, tileInt.x, tileInt.y);

		foreach (Tile myTile in tempTileList) 
		{			
			myTile.myTileInteraction = tileInt;
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}




}
