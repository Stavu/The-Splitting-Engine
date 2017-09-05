using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEngine.SceneManagement;



public class EditorRoomManager : MonoBehaviour {


	// Singleton //

	public static EditorRoomManager instance = null;

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	// Handlers

	public static EditorFurnitureHandler editorFurnitureHandler;
	public static EditorCharacterHandler editorCharacterHandler;
	public static EditorTileInteractionHandler editorTileInteractionHandler;


	public Room room; 
	public GameObject roomBackground;

	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;
	public Dictionary<Character,GameObject> characterGameObjectMap;

	public static Dictionary<string,GameObject> stringPrefabMap;

	public static string roomToLoad;
	public static bool loadRoomFromMemory = false;



	public void Initialize ()
	{

		if (editorFurnitureHandler == null) 
		{
			editorFurnitureHandler = gameObject.AddComponent<EditorFurnitureHandler> ();
			editorFurnitureHandler.Initialize ();
		}

		if (editorCharacterHandler == null) 
		{
			editorCharacterHandler = gameObject.AddComponent<EditorCharacterHandler> ();
			editorCharacterHandler.Initialize ();
		}

		if (editorTileInteractionHandler == null) 
		{
			editorTileInteractionHandler = gameObject.AddComponent<EditorTileInteractionHandler> ();
			editorTileInteractionHandler.Initialize ();
		}

		// loading the prefabs

		if (stringPrefabMap == null) 
		{
			LoadPrefabs ();
		}
	}



	// Load Prefabs

	public void LoadPrefabs()
	{
		stringPrefabMap = new Dictionary<string, GameObject> ();

		GameObject[] furnitureArray = Resources.LoadAll<GameObject> ("Prefabs/Furniture");
		GameObject[] characterArray = Resources.LoadAll<GameObject> ("Prefabs/Characters");

		foreach (GameObject obj in furnitureArray) 
		{
			stringPrefabMap.Add (obj.name, obj);
		}

		foreach (GameObject obj in characterArray) 
		{
			stringPrefabMap.Add (obj.name, obj);
		}
	}



	// Create Empty Room

	public Room CreateEmptyRoom(int myWidth, int myHeight)
	{
		Room tempRoom = new Room (myWidth,myHeight);

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		characterGameObjectMap = new Dictionary<Character, GameObject> ();

		return tempRoom;
	}


	// adding background image 

	public void InitializeRoom(string name = "abandoned_lobby_bg")
	{
		if (roomToLoad == null) 
		{
			CreateNewRoomFromSprite (name);
		}

		room = LoadRoom (roomToLoad);
			
		EventsHandler.Invoke_cb_editorNewRoomCreated (room);

		// Creating room object

		SetBackgroundObject (room);

		Utilities.AdjustOrthographicCamera_Editor (room);
	}



	// Create New Room

	public void CreateNewRoomFromSprite(string name)
	{		
		Sprite roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + name);

		int myWidth = (int)roomSprite.bounds.size.x;
		int myHeight = (int)roomSprite.bounds.size.y;

		Room room = CreateEmptyRoom (myWidth, myHeight);
		room.bgName = name;	

		roomToLoad = JsonUtility.ToJson (room);

