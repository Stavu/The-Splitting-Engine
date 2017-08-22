using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public enum RoomState
{
	Real,
	Mirror
}


[Serializable]
public class Room {

	public string myName;
	public string myMusic;
	public string mapArea;

	public int myWidth;
	public int myHeight;

	public Grid myGrid;
	public Grid MyGrid
	{
		get
		{ 
			if (RoomState == RoomState.Mirror) 
			{
				if (myMirrorRoom.inTheShadow == true) 
				{
					return myMirrorRoom.shadowGrid;
				}

//				return myGrid;
			}

			return myGrid;
		}

		set 
		{
			if (RoomState == RoomState.Mirror) 
			{
				if (myMirrorRoom.inTheShadow == true) 
				{
					myMirrorRoom.shadowGrid = value;
					return;
				}

//				myGrid = value;
//				return;
			}

			myGrid = value;
		}
	}


	public string bgName;
	public bool bgFlipped;

	public List <Furniture> myFurnitureList;
	public List <Character> myCharacterList;
	public List <TileInteraction> myTileInteractionList;

	public Dictionary<Furniture,Vector2> furniturePositionMap;

	public RoomState roomState;
	public RoomState RoomState
	{
		get 
		{
			return roomState;
		}

		set 
		{
			roomState = value;

			if ((roomState == RoomState.Mirror) && (myMirrorRoom == null))
			{
				myMirrorRoom = new RoomMirror ();
			}
		}
	}


	public RoomMirror myMirrorRoom;


	// Shallow Room - for editor

	public Room(int myWidth, int myHeight)
	{
		//this.myName = myName;

		this.myWidth = myWidth;
		this.myHeight = myHeight;

		RoomState = RoomState.Real;

		//this.bgName = bgName;
	
		myGrid = new Grid (myWidth,myHeight);

		this.myFurnitureList = new List<Furniture> ();
		this.myCharacterList = new List<Character> ();
		this.myTileInteractionList = new List<TileInteraction> ();
	}


	// Real Room - for game 

	public Room(Room clone)
	{
		this.myName = clone.myName;
		this.myMusic = clone.myMusic;

		this.myWidth = clone.myWidth;
		this.myHeight = clone.myHeight;

		this.bgFlipped = clone.bgFlipped;

		this.bgName = clone.bgName;

		this.myFurnitureList = clone.myFurnitureList;
		this.myCharacterList = clone.myCharacterList;
		this.myTileInteractionList = clone.myTileInteractionList;

		this.myMirrorRoom = clone.myMirrorRoom;
		this.RoomState = clone.RoomState;
		this.mapArea = clone.mapArea;

		myGrid = new Grid (myWidth,myHeight);

		if (myMirrorRoom != null) 
		{
			myMirrorRoom.shadowGrid = new Grid (myWidth, myHeight);
			myMirrorRoom.myShadowMusic = clone.myMirrorRoom.myShadowMusic;
		}
	
		CreateRoomInteractables ();
	}


	// Create room interactables

	public void CreateRoomInteractables()
	{
		// Furniture

		foreach (Furniture furn in myFurnitureList) 
		{	

			List<Tile> FurnitureTiles;

			if (furn.CurrentGraphicState().coordsList.Count > 0)
			{		
				FurnitureTiles = GetMyTiles (myGrid, furn.currentGraphicState.coordsList);

			} else {
				
				FurnitureTiles = GetMyTiles(myGrid,furn.mySize, furn.x, furn.y);
			}

			FurnitureTiles.ForEach (tile => tile.PlaceFurnitureInTile (furn));
		}

		// Character

		foreach (Character character in myCharacterList) 
		{

			List<Tile> CharacterTiles;

			if (character.CurrentGraphicState().coordsList.Count > 0)
			{		
				CharacterTiles = GetMyTiles (myGrid, character.currentGraphicState.coordsList);

			} else {

				CharacterTiles = GetMyTiles(myGrid,character.mySize, character.x, character.y);
			}

			CharacterTiles.ForEach (tile => tile.PlaceCharacterInTile (character));
		}

		// Tile interactions

		foreach (TileInteraction tileInteraction in myTileInteractionList) 
		{
			List<Tile> TileInteractionTiles = GetMyTiles(myGrid,tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
			TileInteractionTiles.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));
		}



		// ------ MIRROR ------ // 

		if (RoomState == RoomState.Mirror) 
		{
			// -- SHADOW ROOM -- //

			// Furniture

			foreach (Furniture furn in myMirrorRoom.myFurnitureList_Shadow)
			{
				List<Tile> FurnitureTiles;

				if (furn.CurrentGraphicState ().coordsList.Count > 0)
				{		
					FurnitureTiles = GetMyTiles (myMirrorRoom.shadowGrid, furn.currentGraphicState.coordsList);

				}
				else
				{

					FurnitureTiles = GetMyTiles (myMirrorRoom.shadowGrid, furn.mySize, furn.x, furn.y);
				}
						
				FurnitureTiles.ForEach (tile => tile.PlaceFurnitureInTile (furn));
			}

				// Tile interactions

			foreach (TileInteraction tileInteraction in myMirrorRoom.myTileInteractionList_Shadow)
			{
				List<Tile> TileInteractionTiles = GetMyTiles (myMirrorRoom.shadowGrid, tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
				TileInteractionTiles.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));
			}

