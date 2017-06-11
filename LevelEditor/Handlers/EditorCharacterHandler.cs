using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCharacterHandler : MonoBehaviour {


	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_editorNewRoomCreated += CharacterFactory;
		EventsHandler.cb_editorCharacterModelChanged += CreateCharacterObject;
	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorNewRoomCreated -= CharacterFactory;
		EventsHandler.cb_editorCharacterModelChanged -= CreateCharacterObject;
	}



	// Update is called once per frame

	void Update () 
	{

	}



	// Placing Furniture in the editor 

	public void PlaceCharacter(Tile tile, string characterName)
	{


		if(characterName == null)
		{
			return;
		}

		Room myRoom = EditorRoomManager.instance.room;

		// If there's already a character on this tile, destroy it before creating a new character

		if (tile.myCharacter != null)
		{
			Character oldChar = tile.myCharacter;

			myRoom.myCharacterList.Remove (tile.myCharacter);

			Destroy(EditorRoomManager.instance.characterGameObjectMap [oldChar]);
			EditorRoomManager.instance.characterGameObjectMap.Remove (oldChar);

			foreach (Tile oldTile in myRoom.MyGrid.gridArray) 
			{
				if (oldTile.myCharacter == oldChar) 
				{
					oldTile.myCharacter = null;
				}
			}
		}

		// create furniture

		Character character = new Character (characterName, tile.x, tile.y);


		// set size

		character.mySize = Vector2.one;

		myRoom.myCharacterList.Add (character);
		tile.myCharacter = character;

		EventsHandler.Invoke_cb_editorCharacterModelChanged (character);
		PlaceCharacterInTiles (character, myRoom, myRoom.MyGrid);	

	}



	public void CharacterFactory(Room room)
	{

		//Debug.Log ("FurnitureFactory");

		foreach (Character character in room.myCharacterList) 
		{	
			EventsHandler.Invoke_cb_editorCharacterModelChanged (character);
			PlaceCharacterInTiles (character, room, room.MyGrid);	
		}

	}



	public void CreateCharacterObject(Character myCharacter)
	{	

		GameObject obj = Utilities.CreateCharacterGameObject (myCharacter, this.transform);

		if (myCharacter == null) 
		{
			Debug.Log ("character = null");
		}

		if (EditorRoomManager.instance.characterGameObjectMap == null) 
		{
			EditorRoomManager.instance.characterGameObjectMap = new Dictionary<Character, GameObject> ();
		}

		EditorRoomManager.instance.characterGameObjectMap.Add (myCharacter, obj);	


		// populate list of graphic states

		if (myCharacter.graphicStates.Count == 0) 
		{
			myCharacter.graphicStates = Utilities.GetGraphicStateList (myCharacter);
		}

		myCharacter.currentGraphicState = myCharacter.graphicStates [0];

	}




	public void PlaceCharacterInTiles(Character character, Room room, Grid grid)
	{
		List<Tile> tempTileList = room.GetMyTiles (grid, character.GetMyCoordsList ());
		Debug.Log ("tile list count" + tempTileList.Count);

		foreach (Tile myTile in tempTileList) 
		{			
			Debug.Log ("tile x " + myTile.x + "y " + myTile.y);
			myTile.myCharacter = character;
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}




}
