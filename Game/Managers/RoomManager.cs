using System.Collections;
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


	public void BuildRoom () 
	{
		CreateRoom ();
		nameSpeakerMap = new Dictionary<string, ISpeaker> ();


		if (myRoom.RoomState == RoomState.Real) 
		{
			// REAL ROOM

			myRoom.myFurnitureList.ForEach (furn => CreateFurniture (furn));
			myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

		} else {

			// SHADOW ROOM

			myRoom.myMirrorRoom.myFurnitureList_Shadow.ForEach (furn => CreateFurniture (furn));
			myRoom.myMirrorRoom.myTileInteractionList_Shadow.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			// MIRROR ROOM

			myRoom.myFurnitureList.ForEach (furn => CreateFurniture (furn));
			myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			// PERSISTENT INTERACTABLES

			myRoom.myMirrorRoom.myFurnitureList_Persistant.ForEach (furn => CreateFurniture (furn));
			myRoom.myMirrorRoom.myTileInteractionList_Persistant.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			SwitchObjectByShadowState (true);
		}


		// Characters

		myRoom.myCharacterList.ForEach (character => {
			CreateCharacter (character);
			nameSpeakerMap.Add (character.identificationName, character);
		});


		// Players

		foreach (Player player in PlayerManager.playerList) 
		{
			if (player.currentRoom == myRoom.myName)
			{
				Vector3 playerCurrentPos = GameManager.userData.GetPlayerDataByPlayerName (player.identificationName).currentPos;
							
				if (playerCurrentPos == Vector3.zero) 
				{					
					playerCurrentPos = player.startingPos;
				}

				if (player.isActive == false) {
					PlayerManager.instance.ParkPlayerInTiles (player, playerCurrentPos);
				}

				EventsHandler.Invoke_cb_playerChanged (player);
				nameSpeakerMap.Add (player.identificationName, player);
			}
		}

		// sorting order by tags. Any object tagged with "in_the_back" will get -65 sorting order

		GameObject[] backObjects = GameObject.FindGameObjectsWithTag ("in_the_back");

		// FIXME: add sorting order calculation to designated function, located in Utilities
		foreach (GameObject anotherObj in backObjects) 
		{
			SpriteRenderer sr;
			sr = anotherObj.GetComponent<SpriteRenderer> ();
			sr.sortingOrder = sr.sortingOrder - 65;
		}

		// ----------------

		EventsHandler.Invoke_cb_inputStateChanged ();
	}


	void CreateRoom()	
	{
		myRoom = new Room(GameManager.roomToLoad);
		EventsHandler.Invoke_cb_roomCreated (myRoom);
		Utilities.AdjustOrthographicCamera (myRoom);

		CreateRoomObject (myRoom);
		CreateBlackEdgeFrame (myRoom.myWidth, myRoom.myHeight);
	}

	void CreateBlackEdgeFrame (float width, float height) 
	{
		GameObject blackFrame = Instantiate(Resources.Load<GameObject> ("Prefabs/BlackEdgeFrame"));
		blackFrame.transform.Find ("TopRight").position = new Vector2 (width, height);
	}




	// --- ROOM OBJECT --- //

	public void CreateRoomObject(Room room)
	{

		//Debug.Log (room.bgFlipped + " - normal room");
		bgObject = new GameObject (room.myName);

		SpriteRenderer sr = bgObject.AddComponent<SpriteRenderer> ();

		sr.sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
		sr.sortingOrder = -10;
		sr.sortingLayerName = Constants.room_layer;
		sr.flipX = room.bgFlipped;

		bgObject.transform.position = new Vector3 (room.myWidth/2f, 0, 0);
		bgObject.transform.SetParent (this.transform);

		if (myRoom.myMirrorRoom != null) 
		{		
			//Debug.Log (room.myMirrorRoom.bgFlipped_Shadow + " - shadw room");
			bgObject_Shadow = new GameObject (room.myName + "_shadow");

			SpriteRenderer srShadow = bgObject_Shadow.AddComponent<SpriteRenderer> ();

			srShadow.sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.myMirrorRoom.bgName_Shadow);
			srShadow.flipX = room.myMirrorRoom.bgFlipped_Shadow;
			srShadow.sortingLayerName = Constants.room_layer;

			bgObject_Shadow.transform.position = new Vector3 (room.myWidth/2f, 0, 0);
			bgObject_Shadow.transform.SetParent (this.transform);
		}
	}


	// -- SWITCH BETWEEN SHADOW AND MIRROR -- //

	public void SwitchObjectByShadowState(bool immediately)
	{
		List<SpriteRenderer> fadeInSprites = new List<SpriteRenderer> ();
		List<SpriteRenderer> fadeOutSprites = new List<SpriteRenderer> ();

	//	Debug.Log ("myRoom.myMirrorRoom.inTheShadow is " + myRoom.myMirrorRoom.inTheShadow);

		if (myRoom.myMirrorRoom.inTheShadow == true) 
		{
			//fadeOutSprites.Add (bgObject.GetComponent<SpriteRenderer>());
			fadeInSprites.Add (bgObject_Shadow.GetComponent<SpriteRenderer>());

			foreach (Furniture furn in myRoom.myFurnitureList) 
			{
				if (PI_Handler.instance.PI_gameObjectMap.ContainsKey(furn) == false)
					continue;
				
				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeOutSprites.AddRange (srs);
			}

			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
			{
				if (PI_Handler.instance.PI_gameObjectMap.ContainsKey(furn) == false)
					continue;

				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeInSprites.AddRange (srs);
			}

		} else {

			fadeOutSprites.Add (bgObject_Shadow.GetComponent<SpriteRenderer>());
			//fadeInSprites.Add (bgObject.GetComponent<SpriteRenderer>());

			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
			{
				if (PI_Handler.instance.PI_gameObjectMap.ContainsKey(furn) == false)
					continue;

				SpriteRenderer[] srs = PI_Handler.instance.PI_gameObjectMap [furn].GetComponentsInChildren<SpriteRenderer>();
				fadeOutSprites.AddRange (srs);
			}

			foreach (Furniture furn in myRoom.myFurnitureList) 
			{
				if (PI_Handler.instance.PI_gameObjectMap.ContainsKey(furn) == false)
					continue;

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


	void CreateFurniture (Furniture furn) 
	{
		if (GameManager.userData.hiddenFurnitureList.Contains (furn.identificationName)) 
		{
			furn.hidden = true;
		}

		if (furn.hidden == false)
		{
			EventsHandler.Invoke_cb_furnitureChanged (furn);
		}
	}

	void CreateCharacter (Character character) 
	{
		if (GameManager.userData.hiddenCharacterList.Contains (character.identificationName)) 
		{
			character.hidden = true;
		}

		if (character.hidden == false)
		{			
			EventsHandler.Invoke_cb_characterChanged (character);
		}
	}



	// Get PI by Name


	public Furniture getFurnitureByName(string furniture_name)
	{	

		foreach (Furniture furn in myRoom.myFurnitureList) 
		{
			if (furn.identificationName == furniture_name) 
			{
				return furn;
			}
		}

		if (myRoom.myMirrorRoom != null)
		{
			if (myRoom.myMirrorRoom.inTheShadow == true) 
			{
				foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
				{
					if (furn.identificationName == furniture_name) 
					{
						return furn;
					}
				}
			} 


			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Persistant)
			{
				if (furn.identificationName == furniture_name) 
				{
					return furn;
				}
			}



		} 



		Debug.Log ("furniture is null");

		return null;
	}


	public Character getCharacterByName(string character_name)
	{			
		foreach (Character character in myRoom.myCharacterList)
		{
			if (character.identificationName == character_name)
			{
				return character;
			}
		}

		return null;
	}


}