			// -- PERSISTENT -- //

			// Furniture

			foreach (Furniture furn in myMirrorRoom.myFurnitureList_Persistant) 
			{
				List<Tile> FurnitureTiles;

				if (furn.CurrentGraphicState().coordsList.Count > 0)
				{		
					FurnitureTiles = GetMyTiles (myGrid, furn.currentGraphicState.coordsList);

				} else {

					FurnitureTiles = GetMyTiles(myGrid, furn.mySize, furn.x, furn.y);
				}

				FurnitureTiles.ForEach (tile => tile.PlaceFurnitureInTile (furn));


				List<Tile> FurnitureTiles_Shadow;

				if (furn.CurrentGraphicState().coordsList.Count > 0)
				{		
					FurnitureTiles_Shadow = GetMyTiles (myMirrorRoom.shadowGrid, furn.currentGraphicState.coordsList);

				} else {

					FurnitureTiles_Shadow = GetMyTiles(myMirrorRoom.shadowGrid, furn.mySize, furn.x, furn.y);
				}

				FurnitureTiles_Shadow.ForEach (tile => tile.PlaceFurnitureInTile (furn));
			}


			// Tile interactions

			foreach (TileInteraction tileInteraction in myMirrorRoom.myTileInteractionList_Persistant)
			{
				List<Tile> TileInteractionTiles = GetMyTiles(myGrid, tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
				TileInteractionTiles.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));

				List<Tile> TileInteractionTiles_Shadow = GetMyTiles(myMirrorRoom.shadowGrid, tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
				TileInteractionTiles_Shadow.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));
			}
		}
	}




	// Placing interactable in tile list

	public void PlaceInteractableIntileList(List<Tile> tileList, Interactable interactable)
	{
		foreach (Tile tile in tileList) 
		{
			if (interactable is Furniture) 
			{
				tile.myFurnitureList.Add((Furniture)interactable);
			}

			if (interactable is Character) 
			{
				tile.myCharacter = (Character)interactable;
			}

			if (interactable is TileInteraction) 
			{
				tile.myTileInteraction = (TileInteraction)interactable;
			}			
		}
	}



	// Get all the tiles associated with this interactable object

	public List<Tile> GetMyTiles (Grid grid, Vector2 mySize, int x ,int y)
	{
		List<Tile> myTilesList = new List<Tile>();

		for (int i = 0; i < mySize.x; i++) {

			for (int j = 0; j < mySize.y; j++) {

				Tile tempTile = grid.GetTileAt (x + i, y + j);

				if (tempTile == null) 
				{
					Debug.LogError("Room: CreateFurniture temptile is null. something is wrong.");
					continue;
				}

				myTilesList.Add (tempTile);
			}
		}

		return myTilesList;
	}



	// Get tile list using a grid and a list of coords

	public List<Tile> GetMyTiles (Grid grid, List<Coords> coordsList)
	{
		List<Tile> myTilesList = new List<Tile>();

		foreach (Coords coords in coordsList) 
		{
			myTilesList.Add (grid.GetTileAt (coords.x, coords.y));
		}

		return myTilesList;
	}



	// When changing a graphic state - change physical interactable tiles

	public void ChangePIInTiles(PhysicalInteractable physicalInteractable, GraphicState newState, List<Grid> grids = null)
	{
		if (grids == null)
		{
			grids = new List<Grid> (){ MyGrid };
		}

		foreach (Grid grid in grids)
		{		
			List<Tile> oldTiles;

			if (physicalInteractable.CurrentGraphicState ().coordsList.Count > 0)
			{
				oldTiles = GetMyTiles (grid, physicalInteractable.CurrentGraphicState ().coordsList);	

			}
			else
			{

				oldTiles = GetMyTiles (grid, physicalInteractable.mySize, physicalInteractable.x, physicalInteractable.y);
			}

			List<Tile> newTiles;

			if (newState.coordsList.Count > 0)
			{
				newTiles = GetMyTiles (grid, newState.coordsList);

			}
			else
			{

				newTiles = GetMyTiles (grid, physicalInteractable.mySize, physicalInteractable.x, physicalInteractable.y);
			}

			if (physicalInteractable is Furniture)
			{
				foreach (Tile oldTile in oldTiles)
				{
					if (oldTile.myFurnitureList.Contains ((Furniture)(physicalInteractable)) == true) 
					{
						oldTile.myFurnitureList.Remove ((Furniture)physicalInteractable);
					
					} else {

						Debug.LogError ("not supposed to be here - furniture doesn't exist in tile futnirue list");
					}
				}

				foreach (Tile newTile in newTiles)
				{
					if (newTile.myFurnitureList.Contains ((Furniture)(physicalInteractable)) == false) 
					{
						newTile.myFurnitureList.Add ((Furniture)physicalInteractable);

					} else {

						Debug.LogError ("not supposed to be here - furniture already exists in tile futnirue list");
					}
				}
			}

			if (physicalInteractable is Character)
			{
				foreach (Tile oldTile in oldTiles)
				{
					oldTile.myCharacter = null;
				}

				foreach (Tile newTile in newTiles)
				{
					newTile.myCharacter = (Character)physicalInteractable;
				}
			}
		}

		//EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


}
