using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorRoomHelper {





	// Creating a new flipped room in the editor (helper)

	public static Room CreateFlippedRoom (Room room)
	{

		Room newRoom = EditorRoomManager.instance.CreateEmptyRoom (room.myWidth, room.myHeight);
		newRoom.bgName = room.bgName;
		newRoom.bgFlipped = !room.bgFlipped;
		newRoom.RoomState = room.RoomState;

		newRoom.myGrid = new Grid (newRoom.myWidth, newRoom.myHeight);



		// Interactables

		foreach (Furniture furn in room.myFurnitureList) 
		{			
			Furniture flippedFurn = new Furniture (room, furn);

			newRoom.myFurnitureList.Add (flippedFurn);

			Tile tile = newRoom.MyGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
			tile.myFurniture = flippedFurn;

		}

		newRoom.myFurnitureList.ForEach (furn => Debug.Log (furn.identificationName));



		foreach (TileInteraction tileInt in room.myTileInteractionList) 
		{		
			TileInteraction flippedTileInteraction = new TileInteraction (room, tileInt);

			newRoom.myTileInteractionList.Add (flippedTileInteraction);

			Tile tile = newRoom.MyGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
			tile.myTileInteraction = flippedTileInteraction;
		}


		// Also flip the interactables of the mirror room 

		if (room.RoomState == RoomState.Mirror) 
		{

			newRoom.myMirrorRoom = new RoomMirror ();


			// Persistant Lists

			foreach (Furniture furn in room.myMirrorRoom.myFurnitureList_Persistant) 
			{			
				Furniture flippedFurn = new Furniture (room, furn);

				newRoom.myMirrorRoom.myFurnitureList_Persistant.Add (flippedFurn);

				Tile tile = newRoom.MyGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
				tile.myFurniture = flippedFurn;

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
				shadowTile.myFurniture = flippedFurn;
			}

			foreach (TileInteraction tileInt in room.myMirrorRoom.myTileInteractionList_Persistant) 
			{			
				TileInteraction flippedTileInteraction = new TileInteraction (room, tileInt);

				newRoom.myMirrorRoom.myTileInteractionList_Persistant.Add (flippedTileInteraction);

				Tile tile = newRoom.MyGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
				tile.myTileInteraction = flippedTileInteraction;

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
				shadowTile.myTileInteraction = flippedTileInteraction;
			}



			// Shadow Lists


			foreach (Furniture furn in room.myMirrorRoom.myFurnitureList_Shadow) 
			{			
				Furniture flippedFurn = new Furniture (room, furn);

				newRoom.myMirrorRoom.myFurnitureList_Shadow.Add (flippedFurn);

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
				shadowTile.myFurniture = flippedFurn;
			}


			foreach (TileInteraction tileInt in room.myMirrorRoom.myTileInteractionList_Shadow) 
			{			
				TileInteraction flippedTileInteraction = new TileInteraction (room, tileInt);

				newRoom.myMirrorRoom.myTileInteractionList_Shadow.Add (flippedTileInteraction);

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
				shadowTile.myTileInteraction = flippedTileInteraction;
			}


		}



		return newRoom;


	}



	// -- PERSISTENCY -- //


	public static void SetFurniturePersistency(bool isPersistent, Furniture furn)
	{

		DestroyAllInteractablesInPos (furn.x, furn.y);

		if (isPersistent == true) 
		{				
			EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Persistant.Add (furn);

		} else {

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{				
				EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Shadow.Add (furn);

			} else {

				EditorRoomManager.instance.room.myFurnitureList.Add (furn);
			}
		}
	}



	public static void SetTileInteractionPersistency(bool isPersistent, TileInteraction tileInt)
	{

		DestroyAllInteractablesInPos (tileInt.x, tileInt.y);

		if (isPersistent == true) 
		{	
			EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Persistant.Add (tileInt);

		} else {

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{				
				EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Shadow.Add (tileInt);

			} else {

				EditorRoomManager.instance.room.myTileInteractionList.Add (tileInt);
			}
		}
	}




	public static void DestroyAllInteractablesInPos(int x ,int y)
	{
		
		RemoveFurnitureFromList(EditorRoomManager.instance.room.myFurnitureList,x,y);
		RemoveCharacterFromList(EditorRoomManager.instance.room.myCharacterList,x,y);
		RemoveTileInteractionFromList(EditorRoomManager.instance.room.myTileInteractionList,x,y);

		RemoveFurnitureFromList(EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Shadow,x,y);
		RemoveTileInteractionFromList(EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Shadow,x,y);

		RemoveFurnitureFromList(EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Persistant,x,y);
		RemoveTileInteractionFromList(EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Persistant,x,y);


	}



	public static void RemoveFurnitureFromList(List<Furniture> originList, int x, int y)
	{
		for (int i = originList.Count - 1; i >= 0; i--) 
		{
			if ((x == originList[i].x) && (y == originList[i].y)) 
			{
				originList.RemoveAt (i);
			}
		}
	}


	public static void RemoveCharacterFromList(List<Character> originList, int x, int y)
	{
		for (int i = originList.Count - 1; i >= 0; i--) 
		{
			if ((x == originList[i].x) && (y == originList[i].y)) 
			{
				originList.RemoveAt (i);
			}
		}
	}


	public static void RemoveTileInteractionFromList(List<TileInteraction> originList, int x, int y)
	{

		for (int i = originList.Count - 1; i >= 0; i--) 
		{
			if ((x == originList[i].x) && (y == originList[i].y)) 
			{
				originList.RemoveAt (i);
			}
		}

	}




}
