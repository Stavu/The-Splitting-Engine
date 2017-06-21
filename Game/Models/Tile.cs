using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Tile {



	public int x {get; protected set;}
	public int y {get; protected set;}

	public Furniture myFurniture; 
	public Character myCharacter;
	public Player myInactivePlayer;
	public TileInteraction myTileInteraction;

	public bool walkable;



	public Tile(int x, int y)
	{
		this.x = x;
		this.y = y;
	}


	public void PlaceFurnitureInTile(Furniture furniture)
	{		
		if (furniture == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject myObject is null");
		
			return;
		}

		if(myFurniture != null)
		{
			if (myFurniture != furniture) 
			{
				Debug.LogError("Tile: PlaceRoomObject myRoomObject exists");

				return;
			}

		}

		// if everything's okay, set myFurniture

		myFurniture = furniture;
	}


	public void PlaceCharacterInTile(Character character)
	{
		if (character == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject myObject is null");

			return;
		}

		if(myCharacter != null)
		{
			if (myCharacter != character) 
			{
				Debug.LogError("Tile: PlaceRoomObject myRoomObject exists");

				return;
			}
		}

		// if everything's okay, set myCharacter

		myCharacter = character;
	}



	public void PlaceInactivePlayerInTile(Player player)
	{
		if (player == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject myObject is null");

			return;
		}

		if(myInactivePlayer != null)
		{
			if (myInactivePlayer != player) 
			{
				Debug.LogError("Tile: PlaceRoomObject myRoomObject exists");

				return;
			}
		}

		// if everything's okay, setmyInactivePlayer

		myInactivePlayer = player;
	}


	public void PlaceTileInteraction(TileInteraction tileInteraction)
	{
		if (tileInteraction == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject tileInteraction is null");

			return;
		}

		if(myTileInteraction != null)
		{
			if (myTileInteraction != tileInteraction) 
			{
				Debug.LogError("Tile: PlaceRoomObject tileInteraction exists");

				return;
			}
		}

		// if everything's okay, set myFurniture

		myTileInteraction = tileInteraction;
	}


	public bool IsWalkable()
	{

		if (myCharacter != null) 
		{		
			return false;
		}

		if (myFurniture != null) 
		{		
			if (myFurniture.walkable == false) 
			{
				return false;
			}		
		}

		if (myTileInteraction != null) 
		{	
			if (myTileInteraction.walkable == false) 
			{
				return false;
			}
		}

		return true;
	}

}