		SceneManager.LoadScene ("LevelEditor");
	}



	// PI tiles 

	public void SetPICoords(Tile tile)
	{
		Coords coords = new Coords (tile.x, tile.y);

		PhysicalInteractable chosenPhysicalInteractable = InspectorManager.instance.GetChosenPI ();

		for (int i = 0; i < chosenPhysicalInteractable.currentGraphicState.coordsList.Count; i++)
		{			
			if ((chosenPhysicalInteractable.currentGraphicState.coordsList[i].x == coords.x) && (chosenPhysicalInteractable.currentGraphicState.coordsList[i].y == coords.y)) 
			{
				chosenPhysicalInteractable.currentGraphicState.coordsList.RemoveAt (i);
				tile.myFurniture  = null;
				tile.myCharacter  = null;
				EventsHandler.Invoke_cb_tileLayoutChanged ();

				return;
			}
		}

		// Get furniture out of old tiles if there wasn't a coords list before

		if (chosenPhysicalInteractable.currentGraphicState.coordsList.Count == 0) 
		{
			List<Tile> oldTileList = EditorRoomManager.instance.room.GetMyTiles (EditorRoomManager.instance.room.MyGrid, chosenPhysicalInteractable.mySize, chosenPhysicalInteractable.x, chosenPhysicalInteractable.y);

			foreach (Tile oldTile in oldTileList) 
			{
				if (chosenPhysicalInteractable is Furniture) 
				{
					oldTile.myFurniture = null;
				}

				if (chosenPhysicalInteractable is Character) 
				{
					oldTile.myCharacter = null;
				}
			}		
		}

		chosenPhysicalInteractable.currentGraphicState.coordsList.Add (coords);

		if (chosenPhysicalInteractable is Furniture) 
		{
			tile.myFurniture = (Furniture)chosenPhysicalInteractable;
		}

		if (chosenPhysicalInteractable is Character) 
		{
			tile.myCharacter = (Character)chosenPhysicalInteractable;
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}




	// Room Music 

	public void ChangeRoomMusic(string name = "maze_music")
	{
		if (room.RoomState == RoomState.Real) 
		{
			room.myMusic = name;

		} else {

			if (room.myMirrorRoom.inTheShadow == true) 
			{
				room.myMirrorRoom.myShadowMusic = name;

			} else {

				room.myMusic = name;
			}
		}
	}


	public void ChangeMapArea (string mapArea = "None")
	{
		room.mapArea = mapArea;
	}


	// Room background 

	public void ChangeRoomBackground(string name = "abandoned_lobby_bg")
	{
		if (room.RoomState == RoomState.Real) 
		{
			room.bgName = name;
		
		} else {

			if (room.myMirrorRoom.inTheShadow == true) 
			{
				room.myMirrorRoom.bgName_Shadow = name;

			} else {
				
				room.bgName = name;
			}
		}

		SetBackgroundObject (room);
	}



	public void SetBackgroundObject(Room room)
	{

		if (roomBackground != null) 
		{
			Destroy (roomBackground);			
		}	
	
		SpriteRenderer sr;	
		Sprite roomSprite;

		if (room.RoomState == RoomState.Real) 
		{
			roomBackground = new GameObject (room.myName);
			sr = roomBackground.AddComponent<SpriteRenderer> ();
			roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
			sr.flipX = room.bgFlipped;
		
		} else {

			if (room.myMirrorRoom.inTheShadow == true) 
			{
				roomBackground = new GameObject (room.myName + "_shadow");
				sr = roomBackground.AddComponent<SpriteRenderer> ();
				roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.myMirrorRoom.bgName_Shadow);
				sr.flipX = room.myMirrorRoom.bgFlipped_Shadow;

			} else {

				roomBackground = new GameObject (room.myName);
				sr = roomBackground.AddComponent<SpriteRenderer> ();
				roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
				sr.flipX = room.bgFlipped;
			}
		}

		sr.sprite = roomSprite;
		roomBackground.transform.position = new Vector3 (room.myWidth/2f, 0, 0);
		sr.sortingLayerName = Constants.room_layer;
		roomBackground.transform.SetParent (this.transform);

		Utilities.AdjustOrthographicCamera_Editor (room);

	}


	// Checking if tiles are clean

	public bool CheckNewTiles(List<Tile> tileList, Interactable interactable)
	{
		foreach (Tile tile in tileList) 
		{
			if (interactable is Furniture) 
			{
				if (tile.myFurniture != null) 
				{
					if (tile.myFurniture != interactable) 
					{
						return false;
					}
				}
			}

			if (interactable is Character) 
			{
				if (tile.myCharacter != null) 
				{
					if (tile.myCharacter != interactable) 
					{
						return false;
					}
				}
			}

			if (interactable is TileInteraction) 
			{
				if (tile.myTileInteraction != null) 
				{
					if (tile.myTileInteraction != interactable) 
					{
						return false;
					}
				}
			}
		}

		return true;
	}




	// CHANGING INTERACTABLE //

	public void ChangeInteractableWidth(int width, Interactable interactable)
	{	
		if (interactable is Furniture) 
		{			
			Furniture furn = (Furniture)interactable;

			if (furn.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The furniture has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			Vector2 targetSize = new Vector2 (width, furn.mySize.y);
			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, targetSize, furn.x, furn.y);

			if (CheckNewTiles (tileList, furn) == false) 
			{
				EditorUI.DisplayAlert_B ("There is furniture in the way."); 
				return;
			}

			furn.mySize = targetSize;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myFurniture == furn) 
				{
					tile.myFurniture = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, furn);

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);
		}

		if (interactable is Character) 
		{
			Character character = (Character)interactable;

			if (character.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The character has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			Vector2 targetSize = new Vector2 (width, character.mySize.y);
			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, targetSize, character.x, character.y);

			if (CheckNewTiles (tileList, character) == false) 
			{
				EditorUI.DisplayAlert_B ("There are characters in the way."); 
				return;
			}

			character.mySize = targetSize;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myCharacter == character) 
				{
					tile.myCharacter = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, character);

			characterGameObjectMap[character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);
		}


		if (interactable is TileInteraction) 
		{			
			TileInteraction tileInt = (TileInteraction)interactable;

			Room myRoom = EditorRoomManager.instance.room;

			Vector2 targetSize = new Vector2 (width, tileInt.mySize.y);
			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, targetSize, tileInt.x, tileInt.y);

			if (CheckNewTiles (tileList, tileInt) == false) 
			{
				EditorUI.DisplayAlert_B ("There is a tile interaction in the way."); 
				return;
			}

			tileInt.mySize = targetSize;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myTileInteraction == tileInt) 
				{
					tile.myTileInteraction = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, tileInt);

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableHeight(int height, Interactable interactable)
	{		
		if (interactable is Furniture) 
		{			
			Furniture furn = (Furniture)interactable;

			if (furn.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The furniture has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			Vector2 targetSize = new Vector2 (furn.mySize.x, height);
			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, targetSize, furn.x, furn.y);

			if (CheckNewTiles (tileList, furn) == false) 
			{
				EditorUI.DisplayAlert_B ("There is furniture in the way."); 
				return;
			}

			furn.mySize = targetSize;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myFurniture == furn) 
				{
					tile.myFurniture = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, furn);

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);
		}


		if (interactable is Character) 
		{
			Character character = (Character)interactable;

			if (character.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The character has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			Vector2 targetSize = new Vector2 (character.mySize.x, height);
			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, targetSize, character.x, character.y);

			if (CheckNewTiles (tileList, character) == false) 
			{
				EditorUI.DisplayAlert_B ("There are characters in the way."); 
				return;
			}

			character.mySize = targetSize;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myCharacter == character) 
				{
					tile.myCharacter = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, character);

			characterGameObjectMap[character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);
		}


		if (interactable is TileInteraction) 
		{			
			TileInteraction tileInt = (TileInteraction)interactable;

			Room myRoom = EditorRoomManager.instance.room;

			Vector2 targetSize = new Vector2 (tileInt.mySize.x, height);
			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, targetSize, tileInt.x, tileInt.y);

			if (CheckNewTiles (tileList, tileInt) == false) 
			{
				EditorUI.DisplayAlert_B ("There is a tile interaction in the way."); 
				return;
			}

			tileInt.mySize = targetSize;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myTileInteraction == tileInt) 
				{
					tile.myTileInteraction = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, tileInt);
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableTileX(int x, Interactable interactable)
	{
		if (interactable is Furniture) 
		{			
			Furniture furn = (Furniture)interactable;

			if (furn.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The furniture has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, furn.mySize, x, furn.y);

			if (CheckNewTiles (tileList, furn) == false) 
			{
				EditorUI.DisplayAlert_B ("There is furniture in the way."); 
				return;
			}
		
			furn.x = x;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myFurniture == furn) 
				{
					tile.myFurniture = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, furn);

			// Game object

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);
					
		} 

		if (interactable is Character) 
		{	
			Character character = (Character)interactable;

			if (character.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The character has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, character.mySize, x, character.y);

			if (CheckNewTiles (tileList, character) == false) 
			{
				EditorUI.DisplayAlert_B ("There is furniture in the way."); 
				return;
			}

			character.x = x;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myCharacter == character) 
				{
					tile.myCharacter = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, character);

			// Game object

			characterGameObjectMap [character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);

		}

		if (interactable is TileInteraction)		
		{				
			TileInteraction tileInt = (TileInteraction)interactable;

			Room myRoom = EditorRoomManager.instance.room;

			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, tileInt.mySize, x, tileInt.y);

			if (CheckNewTiles (tileList, tileInt) == false) 
			{
				EditorUI.DisplayAlert_B ("There is a tile interaction in the way."); 
				return;
			}

			tileInt.x = x;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myTileInteraction == tileInt) 
				{
					tile.myTileInteraction = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, tileInt);

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableTileY(int y, Interactable interactable)
	{
		if (interactable is Furniture) 
		{			
			Furniture furn = (Furniture)interactable;

			if (furn.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The furniture has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, furn.mySize, furn.x, y);

			if (CheckNewTiles (tileList, furn) == false) 
			{
				EditorUI.DisplayAlert_B ("There is furniture in the way."); 
				return;
			}

			furn.y = y;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myFurniture == furn) 
				{
					tile.myFurniture = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, furn);

			// Game object

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		} 

		if (interactable is Character) 
		{	
			Character character = (Character)interactable;

			if (character.currentGraphicState.coordsList.Count > 0) 
			{
				EditorUI.DisplayAlert_B ("The character has a coords list."); 
				return;
			}

			Room myRoom = EditorRoomManager.instance.room;

			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, character.mySize, character.x, y);

			if (CheckNewTiles (tileList, character) == false) 
			{
				EditorUI.DisplayAlert_B ("There is furniture in the way."); 
				return;
			}

			character.y = y;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myCharacter == character) 
				{
					tile.myCharacter = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, character);

			// Game object

			characterGameObjectMap [character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);

		}

		if (interactable is TileInteraction)		
		{				
			TileInteraction tileInt = (TileInteraction)interactable;

			Room myRoom = EditorRoomManager.instance.room;

			List<Tile> tileList = myRoom.GetMyTiles (myRoom.MyGrid, tileInt.mySize, tileInt.x, y);

			if (CheckNewTiles (tileList, tileInt) == false) 
			{
				EditorUI.DisplayAlert_B ("There is a tile interaction in the way."); 
				return;
			}

			tileInt.y = y;

			// clean tiles

			foreach (Tile tile in myRoom.MyGrid.gridArray) 
			{
				if (tile.myTileInteraction == tileInt) 
				{
					tile.myTileInteraction = null;
				}
			}

			// place interactable in new tiles

			myRoom.PlaceInteractableIntileList (tileList, tileInt);

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	// OFFSETS //

	public void ChangeInteractableOffsetX(float offsetX, Interactable interactable)
	{		
		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furn.offsetX = offsetX;

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		} else if (interactable is Character) 
		{
			Character character = (Character)interactable;
			character.offsetX = offsetX;

			characterGameObjectMap [character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);
		} 

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableOffsetY(float offsetY, Interactable interactable)
	{
		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furn.offsetY = offsetY;
		
			furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		} else if (interactable is Character) 
		{
			Character character = (Character)interactable;
			character.offsetY = offsetY;

			characterGameObjectMap[character].transform.position = new Vector3 (character.x + character.mySize.x/2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);
		} 

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableLayerOffset(int layerOffset, Interactable interactable)
	{
		Debug.Log ("ChangeInteractableLayerOffset");

		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furn.layerOffset = layerOffset;

			SpriteRenderer[] srArray = furnitureGameObjectMap [furn].GetComponentsInChildren<SpriteRenderer> ();

			foreach (SpriteRenderer sr in srArray) 
			{				
				sr.sortingOrder = -(furn.y * 10) + layerOffset; // FIXME
			}


		} else if (interactable is Character) 
		{
			Character character = (Character)interactable;
			character.layerOffset = layerOffset;

			SpriteRenderer[] srArray = characterGameObjectMap [character].GetComponentsInChildren<SpriteRenderer> ();

			foreach (SpriteRenderer sr in srArray) 
			{
				sr.sortingOrder = -(character.y * 10) + layerOffset; // FIXME 
			}
		
		} 

		EventsHandler.Invoke_cb_tileLayoutChanged ();


	}






	// ------- GRAPHIC STATE ------- //


	public void ChangeInteractableCurrentGraphicState(string state, PhysicalInteractable interactable)
	{
		foreach (GraphicState graphicState in interactable.graphicStates) 
		{
			if (graphicState.graphicStateName == state) 
			{
				// clean previous graphic state

				EditorRoomManager.instance.room.ChangePIInTiles (interactable, graphicState);
				interactable.currentGraphicState = graphicState;

				// go to animation state

				GameObject obj = GetPhysicalInteractableGameObject (interactable);
				Animator animator = obj.GetComponent<Animator> ();
				animator.PlayInFixedTime (state);
				Debug.Log ("ChangeInteractableCurrentGraphicState --- " + state);
				EventsHandler.Invoke_cb_tileLayoutChanged ();
				return;
			}
		}

		Debug.LogError ("I shouldn't be here. " + state + " " + interactable.identificationName);
	}




	// ------ FRAME SIZE ------ //


	public void ChangeInteractableFrameWidth(float width, PhysicalInteractable interactable)
	{
		width = Mathf.Abs (width);
		interactable.currentGraphicState.frameExtents.x = width;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	public void ChangeInteractableFrameHeight(float height, PhysicalInteractable interactable)
	{
		height = Mathf.Abs (height);
		interactable.currentGraphicState.frameExtents.y = height;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}

	// FRAME OFFSETS //

	public void ChangeInteractableFrameOffsetX(float offsetX, PhysicalInteractable interactable)
	{
		interactable.currentGraphicState.frameOffsetX = offsetX;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	public void ChangeInteractableFrameOffsetY(float offsetY, PhysicalInteractable interactable)
	{
		interactable.currentGraphicState.frameOffsetY = offsetY;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	// ---- LOAD ROOM ---- //

	public Room LoadRoom(string roomString)
	{
		Room tempRoom = JsonUtility.FromJson<Room> (roomString);
		tempRoom.myGrid = new Grid (tempRoom.myWidth, tempRoom.myHeight);


		// MIRROR ROOM

		if (tempRoom.RoomState == RoomState.Mirror)
		{
			tempRoom.myMirrorRoom.shadowGrid = new Grid (tempRoom.myWidth, tempRoom.myHeight);

			if (tempRoom.myMirrorRoom.inTheShadow == true) 
			{
				// SHADOW ROOM

				foreach (Furniture furn in tempRoom.myMirrorRoom.myFurnitureList_Shadow) 
				{
					if (furn.graphicStates != null) 
					{
						furn.currentGraphicState = furn.graphicStates [0];
					
					} else {

						Debug.LogError ("graphicStates is null");
					}
				}

				foreach (TileInteraction tileInt in tempRoom.myMirrorRoom.myTileInteractionList_Shadow) 
				{
					Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (tileInt.x, tileInt.y);
					tileShadow.myTileInteraction = tileInt;

				}

			} else {

				// MIRROR ROOM

				foreach (Furniture furn in tempRoom.myFurnitureList) 
				{
					if (furn.graphicStates != null) 
					{
						furn.currentGraphicState = furn.graphicStates [0];

					} else {

						Debug.LogError ("graphicStates is null");
					}
				}

				foreach (TileInteraction tileInt in tempRoom.myTileInteractionList) 
				{
					Tile tile = tempRoom.MyGrid.GetTileAt (tileInt.x, tileInt.y);

					// place interactable in new tiles

					List<Tile> tileList = tempRoom.GetMyTiles (tempRoom.MyGrid, tileInt.mySize, tileInt.x, tileInt.y);
					tempRoom.PlaceInteractableIntileList (tileList, tileInt);

				}
			}


			// Persistant Interactables 

			foreach (Furniture furn in tempRoom.myMirrorRoom.myFurnitureList_Persistant) 
			{
				if (furn.graphicStates != null) 
				{
					furn.currentGraphicState = furn.graphicStates [0];

				} else {

					Debug.LogError ("graphicStates is null");
				}
			}

			foreach (Character character in tempRoom.myCharacterList) 
			{				
				if (character.graphicStates != null) 
				{
					character.currentGraphicState = character.graphicStates [0];

				} else {

					Debug.LogError ("graphicStates is null");
				}
			}


			foreach (TileInteraction tileInt in tempRoom.myMirrorRoom.myTileInteractionList_Persistant) 
			{				
				Tile tile = tempRoom.MyGrid.GetTileAt (tileInt.x, tileInt.y);

				List<Tile> tileList = tempRoom.GetMyTiles (tempRoom.MyGrid, tileInt.mySize, tileInt.x, tileInt.y);
				tempRoom.PlaceInteractableIntileList (tileList, tileInt);

				Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (tileInt.x, tileInt.y);
			
				List<Tile> tileList_shadow = tempRoom.GetMyTiles (tempRoom.myMirrorRoom.shadowGrid, tileInt.mySize, tileInt.x, tileInt.y);
				tempRoom.PlaceInteractableIntileList (tileList_shadow, tileInt);
			}



		} else {
			
			
			// REAL ROOM 

			foreach (Furniture furn in tempRoom.myFurnitureList) 
			{				
				if (furn.graphicStates != null) 
				{
					furn.currentGraphicState = furn.graphicStates [0];

				} else {

				Debug.LogError ("graphicStates is null");
				}
			}

			foreach (Character character in tempRoom.myCharacterList) 
			{
				if (character.graphicStates != null) 
				{
					character.currentGraphicState = character.graphicStates [0];

				} else {

					Debug.LogError ("graphicStates is null");
				}
			}

			foreach (TileInteraction tileInt in tempRoom.myTileInteractionList) 
			{
				Tile tile = tempRoom.MyGrid.GetTileAt (tileInt.x, tileInt.y);

				List<Tile> tileList = tempRoom.GetMyTiles (tempRoom.MyGrid, tileInt.mySize, tileInt.x, tileInt.y);
				tempRoom.PlaceInteractableIntileList (tileList, tileInt);

			}
		}

		return tempRoom;
	}



	public GameObject GetPhysicalInteractableGameObject(PhysicalInteractable physicalInteractable)
	{
		GameObject obj = null;

		if (physicalInteractable is Furniture) 
		{
			Furniture furn = (Furniture)physicalInteractable;
			obj = furnitureGameObjectMap [furn];
		}

		if (physicalInteractable is Character) 
		{
			Character character = (Character)physicalInteractable;
			obj = characterGameObjectMap [character];
		}

		return obj;
	}




	// SERIALIZE ROOM //


	public string SerializeRoom ()
	{
		string roomString = JsonUtility.ToJson (room);

		string path = ("Assets/Resources/Jsons/Rooms/" + room.myName + ".json");

		using (FileStream fs = new FileStream(path, FileMode.Create))
		{
			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.Write(roomString);
			}
		}

		try
		{
			UnityEditor.AssetDatabase.Refresh ();
		}
		catch (Exception ex)
		{
			
		}


		return roomString;
	}

}
