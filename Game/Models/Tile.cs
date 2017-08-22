using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile {


	public int x {get; protected set;}
	public int y {get; protected set;}

	public List<Furniture> myFurnitureList; // TODO - furniture list instead of furniture 
	public Character myCharacter;
	public Player myInactivePlayer;
	public TileInteraction myTileInteraction;

	public bool walkable;


	public Tile(int x, int y)
	{
		this.x = x;
		this.y = y;

		myFurnitureList = new List<Furniture> ();
	}


	public void PlaceFurnitureInTile(Furniture furniture)
	{		
		if (furniture == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject myObject is null");
		
			return;
		}

		if (myFurnitureList.Contains (furniture) == true) 
		{
			Debug.LogError("Tile: PlaceRoomObject myRoomObject exists:" + furniture.identificationName);

			return;
		}


		// if everything's okay, add furniture to list

		myFurnitureList.Add (furniture);

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

		if (myFurnitureList != null) 
		{	
			foreach (Furniture furn in myFurnitureList) 
			{
				if (furn.walkable == false) 
				{
					return false;
				}	
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
