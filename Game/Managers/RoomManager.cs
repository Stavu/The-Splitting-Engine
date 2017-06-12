﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public class RoomManager : MonoBehaviour {


	// Singleton //

	public static RoomManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public Room myRoom;
	public Dictionary <string,ISpeaker> nameSpeakerMap;

	public GameObject bgObject;
	public GameObject bgObject_Shadow;

	public GameObject roomStarter;



	public void Initialize ()
	{	
		//EventsHandler.cb_furnitureChanged += OnFurnitureChanged;	
	}
	

	public void OnDestroy()
	{	
		//EventsHandler.cb_furnitureChanged -= OnFurnitureChanged;	
	}


	// Update is called once per frame

	void Update () 
	{

	}


	public void BuildRoom () 
	{
		CreateRoom ();
		nameSpeakerMap = new Dictionary<string, ISpeaker> ();


		if (myRoom.RoomState == RoomState.Real) 
		{
			// REAL ROOM

			myRoom.myFurnitureList.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
			myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

		} else {

			// SHADOW ROOM

			myRoom.myMirrorRoom.myFurnitureList_Shadow.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
			myRoom.myMirrorRoom.myTileInteractionList_Shadow.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			// MIRROR ROOM

			myRoom.myFurnitureList.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
			myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			// PERSISTENT INTERACTABLES

			myRoom.myMirrorRoom.myFurnitureList_Persistant.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
			myRoom.myMirrorRoom.myTileInteractionList_Persistant.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			SwitchObjectByShadowState (true);
		}


		// Characters

		myRoom.myCharacterList.ForEach (character => {
			EventsHandler.Invoke_cb_characterChanged (character);
			nameSpeakerMap.Add (character.identificationName, character);
		});


		// Players

		foreach (Player player in PlayerManager.playerList) 
		{
			if (player.currentRoom == myRoom.myName)
			{
				//Debug.Log ("Player in room " + player.identificationName);

				// active player should get the position according to the entrance pos

				Vector3 playerCurrentPos = GameManager.userData.GetPlayerDataByPlayerName (player.identificationName).currentPos;

				if (playerCurrentPos == Vector3.zero) 
				{
					playerCurrentPos = player.startingPos;
				}

				if (player.isActive == false) 
				{
					PlayerManager.instance.ParkPlayerInTiles (player, playerCurrentPos);
				}

				EventsHandler.Invoke_cb_inactivePlayerChanged (player);
				nameSpeakerMap.Add (player.identificationName, player);
			}
		}


		// adding the player to the speaker map

		//nameSpeakerMap.Add (PlayerManager.myPlayer.identificationName, PlayerManager.myPlayer);


		// sorting order by tags. Any object tagged with "in_the_back" will get -65 sorting order

		GameObject[] backObjects = GameObject.FindGameObjectsWithTag ("in_the_back");
		Debug.Log (backObjects.Length);


		foreach (GameObject anotherObj in backObjects) 
		{
			SpriteRenderer sr;
			sr = anotherObj.GetComponent<SpriteRenderer> ();
			sr.sortingOrder = sr.sortingOrder - 65;
		}

		EventsHandler.Invoke_cb_inputStateChanged ();
	}


	void CreateRoom()	
	{
		myRoom = new Room(GameManager.roomToLoad);
		EventsHandler.Invoke_cb_roomCreated (myRoom);
		Utilities.AdjustOrthographicCamera (myRoom);

		CreateRoomObject (myRoom);
	}


	// -- Player -- // 

	/*
	// Create Character 

	public void CreatePlayer(Room myRoom)	
	{		

		PlayerData playerData = GameManager.userData.GetPlayerDataByPlayerName (myPlayer.identificationName);

		if (playerData.currentPos != Vector3.zero)
		{
			myPlayer.myPos = playerData.currentPos;

		} else {

			myPlayer.myPos = myPlayer.startingPos;
		}

		CreatePlayerObject (myPlayer);
	}




	// Player object

	public void CreatePlayerObject(Player myPlayer)
	{
		//Debug.Log ("created character object");

		playerObject = (Instantiate (Resources.Load<GameObject>("Prefabs/Characters/" + myPlayer.fileName))).AddComponent<PlayerObject>();

		Debug.Log (myPlayer.fileName);

		playerObject.gameObject.name = myPlayer.fileName;
		playerObject.transform.position = myPlayer.myPos;

		//obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.furniture_character_layer;

		playerGameObjectMap.Add (myPlayer,playerObject.gameObject);
	}

	*/

	// --- ROOM OBJECT --- //

	public void CreateRoomObject(Room room)
	{
		bgObject = new GameObject (room.myName);

		bgObject.AddComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
		bgObject.transform.position = new Vector3 (room.myWidth/2f, 0, 0);

		bgObject.GetComponent<SpriteRenderer> ().sortingOrder = -10;
		bgObject.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;
		bgObject.transform.SetParent (this.transform);

		if (myRoom.myMirrorRoom != null) 
		{		
			bgObject_Shadow = new GameObject (room.myName + "_shadow");

			bgObject_Shadow.AddComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.myMirrorRoom.bgName_Shadow);
			bgObject_Shadow.transform.position = new Vector3 (room.myWidth/2f, 0, 0);

			bgObject_Shadow.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;
			bgObject_Shadow.transform.SetParent (this.transform);
		}
	}


	// -- SWITCH BETWEEN SHADOW AND MIRROR -- //

	public void SwitchObjectByShadowState(bool immediately)
	{
		List<SpriteRenderer> fadeInSprites = new List<SpriteRenderer> ();
		List<SpriteRenderer> fadeOutSprites = new List<SpriteRenderer> ();

		if (myRoom.myMirrorRoom.inTheShadow == true) 
		{
			//fadeOutSprites.Add (bgObject.GetComponent<SpriteRenderer>());
			fadeInSprites.Add (bgObject_Shadow.GetComponent<SpriteRenderer>());

			foreach (Furniture furn in myRoom.myFurnitureList) 
			{
				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeOutSprites.AddRange (srs);
			}

			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
			{
				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeInSprites.AddRange (srs);
			}

		} else {

			fadeOutSprites.Add (bgObject_Shadow.GetComponent<SpriteRenderer>());
			//fadeInSprites.Add (bgObject.GetComponent<SpriteRenderer>());

			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
			{
				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeOutSprites.AddRange (srs);
			}

			foreach (Furniture furn in myRoom.myFurnitureList) 
			{
				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeInSprites.AddRange (srs);
			}
		}


		// if switching immediatly or not

		if (immediately == true) 
		{			
			Utilities.SwitchBetweenSprites (fadeOutSprites, fadeInSprites);
		
		} else {

			StartCoroutine (Utilities.FadeBetweenSprites (fadeOutSprites, fadeInSprites));
		}


	}


}
